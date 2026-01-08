using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprint0.Player;
using Sprint2.Classes;
using System;
namespace Sprint2.GameStates
{
    public class InventoryMenu : IGameState
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;
        private Texture2D inventoryScreen;
        private Vector2 _scale;
        private Vector2 _position;
        private bool _isTransitioning;
        private float _transitionSpeed;
        private float _targetY;
        private GameHUD _gameHUD;
        private bool _isOpen;  
        private Link _link; 

       
        public InventoryMenu(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content, GameHUD gameHUD, Link link)
        {
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
            _gameHUD = gameHUD;
            _link = link;
            inventoryScreen = content.Load<Texture2D>("NES - The Legend of Zelda - HUD & Pause Screen");
            _scale = new Vector2(4.2f, 5f);

            _position = new Vector2(0, -500);
            _targetY = -500;  // Start with closed position
            _transitionSpeed = 4f;
            _isTransitioning = false;  // Don't start transitioning immediately
            _isOpen = false;

        }

        public void StartTransitionIn()
        {
            if (!_isOpen)  // Only transition in if we're not already open
            {
                _position.Y = -500;  // Force to start position
                _targetY = 0;        // Set target to open position
                _isTransitioning = true;
                _isOpen = true;
            }
        }

        public void StartTransitionOut()
        {
            if (_isOpen)  // Only transition out if we're currently open
            {
                _targetY = -500;     // Set target to closed position
                _isTransitioning = true;
                _isOpen = false;
            }
        }



        public void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }

            if (_isTransitioning)
            {
                float lerpAmount = MathHelper.Clamp(_transitionSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f, 0.1f);
                float newY = MathHelper.Lerp(_position.Y, _targetY, lerpAmount);

                if (Math.Abs(newY - _targetY) > 0.5f)
                {
                    _position.Y = newY;
                    // Update HUD position
                    _gameHUD.SetPosition(_position);
                }
                else
                {
                    // We've reached the target
                    _position.Y = _targetY;
                    _gameHUD.SetPosition(_position);
                    _isTransitioning = false;
                }
            }
        }
        public bool IsTransitionComplete()
        {
            return !_isTransitioning;
        }

        public void Reset()
        {
            _position.Y = -500;
            _targetY = -500;
            _isTransitioning = false;
            _isOpen = false;
        }
        public void Draw()
        {
            // inventory background
            Rectangle inventorySource = new Rectangle(1, 11, 256, 88);
            _spriteBatch.Draw(inventoryScreen, _position, inventorySource, Color.White,
                             0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);




            //   inventory items
            if (_link?.inventory != null)
            {
                _link.inventory.Draw(_spriteBatch, _position);
            }

            DrawCurrentItem();
            // Draw HUD at bottom of inventory
            Vector2 hudPosition = new Vector2(_position.X, _position.Y + (88 * _scale.Y));
            _gameHUD.SetPosition(hudPosition);
            _gameHUD.Draw();
        }
        
        private void DrawCurrentItem()
        {
             
            Vector2 currentItemPosition = new Vector2(280, 230);  

            if (_link?.inventory?.SelectedItem != null)
            {
                Iitem currentItem = _link.inventory.SelectedItem;
                _spriteBatch.Draw(
                    currentItem.Sprite,
                    new Vector2(
                        _position.X + currentItemPosition.X,
                        _position.Y + currentItemPosition.Y
                    ),
                    currentItem.SourceRectangles[0],  
                    Color.White,
                    0f,
                    Vector2.Zero,
                    _scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public int GetLinkHealth()
        {
            return 1;
        }



        public void LoadContent(ContentManager Content)
        {
        }
    }
}