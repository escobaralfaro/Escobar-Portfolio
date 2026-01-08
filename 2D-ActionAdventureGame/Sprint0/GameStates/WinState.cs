using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprint0.Player;
using Sprint2.Map;

namespace Sprint2.GameStates
{
    public class WinState : IGameState
    {
        public GameStage currentGameStage;
        public SpriteBatch _spriteBatch;
        public Vector2 _scale;
        static GraphicsDevice _graphicsDevice;
        private Link _link;

        public Texture2D endScreen;
        public SpriteFont font;
        public float timer;
        public bool showText;
        Song endSequence;
        Song backgroundMusic;

        public WinState(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content,Vector2 scale)
        {

            currentGameStage = GameStage.StartMenu;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
            _scale = scale;

            endScreen = content.Load<Texture2D>("EndingofZelda");
            font = content.Load<SpriteFont>("File");
            timer = 0f;
            showText = true;

            //Music
            endSequence = content.Load<Song>("EndingTheme");
            backgroundMusic = content.Load<Song>("DungeonTheme");

        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            UpdateEnd(gameTime);

        }
        public int GetLinkHealth()
        {
            return 1;
        }

        public void UpdateEnd(GameTime gameTime)
        {
            
                if (MediaPlayer.State == MediaState.Playing && MediaPlayer.Queue.ActiveSong == backgroundMusic)
                {
                    MediaPlayer.Stop();
                }
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(endSequence);
                    MediaPlayer.IsRepeating = true; // loop the music
                }
            
        }

        public void Draw()
        {

            DrawEnd();

        }

 

        public void DrawEnd()
        {
            Vector2 position = new Vector2(100, 240);
            Vector2 scale = new Vector2(0.8f, 0.8f);
            _spriteBatch.Draw(endScreen, position, null, Color.White, 0f, Vector2.Zero, scale, 0, 0f);
        }
    }
}
