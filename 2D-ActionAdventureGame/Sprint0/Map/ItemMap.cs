using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Classes;
using Sprint0.Player;
using Sprint2.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using static Sprint0.Player.ILinkState;
using static Sprint2.Classes.Iitem;

namespace Sprint2.Map
{
    public class ItemMap
    {
        public List<List<Iitem>> _itemMap;
        private List<int[,]> rooms;
        private int roomHeight;
        private int roomWidth;
        private Vector2 _scale;
        private GraphicsDevice _GraphicsDevice;
        private ContentManager _ContentManager;
        public Link _link;
        public Link _link2;
        private Fairy fairy;
        private Clock clock;
        private Potion potion;
        private bool AddedKey3;
        private bool TwoPlayer;
        public ItemMap(String filename, Vector2 scale, GraphicsDevice graphicsDevice, ContentManager content, Link link, Link link2)
        {
            string[] lines = File.ReadAllLines(filename);
            AddedKey3 = false;
           // TwoPlayer = false;
            rooms = new List<int[,]>();
            _itemMap = new List<List<Iitem>>();
            roomHeight = 7;
            roomWidth = 12;
            _scale = scale;
            _GraphicsDevice = graphicsDevice;
            _ContentManager = content;
            _link = link;
            _link2 = link2;


     


            int[,] currentRoom = new int[roomHeight, roomWidth];
            int row = 0;

            //initializing fairy
            Vector2 W = Vector2.Zero;
            if (TwoPlayer)
            {
                fairy = new Fairy(W, _link, _link2);
            } else
            {
                fairy = new Fairy(W, _link, null);
            }
          
            fairy.follow = false;


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
                            currentRoom[row, col] = 0;
                        }
                    }
                    row++;
                }
            }
            rooms.Add(currentRoom);

            for (int x = 0; x < rooms.Count; x++)
            {
                int[,] _currentRoom = GetRoom(x);
                List<Iitem> items = GetItemsInRoom(_currentRoom);
                _itemMap.Add(items);
            }

            
        }
        public List<Iitem> GetItems(int roomNum)
        {
            if (roomNum < 0 || roomNum > _itemMap.Count)
            {
                throw new ArgumentOutOfRangeException("Room out of range!");
            }
            return _itemMap.ElementAt(roomNum);
        }

        public int[,] GetRoom(int roomNum)
        {
            if (roomNum < 0 || roomNum > rooms.Count)
            {
                throw new ArgumentOutOfRangeException("Room out of range!");
            }
            return rooms.ElementAt(roomNum);
        }

        public List<Iitem> GetItemsInRoom(int[,] room)
        {
            List<Iitem> ItemsInRoom = new List<Iitem>();

            Vector2 ItemPosition = new Vector2(32 * _scale.X, 87 * _scale.Y);
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    int tileIdx = room[i, j];

                    switch (tileIdx)
                    {
                        case 1:
                            Fire fire = new Fire(ItemPosition, _link, _link2);
                            fire.LoadContent(_ContentManager, "zeldaLink", _GraphicsDevice, ItemType.fire, _scale);
                            ItemsInRoom.Add(fire);
                            break;
                        case 2:
                            Health health = new Health(ItemPosition, _link, _link2);
                            health.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.health, _scale);
                            ItemsInRoom.Add(health);
                            break;
                        case 3:
                            Heart heart = new Heart(ItemPosition, _link, _link2);
                            heart.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.heart, _scale);
                            ItemsInRoom.Add(heart);
                            break;
                        case 4:
                            clock = new Clock(ItemPosition, _link, _link2);
                            clock.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.clock, _scale);
                            ItemsInRoom.Add(clock);
                            break;
                        case 5:
                            fairy = new Fairy(ItemPosition, _link, _link2);
                            fairy.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.fairy, _scale);
                            ItemsInRoom.Add(fairy);
                            break;
                        case 6:
                            Diamond diamond = new Diamond(ItemPosition, _link, _link2);
                            diamond.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.diamond, _scale);
                            ItemsInRoom.Add(diamond);
                            break;
                        case 7:
                            potion = new Potion(ItemPosition, _link, _link2);
                            potion.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.potion, _scale);
                            ItemsInRoom.Add(potion);
                            break;
                        case 8:
                            Triangle triangle = new Triangle(ItemPosition, _link, _link2);
                            triangle.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.triangle, _scale);
                            ItemsInRoom.Add(triangle);
                            break;
                        case 9:
                            Maps map = new Maps(ItemPosition, _link, _link2);
                            map.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.map, _scale);
                            ItemsInRoom.Add(map);
                            break;
                        case 10:
                            Key key = new Key(ItemPosition, _link, _link2);
                            key.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.key, _scale);
                            ItemsInRoom.Add(key);
                            break;
                        case 11:
                            Bow bow = new Bow(ItemPosition, _link, _link2);
                            bow.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.bow, _scale);
                            ItemsInRoom.Add(bow);
                            break;
                        case 12:
                            Boom boom = new Boom(ItemPosition, _link, _link2);
                            boom.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.boom, _scale);
                            ItemsInRoom.Add(boom);
                            break;
                        case 13:
                            Compass compass= new Compass(ItemPosition, _link,_link2);
                            compass.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.compass, _scale);
                            ItemsInRoom.Add(compass);
                            break;
                        case 14:
                            Ak47 ak47 = new Ak47(ItemPosition, _link,_link2);
                            ak47.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.ak47, _scale);
                            ItemsInRoom.Add(ak47);
                            break;

                    }
                    ItemPosition.X += 16 * _scale.X;
                }
                ItemPosition.X = 32 * _scale.X;
                ItemPosition.Y += 16 * _scale.Y;
            }
            
            return ItemsInRoom;
        }

        public void Update(int currentStage, GameTime gameTime)
        {
            List<Iitem> items = GetItems(currentStage);

            if (_link.GetClockCount() > 0)
            {
                items.Add(clock);
                clock.Position.X += 20000;
                clock.Position.Y += 20000;
            }
            if (_link.GetPotionCount() > 0)
            {
                items.Add(potion);
                potion.Position.X += 20000;
                potion.Position.Y += 20000;
            }

            if (fairy.follow && _link.transitioning)
                {
                    items.Add(fairy);
                    fairy.Position.X = _link._position.X;
                    fairy.Position.Y = _link._position.Y;
                }
                else if (fairy.F1 && _link.transitioning || fairy.F1 && _link2.transitioning)
                {
                    items.Add(fairy);
                    fairy.Position.X = _link._position.X;
                    fairy.Position.Y = _link._position.Y;
                } 
                else if (fairy.F2 && _link2.transitioning || fairy.F2 && _link.transitioning)
                {
                        items.Add(fairy);
                        fairy.Position.X = _link2._position.X;
                        fairy.Position.Y = _link2._position.Y;
                 }
                foreach (Iitem item in items)
                {
                item.Update(gameTime);                              
                }
        }

        public void SpawnKey(int roomNum)
        {
            switch(roomNum)
            {
                case 3:
                    if (!AddedKey3)
                    {
                        Vector2 ItemPosition = new Vector2(208 * _scale.X, 127 * _scale.Y);
                        Key key = new Key(ItemPosition, _link, _link2);
                        key.LoadContent(_ContentManager, "NES - The Legend of Zelda - Items & Weapons", _GraphicsDevice, ItemType.key, _scale);
                        _itemMap[3].Add(key);
                        AddedKey3 = true;
                    }
                    break;
                default:
                    break;
            }        
        }
    }
}
