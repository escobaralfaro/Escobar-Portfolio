using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint0.Classes;
using Sprint0.Collisions;
using Sprint0.Player;
using Sprint2.GameStates;
using Sprint2.Map;

namespace Sprint0
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;    
        private IGameState CurrentGameState;
        public Vector2 _scale;


        private GameStateManager _GameStateManager;
        public int enemyDefeatedCount = 0;
        public int itemCollectedCount = 0;
        public bool isDungeonComplete = false;
        private StageManager _stageManager;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1020;
            _graphics.PreferredBackBufferHeight = 920;
         
            _graphics.ApplyChanges(); 
        }

        protected override void Initialize()
        {
            //DEBUG FOR ENEMY HITBOXES
            DebugDraw.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
         
            Content.RootDirectory = "Content";
            _scale.X = (float)GraphicsDevice.Viewport.Width / 256.0f;
            _scale.Y = (float)GraphicsDevice.Viewport.Height / 230.0f;
            CurrentGameState = new StartMenu(GraphicsDevice,_spriteBatch, Content, _scale);
            CurrentGameState.LoadContent(Content);
          

            _GameStateManager = new GameStateManager(_graphics, GraphicsDevice, _spriteBatch, _scale, this);
            _GameStateManager.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
           
        
            _GameStateManager.Update(gameTime);
                   base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

          //  _spriteBatch.Begin(SpriteSortMode.Immediate);
            GraphicsDevice.Clear(Color.Black);
            _GameStateManager.Draw();
           _spriteBatch.End();
            base.Draw(gameTime);
        }      
    }
}


