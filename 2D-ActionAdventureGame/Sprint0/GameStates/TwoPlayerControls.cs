using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
namespace Sprint2.GameStates
{
    public class TwoPlayerControls : IGameState
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

        private string PauseText = "HOW TO PLAY TWO PLAYER";
        private string Return = "Space to Continue...";

        string play1 = "---PLAYER 1---";
        string volUp = "USE SWORD || Z";
        string volDown = "CYCLE EQUIPPED ITEM || P (SHARED)";
        string mute = "VIEW INVENTORY || i (SHARED)";
        string controls = "USE EQUPPIED || 3";
        string restart = "GO THROUGH/UNLOCK DOORS || M1 (SHARED)";
        string start = "TALK/BUY SHOPKEEPER || F (SHARED)";
        string quit = "PRESS ESC TO GO BACK";

        string play2 = "\n\n---PLAYER 2---";
        string volUp2 = "USE SWORD || NumPad1";
        string volDown2 = "CYCLE EQUIPPED ITEM || P (SHARED)";
        string mute2 = "VIEW INVENTORY || i (SHARED)";
        string controls2 = "USE EQUPPIED || ENTER";
        string restart2 = "GO THROUGH/UNLOCK DOORS || M1 (SHARED)";
        string start2 = "TALK/BUY SHOPKEEPER || F (SHARED)";
        string quit2 = "PRESS SPACE TO ENTER PAUSE MENU";


        Vector2 PauseSize;
        Vector2 ReturnSize;
        Vector2 UpSize;
        Vector2 DownSize;
        Vector2 MuteSize;
        Vector2 controlSize;
        Vector2 restartSize;
        Vector2 startSize;
        Vector2 quitSize;


        float PauseScale;
        float ReturnScale;
        float UpScale;
        float DownScale;
        float MuteScale;
        float ControlScale;
        float restartScale;
        float startScale;
        float quitScale;
        public TwoPlayerControls(SpriteBatch spriteBatch, ContentManager content, GraphicsDevice graphics)
        {
            font = content.Load<SpriteFont>("File");
            titleSequence = content.Load<Song>("TitleTheme");
            showText = true;
            timer = 0f;

            _spriteBatch = spriteBatch;
            _graphics = graphics;

            float dpi = 96;

            float targetHeight1 = (1.5f / 2.45f) * dpi;
            float targetHeight2 = (1f / 2.45f) * dpi;

            PauseSize = font.MeasureString(PauseText);
            ReturnSize = font.MeasureString(Return);
            UpSize = font.MeasureString(volUp);
            DownSize = font.MeasureString(volDown);
            MuteSize = font.MeasureString(mute);
            controlSize = font.MeasureString(controls);
            restartSize = font.MeasureString(restart);
            startSize = font.MeasureString(start);
            quitSize = font.MeasureString(quit);

            PauseScale = targetHeight1 / PauseSize.Y;
            ReturnScale = targetHeight2 / ReturnSize.Y;
            UpScale = targetHeight2 / UpSize.Y;
            DownScale = targetHeight2 / DownSize.Y;
            MuteScale = targetHeight2 / MuteSize.Y;
            ControlScale = targetHeight2 / controlSize.Y;
            restartScale = targetHeight2 / restartSize.Y;
            startScale = targetHeight2 / startSize.Y;
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
            DrawPauseMenu();
        }

        public void DrawPauseMenu()
        {
            if (showText)
            {
                _spriteBatch.DrawString(font, PauseText, GetCenter(PauseSize, 20, PauseScale), Color.White, 0f, Vector2.Zero, PauseScale, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(font, Return, GetCenter(ReturnSize, 80, ReturnScale), Color.White, 0f, Vector2.Zero, ReturnScale, SpriteEffects.None, 0f);
            }
            //play1
            _spriteBatch.DrawString(font, play1, GetCenter(UpSize, 140, UpScale), Color.White, 0f, Vector2.Zero, UpScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, volUp, GetCenter(UpSize, 190, UpScale), Color.White, 0f, Vector2.Zero, UpScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, volDown, GetCenter(DownSize, 290, DownScale), Color.White, 0f, Vector2.Zero, DownScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, mute, GetCenter(MuteSize, 340, MuteScale), Color.White, 0f, Vector2.Zero, MuteScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, controls, GetCenter(controlSize, 240, ControlScale), Color.White, 0f, Vector2.Zero, ControlScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, restart, GetCenter(restartSize, 390, restartScale), Color.White, 0f, Vector2.Zero, restartScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, start, GetCenter(startSize, 440, startScale), Color.White, 0f, Vector2.Zero, startScale, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(font, quit, GetCenter(quitSize, 1000, quitScale), Color.White, 0f, Vector2.Zero, quitScale, SpriteEffects.None, 0f);

            //player2
            _spriteBatch.DrawString(font, play2, GetCenter(UpSize, 450, UpScale), Color.White, 0f, Vector2.Zero, UpScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, volUp2, GetCenter(UpSize, 590, UpScale), Color.White, 0f, Vector2.Zero, UpScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, volDown2, GetCenter(DownSize, 690, DownScale), Color.White, 0f, Vector2.Zero, DownScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, mute2, GetCenter(MuteSize, 740, MuteScale), Color.White, 0f, Vector2.Zero, MuteScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, controls2, GetCenter(controlSize, 640, ControlScale), Color.White, 0f, Vector2.Zero, ControlScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, restart2, GetCenter(restartSize, 790, restartScale), Color.White, 0f, Vector2.Zero, restartScale, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, start2, GetCenter(startSize, 940, startScale), Color.White, 0f, Vector2.Zero, startScale, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(font, quit2, GetCenter(quitSize, 990, quitScale), Color.White, 0f, Vector2.Zero, quitScale, SpriteEffects.None, 0f);



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