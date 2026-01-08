using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint2.Enemy
{
    public interface IEnemy
    {
        Vector2 Position { get; set; }
        int Width { get; }
        int Height { get; }
        void LoadContent(ContentManager content, string texturePath, GraphicsDevice graphicsdevice, Vector2 scale);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void TakeDamage();

        Boolean GetState();
       
        void Reset();

    }
}
