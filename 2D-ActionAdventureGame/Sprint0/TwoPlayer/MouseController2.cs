using Microsoft.Xna.Framework.Input;
using Sprint2.Map;
using Sprint2.TwoPlayer;

namespace Sprint0.Classes
{
    public class MouseController2
    {
        private MouseState previousState;

        private StageManager2 _StageManager2;
        public MouseController2(StageManager2 stageManager2)
        {

            _StageManager2 = stageManager2;
        }

        public void Update()
        {
            MouseState currentState = Mouse.GetState();

            if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
            {
                _StageManager2.NextStage();
            }

            previousState = currentState;
        }
    }

}
