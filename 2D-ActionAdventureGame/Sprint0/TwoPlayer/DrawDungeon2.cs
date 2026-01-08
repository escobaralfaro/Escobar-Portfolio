using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using Sprint0.Player;
using Sprint2.Classes;
using Sprint2.Collisions;
using Sprint2.Enemy;
using Sprint2.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sprint2.Classes.Iitem;

namespace Sprint2.TwoPlayer
{
    public class DrawDungeon2
    {
        private Link _link;
        private Link _link2;
        private Rectangle[] _sourceRectangles;
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;
        private Vector2 _scale;
        private int stage;
        private DoorDecoder _doorDecoder;
        private DoorMap _doorMap;
        private DungeonMap _dungeonMap;
        private SpriteEffects _spriteEffects;
        private Enemy_Item_Map _EnemyItem;
        private ItemMap _itemMap;
        private Rectangle[] _cutOuts;
        private Texture2D _hudTexture;
        private Texture2D _itemTexture;
        private const int Spacing = 8;
        public DrawDungeon2(Rectangle[] sourceRectangles, Texture2D texture, SpriteBatch spriteBatch, Vector2 scale, Link link, Link link2, DungeonMap dungeon, DoorMap doorMap, Enemy_Item_Map enemy_Item_Map, ItemMap itemMap)
        {
            _sourceRectangles = sourceRectangles;
            
            _texture = texture;
           // _texture.Filter = TextureFilter.Linear;
            _spriteBatch = spriteBatch;
            _scale = scale;
            stage = 0;
            _doorDecoder = new DoorDecoder();
            _spriteEffects = SpriteEffects.None;
            _link = link;
            _link2 = link2;
            _doorMap = doorMap;
            _dungeonMap = dungeon;
            _EnemyItem = enemy_Item_Map;
            _itemMap = itemMap;
        }
        

        public void Update(int currentStage)
        {
            stage = currentStage;
        }

        private int[] GetDoor(int currentStage)
        {
            return _doorMap.GetDoors(currentStage);
        }

        private int[,] GetStage(int currentStage)
        {
            return _dungeonMap.GetRoom(currentStage);
        }
        private List<IEnemy> GetEnemies(int currentStage)
        {
            return _EnemyItem.GetEnemies(currentStage);
        }

        private List<Iitem> Getitems(int currentStage)
        {
            return _itemMap.GetItems(currentStage);
        }

        public void Draw(Vector2 Offset, bool Transitioning, int currentStage)
        {
            DrawWalls(Offset);
            if (currentStage != 9)
            {
                int[,] tiles = GetStage(currentStage);
                if (currentStage == 12)
                {
                    tiles[0, 0] = 3;
                }
                DrawTiles(tiles, Offset, Transitioning);
            }
            else
            {
                DrawShopItems(Offset);
            }
            DrawDoors(GetDoor(currentStage), Offset);

            if (!Transitioning)
            {
                _link.transitioning = false;
                _link2.transitioning = false;
                DrawEnemies(GetEnemies(currentStage));
                DrawItems(Getitems(currentStage));

            }
        }
        public void DrawWalls(Vector2 Offset)
        {
            _spriteBatch.Draw(_texture, new Vector2(0 + Offset.X, 55.0f * _scale.Y + Offset.Y), _sourceRectangles[6], Color.White, 0f, Vector2.Zero, _scale, _spriteEffects, 0f);
            _spriteBatch.Draw(_texture, new Vector2(0 + Offset.X, 87.0f * _scale.Y + Offset.Y), _sourceRectangles[7], Color.White, 0f, Vector2.Zero, _scale, _spriteEffects, 0f);
            _spriteBatch.Draw(_texture, new Vector2(0 + Offset.X, 198.0f * _scale.Y + Offset.Y), _sourceRectangles[8], Color.White, 0f, Vector2.Zero, _scale, _spriteEffects, 0f);
            _spriteBatch.Draw(_texture, new Vector2(224.0f * _scale.X + Offset.X, 87.0f * _scale.Y + Offset.Y), _sourceRectangles[9], Color.White, 0f, Vector2.Zero, _scale, _spriteEffects, 0f);

        }

        public void DrawDoors(int[] doorCodes, Vector2 Offset)
        {
            Vector2 doorPosition = new Vector2(0, 0);
            for (int i = 0; i < 4; i++)
            {
                int doorIdx = _doorDecoder.DecodeDoor(i, doorCodes[i]);
                switch (i)
                {
                    case 0:
                        doorPosition.X = 112 * _scale.X + Offset.X;
                        doorPosition.Y = 55 * _scale.Y + Offset.Y;
                        break;
                    case 1:
                        doorPosition.X = 0 + Offset.X;
                        doorPosition.Y = 127 * _scale.Y + Offset.Y;
                        break;
                    case 2:
                        doorPosition.X = 224 * _scale.X + Offset.X;
                        doorPosition.Y = 127 * _scale.Y + Offset.Y;
                        break;
                    case 3:
                        doorPosition.X = 112 * _scale.X + Offset.X;
                        doorPosition.Y = 198 * _scale.Y + Offset.Y;
                        break;
                    default: break;

                }
                _spriteBatch.Draw(_texture, doorPosition, _sourceRectangles[doorIdx], Color.White, 0f, Vector2.Zero, _scale, _spriteEffects, 0f);

            }
        }
        public void DrawTiles(int[,] room, Vector2 Offset, bool Transitioning)
        {
            Vector2 tilePosition = new Vector2(32 * _scale.X + Offset.X, 87 * _scale.Y + Offset.Y);
            List<IEnemy> enemiesInRoom = _EnemyItem.GetEnemies(stage);
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    int tileIdx = room[i, j];

                    _spriteBatch.Draw(_texture, tilePosition, _sourceRectangles[tileIdx], Color.White, 0f, Vector2.Zero, _scale, _spriteEffects, 0f);
                    if (!Transitioning)
                    {
                        if (tileIdx == 1 || tileIdx == 2 || tileIdx == 3)
                        {
                            Vector2 EasierAccessTilePosition = tilePosition + new Vector2(3, 3);
                            HandlePlayerBlockCollision playerBlockCollision = new HandlePlayerBlockCollision(_link._position, EasierAccessTilePosition, 16, 16, 13, 13);
                            playerBlockCollision.PlayerBlockCollision(ref _link._position, _link._previousPosition, _scale);
                            HandlePlayerBlockCollision player2BlockCollision = new HandlePlayerBlockCollision(_link2._position, EasierAccessTilePosition, 16, 16, 13, 13);
                            player2BlockCollision.PlayerBlockCollision(ref _link2._position, _link2._previousPosition, _scale);

                            foreach (IEnemy enemy in enemiesInRoom)
                            {
                                HandleEnemyBlockCollision enemyBlockCollision = new HandleEnemyBlockCollision(EasierAccessTilePosition, 16, 16, 13, 13);
                                enemyBlockCollision.EnemyBlockCollision(_EnemyItem, stage, _scale);
                            }
                        }
                    }
                    tilePosition.X += 16 * _scale.X;
                }
                tilePosition.X = 32 * _scale.X + Offset.X;
                tilePosition.Y += 16 * _scale.Y;
            }
        }

        public void DrawEnemies(List<IEnemy> enemies)
        {
            foreach (IEnemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }
        }
        public void DrawItems(List<Iitem> items)
        {
            foreach (Iitem item in items)
            {
                item.Draw(_spriteBatch);
            }
        }
        public void SetHUDResources(GameHUD2 gameHUD2)
        {
            _cutOuts = gameHUD2.GetCutOuts();
            _hudTexture = gameHUD2.HUDTexture;
        }

        public void SetItemTexture(Texture2D itemTexture)
        {
            _itemTexture = itemTexture;
        }

        private void DrawShopItems(Vector2 Offset)
        {
            List<Iitem> items = Getitems(stage);
            Clock clockItem = (Clock)items.FirstOrDefault(item => item.CurrentItemType == ItemType.clock);
            Ak47 ak47Item = (Ak47)items.FirstOrDefault(item => item.CurrentItemType == ItemType.ak47);

            // Draw Clock price display number "X 2"
            if (clockItem != null && !clockItem.isPurchased)
            {
                // First item position
                Vector2 gemPosition = new Vector2(53 * _scale.X + Offset.X, 172 * _scale.Y + Offset.Y);
                DrawPriceDisplay(gemPosition, Offset, "2");
            }

            // Draw AK47 price display number "X 5"
            if (ak47Item != null && !ak47Item.isPurchased)
            {
                // Second item position  
                Vector2 gemPosition = new Vector2(100 * _scale.X + Offset.X, 172 * _scale.Y + Offset.Y);
                DrawPriceDisplay(gemPosition, Offset, "5");
            }
        }

        private void DrawPriceDisplay(Vector2 gemPosition, Vector2 Offset, string price)
        {
            // Draw Diamond (GEM) this is just display of Diamond that you will not able to pick it up
            Rectangle diamondRect = SpriteSheetHelper.CreateDiamondItemFrames()[0];
            _spriteBatch.Draw(_itemTexture, gemPosition, diamondRect, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);

            // vertical center ( X number should display in the middle of the diamond)
            float verticalCenter = gemPosition.Y + (16 - 8) / 2 * _scale.Y;
            Vector2 xPosition = new Vector2(gemPosition.X + 12 * _scale.X, verticalCenter);

            // Draw X
            _spriteBatch.Draw(
                _hudTexture,
                new Rectangle(
                    (int)xPosition.X,
                    (int)xPosition.Y,
                    (int)(8 * _scale.X),
                    (int)(8 * _scale.Y)
                ),
                _cutOuts[4],
                Color.White
            );

            // Draw price number
            for (int i = 0; i < price.Length; i++)
            {
                int digit = int.Parse(price[i].ToString());
                Rectangle digitSource = _cutOuts[digit + 5];

                float xDigitPos = xPosition.X + (16 * _scale.X) + (i * Spacing * _scale.X);
                float yDigitPos = verticalCenter;

                _spriteBatch.Draw(
                    _hudTexture,
                    new Rectangle(
                        (int)xDigitPos,
                        (int)yDigitPos,
                        (int)(8 * _scale.X),
                        (int)(8 * _scale.Y)
                    ),
                    digitSource,
                    Color.White
                );
            }
        }

    }
}