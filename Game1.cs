using Microsoft.Xna.Framework;
using InputHelper = Apos.Input.InputHelper;

namespace OptimizedRaycasting
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private Managers.GameManager gameManager;

        public Game1()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameManager = new(this, graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            InputHelper.Setup(this);

            gameManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.UpdateSetup();

            gameManager.Update(gameTime);

            base.Update(gameTime);
            InputHelper.UpdateCleanup();
        }

        protected override void Draw(GameTime gameTime)
        {
            gameManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}