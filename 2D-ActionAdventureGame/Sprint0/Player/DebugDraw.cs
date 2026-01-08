using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using Sprint0.Player;
using Sprint2.Enemy;
using Sprint2.Map;
using System.Collections.Generic;

namespace Sprint0.Collisions
{
    public static class DebugDraw
    {
        private static Texture2D _pixel;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, Vector2 scale, int lineWidth = 1)
        {
            // Scale only the size, not the position
            Rectangle scaledRectangle = new Rectangle(
                rectangle.Left, // Keep the original position
                rectangle.Top,  // Keep the original position
                (int)(rectangle.Width * scale.X), // Scale the width
                (int)(rectangle.Height * scale.Y) // Scale the height
            );

            // Draw the left line
            spriteBatch.Draw(_pixel, new Rectangle(scaledRectangle.Left, scaledRectangle.Top, lineWidth, scaledRectangle.Height + lineWidth), color);

            // Draw the right line
            spriteBatch.Draw(_pixel, new Rectangle(scaledRectangle.Right, scaledRectangle.Top, lineWidth, scaledRectangle.Height + lineWidth), color);

            // Draw the top line
            spriteBatch.Draw(_pixel, new Rectangle(scaledRectangle.Left, scaledRectangle.Top, scaledRectangle.Width + lineWidth, lineWidth), color);

            // Draw the bottom line
            spriteBatch.Draw(_pixel, new Rectangle(scaledRectangle.Left, scaledRectangle.Bottom, scaledRectangle.Width + lineWidth, lineWidth), color);
        }




        public static void DrawHitboxes(SpriteBatch spriteBatch, Link link, Enemy_Item_Map enemyMap, int currentRoom, Vector2 scale, BulletManager bulletManager)
        {
            // Draw Link's hitbox
            Rectangle linkHitbox = new Rectangle((int)link._position.X, (int)link._position.Y, LinkEnemyCollision.LinkHitboxWidth, LinkEnemyCollision.LinkHitboxHeight);
            DrawRectangle(spriteBatch, linkHitbox, Color.Blue, scale);

            //bullets that may be present whenever
            List<Bullet> activeBullets = bulletManager.GetActiveBullets();
            foreach (Bullet bullet in activeBullets)
            {
                Rectangle BulletHitbox = LinkEnemyCollision.GetBulletHitbox(scale, bullet);
                DrawRectangle(spriteBatch, BulletHitbox, Color.Crimson, new Vector2(1.0f, 1.0f)); //I already scale it in GetArrowHitbox
            }

            // Draw sword hitbox if Link is attacking
            if (link.currentState is SwordRight || link.currentState is SwordLeft || link.currentState is SwordUp || link.currentState is SwordDown)
            {
                Rectangle swordHitbox = LinkEnemyCollision.GetSwordHitbox(link, scale);
                DrawRectangle(spriteBatch, swordHitbox, Color.Red, new Vector2(1.0f, 1.0f)); //I already scale it in GetSwordHitbox
            } else
            //Draw arrow hitbox if Link is arrow attack
            if (link.currentState is ArrowRight || link.currentState is ArrowLeft || link.currentState is ArrowUp || link.currentState is ArrowDown)
            {
                Rectangle ArrowHitbox = LinkEnemyCollision.GetArrowHitbox(link, scale);
                DrawRectangle(spriteBatch, ArrowHitbox, Color.Azure, new Vector2(1.0f, 1.0f)); //I already scale it in GetArrowHitbox
            }
            else
            //Draw boomerang hitbox if Link is boomerang attack
            if (link.currentState is BoomerangRight || link.currentState is BoomerangLeft || link.currentState is BoomerangUp || link.currentState is BoomerangDown)
            {
                Rectangle BoomerangHitbox = LinkEnemyCollision.GetBoomerangHitbox(link, scale);
                DrawRectangle(spriteBatch, BoomerangHitbox, Color.Azure, new Vector2(1.0f, 1.0f)); //I already scale it in GetArrowHitbox
            } else
            //Draw bomb hitbox if Link is bomb attack
            if (link.currentState is BombRight || link.currentState is BombLeft || link.currentState is BombUp || link.currentState is BombDown)
            {
                Rectangle BombHitbox = LinkEnemyCollision.GetBombHitbox(link, scale);
                DrawRectangle(spriteBatch, BombHitbox, Color.Azure, new Vector2(1.0f, 1.0f)); //I already scale it in GetArrowHitbox
            }

            // Draw enemy hitboxes
            List<IEnemy> enemiesInRoom = enemyMap.GetEnemies(currentRoom);
            foreach (IEnemy enemy in enemiesInRoom)
            {
                Rectangle enemyHitbox = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Width, enemy.Height);
                DrawRectangle(spriteBatch, enemyHitbox, Color.Green, scale);

                if (enemy is Dragon dragon)
                {
                    foreach (var fireball in dragon.fireballs)
                    {
                        Rectangle fireballHitbox = LinkEnemyCollision.GetFireballHitbox(fireball, scale);
                        DrawRectangle(spriteBatch, fireballHitbox, Color.Firebrick, new Vector2(1.0f, 1.0f));

                    }

                }
                if (enemy is Goriya goriya)
                {
                    foreach (var boomerang in goriya.projectiles)
                    {
                        Rectangle boomerangHitbox = LinkEnemyCollision.GetBoomerangHitbox(boomerang, scale);
                        DrawRectangle(spriteBatch, boomerangHitbox, Color.Firebrick, new Vector2(1.0f, 1.0f));

                    }

                }

            }
            
        }

    }
}
