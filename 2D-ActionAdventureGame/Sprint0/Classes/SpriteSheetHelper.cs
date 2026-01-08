using Microsoft.Xna.Framework;

namespace Sprint0.Classes
{
    public static class SpriteSheetHelper
    {
        public static Rectangle[] CreateDragonFrames()
        {
            // Define frames based on your sprite sheet dimensions
            return new Rectangle[]
            {
            new Rectangle(1, 11, 24, 32),   // Frame 1  
            new Rectangle(26, 11, 24, 32),   // Frame 2
            new Rectangle(51, 11, 24, 32),   // Frame 3
            new Rectangle(76, 11, 24, 32),   // Frame 4
           
            };
        }
        public static Rectangle[] CreateFireballFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(101, 14, 8, 10), // Projectile frame 1
            new Rectangle(110, 14, 8, 10), // Projectile frame 2
            new Rectangle(119, 14, 8, 10), // Projectile frame 3
            new Rectangle(128, 14, 8, 10), // Projectile frame 4
            };
        }
        public static Rectangle[] CreateGoriyaFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(222, 11, 16, 16),   // Goriya Frame 1
            new Rectangle(239, 11, 16, 16),// Goriya Frame 2
            new Rectangle(256, 11, 16, 16),  // Goriya Frame 3
            new Rectangle(273, 11, 16, 16),  // Goriya Frame 4
            };
        }
        // Boomerang frames for Goriya
        public static Rectangle[] CreateBoomerangFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(291, 15, 5, 8),   // Boomerang Frame 1
            new Rectangle(299, 15, 8, 8),// Boomerang Frame 2
            new Rectangle(308, 17, 8, 5),  // Boomerang Frame 3
            };
        }

        public static Rectangle[] CreateStalfosFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(1, 59, 16, 16),    
          
            };
        }

        public static Rectangle[] CreateKeeseFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(183, 11, 16, 16),    
            new Rectangle(200, 11, 16, 16), 
            
            };
        }

        public static Rectangle[] CreateGelFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(1, 11, 8, 16),
            new Rectangle(10, 11, 8, 16),

            };
        }

        public static Rectangle[] CreateWizzrobeFrames()
        {
            return new Rectangle[]
            {
            new Rectangle(127, 90, 14, 16),
            new Rectangle(144, 90, 14, 16),

            };
        }
        public static Rectangle[] CreateBowItemFrames()
        {
            return new Rectangle[] { new Rectangle(144, 0, 8, 16), new Rectangle(144, 0, 8, 16) };
        }
        public static Rectangle[] CreateBoomItemFrames()
        {
            return new Rectangle[] { new Rectangle(136, 0, 8, 14), new Rectangle(136, 0, 8, 14) };
        }
        public static Rectangle[] CreateCompassItemFrames()
        {
            return new Rectangle[] { new Rectangle(258, 1, 11, 12), new Rectangle(258, 1, 11, 12) };
        }
        public static Rectangle[] CreateTriangleItemFrames()
        {
            return new Rectangle[] { new Rectangle(275, 3, 10, 10), new Rectangle(275, 19, 10, 10) };
        }
        public static Rectangle[] CreateKeyItemFrames()
        {
            return new Rectangle[] { new Rectangle(240, 0, 8, 16), new Rectangle(240, 0, 8, 16) };
        }
        public static Rectangle[] CreateFairyItemFrames()
        {
            return new Rectangle[] { new Rectangle(40, 0, 7, 16), new Rectangle(49, 0, 7, 16) };
        }
        public static Rectangle[] CreateClockItemFrames()
        {
            return new Rectangle[] { new Rectangle(58, 0, 11, 16), new Rectangle(58, 0, 11, 16) };
        }
        public static Rectangle[] CreateDiamondItemFrames()
        {
            return new Rectangle[] { new Rectangle(72, 0, 8, 16), new Rectangle(72, 16, 8, 16) };
        }
        public static Rectangle[] CreatePotionItemFrames()
        {
            return new Rectangle[] { new Rectangle(81, 0, 6, 14), new Rectangle(81, 16, 6, 14) };
        }
        public static Rectangle[] CreateMapItemFrames()
        {
            return new Rectangle[] { new Rectangle(88, 0, 8, 16), new Rectangle(88, 16, 8, 16) };
        }
        public static Rectangle[] CreateHealthItemFrames()
        {
            return new Rectangle[] { new Rectangle(0, 0, 7, 8), new Rectangle(0, 8, 7, 8) }; 
        }
        public static Rectangle[] CreateHeartItemFrames()
        {
            return new Rectangle[] { new Rectangle(25, 0, 13, 14), new Rectangle(25, 0, 13, 14) }; ;
        }
        public static Rectangle[] CreateFireItemFrames()
        {
            return new Rectangle[] { new Rectangle(191, 185, 16, 16), new Rectangle(191, 185, 16, 16) }; ;
        }
        public static Rectangle[] CreateAk47ItemFrames()
        {
            return new Rectangle[] { new Rectangle(16, 41, 8, 23), new Rectangle(16, 41, 8, 23) }; ;
        }


        public static Rectangle[] CreateBlockFrames()
        {

            return new Rectangle[]
             {
                 //https://pixspy.com/
                 new Rectangle(196, 307, 16, 16),
                 new Rectangle(212, 323, 16, 16),
                 new Rectangle(212, 272, 16, 16),
                 new Rectangle(212, 438, 16, 16),
                 new Rectangle(893, 799, 16, 16),
                 new Rectangle(421, 1009, 16, 16),
                 new Rectangle(469, 1009, 16, 16),
                 new Rectangle(533, 1040, 16, 16),
             }; 
        }
    }
}
