using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint0.Classes;
using System;
using Microsoft.Xna.Framework.Audio;
using Sprint0.Player;
using Sprint0;

namespace Sprint2.Enemy
{
    public class Keese : IEnemy
    {
        private Texture2D spriteSheet;
        private Rectangle[] sourceRectangles;
        private Vector2 position;
        private Vector2 initialPosition;
        private int currentFrame;
        private bool movingRight = true;
        private float movementRange = 100f;
        private float timePerFrame = 0.1f;
        private float timeElapsed;
        private Color currentColor = Color.White;
        private float damageColorTimer = 0f;
        private const float DAMAGE_COLOR_DURATION = 0.5f;
        private int health;
        private Vector2 speed;
        private Vector2 _scale;
        private Random random;
        private Boolean alive;
        private Direction currentDirection;
        private int randCount;

        public SpriteBatch spriteBatch;
        public Texture2D enemyDeath;
        private bool isDying;
        private float deathAnimationTimer = 0f;
        private const float DEATH_ANIMATION_DURATION = 0.5f;
        public SoundEffect deathSound;
        private int immunityDuration = 10;
        private int remainingImmunityFrames = 0;
        private bool isImmune;
        public Link _link;

        private const float CHASE_SPEED = 2.5f; //chase speed
        private const float MIN_DISTANCE = 1f;  
        private const float STUCK_THRESHOLD = 0.1f; // stuck threshold
        private Vector2 lastPosition;  
        private float stuckTimer = 0f;



        private int currentDeathFrame = 0;
        private float deathFrameTime = 0.1f; // Time each death frame is displayed
        private float deathFrameElapsed = 0f;
        private Rectangle[] deathSourceRectangles = { new Rectangle(0, 0, 15, 15), new Rectangle(16, 0, 15, 15), new Rectangle(32, 0, 15, 15), new Rectangle(48, 0, 15, 15)
        };

        //public int enemyDefeatedCount { get; private set; }
        private Game1 game;

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
   
        public Vector2 Position { get => position; set => position = value; }
        public int Width { get; } = 16;
        public int Height { get; } = 16;


        public Keese(Vector2 startPosition, Link link, Game1 game)
        {
            health = 2;
            position = startPosition;
            initialPosition = startPosition;
            alive = true;
            random = new Random();
            SetRandomDirection();
            _link = link;
            this.game = game;


        }

        public void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, Vector2 scale)
        {
            spriteSheet = content.Load<Texture2D>(texturePath);
            sourceRectangles = SpriteSheetHelper.CreateKeeseFrames();
            _scale = scale;
            speed = Vector2.One;

            enemyDeath = content.Load<Texture2D>("EnemyDeath");
            deathSound = content.Load<SoundEffect>("OOT_Enemy_Poof1");
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
                        position = new Vector2(20000, 20000); // Move off screen
                        //zgame.enemyDefeatedCount = game.enemyDefeatedCount + 1;
                        _link.IncrementEnemyDefeatedCount();
                    }
                }
            }
            else if (alive)
            {
                randCount++;
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                damageColorTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                 
                MoveKeese();

                if (timeElapsed > timePerFrame)
                {
                    currentFrame = (currentFrame + 1) % sourceRectangles.Length;
                    timeElapsed = 0f;
                }
                // Reset color 
                if (damageColorTimer <= 0)
                {
                    currentColor = Color.White;
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
        private void SetRandomDirection()
        {
            currentDirection = (Direction)random.Next(0,4);
            
        }

        private void MoveKeese()
        {
            if (alive)
            {
                Vector2 linkPosition = _link.GetLocation();
                Vector2 direction = linkPosition - position;
                float distanceToLink = direction.Length();

                // check if stuck
                float movementDelta = Vector2.Distance(position, lastPosition);
                if (movementDelta < STUCK_THRESHOLD)
                {
                    stuckTimer += 0.016f;  
                    if (stuckTimer > 1.0f) // if keese stuck here over 1s
                    {
                        //  bread out of stuck
                        position += new Vector2(
                            (float)(random.NextDouble() - 0.5) * 5f,
                            (float)(random.NextDouble() - 0.5) * 5f
                        );
                        stuckTimer = 0f;
                    }
                }
                else
                {
                    stuckTimer = 0f;
                }

               
                if (distanceToLink > 0)
                {
                    direction.Normalize();

                    // Calculate new position of Link
                    Vector2 newPosition = position + direction * CHASE_SPEED;

                    //   update position
                    float boundaryBuffer = 4f;  
                    if (newPosition.X > (32 + boundaryBuffer) * _scale.X &&
                        newPosition.X < (214 - boundaryBuffer) * _scale.X &&
                        newPosition.Y > (87 + boundaryBuffer) * _scale.Y &&
                        newPosition.Y < (143 - boundaryBuffer) * _scale.Y)
                    {
                        position = newPosition;
                    }
                    else
                    {
                        // If hitting boundary, try moving only in valid direction
                        if (newPosition.X >= (32 + boundaryBuffer) * _scale.X &&
                            newPosition.X <= (214 - boundaryBuffer) * _scale.X)
                        {
                            position.X = newPosition.X;
                        }
                        if (newPosition.Y >= (87 + boundaryBuffer) * _scale.Y &&
                            newPosition.Y <= (143 - boundaryBuffer) * _scale.Y)
                        {
                            position.Y = newPosition.Y;
                        }
                    }

                    // Update direction 
                    if (Math.Abs(direction.X) > Math.Abs(direction.Y))
                    {
                        currentDirection = direction.X > 0 ? Direction.Right : Direction.Left;
                    }
                    else
                    {
                        currentDirection = direction.Y > 0 ? Direction.Down : Direction.Up;
                    }
                }

                lastPosition = position; // Update position for stuck
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDying)
            {
                spriteBatch.Draw(enemyDeath, position, deathSourceRectangles[currentDeathFrame], Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
            else if (alive)
            {
                spriteBatch.Draw(spriteSheet, position, sourceRectangles[currentFrame], currentColor, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
        }

     
        public void TakeDamage()
        {
            currentColor = Color.Red;
            damageColorTimer = DAMAGE_COLOR_DURATION;
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

            if (health <= 0 && alive)
            {
                alive = false;
                isDying = true;
                deathAnimationTimer = DEATH_ANIMATION_DURATION;
                deathSound.Play();
            }

            //if (hurtTime <= 0)
            //{
            //    alive = false;
            //    position.X = 20000;
            //    position.Y = 20000;
            //}
        }

        

        public void Reset()
        {
            position = initialPosition;
            currentFrame = 0;
            timeElapsed = 0f;
            damageColorTimer = 0f;
            currentColor = Color.White;
        }
        public Boolean GetState()
        {
            return alive;
        }

    }
}
