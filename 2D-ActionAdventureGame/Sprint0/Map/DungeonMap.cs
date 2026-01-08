using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Sprint2.Map
{
    public class DungeonMap
    {
        public List<int[,]> rooms;
        static int roomHeight;
        static int roomWidth;
        public DungeonMap(String filename)
        {
            string[] lines = File.ReadAllLines(filename); 

            rooms = new List<int[,]>();
            roomHeight = 7;
            roomWidth = 12;
            int[,] currentRoom = new int[roomHeight, roomWidth];
            int row = 0;

            foreach (string line in lines)
            {
                if (line.Length < 20)
                {
                    rooms.Add(currentRoom);
                    currentRoom = new int[roomHeight, roomWidth];
                    row = 0;

                }
                else
                {
                    string[] values = line.Split(',');
                    for (int col = 0; col < 12; col++)
                    {
                        string value = values[col].Trim();
                        if (int.TryParse(value, out int intValue))
                        {
                            currentRoom[row, col] = intValue;
                        }
                        else
                        {
                           // currentRoom[row, col] = 0;
                        }
                    }
                    row++;
                }
            }
            rooms.Add(currentRoom);

        }
        public int[,] GetRoom(int roomNum)
        {
            if (roomNum < 0 || roomNum > rooms.Count)
            {
                throw new ArgumentOutOfRangeException("Room out of range!");
            }
            return rooms.ElementAt(roomNum);
        }

    }
}
