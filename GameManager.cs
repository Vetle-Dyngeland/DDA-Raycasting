using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Apos.Input;

namespace OptimizedRaycasting.Managers
{
    public class GameManager
    {
        public readonly Game game;

        public Vector2 screenSize = new(1600, 900);
        public readonly bool isFullScreen = false;

        public DrawManager drawManager;
        public LevelManager levelManager;
        public Raycaster raycaster;

        private readonly ICondition exitCondition = new KeyboardCondition(Keys.Escape);

        public GameManager(Game game, GraphicsDeviceManager graphics)
        {
            this.game = game;

            SetScreenSettings(graphics);
            InitializeClasses();
        }

        private void SetScreenSettings(GraphicsDeviceManager graphics)
        {
            if(isFullScreen) {
                int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                screenSize = new(w, h);
            }
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();
        }

        private void InitializeClasses()
        {
            raycaster = new(LevelManager.tileSize);
            levelManager = new(screenSize);
            drawManager = new(game.GraphicsDevice, screenSize, LevelManager.tileSize, raycaster);
        }

        public void LoadContent()
        {
            ContentLoader.LoadContent(game.Content);

            levelManager.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if(exitCondition.Pressed()) game.Exit();

            levelManager.Update();
            raycaster.Update(gameTime, levelManager.level);
        }

        public void Draw(GameTime gameTime)
        {
            drawManager.sprites.Add(new());

            foreach(var tile in levelManager.board.board)
                drawManager.sprites[0].Add(tile);

            drawManager.Draw();
        }
    }
}