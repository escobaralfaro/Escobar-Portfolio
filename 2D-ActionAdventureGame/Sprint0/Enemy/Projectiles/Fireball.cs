using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint2.Enemy.Projectiles
{
    public class Fireball
    {
        protected Texture2D spriteSheet;
        protected Rectangle[] sourceRectangles;
        protected int currentFrame;
        public Vector2 position;
        protected Vector2 velocity;
        private float timePerFrame = 0.1f; 
        private float timeElapsed;
        private float scale = 2.0f;

        public Vector2 Position
        {
            get { return position; }
        }

        public Fireball(Texture2D spriteSheet, Vector2 startPosition, Vector2 velocity, Rectangle[] fireballFrames)
        {
            this.spriteSheet = spriteSheet;
            this.position = startPosition;
            this.velocity = velocity;
            this.sourceRectangles = fireballFrames;
            this.currentFrame = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
          
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

          
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timePerFrame)
            {
                currentFrame = (currentFrame + 1) % sourceRectangles.Length;
                timeElapsed = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            System.Diagnostics.Debug.WriteLine($"Drawing fireball at position: {position}");
            spriteBatch.Draw(
                spriteSheet,
                position,
                sourceRectangles[currentFrame],  
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public bool IsOffScreen()
        {

            return position.X < -100 || position.Y < -200 || position.X > 1000 || position.Y > 800;
        }
    }
}
