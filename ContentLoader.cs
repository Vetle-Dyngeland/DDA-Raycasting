using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OptimizedRaycasting.Managers
{
    public static class ContentLoader
    {
        public static Texture2D whitePixelTexture;
        public static Texture2D circleTexture;

        public static void LoadContent(ContentManager content)
        {
            whitePixelTexture = content.Load<Texture2D>("whitePixel");
            circleTexture = content.Load<Texture2D>("circle");
        }
    }
}