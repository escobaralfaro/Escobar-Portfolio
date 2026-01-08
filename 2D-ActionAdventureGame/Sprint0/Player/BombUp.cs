using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    internal class BombUp : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int weaponFrame;
        private int remainingFrames;
        public Vector2 _weaponPosition;
        private bool _Explode;
        private float _BombSpeed;
        public int _boomTimer;

        public BombUp(Link link)
        {
            _link = link;
            linkFrame = 11;
            weaponFrame = 23;
            remainingFrames = _link.framesPerSword;
            _weaponPosition.X = _link._position.X + 3 * _link._scale.X;
            _weaponPosition.Y = _link._position.Y - 13 * _link._scale.Y;
            _Explode = false;
            _BombSpeed = 0; //bomb doesnt move
            _boomTimer = 30;
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, linkFrame, false);

            _link.DrawWeapon(_spriteBatch, weaponFrame, false, false, _weaponPosition);


        }
        public void Update(GameTime gameTime)
        {
            _boomTimer--;  //timer ticking down

            if (!_Explode)
            {
                _weaponPosition.Y -= _BombSpeed;
                if (_boomTimer == 0)
                {

                    _link.bombExplosion.Play();
                    _Explode = true;
                    weaponFrame = 23;  // Start explosion animation
                    remainingFrames = _link.framesPerSword;
                }
            }
            else
            {
                if (--remainingFrames <= 0)
                {

                    if (weaponFrame == 23)
                    {
                        weaponFrame = 24;
                    }
                    else if (weaponFrame == 24)
                    {
                        weaponFrame = 25;
                    }
                    else if (weaponFrame == 25)
                    {
                        weaponFrame = 26;
                    }
                    else if (weaponFrame == 26)
                    {
                        linkFrame = 2;
                        _link.BombCount--;
                        _link.SetExplosionCoords(_weaponPosition);
                        _link.DecrementBomb();
                        _link.currentState = new LinkUp(_link);
                    }
                    remainingFrames = _link.framesPerSword;
                }
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
        public void UseAk()
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

    }
}