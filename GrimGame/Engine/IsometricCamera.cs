using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace GrimGame.Engine
{
    public class IsometricCamera
    {
        public OrthographicCamera OrthographicCamera;

        public IsometricCamera(GraphicsDevice graphicsDevice)
        {
            OrthographicCamera = new OrthographicCamera(graphicsDevice);
        }
        
        public IsometricCamera(ViewportAdapter viewportAdapter)
        {
            OrthographicCamera = new OrthographicCamera(viewportAdapter);
        }

        public Vector2 IsometricToScreen(Vector2 position)
        {
            var posX = (position.X - position.Y) * Globals.TILE_WIDTH;
            var posY = (position.X + position.Y) * Globals.TILE_HEIGHT;
            
            return new Vector2(posX, posY);
        }
    }
}