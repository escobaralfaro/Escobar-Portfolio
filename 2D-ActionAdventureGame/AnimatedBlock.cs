//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Sprint0.Classes;
//using Sprint0.Interfaces;

//namespace Sprint0;

//public class AnimatedBlock
//{
//    private Texture2D blocks;
//    private Vector2 position;
//    public Rectangle[] SourceRectangles;
//    private int currentBlock;
//    private float scale;


//    public AnimatedBlock(Vector2 startPosition)
//    {
//        position = startPosition;
//        scale = 4.0f;
       
//        currentBlock = 0;
//    }

//    public void LoadContent(ContentManager content, string texturePath)
//    {
//        blocks = content.Load<Texture2D>(texturePath);
//        SourceRectangles = SpriteSheetHelper.CreateBlockFrames(); // Get frames from helper
//    }

//    public void Update(GameTime gameTime, KeyboardController keyboardController)
//    {

//        if (keyboardController.previousBlock)
//        {
//            currentBlock = (currentBlock - 1 + SourceRectangles.Length) % SourceRectangles.Length;
//        }

//        if (keyboardController.nextBlock)
//        {
//            currentBlock = (currentBlock + 1) % SourceRectangles.Length;
//        }
//    }

//    public void Draw(SpriteBatch spriteBatch)
//    {
//        // Draw the current block at the specified position
//        spriteBatch.Draw(blocks, position, SourceRectangles[currentBlock], Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
//    }
//}