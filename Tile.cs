using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OptimizedRaycasting.Base
{
    public class Tile
    {
        public Texture2D texture;

        public Vector2 postiion;
        public int tileSize;
        public Color color = Color.White;

        public bool shouldDraw = true;

        public virtual Rectangle Rect {
            get { return new(postiion.ToPoint(), new(tileSize)); }
        }

        public virtual bool IsVisible(Vector2 screenSize)
        {
            if(Rect.Left > screenSize.X) return false;
            if(Rect.Right < 0) return false;
            if(Rect.Top > screenSize.Y) return false;
            if(Rect.Bottom < 0) return false;
            return true;
        }

        public Tile(Texture2D texture, int tileSize)
        {
            this.texture = texture;
            this.tileSize = tileSize;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(!shouldDraw) return;
            if(texture == null) throw new("Texture was null");
            spriteBatch.Draw(texture, Rect, color);
        }
    }
}