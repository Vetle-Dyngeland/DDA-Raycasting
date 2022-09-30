using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Apos.Input;
using System;
using Xssp.MonoGame.Primitives2D;
using Microsoft.Xna.Framework.Content;

namespace OptimizedRaycasting.Managers
{
    public class Raycaster
    {
        private Vector2 playerPos;
        private readonly Vector2?[] intersectionPoints = new Vector2?[360];

        private readonly ICondition[] moveConditions = new KeyboardCondition[] {
            new(Keys.W), new(Keys.S), new(Keys.A), new(Keys.D)
        };
        private readonly ICondition drawLineCondition = new MouseCondition(MouseButton.LeftButton);

        private readonly int tileSize;

        private Texture2D circleTexture;

        public Raycaster(int tileSize) => this.tileSize = tileSize;

        public void Update(GameTime gameTime, bool[,] map)
        {
            if(circleTexture == null) circleTexture = ContentLoader.circleTexture;

            MovePoints(gameTime);
            
            if(playerPos.X > 0 && playerPos.Y > 0 && playerPos.X < map.GetLength(0) && playerPos.Y < map.GetLength(1))
                for(int angle = 0; angle < 360; angle++) {
                    intersectionPoints[angle] = null;
                    CastRay(map, playerPos, angle);
                }
        }

        private void MovePoints(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 moveVector = Vector2.Zero;
            moveVector.Y = Convert.ToInt16(moveConditions[0].Held()) - Convert.ToInt16(moveConditions[1].Held());
            moveVector.X = Convert.ToInt16(moveConditions[2].Held()) - Convert.ToInt16(moveConditions[3].Held());
            if(moveVector.Length() != 0) moveVector /= moveVector.Length();

            const int moveSpeed = 164;
            playerPos += -moveVector * moveSpeed * deltaTime;
        }

        private void CastRay(bool[,] map, Vector2 rayStart, int angle)
        {
            Vector2 rayEnd = (rayStart + new Vector2(MathF.Sin(angle), MathF.Cos(angle))) / tileSize;
            rayStart /= tileSize;
            Vector2 rayDir = rayStart - rayEnd; rayDir.Normalize();

            Vector2 rayUnitStepSize = new(MathF.Sqrt(1 + MathF.Pow(rayDir.Y / rayDir.X, 2)), MathF.Sqrt(1 + MathF.Pow(rayDir.X / rayDir.Y, 2)));

            Vector2Int mapCheck = new(rayStart);
            Vector2 rayLength;

            Vector2Int step;

            if(rayDir.X < 0) {
                step.X = -1;
                rayLength.X = rayStart.X - mapCheck.X;
            }
            else {
                step.X = 1;
                rayLength.X = mapCheck.X + 1 - rayStart.X;
            }
            if(rayDir.Y < 0) {
                step.Y = -1;
                rayLength.Y = rayStart.Y - mapCheck.Y;
            }
            else {
                step.Y = 1;
                rayLength.Y = mapCheck.Y + 1 - rayStart.Y;
            }
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

                if(mapCheck.Y >= 0 && mapCheck.X < map.GetLength(0) && mapCheck.Y >= 0 && mapCheck.Y < map.GetLength(1)) {
                    if(map[mapCheck.X, mapCheck.Y])
                        tileFound = true;
                }
                else break;
            }
            if(tileFound)
                intersectionPoints[angle] = rayStart + rayDir * currentDist;
        }
        

        #region Draw
        public void DrawPoints(SpriteBatch spriteBatch)
        {
            DrawCircle(spriteBatch, playerPos, 6, Color.Red);
            foreach(var point in intersectionPoints)
                if(point != null)
                    DrawCircle(spriteBatch, point.Value * tileSize, 6, Color.CornflowerBlue);

        }

        public void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            spriteBatch.Draw(circleTexture, new Rectangle((center - Vector2.One * radius).ToPoint(), new((int)radius * 2)), color);
        }

        public void DrawLine(SpriteBatch spriteBatch)
        {
            foreach(var point in intersectionPoints) 
                if(point != null)
                    spriteBatch.DrawLine(playerPos, point.Value, Color.Yellow * .5f, 0);
        }
        #endregion Draw
    }

    public struct Vector2Int
    {
        public int X, Y;

        #region Constructors
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2Int(Vector2 v)
        {
            X = (int)v.X;
            Y = (int)v.Y;
        }

        public Vector2Int(Vector2Int v)
        {
            X = v.X;
            Y = v.Y;
        }
        #endregion Constructors

        public Vector2 ToVector2()
        {
            return new(X, Y);
        }
    }
}