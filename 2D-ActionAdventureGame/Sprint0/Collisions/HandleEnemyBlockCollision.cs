using Microsoft.Xna.Framework;
using Sprint2.Enemy;
using Sprint2.Map;
using System.Collections.Generic;

namespace Sprint2.Collisions
{
    public class HandleEnemyBlockCollision
    {

        private Vector2 enemyPosition;
        private Vector2 blockPosition;
        private int enemyWidth;
        private int enemyHeight;
        private int blockWidth;
        private int blockHeight;
        public bool collisionDetected;


        public HandleEnemyBlockCollision(Vector2 blockPos, int eWidth, int eHeight, int bWidth, int bHeight)
        {
            blockPosition = blockPos;
            enemyWidth = eWidth;
            enemyHeight = eHeight;
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

        public bool EnemyBlockCollision(Enemy_Item_Map enemyItemMap, int currentRoomNumber, Vector2 scale) // or List<IEnemy> enemies, Vector2 spritePosition, Vector2 scale
            //public static void HandleCollisions(Link link, Enemy_Item_Map enemyItemMap, int currentRoomNumber, Vector2 scale)
        {
             collisionDetected =  false;
            Rectangle blockBoundingBox = GetScaledRectangle((int)blockPosition.X, (int)blockPosition.Y, blockWidth, blockHeight, scale);
             List<IEnemy> enemiesInRoom = enemyItemMap.GetEnemies(currentRoomNumber);

            // Iterate enemies
             foreach (IEnemy enemy in enemiesInRoom)
              {
                  Rectangle enemyBoundingBox = GetScaledRectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Width, enemy.Height, scale);
                     Vector2 newEnemyPosition = enemy.Position;

                if (blockBoundingBox.Intersects(enemyBoundingBox))
                {
                    collisionDetected = true;
                    Rectangle intersection = Rectangle.Intersect(enemyBoundingBox, blockBoundingBox);
                    if (enemy is Stalfos stalfos)
                    {
                        stalfos.SetNewRandomDirection();
                    }
                    if (enemy is Dragon dragon)
                    {
                        dragon.flipDirection();
                    }
                    // keep enemies oustside of the block
                    float offsetDistance = 2.0f;   

            //   vertical collision 
            if (intersection.Height < intersection.Width)
            {
                if (enemy.Position.Y < blockBoundingBox.Y)  
                {
                    newEnemyPosition.Y = blockBoundingBox.Top - (enemy.Height * scale.Y) - offsetDistance;
                }
                else if (enemy.Position.Y > blockBoundingBox.Y)  
                {
                    newEnemyPosition.Y = blockBoundingBox.Bottom + offsetDistance;
                }
            }
            else //  horizontal collision
            {
                if (enemy.Position.X < blockBoundingBox.X)  
                {
                    newEnemyPosition.X = blockBoundingBox.Left - (enemy.Width * scale.X) - offsetDistance;
                }
                else if (enemy.Position.X > blockBoundingBox.X)  
                {
                    newEnemyPosition.X = blockBoundingBox.Right + offsetDistance;
                }
            }
        }

        
        enemy.Position = newEnemyPosition;

            }
            return collisionDetected;
        }

    }
}