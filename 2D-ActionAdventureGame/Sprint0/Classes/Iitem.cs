using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2.Classes
{
    public interface Iitem
    {
        Texture2D Sprite { get; }  // Add these properties
        Rectangle[] SourceRectangles { get; }
        ItemType CurrentItemType { get; }
        public enum ItemType
        {
            fire,
            clock,
            heart, 
            health,
            compass,
            fairy,
            bow,
            boom,
            key,
            map,
            potion,
            triangle,
            diamond,
            ak47
            
        }
        void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, ItemType itemType, Vector2 scale);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
