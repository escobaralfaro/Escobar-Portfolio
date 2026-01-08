using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    internal class LinkRight : ILinkState
    {
        private Link _link;
        private int frame;
        private int remainingFrames;
        public bool CollideWall = false;
        private Rectangle wallBoundingBox;
        private Rectangle playerBoundingBox;

        public LinkRight(Link link)
        {
            _link = link;
            frame = 2;
            remainingFrames = _link.framesPerStep;
        }

        void ILinkState.Draw(SpriteBatch _spriteBatch)
        {
            _link.DrawSprite(_spriteBatch, frame, false);
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
        public void Update(GameTime gameTime)
        {
            CollideWall = false;
            wallBoundingBox = new Rectangle((int)(224 * _link._scale.X), (int)(32 * _link._scale.Y), (int)(32 * _link._scale.X), (int)(300 * _link._scale.Y));
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

            if (!CollideWall)
            {
                _link._position.X += _link.speed;
            }
            else
            {
             //   _link._position.X = wallBoundingBox.Left - (16 * _link._scale.X);
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
        public void MoveLeft()
        {
          _link.currentState = new LinkLeft(_link);
            
        }

        public void UseSword()
        {
            _link.currentState = new SwordRight(_link);
        }
        public void UseArrow()
        {
            _link.currentState = new ArrowRight(_link);

        }
        public void UseAk()
        {
            _link.currentState = new AkRight(_link);
        }
        public void UseBoomerang()
        {
            _link.currentState = new BoomerangRight(_link);
        }
        public void UseBomb()
        {
            _link.currentState = new BombRight(_link);
        }

        public void IsDamaged()
        {
            _link.Damaged = true;
        }
    }
}