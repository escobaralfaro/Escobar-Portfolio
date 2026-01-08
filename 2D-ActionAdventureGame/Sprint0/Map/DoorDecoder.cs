namespace Sprint2.Map
{
    public class DoorDecoder
    {
        public int DecodeDoor(int direction, int door)
        {
            return  10 + (direction * 5) + door;
            
        }
    }
}
