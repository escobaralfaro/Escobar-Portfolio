using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    internal class LinkDown : ILinkState
    {
        private Link _link;
        private Vector2 _scale;
        private int frame;
        private int remainingFrames;
        public bool CollideWall = false;
        private Rectangle wallBoundingBox;
        private Rectangle playerBoundingBox;

        public LinkDown(Link link)
        {
            _link = link;
            frame = 0;
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

            wallBoundingBox = new Rectangle(0, (int)(201 * _link._scale.Y), (int)(256 * _link._scale.X), (int)(32 * _link._scale.Y));
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

            if (!CollideWall)
            {
                _link._position.Y += _link.speed;
            }
            else
            {
            //    _link._position.Y = wallBoundingBox.Top - (16 * _link._scale.Y);
            }
            if (--remainingFrames <= 0)
            {
                if (frame == 0)
                {
                    frame = 1;
                }
                else if (frame == 1)
                {
                    frame = 0;
                }
                remainingFrames = _link.framesPerStep;
            }
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
            _link.currentState = new LinkLeft(_link);
        }
        public void UseSword()
        {
            _link.currentState = new SwordDown(_link);
        }
        public void UseArrow()
        {
            _link.currentState = new ArrowDown(_link);
        }
        public void UseBoomerang()
        {
            _link.currentState = new BoomerangDown(_link);

        }
        public void UseBomb()
        {
            _link.currentState = new BombDown(_link);
        }
        public void UseAk()
        {
            _link.currentState = new AkDown(_link);
        }

        public void IsDamaged()
        {
            _link.Damaged = true;
        }
   
    }
}
