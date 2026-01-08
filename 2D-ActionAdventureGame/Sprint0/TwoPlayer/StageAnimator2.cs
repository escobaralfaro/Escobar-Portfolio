using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Map;

namespace Sprint2.TwoPlayer
{


    public class StageAnimator2
    {
        private DungeonMap _dungeonMap;
        private DoorMap _doorMap;
        private DoorDecoder _doorDecoder;
        private Vector2 _scale;
        private Rectangle[] _sourceRectangles;
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;
        private SpriteEffects _spriteEffects;
        private DrawDungeon2 _drawDungeon2;
        private int[,] currentTiles;
        private int[,] nextTiles;
        private int[] currentDoors;
        private int[] nextDoors;
        Vector2 Offset1;
        Vector2 Offset2;
        int _currentStage;
        int _nextStage;
        int direction;

        public StageAnimator2(DungeonMap dungeonMap, DoorMap doorMap, Vector2 scale, Rectangle[] sourceRectangles, Texture2D texture, SpriteBatch spriteBatch, DrawDungeon2 drawDungeon2)
        {
            _dungeonMap = dungeonMap;
            _doorMap = doorMap;
            _scale = scale;
            _sourceRectangles = sourceRectangles;
            _texture = texture;
            _spriteBatch = spriteBatch;
            _drawDungeon2 = drawDungeon2;
            _spriteEffects = SpriteEffects.None;
            Offset1 = Vector2.Zero;
            Offset2 = Vector2.Zero;
            _doorDecoder = new DoorDecoder();
            _currentStage = 0;
            _nextStage = 0;
            direction = 0;
        }

        public void Animate(int currentStage, int nextStage, int Direction)
        {
            Offset1 = Vector2.Zero;
            Offset2 = Vector2.Zero;
            _currentStage = currentStage;
            _nextStage = nextStage;
            direction = Direction;
            switch (direction)
            {
                case 1:
                    Offset1.X = 0;
                    Offset2.X = 255 * _scale.X;
                    break;
                case 2:
                    Offset1.X = 0;
                    Offset2.X = -(255 * _scale.X);
                    break;
                case 3:
                    Offset1.Y = 0;
                    Offset2.Y = -(175 * _scale.Y);
                    break;
                case 4:
                    Offset1.Y = 0;
                    Offset2.Y = 175 * _scale.Y;
                    break;
                default:
                    break;
            }


            // TO DO
            // refactor draw dungeon so that it takes an offset value? and a boolean value so that you know if its animating
        }

        public void Update()
        {
            switch (direction)
            {
                case 1:
                    Offset1.X -= 2 * _scale.X;
                    Offset2.X -= 2 * _scale.X;
                    break;
                case 2:
                    Offset1.X += 2 * _scale.X;
                    Offset2.X += 2 * _scale.X;
                    break;
                case 3:
                    Offset1.Y += 2 * _scale.Y;
                    Offset2.Y += 2 * _scale.Y;
                    break;
                case 4:
                    Offset1.Y -= 2 * _scale.Y;
                    Offset2.Y -= 2 * _scale.Y;
                    break;
                default:
                    break;
            }

        }

        public void Draw()
        {

            _drawDungeon2.Draw(Offset1, true, _currentStage);
            _drawDungeon2.Draw(Offset2, true, _nextStage);

        }


    }
}