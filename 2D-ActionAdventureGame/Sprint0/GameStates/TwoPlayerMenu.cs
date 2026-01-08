using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint2.GameStates
{
    public class TwoPlayerMenu : IGameState
    {
        private Vector2 _scale;
        static GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private Texture2D titleScreen;
        private SpriteFont font;
        
        Song titleSequence;

        private float timer;
        private bool showText;

        string welcome;
        string welcome2;
        string goback;
        string colorSelect;
        string color1;
        string color2;
        string color3;  
        string color4;

        float scale1;
        float scale2;
        float scale3;
        float scale4;
        float scale5;
   
        public TwoPlayerMenu(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content, Vector2 scale)
        {
            font = content.Load<SpriteFont>("File");
            showText = true;
            _scale = scale;
            timer = 0f;
            _spriteBatch = spriteBatch;
            //Music
            titleSequence = content.Load<Song>("TitleTheme");
            welcome = "WELCOME TO TWO-PLAYER!";
            welcome2 = "In two-player both players will share their inventory and health!";
            goback = "PRESS B TO RETURN TO START";
            colorSelect = "Press a key 1-5 to select player 2's color:";
            color1 = "1: Pink";
            color2 = "2: Cyan";
            color3 = "3: Black";
            color4 = "4: Navy Blue";

            float dpi = 96;

            float targetHeight1 = (1.5f/2.45f) * dpi;
            float targetHeight2 = (1f / 2.45f) * dpi; 
            
            
            Vector2 WelcomeSize = font.MeasureString(welcome);
            Vector2 Welcome2Size = font.MeasureString(welcome2);
            Vector2 SelectionSize = font.MeasureString(colorSelect);
            Vector2 gobackSize = font.MeasureString(goback);
            Vector2 C1 = font.MeasureString(color1);

            scale1 = targetHeight1 / WelcomeSize.Y;
            scale2 = targetHeight1 / gobackSize.Y;
            scale3 = targetHeight2 / Welcome2Size.Y;
            scale4 = targetHeight2 / SelectionSize.Y;
            scale5 = targetHeight2 / C1.Y;
        }

        public void LoadContent(ContentManager Content)
        {
            // Nothing to load
        }

        public void Update(GameTime gameTime)
        {
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
        public int GetLinkHealth()
        {
            return 1;
        }
        
        public void Draw()
        {
            if (showText)
            {
                _spriteBatch.DrawString(font, welcome, new Vector2(150, 20), Color.White, 0f, Vector2.Zero, scale1, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(font, goback, new Vector2(120, 800), Color.White, 0f, Vector2.Zero, scale2, SpriteEffects.None, 0f);
            }
            
            _spriteBatch.DrawString(font, welcome2, new Vector2(50, 80), Color.White, 0f, Vector2.Zero, scale3, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, colorSelect, new Vector2(200, 140), Color.White, 0f, Vector2.Zero, scale4, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, color1, new Vector2(450, 200), Color.White, 0f, Vector2.Zero, scale5, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, color2, new Vector2(450, 260), Color.White, 0f, Vector2.Zero, scale5, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, color3, new Vector2(450, 320), Color.White, 0f, Vector2.Zero, scale5, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, color4, new Vector2(450, 380), Color.White, 0f, Vector2.Zero, scale5, SpriteEffects.None, 0f);
        }
    }
}