using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using OptimizedRaycasting.Managers;

namespace OptimizedRaycasting.Base
{
    public class Board
    {
        public Tile[,] board = new Tile[0, 0];
        private readonly int tileSize;
        private bool hasLoadedContent;

        private readonly int width, height;

        public Board(int tileSize, int width, int height)
        {
            this.tileSize = tileSize;
            this.width = width;
            this.height = height;
        }

        public void LoadContent()
        {
            hasLoadedContent = true;
            foreach(var tile in board)
                tile.texture = ContentLoader.whitePixelTexture;
        }

        public void Generate(bool[,] level)
        {
            board = new Tile[width, height];

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    Texture2D texture = null;
                    if(hasLoadedContent) texture = ContentLoader.whitePixelTexture;

                    board[x, y] = new Tile(texture, tileSize) {
                        postiion = new Vector2(x, y) * tileSize,
                        shouldDraw = level[x, y]
                    };
                }
            }
        }
    }
}