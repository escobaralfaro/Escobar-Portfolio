using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    internal class SwordLeft : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int weaponFrame;
        private int remainingFrames;
        private Vector2 _weaponPosition;

        public SwordLeft(Link link)
        {
            _link = link;
            linkFrame = 9;
            weaponFrame = 14;
            remainingFrames = _link.framesPerSword;
            _weaponPosition.X = _link._position.X - 13 * _link._scale.X;
            _weaponPosition.Y = _link._position.Y + 6 * _link._scale.Y;
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, linkFrame, true);
            switch (weaponFrame) 
            {
                case 14:
                    break;
                case 15:
                    _link.DrawWeapon(_spriteBatch, weaponFrame, true, false, _weaponPosition);
                    break;
                case 16:
                    _weaponPosition.X = _link._position.X - 10 * _link._scale.X;
                    _link.DrawWeapon(_spriteBatch, weaponFrame, true, false, _weaponPosition);
                    break;  
                case 17:
                    _weaponPosition.X = _link._position.X - 5 * _link._scale.X;
                    _link.DrawWeapon(_spriteBatch, weaponFrame, true, false, _weaponPosition);
                    break;
            }

            if (weaponFrame > 14)
            {
                _link.DrawWeapon(_spriteBatch, weaponFrame, true, false, _weaponPosition);
            }

        }
        public void Update(GameTime gameTime)
        {
            if (--remainingFrames <= 0)
            {
                if (weaponFrame == 14)
                {
                    weaponFrame = 15;
                }
                else if (weaponFrame == 15)
                {
                    //Sound effect
                    _link.SwordAttackSound.Play();
                    weaponFrame = 16;
                }
                else if (weaponFrame == 16)
                {
                    weaponFrame = 17;
                }
                else if (weaponFrame == 17)
                {
                    linkFrame = 2;
                    _link.currentState = new LinkLeft(_link);
                }
                remainingFrames = _link.framesPerSword;
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