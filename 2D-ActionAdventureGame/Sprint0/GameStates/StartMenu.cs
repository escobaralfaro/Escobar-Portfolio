using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Sprint2.GameStates
{
    public class StartMenu : IGameState
    {
        private Texture2D _texture;
        private Vector2 _scale;
        static GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
      

        // Start Menu
        private Texture2D titleScreen;
        private SpriteFont font;
        private float timer;
        private bool showText;
        Song titleSequence;
        

        public StartMenu(GraphicsDevice graphicsDevice,SpriteBatch spriteBatch, ContentManager content, Vector2 scale)
        {
            titleScreen = content.Load<Texture2D>("TitleScreen");
            font = content.Load<SpriteFont>("File");
            timer = 0f;
            showText = true;    
            _scale = scale;

            _spriteBatch = spriteBatch;
            //Music
            titleSequence = content.Load<Song>("TitleTheme");

        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {

            UpdateStartMenu(gameTime);

        }
        public int GetLinkHealth()
        {
            return 1;
        }

        public void UpdateStartMenu(GameTime gameTime)
        {
           
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 0.5f)
            {
                showText = !showText;
                timer = 0;
            }

            // Blinking text effect
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 0.5f)
            {
                showText = !showText;
                timer = 0;
            }
            
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(titleSequence);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void Draw()
        {

            DrawStartMenu();

        }

        public void DrawStartMenu()
        {
            //_spriteBatch.Begin();

            Rectangle sourceRectangle = new Rectangle(1, 11, 245, 225);

            Vector2 position = new Vector2(0, 0);

            Vector2 scale = new Vector2(4.16f, 4.1f);

            // Draw the title screen
            _spriteBatch.Draw(titleScreen, position, sourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            // Draw text
            if (showText)
            {
                //string startText = "PUSH START BUTTON";
                ////Vector2 textSize = font.MeasureString(startText);
                //float TextSizeNumber = 10;
                //Vector2 textSize = 40;
                //Vector2 textPosition = new Vector2(
                //    (_spriteBatch.GraphicsDevice.Viewport.Width - textSize.X) / 2,
                //    400
                //);
                //_spriteBatch.DrawString(font, startText, textPosition, Color.White);
                string startText = "PRESS 1 FOR SINGLE PLAYER, 2 FOR 2 PLAYER";
                Vector2 textSize = font.MeasureString(startText); // Measure text size in pixels
                float targetTextHeight = 1f; // Example target height in cm

                // Assuming a screen DPI, e.g., 96 (adjust for actual DPI if known)
                float dpi = 96;
                float targetHeightInPixels = (targetTextHeight / 2.54f) * dpi;
                float scaled = targetHeightInPixels / textSize.Y; // Scale based on target height

                Vector2 textPosition = new Vector2(150,652);

                // Draw the text with scaling
                _spriteBatch.DrawString(font, startText, textPosition, Color.White, 0f, Vector2.Zero, scaled, SpriteEffects.None, 0f);
            }
        }

    }


}
