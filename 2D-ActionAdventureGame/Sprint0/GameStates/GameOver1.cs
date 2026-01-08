using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
namespace Sprint2.GameStates
{
    public class GameOver1 : IGameState
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

        private string PauseText = "GAME OVER";
        private string Return = "YOU DIED!";
        string restart = "PRESS R TO RESTART LEVEL";
        string start = "PRESS S TO RETURN TO START MENU";
        string quit = "PRESS Q TO EXIT";

        Vector2 PauseSize;
        Vector2 ReturnSize;
        Vector2 restartSize;
        Vector2 startSize;
        Vector2 quitSize;


        float PauseScale;
        float ReturnScale;
        float restartScale;
        float startScale;
        float quitScale;
        public GameOver1(SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphics)
        {
            font = content.Load<SpriteFont>("File");

            showText = true;
            timer = 0f;

            _spriteBatch = spriteBatch;
            _graphics = graphics;

            float dpi = 96;

            float targetHeight1 = (2f / 2.45f) * dpi;
            float targetHeight2 = (1f / 2.45f) * dpi;

            PauseSize = font.MeasureString(PauseText);
            ReturnSize = font.MeasureString(Return);
            
            restartSize = font.MeasureString(restart);
            startSize = font.MeasureString(start);
            quitSize = font.MeasureString(quit);

            PauseScale = targetHeight1 / PauseSize.Y;
            ReturnScale = targetHeight2 / ReturnSize.Y;
            startScale =   targetHeight2 / startSize.Y;
      
            restartScale = targetHeight2 / restartSize.Y;
            quitScale = targetHeight2 / quitSize.Y;
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
            DrawGameOverMenu();
        }

        public void DrawGameOverMenu()
        {
            if (showText)
            {
                _spriteBatch.DrawString(font, PauseText, GetCenter(PauseSize, 20, PauseScale), Color.Red, 0f, Vector2.Zero, PauseScale, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(font, Return, GetCenter(ReturnSize, 90, ReturnScale), Color.White, 0f, Vector2.Zero, ReturnScale, SpriteEffects.None, 0f);
            }
            _spriteBatch.DrawString(font, restart, GetCenter(restartSize, 290, restartScale), Color.White, 0f, Vector2.Zero, restartScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, start, GetCenter(startSize, 340, startScale), Color.White, 0f, Vector2.Zero, startScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, quit, GetCenter(quitSize, 390, quitScale), Color.White, 0f, Vector2.Zero, quitScale, SpriteEffects.None, 0f);


        }

        public int GetLinkHealth()
        {
            return 1;
        }

        public Vector2 GetCenter(Vector2 size, int y, float scale)
        {
            return new Vector2((_graphics.Viewport.Width - (size.X * scale)) / 2, y);
        }
    }
}