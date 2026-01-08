using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sprint0.Player
{
    internal class AkDown : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int remainingFrames;
        public Vector2 _weaponPosition;
        private int _currentFrame;
        private int _totalFrames;

        private bool isShooting;
        private float animationTimer;
        private const float FIRE_RATE = 0.15f;
        private float _timeSinceLastShot = 0f;
        private Random _random = new Random();
        private float overheatTimer;
        private Boolean overheating = false;

        public AkDown(Link link)
        {
            _link = link;
            

            linkFrame = 35;
            _totalFrames = 3;
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, linkFrame, false);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            overheatTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer >= FIRE_RATE)
            {
                if (linkFrame == 35) linkFrame = 36;
                else if (linkFrame == 36) linkFrame = 37;
                else linkFrame = 35;

                animationTimer = 0f;
            }

            if (overheatTimer >= 1f)
            {
                overheating = true;
            }

            if (keyboardState.IsKeyDown(Keys.D3) || keyboardState.IsKeyDown(Keys.Enter))
            {
                _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (isShooting && _timeSinceLastShot >= FIRE_RATE)
                {
                    Fire(new Vector2(0, 1)); // Fire bullets downwards
                    _link.ak47shootSound.Play();
                    _timeSinceLastShot = 0f;
                }
            }
            else
            {
                _timeSinceLastShot = FIRE_RATE;
                _link.currentState = new LinkDown(_link);
            }

            if (_link.Damaged)
            {
                if (--_link.RemainingDamagedFrames <= 0)
                {
                    _link.Damaged = false;
                    _link.RemainingDamagedFrames = _link.framesPerDamage;
                }
            }
        }

        public void Fire(Vector2 direction)
        {
            Vector2 bulletStartPosition = new Vector2(
                _link._position.X + 6 * _link._scale.X,
                _link._position.Y + 15 * _link._scale.Y
            );
            bulletStartPosition = RandomizeBullet(bulletStartPosition);

            _link.BulletManager.SpawnBullet(bulletStartPosition, direction);
            //_link._position.Y -= 3;
        }

        private Vector2 RandomizeBullet(Vector2 basePosition)
        {
            float offset;
            if (overheating)
            {
                offset = (float)(-15 + (_random.NextDouble() * 30));
            }
            else
            {
                offset = (float)(-7 + (_random.NextDouble() * 14));
            }

            basePosition.X += offset;
            return basePosition;
        }

        public void MoveDown() { }
        public void MoveUp() { }
        public void MoveRight() { }
        public void MoveLeft() { }
        public void UseSword() { }
        public void UseArrow() { }
        public void UseBoomerang() { }
        public void UseBomb() { }

        public void IsDamaged()
        {
            _link.Damaged = true;
        }

        public void UseAk()
        {
            isShooting = true;
        }
    }
}