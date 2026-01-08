using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Sprint2.GameStates
{
    public class PauseMenu : IGameState
    {
       // public GameStage currentGameStage;
        public Texture2D _texture;
        public SpriteBatch _spriteBatch;
        public Vector2 _scale;
        static GraphicsDevice _graphics;
        private SpriteFont font;
        Song titleSequence;
        private float timer;
        private bool showText;

        public Texture2D pauseScreen;

        private string PauseText = "PAUSE MENU";
        private string Return = "PRESS ESC TO RETURN TO GAME";
        string volUp = "PRESS + T0 RAISE VOLUME";
        string volDown = "PRESS - TO LOWER VOLUME";
        string mute = "PRESS 0 TO MUTE / UNMUTE";
        string controls = "PRESS SPACE TO VIEW INSTRUCTIONS";
        string restart = "PRESS R TO RESTART LEVEL";
        string start = "PRESS S TO RETURN TO START MENU";
        string quit = "PRESS Q TO EXIT";
        string box = "PRESS , TO TOGGLE HITBOXS";

        Vector2 PauseSize;
        Vector2 ReturnSize;
        Vector2 UpSize;
        Vector2 DownSize;
        Vector2 MuteSize;
        Vector2 controlSize;
        Vector2 restartSize;
        Vector2 startSize;
        Vector2 quitSize;
        Vector2 boxSize;



        float PauseScale;
        float ReturnScale;
        float UpScale;
        float DownScale;
        float MuteScale;
        float ControlScale;
        float restartScale;
        float startScale;
        float quitScale;
        float boxScale;

        public PauseMenu(SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphics)
        {
            font = content.Load<SpriteFont>("File");
            titleSequence = content.Load<Song>("TitleTheme");
            showText = true;
            timer = 0f;

            _spriteBatch = spriteBatch;
            _graphics = graphics;

            float dpi = 96;

            float targetHeight1 = (2f / 2.45f) * dpi;
            float targetHeight2 = (1f / 2.45f) * dpi;

            PauseSize = font.MeasureString(PauseText);
            ReturnSize = font.MeasureString(Return);
            UpSize = font.MeasureString(volUp);
            DownSize = font.MeasureString (volDown);
            MuteSize = font.MeasureString(mute);
            controlSize = font.MeasureString(controls);
            restartSize = font.MeasureString(restart);
            startSize = font.MeasureString(start);
            quitSize = font.MeasureString(quit);
            boxSize = font.MeasureString(box);


            PauseScale = targetHeight1 / PauseSize.Y;
            ReturnScale = targetHeight2 / ReturnSize.Y;
            UpScale = targetHeight2 / UpSize.Y;
            DownScale = targetHeight2 / DownSize.Y;
            MuteScale = targetHeight2 / MuteSize.Y;
            ControlScale = targetHeight2 / controlSize.Y;
            restartScale = targetHeight2 / restartSize.Y;
            quitScale = targetHeight2 / quitSize.Y;
            boxScale = targetHeight2 / boxSize.Y;

        }

        // public bool IsPaused => isPaused;

        public void TogglePause()
        {
          
                MediaPlayer.Pause();
            
        }
        public void LoadContent(ContentManager Content)
        {

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

      

        public void Draw()
        {
                DrawPauseMenu();       
        }

        public void DrawPauseMenu()
        {
            if (showText)
            {
                _spriteBatch.DrawString(font, PauseText, GetCenter(PauseSize,20,PauseScale), Color.White, 0f, Vector2.Zero, PauseScale, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(font, Return, GetCenter(ReturnSize,800,ReturnScale), Color.White, 0f, Vector2.Zero, ReturnScale, SpriteEffects.None, 0f);
            }
            _spriteBatch.DrawString(font, volUp, GetCenter(UpSize,190,UpScale), Color.White, 0f, Vector2.Zero, UpScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, volDown, GetCenter(UpSize, 240, UpScale), Color.White, 0f, Vector2.Zero, DownScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, mute, GetCenter(UpSize, 290, UpScale), Color.White, 0f, Vector2.Zero, MuteScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, controls, GetCenter(UpSize, 340, UpScale), Color.White, 0f, Vector2.Zero, ControlScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, restart, GetCenter(UpSize, 390, UpScale), Color.White, 0f, Vector2.Zero, ControlScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, start, GetCenter(UpSize, 440, UpScale), Color.White, 0f, Vector2.Zero, ControlScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, quit, GetCenter(UpSize, 490, UpScale), Color.White, 0f, Vector2.Zero, quitScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, box, GetCenter(UpSize, 540, UpScale), Color.White, 0f, Vector2.Zero, boxScale, SpriteEffects.None, 0f);


        }

        public int GetLinkHealth()
        {
            return 1;
        }

        public Vector2 GetCenter(Vector2 size, int y , float scale)
        {
            return new Vector2((_graphics.Viewport.Width - (size.X *scale))/2, y);
        }
    }
}
