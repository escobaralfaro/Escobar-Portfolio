using Microsoft.Xna.Framework.Input;
using Sprint2.Map;

namespace Sprint0.Classes
{
    public class MouseController
    {
        private MouseState previousState;
    
        private StageManager _StageManager;
        public MouseController(StageManager stageManager)
        {
     
            _StageManager = stageManager;
        }

        public void Update()
        {
            MouseState currentState = Mouse.GetState();

            if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
            {
                _StageManager.NextStage();
            }

            previousState = currentState;
        }
    }

}
