using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint2.Enemy.Projectiles
{
    public class Boomerang
    {
        private Texture2D spriteSheet;
        private Rectangle[] sourceRectangles;
        public Vector2 position;
        private Vector2 velocity;
        private Vector2 startPosition;
        private bool returning = false;
        private float scale; 
        private float distanceToTravel; 
        private int currentFrame;
        private float timePerFrame = 0.1f; 
        private float elapsedTime = 0f;


        public Vector2 Position
        {
            get { return position; }
        }
        
        public Boomerang(Texture2D spriteSheet, Vector2 startPosition, Vector2 velocity, Rectangle[] frames, float scale = 1.0f, float distanceToTravel = 150f)
        {
            this.spriteSheet = spriteSheet;
            this.startPosition = startPosition;
            this.position = startPosition;
            this.velocity = velocity;
            this.sourceRectangles = frames;
            this.scale = scale; 
            this.distanceToTravel = distanceToTravel; 
            this.currentFrame = 0;
        }

      
        public void Update(GameTime gameTime)
        {
           
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

           
            if (!returning && Vector2.Distance(startPosition, position) > distanceToTravel)
            {
                velocity *= -1; 
                returning = true;
            }

         
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTime > timePerFrame)
            {
                currentFrame = (currentFrame + 1) % sourceRectangles.Length;
                elapsedTime = 0f;
            }
        }

       
        public bool IsReturned()
        {
            return returning && Vector2.Distance(startPosition, position) < 5f;
        }

       
        public void Draw(SpriteBatch spriteBatch)
        {
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
    }
}
