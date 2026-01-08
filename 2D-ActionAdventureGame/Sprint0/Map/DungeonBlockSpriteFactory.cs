using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using System;

namespace Sprint2.Map
{
    public class DungeonBlockSpriteFactory : ISpriteFactory
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly ContentManager content;
        private Texture2D dungeonblock_Sheet;
        private Rectangle[] sourceRectangles;

        public DungeonBlockSpriteFactory(GraphicsDevice graphicsDevice, ContentManager content, string sheetName)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            LoadTexture(sheetName);
        }
        public void LoadTexture(string sheetName)
        {
            try
            {
                dungeonblock_Sheet = content.Load<Texture2D>(sheetName);
            }
            catch (ContentLoadException e)
            {
                Console.WriteLine($"Error loading content: {e.Message}");
                throw;
            }
        }

        public Rectangle[] CreateFrames()
        {
            sourceRectangles = new Rectangle[]
             {
                 //https://pixspy.com/
                 new Rectangle(196, 307, 16, 16), // 0
                 new Rectangle(212, 323, 16, 16), // 1
                 new Rectangle(212, 272, 16, 16), // 2
                 new Rectangle(212, 438, 16, 16), // 3
                 new Rectangle(893, 799, 16, 16), // 4
                 new Rectangle(1001, 28, 16, 16), // 5
                 new Rectangle(521, 11, 256, 32), // 5 top wall
                 new Rectangle(521, 43, 32, 112),  // 6 left wall
                 new Rectangle(521, 155, 256, 32), // 7 bottom wall
                 new Rectangle(745, 43, 32, 112), // 8 right wall
                 new Rectangle(815, 11, 32, 32), // 9 no door up
                 new Rectangle(848, 11, 32, 32), // 10 open door up
                 new Rectangle(881, 11, 32, 32), // 11 locked door up
                   new Rectangle(914, 11, 32, 32), // 12 vault door up
                   new Rectangle(947, 11, 32, 32), // 13 cave door up
                   new Rectangle(815, 44, 32, 32), // 14 no door left
                 new Rectangle(848, 44, 32, 32), // 15 open door left
                 new Rectangle(881, 44, 32, 32), // 16 locked door left
                   new Rectangle(914, 44, 32, 32), // 17 vault door left
                   new Rectangle(947, 44, 32, 32), // 18 cave door left
                   new Rectangle(815, 77, 32, 32), // 19 no door right
                 new Rectangle(848, 77, 32, 32), // 20 open door right
                 new Rectangle(881, 77, 32, 32), // 21 locked door right
                   new Rectangle(914, 77, 32, 32), // 22 vault door right
                   new Rectangle(947, 77, 32, 32), // 23 cave door right
                   new Rectangle(815, 110, 32, 32), // 24 no door down
                 new Rectangle(848, 110, 32, 32), // 25 open door down
                 new Rectangle(881, 110, 32, 32), // 26 locked door down
                   new Rectangle(914, 110, 32, 32), // 27 vault door down
                   new Rectangle(947, 110, 32, 32), // 28 cave door down
                 
             };

            return sourceRectangles;
        }

    }
}
