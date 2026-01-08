using Microsoft.Xna.Framework;

namespace Sprint2.Collisions
{
    public class HandleDragonBlockCollision
    {

        private Vector2 dragonPosition;
        private Vector2 blockPosition;
        private int dragonWidth;
        private int dragonHeight;
        private int blockWidth;
        private int blockHeight;

        public HandleDragonBlockCollision(Vector2 dragonPos, Vector2 blockPos, int dWidth, int dHeight, int bWidth, int bHeight)
        {
            dragonPosition = dragonPos;
            blockPosition = blockPos;
            dragonWidth = dWidth;
            dragonHeight = dHeight;
            blockWidth = bWidth;
            blockHeight = bHeight;
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

        public void DragonBlockCollision(ref Vector2 spritePosition, Vector2 scale)
        {
            //Rectangle playerBoundingBox = new Rectangle((int)spritePosition.X, (int)spritePosition.Y, playerWidth, playerHeight);
            Rectangle dragonBoundingBox = GetScaledRectangle((int)dragonPosition.X, (int)dragonPosition.Y, dragonWidth, dragonHeight, scale);
            Rectangle blockBoundingBox = GetScaledRectangle((int)blockPosition.X, (int)blockPosition.Y, blockWidth, blockHeight, scale);

            if (blockBoundingBox.Intersects(dragonBoundingBox))
            {
                Rectangle intersection = Rectangle.Intersect(dragonBoundingBox, blockBoundingBox);

                // First, resolve vertical collisions
                if (intersection.Height < intersection.Width)
                {
                    if (spritePosition.Y < blockBoundingBox.Y) // Coming from the top
                    {
                        spritePosition.Y = blockBoundingBox.Top - (dragonHeight * scale.Y);
                        //spritePosition.Y = blockBoundingBox.Top - (100);
                        //spritePosition.Y -= intersection.Height;
                    }
                    else if (spritePosition.Y > blockBoundingBox.Y) // Coming from below
                    {
                        spritePosition.Y = blockBoundingBox.Bottom;
                    }
                }
                // Then, resolve horizontal collisions
                else
                {
                    if (spritePosition.X < blockBoundingBox.X) // Coming from the left
                    {
                        spritePosition.X -= intersection.Width;
                        //spritePosition.X = blockBoundingBox.Left - (playerWidth * scale.X);
                    }
                    else if (spritePosition.X > blockBoundingBox.X) // Coming from the right
                    {
                        spritePosition.X = blockBoundingBox.Right;
                    }
                }
            }
        }
    }
}
