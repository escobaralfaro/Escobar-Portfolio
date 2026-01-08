using Microsoft.Xna.Framework;
using Sprint0.Player;
using Sprint2.Enemy;
using Sprint2.GameStates;
using Sprint2.Map;
using Sprint2.TwoPlayer;
using System.Diagnostics;
using static Sprint2.Classes.Iitem;

namespace Sprint2.Map
{
    public class NextStageDecider
    {
        private Link _link;
        private Vector2 _scale;
        private int _stage;
        private DoorMap _doorMap;
        private StageManager _stageManager;

        private Rectangle Door1;
        private Rectangle Door2;
        private Rectangle Door3;
        private Rectangle Door4;
        public NextStageDecider(Link link, Vector2 scale, DoorMap doorMap, StageManager stageManager)
        {
            _link = link;
   
            _scale = scale;
            _doorMap = doorMap;
            _stageManager = stageManager;

            Door1 = GetDoorRectangle(112, 87, 32, 16, _scale);
            Door2 = GetDoorRectangle(32, 127, 16, 32, _scale);
            Door3 = GetDoorRectangle(208, 127, 16, 32, _scale);
            Door4 = GetDoorRectangle(112, 182, 32, 16, _scale);
        }
        public void Update(int stage)
        {
            _stage = stage;
        }



        public int DecideStage()
        {
            int[] doors = _doorMap.GetDoors(_stage);
            bool canUnlockDoor = _link.HasKey() && _link.inventory?.SelectedItem?.CurrentItemType == ItemType.key;
                    Rectangle playerBoundingBox = GetScaledRectangle((int)_link.GetLocation().X, (int)_link.GetLocation().Y, 16, 16, _link._scale);

            if (playerBoundingBox.Intersects(Door1))
            {
                if (canUnlockDoor && doors[0] == 2)
                {
                    _link.DecrementKey();
                    _doorMap.KeyLogic(_stage);
                }
                else if (doors[0] == 1 || doors[0] == 4)
                {
                    Debug.WriteLine("worked");
                    _link._position.X = 112 * _scale.X;
                    _link._position.Y = 180 * _scale.Y;
                    _link.transitioning = true;
                    switch (_stage)
                    {
                        case 0:
                            _stageManager.Animate(0, 1, 3);
                            return 1;
                        case 1:
                            _stageManager.Animate(1, 4, 3);
                            return 4;
                        case 4:
                            _stageManager.Animate(4, 5, 3);
                            return 5;
                        case 5:
                            _stageManager.Animate(5, 10, 3);
                            return 10;
                        case 6:
                            _stageManager.Animate(6, 8, 3);
                            return 8;
                        case 7:
                            _stageManager.Animate(7, 11, 3);
                            return 11;
                        case 10:
                            _stageManager.Animate(_stage, 12, 3);
                            return 12;
                        case 12:
                            _stageManager.Animate(_stage, 13, 3);
                            return 13;
                        case 15:
                            _stageManager.Animate(_stage, 16, 3);
                            return 16;
                        default:
                            break;
                    }

                }
            }
            else if (playerBoundingBox.Intersects(Door4))
            {
                if (canUnlockDoor && doors[3] == 2)
                {
                    _link.DecrementKey();
                    _doorMap.KeyLogic(_stage);
                }
                else if (doors[3] == 1 || doors[3] == 4)
                {
                    _link._position.X = 120 * _scale.X;
                    _link._position.Y = 87 * _scale.Y;
                    _link.transitioning = true;
                    switch (_stage)
                    {
                        case 4:
                            _stageManager.Animate(4, 1, 4);
                            return 1;
                        case 5:
                            _stageManager.Animate(5, 4, 4);
                            return 4;
                        case 8:
                            _stageManager.Animate(8, 6, 4);
                            return 6;
                        case 10:
                            _stageManager.Animate(10, 5, 4);
                            return 5;
                        case 11:
                            _stageManager.Animate(11, 7, 4);
                            return 7;
                        case 12:
                            _stageManager.Animate(12, 10, 4);
                            return 10;
                        case 13:
                            _stageManager.Animate(13, 12, 4);
                            return 12;
                        case 16:
                            _stageManager.Animate(16, 15, 4);
                            return 15;
                        default:
                            break;
                    }
                }
            }
            else if (playerBoundingBox.Intersects(Door2))
            {
                if (canUnlockDoor && doors[1] == 2)
                {
                    _link.DecrementKey();
                    _doorMap.KeyLogic(_stage);
                }
                else if (doors[1] == 1 || doors[1] == 4)
                {
                    _link._position.X = 210 * _scale.X;
                    _link._position.Y = 135 * _scale.Y;
                    _link.transitioning = true;
                    switch (_stage)
                    {
                        case 1:
                            _stageManager.Animate(1, 2, 2);
                            return 2;
                        case 3:
                            _stageManager.Animate(3, 1, 2);
                            return 1;
                        case 5:
                            _stageManager.Animate(5, 6, 2);
                            return 6;
                        case 7:
                            _stageManager.Animate(7, 5, 2);
                            return 5;
                        case 8:
                            _stageManager.Animate(8, 9, 2);
                            return 9;
                        case 10:
                            _stageManager.Animate(_stage, 11, 2);
                            return 8;
                        case 11:
                            _stageManager.Animate(_stage, 10, 2);
                            return 10;
                        case 13:
                            _stageManager.Animate(_stage, 14, 2);
                            return 14;
                        case 15:
                            _stageManager.Animate(_stage, 11, 2);
                            return 11;
                        case 17:
                            _stageManager.Animate(_stage, 16, 2);
                            return 16;
                        default:
                            break;
                    }
                }
            }
            else if (playerBoundingBox.Intersects(Door3))
            {
                if (canUnlockDoor && doors[2] == 2)
                {
                    _link.DecrementKey();
                    _doorMap.KeyLogic(_stage);
                }
                else if (doors[2] == 1 || doors[2] == 4)
                    {
                        _link._position.X = 32 * _scale.X;
                        _link._position.Y = 135 * _scale.Y;
                        _link.transitioning = true;
                        switch (_stage)
                        {
                            case 2:
                                _stageManager.Animate(2, 1, 1);
                                return 1;
                            case 1:
                                _stageManager.Animate(1, 3, 1);
                                return 3;
                            case 5:
                                _stageManager.Animate(5, 7, 1);
                                return 7;
                            case 6:
                                _stageManager.Animate(6, 5, 1);
                                return 5;
                            case 8:
                                _stageManager.Animate(8, 10, 1);
                                return 10;
                            case 10:
                                _stageManager.Animate(10, 11, 1);
                                return 11;
                            case 9:
                                _stageManager.Animate(9, 8, 1);
                                return 8;
                            case 11:
                                _stageManager.Animate(_stage, 15, 1);
                                return 15;
                            case 14:
                                _stageManager.Animate(14, 13, 1);
                                return 13;
                            case 16:
                                _stageManager.Animate(16, 17, 1);
                                return 17;
                            default:
                                break;
                        }
                    }
            }
            return _stage;
        }
        private static Rectangle GetScaledRectangle(int x, int y, int width, int height, Vector2 scale)
        {
            return new Rectangle(
                x,
                y,
                (int)(width * scale.X),
                (int)(height * scale.Y)
            );
        }

        private static Rectangle GetDoorRectangle(int x, int y, int width, int height, Vector2 scale)
        {
            return new Rectangle(
                (int)(x * scale.X),
                (int)(y * scale.Y),
                (int)(width * scale.X),
                (int)(height * scale.Y)
            );
        }
    }
}
