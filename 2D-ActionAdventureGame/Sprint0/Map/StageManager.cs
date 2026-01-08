using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprint0.Collisions;
using Sprint0.Player;
using System;
using System.Diagnostics;
using Sprint2.Classes;
using System.Reflection.Metadata.Ecma335;
using Sprint0.Classes;
using Sprint2.GameStates;


namespace Sprint2.Map
{
    public enum GameStage
    {
        StartMenu,
        Dungeon,
        PauseMenu,
        End
    }

    public class StageManager
    {
        public GameStage currentGameStage; // To track the current game stage
        public int StageIndex;
        public DrawDungeon _DrawDungeon;
        public Texture2D _texture;
        public SpriteBatch _spriteBatch;
        public Vector2 _scale;
        static GraphicsDevice _graphicsDevice;
        private DoorDecoder _doorDecoder;
        public NextStageDecider _nextStageDecider;
        DungeonMap _DungeonMap;
        DoorMap _DoorMap;
        Enemy_Item_Map _EnemyItem;
        ItemMap _ItemMap;
        Boolean StageAnimating;
        int AnimatingCount;
        private Link _link;
        private StageAnimator _StageAnimator;

        public static StageManager Instance { get; private set; }
        public Boolean drawHitboxes;

        // Start Menu
        public Texture2D titleScreen;
        public Texture2D pauseScreen;
        public Texture2D endScreen;
        public SpriteFont font;
        public float timer;
        public bool showText;
        Song backgroundMusic;
        Song titleSequence;
        Song endSequence;
        public MovableBlock movableBlock14;
        public MovableBlock movableBlock8;
        private BulletManager bulletManager;

        public AchievementManager achievementManager { get; private set; }
        public int enemydefeatedCount { get; private set; }
        public int itemCollectedCount { get; private set; }
        public bool isDungeonComplete { get; private set; }
        private float achievementUpdateCooldown = 10f; // 1 second cooldown  
        private float achievementUpdateTimer = 0f;
        private bool isFirstBloodAchievementUnlockedbool = false;


        public StageManager(Rectangle[] sourceRectangles, Texture2D texture, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Link link, ContentManager content, Vector2 scale)
        {

            currentGameStage = GameStage.StartMenu;
            StageAnimating = false;
            AnimatingCount = 0;
            StageIndex = 0;
            _texture = texture;
            _spriteBatch = spriteBatch;
            _link = link;
            _scale = scale;
            Instance = this;
            drawHitboxes = false;
            _graphicsDevice = graphicsDevice;
            _DungeonMap = new DungeonMap("../../../Map/DungeonMap2.csv");
            _DoorMap = new DoorMap("../../../Map/Dungeon_Doors.csv");
            _EnemyItem = new Enemy_Item_Map("../../../Map/EnemyItem_Map.csv", _scale, graphicsDevice, content, _link, null);
            _ItemMap = new ItemMap("../../../Map/ItemMap.csv", _scale, graphicsDevice, content, _link, null);

            _nextStageDecider = new NextStageDecider(link, _scale, _DoorMap, this);
            _DrawDungeon = new DrawDungeon(sourceRectangles, texture, spriteBatch, _scale, _link, _DungeonMap, _DoorMap, _EnemyItem, _ItemMap);
            //currentStage = new Stage1(this, _DungeonMap, _DoorMap, _link, drawDungeon);
            GameHUD gameHUD = new GameHUD(spriteBatch, graphicsDevice, content, link, scale, this);

            _DrawDungeon.SetHUDResources(gameHUD);

            Texture2D itemTexture = content.Load<Texture2D>("NES - The Legend of Zelda - Items & Weapons");
            _DrawDungeon.SetItemTexture(itemTexture);
            titleScreen = content.Load<Texture2D>("TitleScreen");
            pauseScreen = content.Load<Texture2D>("Pause");
            endScreen = content.Load<Texture2D>("EndingofZelda");
            font = content.Load<SpriteFont>("File");
            timer = 0f;
            showText = true;

            //Music
            titleSequence = content.Load<Song>("TitleTheme");
            backgroundMusic = content.Load<Song>("DungeonTheme");
            endSequence = content.Load<Song>("EndingTheme");

            _StageAnimator = new StageAnimator(_DungeonMap, _DoorMap, _scale, sourceRectangles, _texture, spriteBatch, _DrawDungeon);

            //MovableBlock movableblock14 = new MovableBlock(new Vector2(100, 100));
            Vector2 EasierAccessTilePosition14 = new Vector2(380, 540) + new Vector2(3, 3);
            movableBlock14 = new MovableBlock(_link._position, EasierAccessTilePosition14, 16, 16, 13, 13);
            movableBlock14.LoadContent(content, "DungeonSheet", new Rectangle(212, 323, 16, 16));
            if (texture == null)
            {
                Debug.WriteLine("Texture not loaded");
            }
            else
            {
                Debug.WriteLine("Texture loaded: " + texture.Width + "x" + texture.Height);
            }


            //MovableBlock movableblock8 = new MovableBlock(new Vector2(100, 100));
            Vector2 EasierAccessTilePosition8 = new Vector2(445, 540) + new Vector2(3, 3);
            movableBlock8 = new MovableBlock(_link._position, EasierAccessTilePosition8, 16, 16, 13, 13);
            movableBlock8.LoadContent(content, "DungeonSheet", new Rectangle(212, 323, 16, 16));

            achievementManager = new AchievementManager(_link, _scale);
            InitializeAchievements();
            //enemydefeatedCount = _link.enemyDefeatedCount;


        }
        public void Update(GameTime gameTime)
        {
            if (_link.isPaused)
            {
                _link.pauseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_link.pauseTimer <= 0f)
                {
                    _link.isPaused = false;
                }
                return;
            }
            if (StageAnimating)
            {
                AnimatingCount -= 2;
                _StageAnimator.Update();
            }
            if (AnimatingCount <= 0)
            {
                StageAnimating = false;
                _DoorMap.SpecialDoorLogic(StageIndex);
            }

            if (!StageAnimating)
            {
                _nextStageDecider.Update(StageIndex);
                _DrawDungeon.Update(StageIndex);
                _EnemyItem.Update(StageIndex, gameTime);
                _ItemMap.Update(StageIndex, gameTime);
                LinkEnemyCollision.HandleCollisions(_link, _EnemyItem, StageIndex, _link._scale, _link.BulletManager);
            }
            if (StageIndex == 0)
            {
                Boolean enemiesPresent = _EnemyItem.AreThereEnemies(StageIndex);
                _DoorMap.AllEnemiesDead(StageIndex, enemiesPresent);
            }
            if (StageIndex == 3)
            {
                if (_EnemyItem.AreThereEnemies(StageIndex))
                {

                    _ItemMap.SpawnKey(StageIndex);
                }
            }

            if (StageIndex == 5)
            {
                Vector2 BoomCoords = _link.GetBoomCoords();
                if (BoomCoords.X > 115 * _scale.X && BoomCoords.X < 150 * _scale.X)
                {
                    if (BoomCoords.Y < 125 * _scale.Y && BoomCoords.Y > 75 * _scale.Y)
                    {
                        _DoorMap.BoomLogic(5);
                    }

                }
            }
            if (StageIndex == 7)
            {
                Vector2 BoomCoords = _link.GetBoomCoords();
                if (BoomCoords.X > 115 * _scale.X && BoomCoords.X < 150 * _scale.X)
                {
                    if (BoomCoords.Y < 125 * _scale.Y && BoomCoords.Y > 75 * _scale.Y)
                    {
                        _DoorMap.BoomLogic(7);
                    }

                }
            }
            // stage 14 and 8 have the movable block
            if (StageIndex == 14)
            {
                if (movableBlock14 != null)
                {
                    movableBlock14.Update(ref _link._position, _scale);
                }

            }

            if (StageIndex == 8)
            {
                if (movableBlock8 != null)
                {
                    movableBlock8.Update(ref _link._position, _scale); 
                }
            }

            if (StageIndex == 16)
            {
                if (_EnemyItem.AreThereEnemies(16))
                {
                    Debug.WriteLine("Dragon slane");
                    _DoorMap.SpecialDoorLogic(StageIndex);
                }
            }

            if (StageIndex == 17)
            {
                _link.GameComplete();
            }

            if (MediaPlayer.State == MediaState.Playing && MediaPlayer.Queue.ActiveSong == titleSequence)
            {
                MediaPlayer.Stop();
            }

            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(backgroundMusic);
                MediaPlayer.IsRepeating = true; // loop the music
            }

            //achievementUpdateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (achievementUpdateTimer >= achievementUpdateCooldown)
            //{
            //    achievementUpdateTimer -= achievementUpdateCooldown;
            //    //Debug.WriteLine("Updating achievements...");
            //    achievementManager.Update(gameTime);
            //}

            achievementManager.Update(gameTime);

            _link.SetExplosionCoords(Vector2.Zero);
        }
        public void switchHitbox()
        {
            drawHitboxes = !drawHitboxes;
            Debug.WriteLine($"drawHitboxes: {drawHitboxes}");
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

        public void NextStage()
        {

            StageIndex = _nextStageDecider.DecideStage();
        }
        public void Draw()
        {
            if (!StageAnimating)
            {
                _DrawDungeon.Draw(Vector2.Zero, false, StageIndex);

                // Draw the achievements
                achievementManager.Draw(_spriteBatch, font, _graphicsDevice);
                if (drawHitboxes)
                {
                    DebugDraw.DrawHitboxes(_spriteBatch, _link, _EnemyItem, StageIndex, _scale, _link.BulletManager);
                }

                if (StageIndex == 14)
                {
                    if (movableBlock14 != null)
                    {
                        Vector2 scaling = new Vector2(4f, 4f);
                        movableBlock14.Draw(_spriteBatch, movableBlock14.blockPosition, scaling);
                    }
                }

                if (StageIndex == 8)
                {
                    if (movableBlock8 != null)
                    {
                        Vector2 scaling = new Vector2(4f, 4f);
                        movableBlock8.Draw(_spriteBatch, movableBlock8.blockPosition, scaling);
                        _spriteBatch.Draw(new Texture2D(_graphicsDevice, 1, 1), new Rectangle((int)movableBlock8.blockPosition.X, (int)movableBlock8.blockPosition.Y, 50, 50), Color.Red);
                    }
                }

            }
            else
            {
                _StageAnimator.Draw();
            }
        }
        public void DrawEnd()
        {
            Vector2 position = new Vector2(100, 240);
            Vector2 scale = new Vector2(0.8f, 0.8f);
            _spriteBatch.Draw(endScreen, position, null, Color.White, 0f, Vector2.Zero, scale, 0, 0f);
        }
        public void Animate(int currentStage, int nextStage, int direction)
        {
            StageAnimating = true;
            if (direction <= 2)
            {
                AnimatingCount = 255;
            }
            else
            {
                AnimatingCount = 176;
            }

            _StageAnimator.Animate(currentStage, nextStage, direction);
        }

        public Boolean GetAnimationState()
        {
            return StageAnimating;
        }

        public int GetCurrentStage()
        {
            return StageIndex;
        }
        public bool IsFirstBloodAchievementUnlocked()
        {
            //Debug.WriteLine($"Link's position: {_link._position.X}, {_link._position.Y}");
            enemydefeatedCount = _link.enemyDefeatedCount;
            if (enemydefeatedCount > 0 && !isFirstBloodAchievementUnlockedbool)
            {
                isFirstBloodAchievementUnlockedbool = true;
                return true;
            }
            return false;
        }

        public void InitializeAchievements()
        {
            try
            {
                achievementManager.AddAchievement(new Achievement(
                    "First Blood",
                    "Defeat your first enemy.",
                    () => IsFirstBloodAchievementUnlocked()
                ));

                achievementManager.AddAchievement(new Achievement(
                    "Slayer",
                    "Defeat 10 enemies.",
                    () => _link.enemyDefeatedCount >= 10
                ));

                achievementManager.AddAchievement(new Achievement(
                    "Treasure Hunter",
                    "Collect 5 items.",
                    () => _link.itemCollectedCount >= 5
                ));
                achievementManager.AddAchievement(new Achievement(
                    "Treasure Collector",
                    "Collect 10 items.",
                    () => _link.itemCollectedCount >= 10
                ));

                achievementManager.AddAchievement(new Achievement(
                    "Dungeon Master",
                    "Complete the dungeon.",
                    () => _link.isDungeonComplete
                ));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error initializing achievements: " + ex.Message);
            }
        }
    
    }
}