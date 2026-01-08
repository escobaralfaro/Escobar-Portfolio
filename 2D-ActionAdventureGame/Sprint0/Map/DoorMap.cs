using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Sprint2.Map
{
    public class DoorMap
    {
        public List<int[]> doors;
        static int doorLength;

        public DoorMap(String filename)
        {
            string[] lines = File.ReadAllLines(filename);

            doors = new List<int[]>();
            doorLength = 4;

            int[] currentRoom = new int[doorLength];

            foreach (string line in lines)
            {
                if (line.Length < 5)
                {
                    doors.Add(currentRoom);
                    currentRoom = new int[doorLength];

                }
                else
                {
                    string[] values = line.Split(',');
                    for (int col = 0; col < 4; col++)
                    {
                        string value = values[col].Trim();
                        if (int.TryParse(value, out int intValue))
                        {
                            currentRoom[col] = intValue;
                        }
                        else
                        {
                            currentRoom[col] = 0;
                        }
                    }
                }
            }
            doors.Add(currentRoom);

        }
        public int[] GetDoors(int roomNum)
        {
            if (roomNum < 0 || roomNum > doors.Count)
            {
                throw new ArgumentOutOfRangeException("Room out of range!");
            }

            return doors.ElementAt(roomNum);
        }

        public void AllEnemiesDead(int roomNum, Boolean enemies)
        {
            //if (roomNum ==  0 && enemies)
            //{
            //    doors[0][0] = 1;
            //    doors[0][1] = 1;
            //    doors[0][2] = 1;
            //}
        }

        public void SpecialDoorLogic(int roomNum)
        {
            switch (roomNum)
            {
                case 1:
                    doors[1][3] = 3;
                    break;
                case 6:
                    doors[6][2] = 3;
                    break;
                case 16:
                    doors[16][2] = 1;
                    break;
                default:
                    break;

            }
        }
        public void KeyLogic(int roomNum)
        {
            switch (roomNum)
            {
                case 1:
                    doors[1][0] = 1;
                    break;
                case 6:
                    doors[6][0] = 1;
                    break;
                default:
                    break;

            }
        }
        public void BoomLogic(int roomNum)
        {
            switch (roomNum)
            {
                case 5:
                    doors[5][0] = 4;
                    doors[10][3] = 4;
                    break;
                case 7:
                    doors[7][0] = 4;
                    doors[11][3] = 4;
                    break;
                default:
                    break;
            }
        }
    }
}
