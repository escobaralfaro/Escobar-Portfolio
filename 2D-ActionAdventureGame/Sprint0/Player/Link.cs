using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Sprint0.Classes;
using System;
using System.Reflection.Metadata;
using static Sprint0.Player.ILinkState;
using static Sprint2.Classes.Iitem;
using Sprint2.GameStates;


namespace Sprint0.Player
{
    public class Link
    {

        public ILinkState currentState;
        public Rectangle[] _sourceRectangles;
        public Vector2 _position;
        public Vector2 _previousPosition;
        public Texture2D _texture;
        public Vector2 _scale;
        public Color _color;
        public float speed;
        public float boomerangSpeed;
        public int framesPerStep;
        public int framesPerSword;
        public int framesPerDamage;
        public int framesPerBoomerang;
        public int RemainingDamagedFrames;
        public Boolean Damaged;
        private int immunityDuration;
        private int remainingImmunityFrames;
        private bool isImmune;
        public bool transitioning;
        public bool hasKey;
        public bool hasBow;
        public bool hasAk;
        public bool hasPotion;
        public bool win;
        public bool hasMap;
        public bool isPaused;
        public float pauseTimer = 0f;
        public float pauseDuration = 2f;
        public bool hasCompass;
      
        public Link_Inventory inventory;
        public Direction currentDirection;
        int playerNumber = 1;


        private SpriteEffects spriteEffects;

        //for hud:
        public int Health { get; set; } = 16; // each heart = 2 hp

       // public int keyCount { get; set; } = 0; // start with 0 keys

       // public int GemCount { get; set; } = 0; // start with 0 gems

        public int BombCount; //start with 3 for now

        public SoundEffect SwordAttackSound { get; private set; }
        public SoundEffect bowAttackSound { get; private set; }
        public SoundEffect bombExplosion { get; private set; }
        public SoundEffect BoomerangSound { get; private set; }
        public SoundEffect ak47shootSound { get; private set; }

        private Vector2 BoomCoords;
        public BulletManager BulletManager { get; private set; }

        public int enemyDefeatedCount { get; private set; }
        public int itemCollectedCount { get; private set; }
        public bool isDungeonComplete { get; private set; }
        private bool isFirstBloodAchievementUnlockedbool = false;
        //public AchievementManager achievementManager { get; private set; }
        //public GameTime gameTime;


        public Link(Rectangle[] sourceRectangles, Texture2D texture, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 scale, ContentManager content, SoundEffect swordSound, SoundEffect bowSound, SoundEffect bombSound, SoundEffect boomerangSound, Link_Inventory _inventory, SoundEffect ak47Sound)
        {
            _sourceRectangles = sourceRectangles;
            
            //here is where u alter bullet speed or lifetime
            BulletManager = new BulletManager(texture, _sourceRectangles, Vector2.One, 5f, 3f);

            currentState = new LinkDown(this);

            _position = (playerNumber == 1)
                    ? new Vector2(468.0f, 500.0f)
                    : new Vector2(532.0f, 500.0f); _scale = scale;

            _texture = texture;
            speed = 5.0f;
            boomerangSpeed = 10.0f;
            spriteEffects = SpriteEffects.None;
            framesPerStep = 8;
            framesPerSword = 4;
            framesPerDamage = 50;
            framesPerBoomerang = 3;
            RemainingDamagedFrames = framesPerDamage;
            Damaged = false;
            _color = Color.White;
            _previousPosition = new Vector2(200.0f, 200.0f);
            immunityDuration = 50;
            remainingImmunityFrames = 0;
            transitioning = false;
            hasKey = false;
            hasBow = false;
            hasAk = false;
            win = false;
            hasMap = false;
            isPaused = false;
            hasCompass = false;
            if(_inventory == null)
            {
                inventory = new Link_Inventory(this, spriteBatch, scale, graphicsDevice, content);
                var startingBomb = new Boom(Vector2.Zero, this, null);
                startingBomb.LoadContent(content, "NES - The Legend of Zelda - Items & Weapons", graphicsDevice, ItemType.boom, scale);
                inventory.AddItem(startingBomb);
                inventory.InitializeStartingItems();
            }
            else
            {
                inventory = _inventory;
                _position = new Vector2(532.0f, 500.0f);
            }

            currentDirection = Direction.down;
            BoomCoords = new Vector2(0,0);
            
            SwordAttackSound = swordSound;
            bowAttackSound = bowSound;
            bombExplosion = bombSound;
            BoomerangSound = boomerangSound;
            ak47shootSound = ak47Sound;

        }

        public void MoveDown()
        {
            currentDirection = Direction.down;
            if (!transitioning)
                currentState.MoveDown();
        }

        public void MoveUp()
        {
            currentDirection = Direction.up;
            if (!transitioning)
            currentState.MoveUp();
        }

        public void MoveLeft()
        {
            currentDirection = Direction.left;
            if (!transitioning)
                currentState.MoveLeft();
        }

        public void MoveRight()
        {
            currentDirection = Direction.right;
            if (!transitioning)
                currentState.MoveRight();

        }

        public void SwordAttack()
        {
            currentState.UseSword();
        }

        public void ArrowAttack()
        {
            if (hasBow && inventory?.SelectedItem?.CurrentItemType == ItemType.bow)
            {
                currentState.UseArrow();
            }
        }
        public void ShootAk()
        {
            
                currentState.UseAk();
            
        }
        public void UseBoomerang()
        {
            currentState.UseBoomerang();
        }
        public void UseBomb()
        {
            if (BombCount > 0 && inventory?.SelectedItem?.CurrentItemType == ItemType.boom)
            {
                currentState.UseBomb();
            
                
            }
        }
        public void TakeDamage()
        {
            currentState.IsDamaged();

            if (!isImmune) 
            {
                Health = Math.Max(0, Health - 1);
                remainingImmunityFrames = immunityDuration;
                isImmune = true; 
            }

        }
        public void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);
            UpdateBullets(gameTime);       //update bullets globally


            BombCount = inventory.GetBombCount();
            //keyCount = inventory.GetKeyCount();
            if (isImmune)
            {
                remainingImmunityFrames--;

                if (remainingImmunityFrames <= 0)
                {
                    isImmune = false;
                }
            }
            if (inventory.GetKeyCount() <=0)
            {
                hasKey = false;
            } else { hasKey = true; }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            currentState.Draw(_spriteBatch);

            DrawBullets(_spriteBatch);       //draw bullets globally

        }

        public void DrawSprite(SpriteBatch _spriteBatch, int frame, Boolean flipped)
        {
            if (flipped)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }

            if (Damaged)
            {
                _color = Color.Red;
            }
            else
            {
                _color = Color.White;
            }
            _spriteBatch.Draw(_texture, _position, _sourceRectangles[frame], _color, 0f, Vector2.Zero, _scale, spriteEffects, 0f);
        }

        public void DrawWeapon(SpriteBatch _spriteBatch, int frame, Boolean flippedHorizontal, Boolean flippedVertical, Vector2 _weaponPosition)
        {
            if (flippedHorizontal)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (flippedVertical)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
            _spriteBatch.Draw(_texture, _weaponPosition, _sourceRectangles[frame], Color.White, 0f, Vector2.Zero, _scale, spriteEffects, 0f);


        }

        public void UpdateBullets(GameTime gameTime)
        {
            BulletManager?.Update(gameTime); //null check for safety
        }
        public void DrawBullets(SpriteBatch spriteBatch)
        {
            BulletManager?.Draw(spriteBatch);
        }
        public Vector2 GetLocation()
        {
            return _position;
        }

        public int GetKeyCount()
        {
            return inventory.GetKeyCount();
        }
        public int GetClockCount()
        {
            return inventory.GetClockCount();
        }

        public int GetPotionCount()
        {
            return inventory.GetPotionCount();
        }

        public bool HasKey()
        {
            return hasKey;
        }

        public void DecrementKey()
        {
           inventory.DecrementKeyCount();
        }

        public void SetExplosionCoords(Vector2 boomCoords)
        {
            BoomCoords = boomCoords;
        }

        public Vector2 GetBoomCoords()
        {
            return BoomCoords;
        }

        public void IncrementKey()
        {
            inventory.IncrementKeyCount();
        }

        public Link_Inventory GetInventory()
        {
            return inventory;
        }

        public bool CanUnlockDoor()
        {
            return hasKey && inventory?.SelectedItem?.CurrentItemType == ItemType.key;
        }

        public void DecrementBomb()
        {
            inventory.DecrementBombCount();
        }

        public void IncrementBomb()
        {
            inventory.IncrementBombCount();
        }

        public void IncrementGem()
        {
            inventory.IncrementGemCount();
        }

        public void DecrementGem(int deficit)
        {
            inventory.DecrementGemCount(deficit);
        }

        public int GetGemCount()
        {
            return inventory.GetGemCount();
        }

        public void DecrementClock()
        {
            inventory.DecrementClockCount();
        }

        public void IncrementClock()
        {
            inventory.IncrementClockCount();
        }

        public void DecrementPotion()
        {
            inventory.DecrementPotionCount();
        }

        public void IncrementPotion()
        {
            inventory.IncrementPotionCount();
        }

        public void IncrementEnemyDefeatedCount()
        {
            //Debug.WriteLine($"Link's position: {_link._position.X}, {_link._position.Y}");
            //Debug.WriteLine("Adding enemy count");
            enemyDefeatedCount++;
            //achievementManager.Update(gameTime);
        }

        public void IncrementItemCount()
        {
            itemCollectedCount++;
        }

        public void GameComplete()
        {
            isDungeonComplete = true;
        }
    }
}
