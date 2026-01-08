using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint2.GameStates
{
    enum State
    {
        StartMenu,
        InGame,
        PauseMenu,
        GameOver
    }
    public interface IGameState
    {
        public void Update(GameTime gameTime);
        public void Draw();
        public void LoadContent(ContentManager Content);
        public int GetLinkHealth();
    }
    


}
