using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;

namespace Sprint0.Interfaces
{
    public interface ISprite
    {
        void Update(GameTime gameTime, KeyboardController keyboardController);
        void Draw(SpriteBatch spriteBatch);
    }
}
