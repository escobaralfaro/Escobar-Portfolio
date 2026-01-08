using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Sprint0.Player
{
    internal class BoomerangRight : ILinkState
    {
        private Link _link;
        private int linkFrame;
        private int weaponFrame;
        private int remainingFrames;
        private int rotations;
        private int boomerangStage;
        public Vector2 _weaponPosition;
        private Boolean _return;

        public BoomerangRight(Link link)
        {
            _link = link;
            _return = false;
            linkFrame = 9;
            weaponFrame = 20;
            rotations = 0;
            boomerangStage = 0;
            remainingFrames = _link.framesPerBoomerang;
            _weaponPosition.X = _link._position.X + 20 * _link._scale.X;
            _weaponPosition.Y = _link._position.Y + 6 * _link._scale.Y;
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, linkFrame, false);
            switch (boomerangStage)
            {
                case 0:
                    _link.DrawWeapon(_spriteBatch, 20, false, false, _weaponPosition);
                    break;
                case 1:
                    _link.DrawWeapon(_spriteBatch, 21, false, false, _weaponPosition);
                    break;
                case 2:
                    _link.DrawWeapon(_spriteBatch, 22, false, false, _weaponPosition);
                    break;
                case 3:
                    _link.DrawWeapon(_spriteBatch, 22, true, false, _weaponPosition);
                    break;
                case 4:
                    _link.DrawWeapon(_spriteBatch, 21, true, true, _weaponPosition);
                    break;
                case 5:
                    _link.DrawWeapon(_spriteBatch, 20, true, true, _weaponPosition);
                    break;
                case 6:
                    _link.DrawWeapon(_spriteBatch, 22, false, true, _weaponPosition);
                    break;
                default:
                    break;
            }
            if (_return)
            {
                _weaponPosition.X -= _link.boomerangSpeed;
            } else if (!_return){
                _weaponPosition.X += _link.boomerangSpeed;
            }
        }
        public void Update(GameTime gameTime)
        {
            if (--remainingFrames <= 0)
            {
                if (!_return)
                {
                    _link.BoomerangSound.Play();
                    boomerangStage++;
                } 
                else if (_return)
                {
                    boomerangStage--;
                }

                if(_return && boomerangStage <= 0)
                {
                    linkFrame = 2;
                    _link.currentState = new LinkRight(_link);
                }
                
   
                if(boomerangStage >= 7)
                {
                    _return = true;
                }
                remainingFrames = _link.framesPerBoomerang;
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