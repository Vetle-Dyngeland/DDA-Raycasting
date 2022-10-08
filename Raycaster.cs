using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Xssp.MonoGame.Primitives2D;

namespace OptimizedRaycasting.Managers
{
    public class Raycaster
    {
        private readonly Vector2?[] intersectionPoints = new Vector2?[360];
        private Vector2 rayStart;
        private readonly int tileSize;

        private Texture2D circleTexture;

        public Raycaster(int tileSize) => this.tileSize = tileSize;

        public void Update(bool[,] map)
        {
            circleTexture ??= ContentLoader.circleTexture;

            rayStart = InputHelper.NewMouse.Position.ToVector2() / tileSize;
            for(int angle = 0; angle < 360; angle++)
                CastRay(map, angle);
        }
        private void CastRay(bool[,] map, int angle)
        {
            intersectionPoints[angle] = null;

            float radiants = MathHelper.ToRadians(angle);
            Vector2 rayEnd = rayStart + new Vector2(MathF.Cos(radiants), MathF.Sin(radiants)) / tileSize;
            Vector2 rayDir = rayStart - rayEnd; rayDir.Normalize();

            Vector2 rayUnitStepSize = new(MathF.Sqrt(1 + MathF.Pow(rayDir.Y / rayDir.X, 2)), MathF.Sqrt(1 + MathF.Pow(rayDir.X / rayDir.Y, 2)));

            Vector2Int mapCheck = new(rayStart);
            Vector2 rayLength;

            Vector2Int step;

            step = new(rayDir.X.GetValue(), rayDir.Y.GetValue());
            if(step.X == 0) step.X = 1;
            if(step.Y == 0) step.Y = 1;

            if(rayDir.X < 0) rayLength.X = rayStart.X - mapCheck.X;
            else rayLength.X = mapCheck.X + 1 - rayStart.X;
            if(rayDir.Y < 0) rayLength.Y = rayStart.Y - mapCheck.Y;
            else rayLength.Y = mapCheck.Y + 1 - rayStart.Y;
            rayLength *= rayUnitStepSize;

            float maxDist = 100f;
            float currentDist = 0f;
            bool tileFound = false;
            while(!tileFound && currentDist < maxDist) {
                if(rayLength.X < rayLength.Y) {
                    mapCheck.X += step.X;
                    currentDist = rayLength.X;
                    rayLength.X += rayUnitStepSize.X;
                }
                else {
                    mapCheck.Y += step.Y;
                    currentDist = rayLength.Y;
                    rayLength.Y += rayUnitStepSize.Y;
                }

                if(!(mapCheck.X >= 0 && mapCheck.X < map.GetLength(0) && mapCheck.Y >= 0 && mapCheck.Y < map.GetLength(1)))
                    break;
                if(map[mapCheck.X, mapCheck.Y])
                    tileFound = true;
            }

            intersectionPoints[angle] = tileFound ? rayStart + rayDir * currentDist : null;
        }
        

        #region Draw
        public void DrawPoints(SpriteBatch spriteBatch)
        {
            DrawCircle(spriteBatch, rayStart * tileSize, 6, Color.Red);
            foreach(var point in intersectionPoints)
                if(point != null) {
                    DrawCircle(spriteBatch, point.Value * tileSize, 6, Color.CornflowerBlue);
                }
        }

        public void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            spriteBatch.Draw(circleTexture, new Rectangle((center - Vector2.One * radius).ToPoint(), new((int)radius * 2)), color);
        }

        public void DrawLines(SpriteBatch spriteBatch)
        {
            foreach(var point in intersectionPoints) 
                if(point != null)
                    spriteBatch.DrawLine(rayStart * tileSize, point.Value * tileSize, Color.Yellow * .4f, 0);
        }
        #endregion Draw
    }
}