using Microsoft.Xna.Framework;

namespace Sprint2.Collisions
{
    public class HandlePlayerBlockCollision
    {

        private Vector2 playerPosition;
        private Vector2 blockPosition;
        private int playerWidth;
        private int playerHeight;
        private int blockWidth;
        private int blockHeight;

        public HandlePlayerBlockCollision(Vector2 playerPos, Vector2 blockPos, int pWidth, int pHeight, int bWidth, int bHeight)
        {
            playerPosition = playerPos;
            blockPosition = blockPos;
            playerWidth = pWidth;
            playerHeight = pHeight;
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

        public void PlayerBlockCollision(ref Vector2 spritePosition, Vector2 previousPosition, Vector2 scale)
        {
            //Rectangle playerBoundingBox = new Rectangle((int)spritePosition.X, (int)spritePosition.Y, playerWidth, playerHeight);
            Rectangle playerBoundingBox = GetScaledRectangle((int)playerPosition.X, (int)playerPosition.Y, playerWidth, playerHeight, scale);
            Rectangle blockBoundingBox = GetScaledRectangle((int)blockPosition.X, (int)blockPosition.Y, blockWidth, blockHeight, scale);

            if (blockBoundingBox.Intersects(playerBoundingBox))
            {
                Rectangle intersection = Rectangle.Intersect(playerBoundingBox, blockBoundingBox);

                // First, resolve vertical collisions
                if (intersection.Height < intersection.Width)
                {
                    if (spritePosition.Y < blockBoundingBox.Y) // Coming from the top
                    {
                        spritePosition.Y = blockBoundingBox.Top - (playerHeight * scale.Y);
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
