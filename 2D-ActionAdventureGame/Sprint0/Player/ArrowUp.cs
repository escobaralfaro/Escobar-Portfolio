using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    internal class ArrowUp : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int weaponFrame;
        private int remainingFrames;
        public Vector2 _weaponPosition;
        private bool _arrowFlying;
        private float _arrowSpeed;
        private int arrowTimes = 0;

        public ArrowUp(Link link)
        {
            _link = link;
            linkFrame = 11;
            weaponFrame = 18;
            remainingFrames = _link.framesPerSword;
            _weaponPosition.X = _link._position.X + 3 * _link._scale.X;
            _weaponPosition.Y = _link._position.Y - 13 * _link._scale.Y;
            _arrowFlying = false;
            _arrowSpeed = 10f;
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, linkFrame, false);

            _link.DrawWeapon(_spriteBatch, weaponFrame, false, false, _weaponPosition);


        }
        public void Update(GameTime gameTime)
        {

            if (weaponFrame == 18)
            {
                _arrowFlying = true;
            }
            if (_arrowFlying)
            {
                if (arrowTimes == 0)
                {
                    //SoundEffect
                    _link.bowAttackSound.Play();
                    arrowTimes = 1;
                }
                _weaponPosition.Y -= _arrowSpeed;
                if (_weaponPosition.Y < 200)
                {
                    _arrowFlying = false;
                    linkFrame = 11;
                    _link.currentState = new LinkUp(_link);
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