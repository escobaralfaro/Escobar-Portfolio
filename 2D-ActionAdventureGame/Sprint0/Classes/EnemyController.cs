//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace Sprint0.Classes
//{
    
//    public class EnemyController
//    {
//        private Enemy enemy;
//        public EnemyController(Enemy enemy)
//        {
//            this.enemy = enemy;
//        }

//        // Update the enemy's state based on input and game logic
//        public void Update(GameTime gameTime)
//        {
//            HandleInput();
//            enemy.Update(gameTime); // Update enemy logic (animation, movement, etc.)
//        }

//        // Handle keyboard input for controlling the enemy's direction
//        private void HandleInput()
//        {
//            KeyboardState state = Keyboard.GetState();

//            // Change enemy direction based on key inputs
//            if (state.IsKeyDown(Keys.Left))
//            {
//                enemy.ChangeDirection(Enemy.Direction.Left);
//            }
//            else if (state.IsKeyDown(Keys.Right))
//            {
//                enemy.ChangeDirection(Enemy.Direction.Right);
//            }
//            else if (state.IsKeyDown(Keys.Up))
//            {
//                enemy.ChangeDirection(Enemy.Direction.Up);
//            }
//            else if (state.IsKeyDown(Keys.Down))
//            {
//                enemy.ChangeDirection(Enemy.Direction.Down);
//            }
//        }

//        // Draw the enemy
//        public void Draw(SpriteBatch spriteBatch)
//        {
//            enemy.Draw(spriteBatch);
//        }
//    }
//}
