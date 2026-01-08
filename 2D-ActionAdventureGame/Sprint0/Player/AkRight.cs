using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint0.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Sprint0.Player
{
    internal class AkRight : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int weaponFrame;
        private int remainingFrames;
        public Vector2 _weaponPosition;
        private bool _arrowFlying;
        private float _arrowSpeed;
        private int arrowTimes = 0;
        private int _currentFrame;
        private int _totalFrames;

        private BulletManager _bulletManager;

       private bool isShooting;
        private float animationTimer;
        private const float FIRE_RATE = 0.15f; // Time in seconds between shots
        private float _timeSinceLastShot = 0f;
        private Random _random = new Random();
        private float overheatTimer;
        private Boolean overheating = false;
        private float overheatMult;
        Vector2 bulletStartPosition;




        public AkRight(Link link)//, BulletManager bulletManager)
        {
            _link = link;
            _weaponPosition.X = _link._position.X + 13 * _link._scale.X;
            _weaponPosition.Y = _link._position.Y + 6 * _link._scale.Y;

            linkFrame = 27;
            _totalFrames = 3;  //3 ak animation frames

          

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
                // Cycle through animation frames
                if (linkFrame == 27) linkFrame = 28;
                else if (linkFrame == 28) linkFrame = 29;
                else linkFrame = 27;

                animationTimer = 0f; // Reset the timer
            }
            if (overheatTimer >= 1f) //overheats after 1 seconds
            {
                overheating = true;
            }


            if (keyboardState.IsKeyDown(Keys.D3) || keyboardState.IsKeyDown(Keys.Enter)) 
            {
                _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (isShooting && _timeSinceLastShot >= FIRE_RATE)
                {
                    Fire(new Vector2(1, 0)); // Fire bullets to the right
                    _link.ak47shootSound.Play();
                    _timeSinceLastShot = 0f; // Reset the timer
                }
            }
            else
            {

                _timeSinceLastShot = FIRE_RATE;
                _link.currentState = new LinkRight(_link);

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
            _link._position.X + 30 * _link._scale.X,
            _link._position.Y + 6 * _link._scale.Y
        );
            bulletStartPosition = RandomizeBullet(bulletStartPosition);
           
            _link.BulletManager.SpawnBullet(bulletStartPosition, direction);
            
           // _link._position.X -=3;   //unfixed playerBlock collisions
        }
        private Vector2 RandomizeBullet(Vector2 basePosition)
        {
            float offset;
            if (overheating)
            {
                //overheatMult = (-30f-40f);
                offset = (float)(-15 + (_random.NextDouble() * 30)); //range [-15,15 

            }
            else {

                 offset = (float)(-7 + (_random.NextDouble() * 14)); //range [-7, 7
            }

            basePosition.Y += offset;
            return basePosition;
        }
        public void MoveDown()
        {
        }
        public void MoveUp()
        {
        }
        public void MoveRight()
        {
        }
        public void MoveLeft()
        {
        }
        public void UseSword()
        {
        }
        public void UseArrow()
        {
        }
        public void UseBoomerang()
        {
        }
        public void UseBomb()
        {
        }
       
        public void IsDamaged()
        {
            _link.Damaged = true;
        }

        public void UseAk()
        {
            isShooting = true; // Enable shooting mode

        }
    }
}