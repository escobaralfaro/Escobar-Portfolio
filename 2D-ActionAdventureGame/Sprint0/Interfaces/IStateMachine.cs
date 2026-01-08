using Microsoft.Xna.Framework;

namespace Sprint0.Interfaces
{
    public interface IStateMachine {
        void Update(GameTime gameTime);
        //void ChangeState(State newState); //idk how to get this to see my enum in LinkStateMachine
    }

}
