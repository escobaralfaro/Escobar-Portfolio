using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Sprint2.UI
{
    public class ChatBox
    {
        private Texture2D chatBoxTexture;
        private SpriteFont font;
        private Rectangle bounds;
        private string currentMessage;
        private bool isVisible;
        private Vector2 textPosition;
        private Vector2 _scale;
        private const int TEXT_PADDING_X = 3;
        private const int TEXT_PADDING_Y = 10;
        private const int MIN_WIDTH = 13;
        private Vector2? _referencePosition;


        // Conversation  
        private string[] conversationLines;
        private int currentLineIndex;
        private bool conversationComplete;
        private string finalMessage;

        public ChatBox(GraphicsDevice graphicsDevice, ContentManager content, Vector2 scale)
        {
            _scale = scale;
            chatBoxTexture = content.Load<Texture2D>("ChatBox");
            font = content.Load<SpriteFont>("Font");
            isVisible = false;
            conversationComplete = false;

            //  default bounds
            UpdateBounds("");
        }

        private void UpdateBounds(string message, Vector2? referencePosition = null)
        {
            // get text size
            Vector2 textSize = font.MeasureString(message);

            if (referencePosition.HasValue)
            {
                _referencePosition = referencePosition;
            }

            // calculate  width by text but ensure it's at least MIN_WIDTH
            int requiredWidth = (int)MathHelper.Max(
                MIN_WIDTH * _scale.X,
                (textSize.X/3) * _scale.X
            );

            int xPosition = referencePosition.HasValue
        ? (int)(referencePosition.Value.X - requiredWidth / 2)  // Center horizontally above Wizzrobe
        : (int)(50 * _scale.X);
            int yPosition = referencePosition.HasValue
        ? (int)(referencePosition.Value.Y - 30 * _scale.Y)  // Position above Wizzrobe
        : (int)(100 * _scale.Y);

            bounds = new Rectangle(
             xPosition,          // Dynamic X position
             yPosition,          // Dynamic Y position
             requiredWidth,      // Dynamic width change based on text
            (int)(25 * _scale.Y)// make height fixed
    );

            textPosition = new Vector2(
                bounds.X + TEXT_PADDING_X * _scale.X,
                bounds.Y + TEXT_PADDING_Y * _scale.Y
            );
        }

        public void StartConversation(string[] lines, string lastMessage, Vector2 referencePosition)
        {
            conversationLines = lines;
            currentLineIndex = 0;
            isVisible = true;
            conversationComplete = false;
            finalMessage = lastMessage;
            UpdateBounds(lines[0], referencePosition);
            currentMessage = lines[0];
            

        }

        public bool AdvanceConversation(Vector2? referencePosition = null)
        {
            

            if (!isVisible) return false;

            // check if conversation
            if (conversationLines == null)
            {
                // if not just hide message
                Hide();
                return false;
            }

            if (conversationComplete)
            {
                // Show final message repeatedly  
                currentMessage = finalMessage;
                UpdateBounds(finalMessage, referencePosition ?? _referencePosition);
                return true;
            }

            currentLineIndex++;
            if (currentLineIndex >= conversationLines.Length)
            {
                conversationComplete = true;
                currentMessage = finalMessage;
            }
            else
            {
                currentMessage = conversationLines[currentLineIndex];
            }

            UpdateBounds(currentMessage, referencePosition ?? _referencePosition);
            return true;
        }
        public void Show(string message)
        {
            //reset conversation state
            conversationLines = null;
            conversationComplete = false;
            currentLineIndex = 0;
            finalMessage = null;

            currentMessage = message;
            isVisible = true;
            UpdateBounds(message);
        }


        public void Hide()
        {
            isVisible = false;
            
            conversationLines = null;
            conversationComplete = false;
            currentLineIndex = 0;
            finalMessage = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible) return;

            // chat background
            spriteBatch.Draw(
                chatBoxTexture,
                bounds,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );

          

            // Draw text
            if (currentMessage != null)
            {
                spriteBatch.DrawString(
                    font,
                    currentMessage,
                    textPosition,
                    Color.Red  
                );
            }
        }
        public bool IsVisible => isVisible;
        public bool IsConversationComplete => conversationComplete;
        public void Update()
        {
            // later will update chatbox (more conversation)
        }
    }
}