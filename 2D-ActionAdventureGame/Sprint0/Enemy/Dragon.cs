using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using Sprint0.Classes;
using Sprint0.Player;
using Sprint2.Enemy.Projectiles;
using System;
using System.Collections.Generic;

namespace Sprint2.Enemy
{
    public class Dragon : IEnemy
    {
        private Texture2D spriteSheet;
        private Rectangle[] sourceRectangles;
        public Vector2 position;
        public Vector2 initialPosition;
        private int currentFrame;
        private bool movingRight = true;
        private float movementRange = 100f;
        private float timePerFrame = 0.1f;
        private float timeElapsed;
        private Color currentColor = Color.White; 
        private float damageColorTimer = 0f;
        private const float DAMAGE_COLOR_DURATION = 0.5f;
        private float fireballCooldown = 1f;
        private float timeSinceLastShot;
        private Vector2 _scale;
        private int health;
        //public bool CollideWall = false;
        //private Rectangle wallLeftBoundingBox;
        //private Rectangle wallRightBoundingBox;
        //private Rectangle wallUpBoundingBox;
        //private Rectangle wallDownBoundingBox;
        //private Rectangle dragonBoundingBox;
        private Link _link;

        private Rectangle[] fireballRectangles;

        public SpriteBatch spriteBatch;
        public Texture2D enemyDeath;
        private Boolean alive;
        private bool isDying;
        private float deathAnimationTimer = 0f;
        private const float DEATH_ANIMATION_DURATION = 0.5f;
        public SoundEffect deathSound;
        private int immunityDuration = 10;
        private int remainingImmunityFrames = 0;
        private bool isImmune;

        private int currentDeathFrame = 0;
        private float deathFrameTime = 0.1f; // Time each death frame is displayed
        private float deathFrameElapsed = 0f;
        private Rectangle[] deathSourceRectangles = { new Rectangle(0, 0, 15, 15), new Rectangle(16, 0, 15, 15), new Rectangle(32, 0, 15, 15), new Rectangle(48, 0, 15, 15)
        };

        //public int enemyDefeatedCount { get; private set; }
        private Game1 game;


        public Vector2 Position { get => position; set => position = value; }
        public int Width { get; private set; } = 24;
        public int Height { get; private set; } = 32;


        public List<Fireball> fireballs { get; private set; }
        public Dragon(Vector2 startPosition, Link link, Game1 game)
        {
            health = 70;
            position = startPosition;
            initialPosition = startPosition;
            fireballs = new List<Fireball>();
            _link = link;
            alive = true;
            this.game = game;
        }

        private static Rectangle GetScaledRectangle(int x, int y, int width, int height, Vector2 scale)
        {
            return new Rectangle(
                x,
                y,
                (int)(width * scale.X),
                (int)(height * scale.Y)
            );
        }


        public void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, Vector2 scale)
        {
            spriteSheet = content.Load<Texture2D>(texturePath);
             
            sourceRectangles = SpriteSheetHelper.CreateDragonFrames();
            fireballRectangles = SpriteSheetHelper.CreateFireballFrames(); 
           _scale = scale;

            enemyDeath = content.Load<Texture2D>("EnemyDeath");
            deathSound = content.Load<SoundEffect>("OOT_Enemy_Poof1");
        }

        public void flipDirection()
        {
            movingRight = !movingRight;
        }

        public void Update(GameTime gameTime)
        {
            if (isDying)
            {
                deathFrameElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (deathFrameElapsed >= deathFrameTime)
                {
                    currentDeathFrame++;
                    deathFrameElapsed = 0f;
                    if (currentDeathFrame >= deathSourceRectangles.Length)
                    {
                        isDying = false;
                        position = new Vector2(20000, 20000);
                        //game.enemyDefeatedCount = game.enemyDefeatedCount + 1;
                        _link.IncrementEnemyDefeatedCount();
                    }
                }


                for (int i = 0; i < fireballs.Count; i++)
                {
                    fireballs.RemoveAt(i);
                    i--;
                }
                
            }
            else if (alive)
            {
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                damageColorTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                MoveDragon();

                if (timeElapsed > timePerFrame)
                {
                    currentFrame = (currentFrame + 1) % sourceRectangles.Length;
                    timeElapsed = 0f;
                }

                if (damageColorTimer <= 0)
                {
                    currentColor = Color.White;
                }

                timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceLastShot > fireballCooldown)
                {
                    ShootFireball();
                    timeSinceLastShot = 0f;
                }

                for (int i = 0; i < fireballs.Count; i++)
                {
                    fireballs[i].Update(gameTime);
                    if (fireballs[i].IsOffScreen())
                    {
                        fireballs.RemoveAt(i);
                        i--;
                    }
                    
                }

                if (isImmune)
                {
                    remainingImmunityFrames--;
                    if (remainingImmunityFrames <= 0)
                    {
                        isImmune = false;
                    }
                }
               
            }
        }


        private void MoveDragon()
        {
            if (movingRight)
            {
                position.X += 1f;
                if (position.X >= initialPosition.X + movementRange)
                    movingRight = false;
            }
            else
            {
                position.X -= 1f;
                if (position.X <= initialPosition.X - movementRange)
                    movingRight = true; // Switch direction  
            }

            //if (!CollideWall)
            //{
            //    _link._position.X -= _link.speed;
            //}
            //else
            //{
            //    _link._position.X -= intersection.Width;
            //}

        }


        private void ShootFireball()
        {
            Vector2 fireballPosition = new Vector2(position.X, position.Y);
            
            // Add fireballs going in different directions
            fireballs.Add(new Fireball(spriteSheet, fireballPosition, new Vector2(-200, 0), fireballRectangles)); 
            fireballs.Add(new Fireball(spriteSheet, fireballPosition, new Vector2(-200, -100), fireballRectangles)); 
            fireballs.Add(new Fireball(spriteSheet, fireballPosition, new Vector2(-200, 100), fireballRectangles)); 
             
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDying)
            {
                spriteBatch.Draw(enemyDeath, position, deathSourceRectangles[currentDeathFrame], Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
            else if (alive)
            {
                spriteBatch.Draw(
               spriteSheet,
               position,
               sourceRectangles[currentFrame],
               currentColor,
               0f,
               Vector2.Zero,
               _scale,
               SpriteEffects.None,
               0f
                );


                foreach (var fireball in fireballs)
                {
                     
                    fireball.Draw(spriteBatch);
                }
            }

           
        }

       
        public void Reset()
        {
            position = initialPosition;
            movingRight = true;
            alive = true;
            currentFrame = 0;
            timeElapsed = 0f;
            damageColorTimer = 0f;
            currentColor = Color.White;
            fireballs.Clear(); 
        }

       
        public void TakeDamage()
        {
            if (!isImmune)
            {
                if (_link.hasPotion)
                {
                    health = Math.Max(0, 0);
                    _link.hasPotion = false;
                }
                else
                {
                    health = Math.Max(0, health - 1);
                }
                remainingImmunityFrames = immunityDuration;
                isImmune = true;
            }
            currentColor = Color.Red;
            damageColorTimer = DAMAGE_COLOR_DURATION;

            if (health <= 0 && alive)
            {
                alive = false;
                isDying = true;
                deathAnimationTimer = DEATH_ANIMATION_DURATION;
                deathSound.Play();
            }

            //if (health <= 0)
            //{
            //    position.X = 20000;
            //    position.Y = 20000;
            //}   
        }

        public Boolean GetState()
        {
            return alive;
        }
    }
}

