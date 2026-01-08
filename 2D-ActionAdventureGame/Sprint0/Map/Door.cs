using System.Drawing;
using System.Numerics;

namespace Sprint2.Map
{
    public class Door
    {
        public int[] DoorCode { get; private set; }
        public Rectangle BoundingBox { get; private set; }
        public Vector2 NextStagePosition { get; private set; } // Where the player should appear in the next stage
        public string NextStage { get; private set; } // The name or ID of the next stage to load

        public Door(int[] doorCode, Rectangle boundingBox)
        {
            DoorCode = doorCode;
            BoundingBox = boundingBox;
        }
        public int DecodeDoor(int direction, int door)
        {
            return 9 + direction * 5 + door;

        }


    }
}
