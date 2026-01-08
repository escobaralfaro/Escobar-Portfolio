using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Player;
using Sprint2.Classes;
using System;
using static Sprint0.Player.ILinkState;
using static Sprint2.Classes.Iitem;

namespace Sprint0.Classes
{
    internal class Fairy : Iitem
    {
        public ILinkState currentState;
        public Link _link;
        public Link _link2;

        private bool TwoPlayer;
        public Texture2D Sprite { get; private set; }
        public Rectangle[] SourceRectangles { get; private set; }
        public ItemType CurrentItemType => ItemType.fairy;
        public Vector2 Position;
        public Vector2 OriginalPosition { get; set; }
        private int itemFrame;
        public Vector2 _scale;
        private float timePerFrame = 0.5f; // 100ms per frame
        private float timeElapsed;
        private int currentFrame;
        public bool follow = false;
        public bool F1 = false;
        public bool F2 = false;

        public ItemType currentItemType { get; set; }

        public Fairy(Vector2 position, Link link, Link link2)
        {

            Position = position;
            OriginalPosition = position;
            _link = link;
            TwoPlayer = false;

            if (link2 != null)
            {
                _link2 = link2;
                TwoPlayer = true;
            }
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

        public void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, ItemType itemType, Vector2 scale)
        {
            Sprite = content.Load<Texture2D>(texturePath);

            SourceRectangles = SpriteSheetHelper.CreateFairyItemFrames();
            currentItemType = ItemType.fairy;

            _scale = scale;
        }

        public void Update(GameTime gameTime)
        {

            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timePerFrame)
            {
                currentFrame = (currentFrame + 1) % SourceRectangles.Length;
                timeElapsed = 0f;
            }
            if (!TwoPlayer)
            {
                Rectangle playerBoundingBox = GetScaledRectangle((int)_link._position.X, (int)_link._position.Y, 16, 16, _link._scale);
                Rectangle itemBoundingBox = GetScaledRectangle((int)Position.X, (int)Position.Y, 16, 16, _link._scale);
                if (playerBoundingBox.Intersects(itemBoundingBox))
                {
                    follow = true;
                }
                if (follow)
                {
                    float distanceX = (float)_link._position.X - (float)Position.X;
                    float distanceY = (float)_link._position.Y - (float)Position.Y;

                    if (Math.Abs(distanceX) > 20 * _scale.X || Math.Abs(distanceY) > 20 * _scale.Y)
                    {
                        Vector2 direction = new Vector2(distanceX, distanceY);
                        direction.Normalize();
                        float speed = 200f;
                        Vector2 movement = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        Position += movement;
                    }
                }
            } else
            {
                Rectangle playerBoundingBox = GetScaledRectangle((int)_link._position.X, (int)_link._position.Y, 16, 16, _link._scale);
                Rectangle playerBoundingBox2 = GetScaledRectangle((int)_link2._position.X, (int)_link2._position.Y, 16, 16, _link2._scale);
                Rectangle itemBoundingBox = GetScaledRectangle((int)Position.X, (int)Position.Y, 16, 16, _link._scale);
                if (playerBoundingBox.Intersects(itemBoundingBox))
                {
                    F1 = true;
                  //  follow = true;
                }
                
               else if (playerBoundingBox2.Intersects(itemBoundingBox))
                {
                    F2 = true;
                  //  follow = true;
                }
                
                    if (F1)
                    {
                        float distanceX = (float)_link._position.X - (float)Position.X;
                        float distanceY = (float)_link._position.Y - (float)Position.Y;

                        if (Math.Abs(distanceX) > 20 * _scale.X || Math.Abs(distanceY) > 20 * _scale.Y)
                        {
                            Vector2 direction = new Vector2(distanceX, distanceY);
                            direction.Normalize();
                            float speed = 200f;
                            Vector2 movement = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            Position += movement;
                        }
                    }
                    else if (F2)
                    {
                        float distanceX = (float)_link2._position.X - (float)Position.X;
                        float distanceY = (float)_link2._position.Y - (float)Position.Y;

                        if (Math.Abs(distanceX) > 20 * _scale.X || Math.Abs(distanceY) > 20 * _scale.Y)
                        {
                            Vector2 direction = new Vector2(distanceX, distanceY);
                            direction.Normalize();
                            float speed = 200f;
                            Vector2 movement = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            Position += movement;
                        }
                    }
                
               
            }
            
                

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Sprite, Position, SourceRectangles[currentFrame], Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

        }
    }
}
