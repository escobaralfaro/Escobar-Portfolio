using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using Sprint0.Player;
using Sprint2.Enemy;
using Sprint2.Map;


namespace Sprint2.GameStates
{
    public class LevelOne : IGameState
    {
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        public Link _link;
        public StageManager _StageManager;
        private LinkSpriteFactory _linkSpriteFactory;
        private DungeonBlockSpriteFactory _dungeonBlockSpriteFactory;
        private IEnemy enemy;
        private Texture2D bossSpriteSheet;
        private Texture2D dungeonSpriteSheet;
        private DungeonMap _map;
        public Vector2 _scale;
        private Enemy_Item_Map enemyItemMap;
        private KeyboardController _keyboardController;
        private int currentRoomNumber;
        private GameHUD _gameHUD;
        private MouseController _mouseController;
        public AchievementManager achievementManager { get; private set; }
        public SpriteFont font;

        public LevelOne(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Vector2 scale, GraphicsDevice graphicsDevice, Link link)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            _scale = scale;
            _graphicsDevice = graphicsDevice;
            _link = link;
            achievementManager = new AchievementManager(_link, _scale);
        }

        public void LoadContent(ContentManager Content)
        {
            

            bossSpriteSheet = Content.Load<Texture2D>("Bosses1");
            

            //initalize spritefactory
            _linkSpriteFactory = new LinkSpriteFactory(_graphicsDevice, Content, "LinkSpriteSheet2");
            _dungeonBlockSpriteFactory = new DungeonBlockSpriteFactory(_graphicsDevice, Content, "DungeonSheet");

            Rectangle[] linkFrames = _linkSpriteFactory.CreateFrames();
            Rectangle[] dungeonTiles = _dungeonBlockSpriteFactory.CreateFrames();
            Texture2D dungeonTexture = Content.Load<Texture2D>("DungeonSheet");

            
            _StageManager = new StageManager(dungeonTiles, dungeonTexture, _spriteBatch, _graphicsDevice, _link, Content, _scale);
            _gameHUD = new GameHUD(_spriteBatch, _graphicsDevice, Content, _link, _scale, _StageManager);
            
            _mouseController = new MouseController(_StageManager);
        }


        public void Draw()
        {   
            _StageManager.Draw();
            if (!_StageManager.GetAnimationState())
            {
                _link.Draw(_spriteBatch);
            }

            _gameHUD.Draw();
            //achievementManager.Draw(_spriteBatch, font, _graphicsDevice);
            // do drawing for achievements here
        }

        public int GetLinkHealth()
        {
            return _link.Health;
        }


        public void Update(GameTime gameTime)
        {

            _StageManager.Update(gameTime);
            if (!_StageManager.GetAnimationState())
            {
                _link.Update(gameTime);
                _gameHUD.Update();
                //_keyboardController.Update();
                _mouseController.Update();
            }

        }

       
    }
}
