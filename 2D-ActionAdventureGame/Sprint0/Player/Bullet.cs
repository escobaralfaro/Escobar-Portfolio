using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
namespace Sprint0.Classes
{
    public class Bullet
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Texture2D Texture { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Vector2 Scale { get; private set; }
        public float Speed { get; private set; }
        public float Lifetime { get; private set; }
        private float currentLifetime;
        public bool IsExpired => currentLifetime >= Lifetime;
        private Vector2 Bulldirection;

        private SpriteEffects spriteEffects;



        public Bullet(Vector2 startPosition, Vector2 direction, Texture2D texture, Rectangle sourceRectangle, Vector2 scale, float speed, float lifetime)
        {
            Position = startPosition;
            Bulldirection = direction;
            Velocity = Vector2.Normalize(Bulldirection) * speed;
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Scale = scale;
            Speed = speed;
            Lifetime = lifetime;
            currentLifetime = 0;
            spriteEffects = SpriteEffects.None;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * deltaTime * 60; ;
            currentLifetime += deltaTime;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if ((Bulldirection.Y) > 0) //going down
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            else if ((Bulldirection.X) < 0) //going left
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }

                spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, spriteEffects, 0f);
        }
    }
}
