
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Sprint0.Player
{
    public interface ILinkState
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);

        void MoveUp();
        void MoveDown();
        void MoveLeft();
        void MoveRight();
        void UseSword();
        void UseArrow();
        void UseBoomerang();
        void UseBomb();
        void UseAk();


        void IsDamaged();

        public enum Direction
        {
            up,
            down,
            left,
            right
        }
    }
}
