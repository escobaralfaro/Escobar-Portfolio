using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    internal class LinkLeft : ILinkState
    {
        private Link _link;
        private int frame;
        private int remainingFrames;
        public bool CollideWall = false;
        private Rectangle wallBoundingBox;
        private Rectangle playerBoundingBox;

        public LinkLeft(Link link)
        {
            _link = link;
            frame = 2;
            remainingFrames = _link.framesPerStep;
            
        }

        private static Rectangle GetScaledRectangle(int x, int y, int width, int height, Vector2 scale)
        {
            return new Rectangle(
                x,
                y,
                (int)(width * scale.X),
                (int)(height * scale.Y)
            );
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, frame, true);
        }
        public void Update(GameTime gameTime)
        {
            CollideWall = false;
            wallBoundingBox = new Rectangle(0, (int)(32 * _link._scale.Y), (int)(32 * _link._scale.X), (int)(300 * _link._scale.Y));
            playerBoundingBox = GetScaledRectangle((int)_link._position.X, (int)_link._position.Y, 16, 16, _link._scale);
            if (playerBoundingBox.Intersects(wallBoundingBox))
            {
                CollideWall = true;
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

            _link.currentState = new LinkDown(_link);
        }
        public void MoveUp()
        {
            _link.currentState = new LinkUp(_link);
        }
        public void MoveRight()
        {
            _link.currentState = new LinkRight(_link);
        }
        public void MoveLeft()
        {
            if (!CollideWall)
            {
                _link._position.X -= _link.speed;
            }
            else
            {
              //  _link._position.X = wallBoundingBox.Right;
            }
            if (--remainingFrames <= 0)
            {
                if (frame == 2)
                {
                    frame = 3;
                }
                else if (frame == 3)
                {
                    frame = 2;
                }
                remainingFrames = _link.framesPerStep;
            }


        }

        public void UseSword()
        {
            _link.currentState = new SwordLeft(_link);
        }
        public void UseArrow()
        {
           _link.currentState = new ArrowLeft(_link);
        }
        public void UseBoomerang()
        {
          _link.currentState = new BoomerangLeft(_link);

        }
        public void UseBomb()
        {
          _link.currentState = new BombLeft(_link);
        }
        public void UseAk()
        {
            _link.currentState = new AkLeft(_link);

        }

        public void IsDamaged()
        {
            _link.Damaged = true;
        }
    }
}