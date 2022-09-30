using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OptimizedRaycasting.Base;
using Xssp.MonoGame.Primitives2D;

namespace OptimizedRaycasting.Managers
{
    public class DrawManager
    {
        public Vector2 screenSize;
        public SpriteBatch spriteBatch;

        public List<List<Tile>> sprites = new() { new() };

        private readonly GraphicsDevice graphicsDevice;

        public Color bgColor = Color.Black;
        public bool drawGrid = true;
        private readonly int tileSize;

        private readonly Raycaster raycaster;

        public DrawManager(GraphicsDevice graphicsDevice, Vector2 screenSize, int tileSize, Raycaster raycaster)
        {
            this.graphicsDevice = graphicsDevice;
            spriteBatch = new(graphicsDevice);

            this.screenSize = screenSize;
            this.tileSize = tileSize;
            this.raycaster = raycaster;
        }

        public void Draw()
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin(samplerState: SamplerState.PointWrap);

            if(drawGrid) DrawGrid();

            for(int layer = 0; layer < sprites.Count; layer++)
                DrawLayer(layer);

            raycaster.DrawLine(spriteBatch);
            raycaster.DrawPoints(spriteBatch);

            spriteBatch.End();

            sprites.Clear();
        }

        private void DrawGrid()
        {
            const float transparency = .2f;

            for(int x = -tileSize; x < screenSize.X + tileSize + .1f; x += tileSize)
                spriteBatch.DrawLine(new(x, -tileSize), new(x, screenSize.Y + tileSize), Color.White * transparency, 0);
            for(int y = -tileSize; y < screenSize.Y + tileSize + .1f; y += tileSize)
                spriteBatch.DrawLine(new(-tileSize, y), new(screenSize.X + tileSize, y), Color.White * transparency, 0);
        }

        private void DrawLayer(int layer)
        {
            foreach(var s in sprites[layer])
                s.Draw(spriteBatch);
        }
    }
}