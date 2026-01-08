using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using Sprint0.Player;
using Sprint2.Enemy;
using Sprint2.Map;
using Sprint2.TwoPlayer;


namespace Sprint2.GameStates
{
    public class TwoPlayerMode : IGameState
    {
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        public Link _link;
        public Link _link2;
        public StageManager2 _StageManager2;
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
        private GameHUD2 _gameHUD2;
        private MouseController2 _mouseController2;

        public TwoPlayerMode(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Vector2 scale, GraphicsDevice graphicsDevice, Link link, Link link2)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            _scale = scale;
            _graphicsDevice = graphicsDevice;
            _link = link;
            _link2 = link2;
        }

        public void LoadContent(ContentManager Content)
        {


            bossSpriteSheet = Content.Load<Texture2D>("Bosses1");
            _dungeonBlockSpriteFactory = new DungeonBlockSpriteFactory(_graphicsDevice, Content, "DungeonSheet");

          
            Rectangle[] dungeonTiles = _dungeonBlockSpriteFactory.CreateFrames();
            Texture2D dungeonTexture = Content.Load<Texture2D>("DungeonSheet");


            _StageManager2 = new StageManager2(dungeonTiles, dungeonTexture, _spriteBatch, _graphicsDevice, _link,_link2, Content, _scale);
            _gameHUD2 = new GameHUD2(_spriteBatch, _graphicsDevice, Content, _link,_link2, _scale, _StageManager2);

            _mouseController2 = new MouseController2(_StageManager2);
        }


        public void Draw()
        {
            _StageManager2.Draw();
            if (!_StageManager2.GetAnimationState())
            {
                _link.Draw(_spriteBatch);
                _link2.Draw(_spriteBatch);
            }

             _gameHUD2.Draw();
        }

        public int GetLinkHealth()
        {
            return _link.Health;
        }


        public void Update(GameTime gameTime)
        {
            _StageManager2.Update(gameTime);
            if (!_StageManager2.GetAnimationState())
            {
                _link.Update(gameTime);
                _link2.Update(gameTime);
                _gameHUD2.Update();

                _mouseController2.Update();
            }

        }


    }
}