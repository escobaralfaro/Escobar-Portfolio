using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint0.Classes;
using System;

namespace Sprint0.Player
{
    internal class AkLeft : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int akFrame;
        private int remainingFrames;
        public Vector2 _weaponPosition;
        public Vector2 _origweaponPosition;

        private int _currentFrame;
        private int _totalFrames;

        private bool isShooting;
        private float animationTimer;
        private const float FIRE_RATE = 0.15f;
        private float _timeSinceLastShot = 0f;
        private Random _random = new Random();
        private float overheatTimer;
        private Boolean overheating = false;

        public AkLeft(Link link)
        {
            _link = link;
            _weaponPosition.X = _link._position.X - 18 * _link._scale.X-8;
            _weaponPosition.Y = _link._position.Y + 0 * _link._scale.Y +0;
            _origweaponPosition = _weaponPosition;

            linkFrame = 44;
            _totalFrames = 3;
            akFrame = 41;

        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, linkFrame, true); // Flipped horizontally
            _link.DrawWeapon(_spriteBatch, akFrame, true, true, _weaponPosition);

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            overheatTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer >= FIRE_RATE)
            {

                if (linkFrame == 44)
                {
                    linkFrame = 45;
                    akFrame = 42;
                    _weaponPosition.X = _weaponPosition.X + 1 * _link._scale.X;





                }
                else if (linkFrame == 45)
                {
                    linkFrame = 46;
                    akFrame = 43;
                    _weaponPosition.X = _weaponPosition.X + 1 * _link._scale.X ;
                    _weaponPosition.Y = _weaponPosition.Y + 0 * _link._scale.Y - (float)0.05;





                }
                else
                {
                    linkFrame = 44;
                    akFrame = 41;
                    _weaponPosition = _origweaponPosition; //reset


                }

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
                    Fire(new Vector2(-1, 0)); // Fire bullets to the left
                    _link.ak47shootSound.Play();
                    _timeSinceLastShot = 0f;
                }
            }
            else
            {
                _timeSinceLastShot = FIRE_RATE;
                _link.currentState = new LinkLeft(_link);
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
                _link._position.X - 15 * _link._scale.X,
                _link._position.Y + 6 * _link._scale.Y
            );
            bulletStartPosition = RandomizeBullet(bulletStartPosition);

            _link.BulletManager.SpawnBullet(bulletStartPosition, direction);
            //_link._position.X += 3;
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

            basePosition.Y += offset;
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