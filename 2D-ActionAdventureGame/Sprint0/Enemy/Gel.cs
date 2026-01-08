using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint0.Classes;
using System;
using Microsoft.Xna.Framework.Audio;
using Sprint0.Player;
using Sprint0;
using Sprint2.Map;

namespace Sprint2.Enemy
{
    public class Gel : IEnemy
    {
        private Texture2D spriteSheet;
        private Rectangle[] sourceRectangles;
        public Vector2 position;
        private Vector2 initialPosition;
        private int currentFrame;
        private bool movingRight = true;
        private float movementRange = 100f;
        private float timePerFrame = 0.1f;
        private float timeElapsed;
        private Color currentColor = Color.White;  
        private float damageColorTimer = 0f;
        private const float DAMAGE_COLOR_DURATION = 0.5f;
        private Vector2 _scale;
        private int Health;
        private Boolean alive;
        private Direction currentDirection;
        private Random random;
        private Vector2 speed;
        private int randCount;
        public Link _link;

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

        //public int enemyDefeatedCount { get; private set; }
        private Game1 game;
        //private StageManager _stageManager;


        public Vector2 Position { get => position; set => position = value; }
        public int Width { get; } = 8;
        public int Height { get; } = 16;

        public Gel(Vector2 startPosition, Link link)
        {
            Health = 2;
            position = startPosition;
            initialPosition = startPosition;
            alive = true;
            random = new Random();
            randCount = 0;
            SetRandomDirection();
            _link = link;
            //this.game = game;
            //_stageManager = stageManager;
        }

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public Boolean GetState()
        {
            return alive;
        }

        public void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, Vector2 scale)
        {
            spriteSheet = content.Load<Texture2D>(texturePath);
            sourceRectangles = SpriteSheetHelper.CreateGelFrames();
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
                        //game.enemyDefeatedCount++;
                        //_stageManager.IncrementEnemyDefeatedCount();
                        _link.IncrementEnemyDefeatedCount();
                    }
                }
            }
            else if (alive)
            {
                randCount++;
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                damageColorTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (randCount > 60 * random.Next(1, 3))
                {
                    SetRandomDirection();
                    randCount = 0;
                }
                MoveGel();

                if (timeElapsed > timePerFrame)
                {
                    currentFrame = (currentFrame + 1) % sourceRectangles.Length;
                    timeElapsed = 0f;
                }

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

        private void MoveGel()
        {
            if (alive)
            {
                switch (currentDirection)
                {

                    case (Direction.Left):
                        //if(!blocked){
                        if (position.X - speed.X > 32 * _scale.X)
                        {
                            position.X -= speed.X;
                        }
                        else
                        {
                            currentDirection = Direction.Right;
                        }
                        break;
                    case (Direction.Right):
                        if (position.X + speed.X < 214 * _scale.X)
                        {
                            position.X += speed.X;
                        }
                        else
                        {
                            currentDirection = Direction.Left;
                        }
                        break;
                    case (Direction.Up):
                        if (position.Y - speed.Y > 87 * _scale.Y)
                        {
                            position.Y -= speed.Y;
                        }
                        else
                        {
                            currentDirection = Direction.Down;
                        }
                        break;
                    case (Direction.Down):
                        if (position.Y + speed.Y < 182 * _scale.Y)
                        {
                            position.Y += speed.Y;
                        }
                        else
                        {
                            currentDirection = Direction.Up;
                        }

                        break;
                }

            }
        }

        private void SetRandomDirection()
        {
            currentDirection = (Direction)random.Next(0, 4);

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
            if (!isImmune)
            {
                if (_link.hasPotion)
                {
                    Health = Math.Max(0, 0);
                    _link.hasPotion = false;
                }
                else
                {
                    Health = Math.Max(0, Health - 1);
                }
                remainingImmunityFrames = immunityDuration;
                isImmune = true;
            }
            currentColor = Color.Red;
            damageColorTimer = DAMAGE_COLOR_DURATION;

            if (Health <= 0 && alive)
            {
                alive = false;
                isDying = true;
                deathAnimationTimer = DEATH_ANIMATION_DURATION;
                deathSound.Play();
            }

            //if (Health <= 0)
            //{
            //    alive = false;
            //    position.X = 10000;
            //    position.Y = 10000;
            //}
        }

        

        public void Reset()
        {
            position = initialPosition;
            movingRight = true;
            currentFrame = 0;
            timeElapsed = 0f;
            damageColorTimer = 0f;
            currentColor = Color.White;
            alive = true;
        }
    }
}
