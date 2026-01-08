using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Player;
using Sprint2.Classes;
using Sprint2.Map;
using Sprint2.Player;
using System;
using static Sprint2.Classes.Iitem;

namespace Sprint2
{
    public class GameHUD
    {
        private StageManager stageManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _hudTexture;
        private Rectangle _hudBackground;
        public Rectangle[] cutOuts { get; private set; }
        public Texture2D HUDTexture => _hudTexture;

        private Vector2 _scale;
        private Link _link;
        private Vector2 _position;

        private Rectangle _inventoryRegion;
        private Rectangle _bSlotRegion;
        private bool isInventoryVisible;

        private Rectangle _healthBarPosition;
        private const int HUD_WIDTH = 256;
        private const int HUD_HEIGHT = 48;

        private const int HEART_WIDTH = 9;
        private const int HEART_HEIGHT = 9;
        const int heartsPerRow = 8;  // Set max hearts per row
       

        private int numKeys;
        private int keyPos = 0;
        int Spacing = 8;
        private int Health;

        private MiniMap1 MiniMap;

        private readonly GraphicsDevice graphicsDevice;
        private readonly ContentManager content;
        public GameHUD(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content, Link link, Vector2 scale, StageManager StageManager)
        {
            _spriteBatch = spriteBatch;
            _link = link;
            _scale = scale;
            _position = Vector2.Zero;
            this.graphicsDevice = graphicsDevice;
            Health = _link.Health;
            this.content = content;
            LoadContent(content);
            InitializeHUDPositions();
            stageManager = StageManager;
            MiniMap = new MiniMap1(_scale, stageManager, _link);
            MiniMap.LoadMap(content);
            

        }

        private void LoadContent(ContentManager content)
        {
 
            try
            {
                _hudTexture = content.Load<Texture2D>("NES - The Legend of Zelda - HUD & Pause Screen");
                SetMultipleTransparency();
            }
            catch (ContentLoadException e)
            {
                Console.WriteLine($"Error loading content: {e.Message}");
                throw;
            }
        }
        private void InitializeHUDPositions()
        {

            _hudBackground = new Rectangle(0, 0, graphicsDevice.Viewport.Width, (int)(HUD_HEIGHT * _scale.Y * 1.2));
            _healthBarPosition = new Rectangle(703, 133, (int)(HEART_WIDTH * _scale.X * 1.15), (int)(HEART_HEIGHT * _scale.Y * 1.15));
            //hard coded heart position and scaling -> reason being HUD size is based on a scale and screenwidth. 

            cutOuts = new Rectangle[]
             {
                  new Rectangle(258, 11,255,55) ,  //the background
                  new Rectangle(645, 117, 8, 8),     // 1 full heart
                  new Rectangle(636, 117, 8, 8),     // 1 half heart
                  new Rectangle(627, 117, 8, 8),     // 1 emtpy heart

                  new Rectangle(519, 117, 8, 8), // X [4]
                  new Rectangle(528, 117, 8, 8), //0 [5]
                  new Rectangle(537, 117, 8, 8), //1 [6]
                  new Rectangle(546, 117, 8, 8), //2 [7]
                  new Rectangle(555, 117, 8, 8), //3 [8]
                  new Rectangle(564, 117, 8, 8), //4 [9]
                  new Rectangle(573, 117, 8, 8), //5 [10]
                  new Rectangle(582, 117, 8, 8), //6 [11]
                  new Rectangle(591, 117, 8, 8), //7 [12]
                  new Rectangle(600, 117, 8, 8), //8 [13]
                  new Rectangle(609, 117, 8, 8), //9 [14]
                  new Rectangle(564, 137, 8, 16), //sword [15]

             };

        }
        public Rectangle[] GetCutOuts()
        {
            return cutOuts;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void Draw()
        {

            Rectangle adjustedBackground = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                graphicsDevice.Viewport.Width,
                (int)(HUD_HEIGHT * _scale.Y * 1.2));

            // adjustedBackground 
            _spriteBatch.Draw(_hudTexture, adjustedBackground, cutOuts[0], Color.White);

            DrawHearts();
            DrawKeys();
            DrawGems();
            DrawBombs();
            DrawSword();
            DrawCurrentItem();
            MiniMap.Draw(_spriteBatch);
        }
        private void DrawHearts()
        {
            Health = _link.Health;
            int maxHearts = 16; // Maximum number of hearts to display

            // Adjust heart positions to include offset
            for (int i = 0; i < maxHearts; i++)
            {
                int row = i / heartsPerRow;
                int column = i % heartsPerRow;
                int heartValue = Health - (i * 2);

                Rectangle heartSource;
                if (heartValue >= 2)
                {
                    heartSource = cutOuts[1];  // Full heart
                }
                else if (heartValue == 1)
                {
                    heartSource = cutOuts[2];  // Half heart
                }
                else
                {
                    heartSource = cutOuts[3];  // Empty heart
                }

                // Add position offset to heart positions
                int xPosition = (int)_position.X + _healthBarPosition.X + column * (int)(HEART_WIDTH * _scale.X);
                int yPosition = (int)_position.Y + _healthBarPosition.Y + row * (int)(HEART_HEIGHT * _scale.Y);

                _spriteBatch.Draw(_hudTexture,
                    new Rectangle(xPosition, yPosition, (int)(HEART_WIDTH * _scale.X), (int)(HEART_HEIGHT * _scale.Y)),
                    heartSource, Color.White);

            }
        }

        private void DecrementKeys()
        {
            if (_link.hasKey)
            {

            }
        }

        //public void IncrementHealth()
        //{
        //    Health++;
        //}
        //public void DecrementHealth()
        //{
        //    Health--;
        //}
        private void DrawKeys()
        {

            int keyCount = _link.GetKeyCount();

            Vector2 baseKeyPosition = new Vector2(385, 135); //hardcoded
            Rectangle xSource = cutOuts[4]; // Index 4 is the 'x'

            _spriteBatch.Draw(_hudTexture, new Rectangle((int)_position.X + (int)baseKeyPosition.X, (int)_position.Y + (int)baseKeyPosition.Y, (int)(8 * _scale.X), (int)(8 * _scale.Y)), xSource, Color.White);


            //keys:
            if (keyCount > 0)
            {
                string keyString = keyCount.ToString();
                for (int i = 0; i < keyString.Length; i++)
                {
                    int digit = keyString[i] - '0';
                    Rectangle digitSource = cutOuts[digit + 5];

                    // Calculate position for each digit
                    float xDigitPos = (int)_position.X + baseKeyPosition.X + ((i + 1) * Spacing * _scale.X);
                    float yDigitPos = (int)_position.Y + baseKeyPosition.Y;

                    _spriteBatch.Draw(
                        _hudTexture,
                        new Rectangle(
                            (int)xDigitPos,
                            (int)yDigitPos,
                            (int)(8 * _scale.X),
                            (int)(8 * _scale.Y)
                        ),
                        digitSource,
                        Color.White
                    );
                }
            }
            else
            {
                //0 when no keys
                // Calculate position for each digit
                float xDigitPos = (int)_position.X + baseKeyPosition.X + 1 * Spacing * _scale.X;
                float yDigitPos = (int)_position.Y + baseKeyPosition.Y;

                Rectangle digitSource = cutOuts[5];
                _spriteBatch.Draw(
                    _hudTexture,
                    new Rectangle(
                        (int)xDigitPos,
                        (int)yDigitPos,
                        (int)(8 * _scale.X),
                        (int)(8 * _scale.Y)
                    ),
                    digitSource,
                    Color.White
                );


            }
        }
        private void DrawGems()
        {
            int GemCount = _link.GetGemCount();

            Vector2 baseGemPosition = new Vector2(385, 68); //hardcoded
            Rectangle xSource = cutOuts[4]; // Index 4 is the 'x'

            _spriteBatch.Draw(_hudTexture, new Rectangle((int)_position.X + (int)baseGemPosition.X, (int)_position.Y + (int)baseGemPosition.Y, (int)(8 * _scale.X), (int)(8 * _scale.Y)), xSource, Color.White);


            //keys:
            if (GemCount > 0)
            {
                string GemString = GemCount.ToString();
                for (int i = 0; i < GemString.Length; i++)
                {
                    int digit = GemString[i] - '0';
                    Rectangle digitSource = cutOuts[digit + 5];

                    // Calculate position for each digit
                    float xDigitPos = (int)_position.X + baseGemPosition.X + ((i + 1) * Spacing * _scale.X);
                    float yDigitPos = (int)_position.Y + baseGemPosition.Y;

                    _spriteBatch.Draw(
                        _hudTexture,
                        new Rectangle(
                            (int)xDigitPos,
                            (int)yDigitPos,
                            (int)(8 * _scale.X),
                            (int)(8 * _scale.Y)
                        ),
                        digitSource,
                        Color.White
                    );
                }
            }
            else
            {
                //0 when no gems
                // Calculate position for each digit
                float xDigitPos = (int)_position.X + baseGemPosition.X + 1 * Spacing * _scale.X;
                float yDigitPos = (int)_position.Y + baseGemPosition.Y;

                Rectangle digitSource = cutOuts[5];
                _spriteBatch.Draw(
                    _hudTexture,
                    new Rectangle(
                        (int)xDigitPos,
                        (int)yDigitPos,
                        (int)(8 * _scale.X),
                        (int)(8 * _scale.Y)
                    ),
                    digitSource,
                    Color.White
                );


            }
        }
        private void DrawBombs()
        {
            int BombCount = _link.BombCount;

            Vector2 baseBombPosition = new Vector2(385, 169); //hardcoded
            Rectangle xSource = cutOuts[4]; // Index 4 is the 'x'

            _spriteBatch.Draw(_hudTexture, new Rectangle((int)_position.X + (int)baseBombPosition.X, (int)_position.Y + (int)baseBombPosition.Y, (int)(8 * _scale.X), (int)(8 * _scale.Y)), xSource, Color.White);


            //keys:
            if (BombCount > 0)
            {
                string BombString = BombCount.ToString();
                for (int i = 0; i < BombString.Length; i++)
                {
                    int digit = BombString[i] - '0';
                    Rectangle digitSource = cutOuts[digit + 5];

                    // Calculate position for each digit
                    float xDigitPos = (int)_position.X + baseBombPosition.X + ((i + 1) * Spacing * _scale.X);
                    float yDigitPos = (int)_position.Y + baseBombPosition.Y;

                    _spriteBatch.Draw(
                        _hudTexture,
                        new Rectangle(
                            (int)xDigitPos,
                            (int)yDigitPos,
                            (int)(8 * _scale.X),
                            (int)(8 * _scale.Y)
                        ),
                        digitSource,
                        Color.White
                    );
                }
            }
            else
            {
                //0 when no bombs
                // Calculate position for each digit
                float xDigitPos = (int)_position.X + baseBombPosition.X + 1 * Spacing * _scale.X;
                float yDigitPos = (int)_position.Y + baseBombPosition.Y;

                Rectangle digitSource = cutOuts[5];
                _spriteBatch.Draw(
                    _hudTexture,
                    new Rectangle(
                        (int)xDigitPos,
                        (int)yDigitPos,
                        (int)(8 * _scale.X),
                        (int)(8 * _scale.Y)
                    ),
                    digitSource,
                    Color.White
                );


            }
        }
        private void DrawSword()
        {
            Vector2 baseSwordPosition = new Vector2(610, 102);
 
            Rectangle swordPosition = new Rectangle(
                (int)(_position.X + baseSwordPosition.X),   //transition
                (int)(_position.Y + baseSwordPosition.Y),   
                (int)(8 * _scale.X),    // width
                (int)(16 * _scale.Y)    // height
            );

            Rectangle swordSource = cutOuts[15];

            _spriteBatch.Draw(
                _hudTexture,
                swordPosition,
                swordSource,
                Color.White
            );

        }
        private void DrawCurrentItem()
        {
            // B   slot  
            Vector2 bSlotPosition = new Vector2(510, 102);

            if (_link?.inventory?.SelectedItem != null)
            {
                Iitem currentItem = _link.inventory.SelectedItem;

                // Check if item need to be displayed (in case key decremented to 0)
                bool shouldDisplayItem = true;
                if (currentItem.CurrentItemType == ItemType.boom && _link.BombCount <= 0)
                {
                    shouldDisplayItem = false;
                }
                else if (currentItem.CurrentItemType == ItemType.key && _link.GetKeyCount() <= 0)
                {
                    shouldDisplayItem = false;
                }
                else if ( currentItem.CurrentItemType == ItemType.diamond && _link.GetGemCount() <= 0)
                {
                    shouldDisplayItem = false;
                }
                else if (currentItem.CurrentItemType == ItemType.clock && _link.GetClockCount() <= 0)
                {
                    shouldDisplayItem = false;
                }
                else if (currentItem.CurrentItemType == ItemType.potion && _link.GetPotionCount() <= 0)
                {
                    shouldDisplayItem = false;
                }


                if (shouldDisplayItem)
                {
                    _spriteBatch.Draw(
                        currentItem.Sprite,
                        new Vector2(
                            _position.X + bSlotPosition.X,
                            _position.Y + bSlotPosition.Y
                        ),
                        currentItem.SourceRectangles[0],
                        Color.White,
                        0f,
                        Vector2.Zero,
                        _scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }

        // remove multiple colors of background for inventory
        private void SetMultipleTransparency()
        {
            if (_hudTexture == null) return;

            Color[] data = new Color[_hudTexture.Width * _hudTexture.Height];
            _hudTexture.GetData(data);

            //  color background replace
            for (int i = 0; i < data.Length; i++)
            {
                Color pixel = data[i];

                // Grey (116, 116, 116)
                if (pixel.R == 116 && pixel.G == 116 && pixel.B == 116)
                {
                    data[i] = Color.Transparent;
                }
                // Pink (255, 0, 255)
                else if (pixel.R == 255 && pixel.G == 0 && pixel.B == 255)
                {
                    data[i] = Color.Transparent;
                }
                // Blue (0, 255, 255)
                else if (pixel.R == 0 && pixel.G == 255 && pixel.B == 255)
                {
                    data[i] = Color.Transparent;
                }
                // Cyan (0, 128, 128)
                else if (pixel.R == 0 && pixel.G == 128 && pixel.B == 128)
                {
                    data[i] = Color.Transparent;
                }
                // Purple (128, 0, 128)
                else if (pixel.R == 128 && pixel.G == 0 && pixel.B == 128)
                {
                    data[i] = Color.Transparent;
                }
            }

            _hudTexture.SetData(data);
        }

        public void Update()
        {
            MiniMap.Update();
        }
    }
}