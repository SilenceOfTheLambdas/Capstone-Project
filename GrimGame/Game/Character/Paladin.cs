using System;
using System.Collections.Generic;
using System.Linq;
using GrimGame.Engine;
using GrimGame.Engine.AI;
using GrimGame.Engine.Models;
using GrimGame.Game.Character.AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GrimGame.Game.Character
{
    public class Paladin : Enemy
    {
        private          AnimationManager _animationManager;
        private readonly MapSystem        _mapSystem;
        private readonly List<Vector2>    _spawnPoints;
        private readonly Player           _player;
        private          BtNode           _rootNode;

        public Paladin(MapSystem mapSystem, Player player)
        {
            _mapSystem = mapSystem;
            _spawnPoints = new List<Vector2>();
            _player = player;
        }

        public override void Init()
        {
            // Set spawn position if this enemy
            foreach (var objectLayer in _mapSystem.Map.ObjectLayers)
                foreach (var layerObject in objectLayer.Objects)
                {
                    if (layerObject.Name.ToLower().Equals("enemyspawn"))
                    {
                        _spawnPoints.Add(layerObject.Position);
                    }
                }
            
            // Set random spawn point from list
            var random = new Random(Globals.GameTime.ElapsedGameTime.Milliseconds);
            Position = _spawnPoints[random.Next(0, _spawnPoints.Count)];
            
            Scale = new Vector2(1.4f, 1.4f);
            Sprite = new Sprite(new Dictionary<string, Animation>
            {
                {
                    "walk_up",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Enemies/Paladin/Animations/walk_up"), 2)
                },
                {
                    "walk_down",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Enemies/Paladin/Animations/walk_down"), 2)
                },
                {
                    "walk_left",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Enemies/Paladin/Animations/walk_left"), 2)
                },
                {
                    "walk_right",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Enemies/Paladin/Animations/walk_right"), 2)
                }
            })
            {
                Width = 19,
                Height = 29
            };
            Texture = Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_up");
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height);
            Width = (int) (Sprite.Width * Scale.X);
            Height = (int) (Sprite.Height * Scale.Y);
            
            _animationManager = new AnimationManager(Sprite.Animations.FirstOrDefault().Value);

            BoxCollider = new BoxCollider(new Vector2(Position.X, Position.Y),
                new Point(19, 16));
            
            ConstructBehaviourTree();
        }

        private void ConstructBehaviourTree()
        {
            var chaseNode     = new ChaseNode(_player, this);
            var chaseSequence = new BtSequencer(new List<BtNode> { chaseNode });
            _rootNode = new BtSelector(new List<BtNode>()
            {
                chaseSequence
            });
        }
        
        public override void Update(GameTime gameTime)
        {
            BoxCollider.Update(gameTime);

            _animationManager.Position = Position;
            _animationManager.Origin = Origin;
            _animationManager.Scale = Scale;
            _animationManager.Rotation = Rotation;
            _animationManager.Update(gameTime);

            
            _rootNode.Execute(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(),
                samplerState: new SamplerState() {Filter = TextureFilter.Point });
            _animationManager.Draw();
            Globals.SpriteBatch.End();
        }
    }
}