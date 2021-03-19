#nullable enable
using System;
using System.Collections.Generic;
using GrimGame.Engine;
using GrimGame.Engine.GUI;
using GrimGame.Engine.GUI.Components;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    public class Shop
    {
        private readonly Canvas       _canvas;
        private readonly Player       _player;
        private readonly SpriteFont   _buttonFont  = Globals.ContentManager.Load<SpriteFont>("Fonts/buttonText");
        private readonly List<Button> _itemButtons = new List<Button>();
        private          Rectangle    _mouseBounds;
        public           bool         IsActive;

        /// <summary>
        /// Represents a list of purchasable upgrades.
        /// </summary>
        private readonly Dictionary<string, Tuple<int, float>> _items = new Dictionary<string, Tuple<int, float>>
        {
            {"Run Speed", new Tuple<int, float>(9, 1.2f)},
            {"Max HP", new Tuple<int, float>(13, 5)},
            {"Attack Speed", new Tuple<int, float>(11, 0.2f)}
        };

        public Shop(Player player)
        {
            _player = player;
            _canvas = new Canvas();

            Panel backgroundPanel = new Panel(Panel.Positions.CenterMiddle, new Vector2(400, 400), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG")
            };

            TextBox shopTitle = new TextBox(new Vector2(backgroundPanel.Bounds.Left + 70,
                    backgroundPanel.Bounds.Top),
                new Vector2(380,
                    40),
                Color.White);
            shopTitle.SetText("PURCHASE UPGRADES", Color.White, _buttonFont);

            Panel itemListPanel = new Panel(Panel.Positions.CenterMiddle, new Vector2(280, 300), Color.Pink)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG")
            };

            var padding = 0;
            foreach (var (name, upgradeInfo) in _items)
            {
                padding += 80;
                var newButton = new Button($"{name}\nPrice: {upgradeInfo.Item1}", new Vector2(
                        itemListPanel.Bounds.Right - 140,
                        itemListPanel.Bounds.Top + padding), new Vector2(140, 60),
                    Color.White,
                    Color.Black, _buttonFont)
                {
                    ButtonHoverColor = Color.WhiteSmoke,
                    TextHoverColor = Color.DarkGray
                };
                _itemButtons.Add(newButton);
                itemListPanel.AddComponent(newButton);
            }

            _itemButtons[0].Click += PurchaseRunSpeedUpgrade;
            _itemButtons[1].Click += PurchaseHealthUpgrade;
            _itemButtons[2].Click += PurchaseAttackSpeedUpgrade;

            backgroundPanel.AddComponent(shopTitle);
            backgroundPanel.AddPanel(itemListPanel);
            _canvas.AddPanel(backgroundPanel);
        }

        public void Update()
        {
            if (!IsActive) return;

            _mouseBounds = new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1);
            _canvas.Update();
        }

        private void PurchaseHealthUpgrade(object? sender, EventArgs e)
        {
            if (_player.Coins >= _items["Max HP"].Item1)
            {
                if (_itemButtons[1].Bounds.Intersects(_mouseBounds))
                {
                    _player.MaxHp += (int) _items["Max HP"].Item2;
                    _player.Coins -= _items["Max HP"].Item1;
                    Console.WriteLine("Purchased player HP");
                }
            }
        }

        private void PurchaseRunSpeedUpgrade(object? sender, EventArgs e)
        {
            if (_player.Coins >= _items["Run Speed"].Item1)
            {
                if (_itemButtons[0].Bounds.Intersects(_mouseBounds))
                {
                    _player.RunningSpeed += _items["Run Speed"].Item2;
                    _player.Coins -= _items["Run Speed"].Item1;
                    Console.WriteLine("Purchased player Run Speed");
                }
            }
        }

        private void PurchaseAttackSpeedUpgrade(object? sender, EventArgs e)
        {
            if (_player.Coins >= _items["Attack Speed"].Item1)
            {
                if (_itemButtons[2].Bounds.Intersects(_mouseBounds))
                {
                    _player.AttackTimer -= _items["Attack Speed"].Item2;
                    _player.Coins -= _items["Attack Speed"].Item1;
                    Console.WriteLine("Purchased player Attack Speed");
                }
            }
        }

        public void Draw()
        {
            if (IsActive)
                _canvas.Draw();
        }
    }
}