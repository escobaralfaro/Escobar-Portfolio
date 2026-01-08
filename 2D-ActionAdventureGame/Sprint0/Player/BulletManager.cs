using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Diagnostics;
namespace Sprint0.Classes
{
    public class BulletManager
    {
        private List<Bullet> activeBullets;
        private Texture2D bulletTexture;
        private Rectangle[] bulletSourceRectangles;
        private Vector2 bulletScale;
        private float bulletSpeed;
        private float bulletLifetime;
        private Rectangle bulletDirection;
        public BulletManager(Texture2D texture, Rectangle[] sourceRectangles, Vector2 scale, float speed, float lifetime)
        {
            activeBullets = new List<Bullet>();
            bulletTexture = texture;
            bulletSourceRectangles = sourceRectangles;
            bulletScale = scale*4;
            bulletSpeed = speed;
            bulletLifetime = lifetime;

        }

        public void SpawnBullet(Vector2 startPosition, Vector2 direction)
        {
            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                bulletDirection = bulletSourceRectangles[31]; //horizontal if shooting left or right
            }
            else
            {
                bulletDirection = bulletSourceRectangles[30]; //verticle if shooting up or down
            }


            Bullet newBullet = new Bullet(startPosition, direction, bulletTexture, bulletDirection, bulletScale, bulletSpeed, bulletLifetime);
            activeBullets.Add(newBullet);

        }

        public void Update(GameTime gameTime)
        {
            foreach (var bullet in activeBullets)
            {
                bullet.Update(gameTime);
            }
            activeBullets.RemoveAll(b => b.IsExpired);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in activeBullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public List<Bullet> GetActiveBullets()
        {
            return activeBullets;
        }
    }
}
