using Microsoft.Xna.Framework;
using OptimizedRaycasting.Base;
using Apos.Input;
using System;

namespace OptimizedRaycasting.Managers
{
    public class LevelManager
    {
        private readonly ICondition createTileCondition = new MouseCondition(MouseButton.RightButton);

        public readonly bool[,] level;

        public readonly Board board;

        public const int tileSize = 32;
        public readonly int width, height;

        private int mouseTileX, mouseTileY;
        private bool? createTileType;

        public LevelManager(Vector2 screenSize)
        {
            width = (int)MathF.Ceiling(screenSize.X / tileSize);
            height = (int)MathF.Ceiling(screenSize.Y / tileSize);

            board = new(tileSize, width, height);
            level = new bool[width, height];

            for(int x = 0; x < level.GetLength(0); x++) {
                level[x, 0] = true;
                level[x, level.GetLength(1) - 1] = true;
            }
            for(int y = 0; y < level.GetLength(1); y++) {
                level[0, y] = true;
                level[level.GetLength(0) - 1, y] = true;
            }
        }

        public void LoadContent()
        {
            board.LoadContent();
        }

        public void Update()
        {
            board.Generate(level);

            LevelCreation();
        }

        #region Level Creation
        private void LevelCreation()
        {
            GetMouseTilePosition();
            if(createTileCondition.Held())
                CreateTile();
            else createTileType = null;
        }

        private void GetMouseTilePosition()
        {
            Vector2 mouseTileVector = Vector2.Floor(InputHelper.NewMouse.Position.ToVector2() / tileSize);
            mouseTileX = (int)mouseTileVector.X;
            mouseTileY = (int)mouseTileVector.Y;

            if(mouseTileX < 0) mouseTileX = 0;
            if(mouseTileY < 0) mouseTileY = 0;
            if(mouseTileX >= width) mouseTileX = width - 1;
            if(mouseTileY >= height) mouseTileY = height - 1;
        }

        private void CreateTile()
        {
            createTileType ??= !level[mouseTileX, mouseTileY];
            level[mouseTileX, mouseTileY] = createTileType.Value;
        }
        #endregion Level Creation
    }
}