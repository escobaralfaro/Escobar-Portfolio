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
    public class Stalfos : IEnemy
    {
        public Link _link;
        private Texture2D spriteSheet;
        private Rectangle[] sourceRectangles;
        private Vector2 position;
        private Vector2 initialPosition;
        private int currentFrame;
        private bool movingRight = true;
        private float movementRange = 2000f;
        private float distanceMovedInDirection = 0f;
        private float timePerFrame = 0.1f;
        private float timeElapsed;
        private Color currentColor = Color.White;
        private float damageColorTimer = 0f;
        private const float DAMAGE_COLOR_DURATION = 0.5f;
        private int healthCount;
        private bool isFliped = false;

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

        private int currentDeathFrame = 0;
        private float deathFrameTime = 0.1f; // Time each death frame is displayed
        private float deathFrameElapsed = 0f;
        private Rectangle[] deathSourceRectangles = { new Rectangle(0, 0, 15, 15), new Rectangle(16, 0, 15, 15), new Rectangle(32, 0, 15, 15), new Rectangle(48, 0, 15, 15)
        };

        public Vector2 Position { get => position; set => position = value; }
        public int Width { get; } = 16;
        public int Height { get; } = 16;

        //public int enemyDefeatedCount { get; private set; }
        private Game1 game;



        public Stalfos(Vector2 startPosition, Link link, Game1 game)
        {
            position = startPosition;
            initialPosition = startPosition;
            alive = true;
            random = new Random();
            SetNewRandomDirection();
            _link = link;
            this.game = game;
           
        }
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, Vector2 scale)
        {
            healthCount = 3;
            spriteSheet = content.Load<Texture2D>(texturePath);
            sourceRectangles = SpriteSheetHelper.CreateStalfosFrames();
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
                        //game.enemyDefeatedCount = game.enemyDefeatedCount + 1;
                        _link.IncrementEnemyDefeatedCount();
                    }
                }
            }
            else if (alive)
            {   
                randCount++;
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                damageColorTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(randCount > 60 * random.Next(3, 6))
                {
                    SetNewRandomDirection();
                    randCount = 0;
                }
                MoveStalfos();

                if (timeElapsed > 0.1f)
                {
                    isFliped = !isFliped;
                    timeElapsed = 0f;
                }
                // Reset color after damage timer expires
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

        private void MoveStalfos()
        {
            if (alive)
            {
                float moveDistance = speed.X * 3; //   movement speed
                Vector2 newPosition = position;

                switch (currentDirection)
                {
                    case Direction.Left:
                        if (position.X - moveDistance > 32 * _scale.X)
                        {
                            newPosition.X -= moveDistance;
                        }
                        else
                        {
                            SetNewRandomDirection(); // Change direction if hitting boundary
                        }
                        break;
                    case Direction.Right:
                        if (position.X + moveDistance < 208 * _scale.X)
                        {
                            newPosition.X += moveDistance;
                        }
                        else
                        {
                            SetNewRandomDirection();
                        }
                        break;
                    case Direction.Up:
                        if (position.Y - moveDistance > 87 * _scale.Y)
                        {
                            newPosition.Y -= moveDistance;
                        }
                        else
                        {
                            SetNewRandomDirection();
                        }
                        break;
                    case Direction.Down:
                        if (position.Y + moveDistance < 143 * _scale.Y)
                        {
                            newPosition.Y += moveDistance;
                        }
                        else
                        {
                            SetNewRandomDirection();
                        }
                        break;
                }

                distanceMovedInDirection += moveDistance;

                // Only change direction hit a wall 
                if (distanceMovedInDirection >= movementRange)
                {
                   
                    if (random.Next(100) < 70)  
                    {
                        SetNewRandomDirection();
                        distanceMovedInDirection = 0f;
                    }
                }

                position = newPosition;
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
                SpriteEffects spriteEffect = isFliped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(spriteSheet, position, sourceRectangles[currentFrame], currentColor, 0f, Vector2.Zero, _scale, spriteEffect, 0f);
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
                    healthCount = Math.Max(0, 0);
                    _link.hasPotion = false;
                }
                else
                {
                    healthCount = Math.Max(0, healthCount - 1);
                }
                remainingImmunityFrames = immunityDuration;
                isImmune = true;
            }


            if (healthCount <= 0 && alive)
            {
                alive = false;
                isDying = true;
                deathAnimationTimer = DEATH_ANIMATION_DURATION;
                deathSound.Play();
            }
        }


        public void Reset()
        {
            position = initialPosition;
            movingRight = true;
            currentFrame = 0;
            timeElapsed = 0f;
            damageColorTimer = 0f;
            currentColor = Color.White;
        }

        public void SetNewRandomDirection()
        {
            Direction newDirection;
            bool isCurrentHorizontal = (currentDirection == Direction.Left || currentDirection == Direction.Right);

            // alternate between horizontal and vertical movement
            if (isCurrentHorizontal)
            {
                 
                newDirection = (random.Next(2) == 0) ? Direction.Up : Direction.Down;
            }
            else
            {
                 
                newDirection = (random.Next(2) == 0) ? Direction.Left : Direction.Right;
            }
 
            if (position.X < 40 * _scale.X)
            {
                newDirection = Direction.Right;
            }
            else if (position.X > 200 * _scale.X)
            {
                newDirection = Direction.Left;
            }
            else if (position.Y < 90 * _scale.Y)
            {
                newDirection = Direction.Down;
            }
            else if (position.Y > 140 * _scale.Y)
            {
                newDirection = Direction.Up;
            }

            currentDirection = newDirection;
        }

        public Boolean GetState()
        {
            return alive;
        }

    }
}

