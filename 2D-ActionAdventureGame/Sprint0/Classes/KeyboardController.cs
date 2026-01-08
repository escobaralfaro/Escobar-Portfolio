using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprint0.Player;
using Sprint2.Enemy;
using Sprint2.Map;
using Sprint2.TwoPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using  static Sprint2.Classes.Iitem;


namespace Sprint0.Classes
{
    public class KeyboardController
    {
        KeyboardState previousState;
        public Link _link;
        public Link _link2;
        private StageManager _stageManager;
        private StageManager2 _stageManager2;

        private List<Wizzrobe> _wizzrobes;
        int previousIdx;
        float VolumeSave;
        bool fromPause;
        public KeyboardController(Link link, Link link2)
        {
            _link = link;
            _link2 = link2;

            _wizzrobes = new List<Wizzrobe>();
            previousIdx = 0;
            VolumeSave = MediaPlayer.Volume;
            fromPause = false;

        }
        public void SetWizzrobe(Wizzrobe wizzrobe)
        {
            if (!_wizzrobes.Contains(wizzrobe))
            {
                _wizzrobes.Add(wizzrobe);
            }
        }
        
        public int Update(int GameStateIndex)
        {
            KeyboardState state = Keyboard.GetState();
            int returnVal = GameStateIndex;
            switch (GameStateIndex)
            {
                // Start Menu
                case 0:
                    if (state.IsKeyDown(Keys.D1))
                    {
                        // Start => Game
                        previousIdx = 1;
                        returnVal = 6;
                    }
                    else if (state.IsKeyDown(Keys.D2))
                    {
                        previousIdx = 2;
                        returnVal = 2;
                    }
                    break;
                // Game state
                case 1:
                    previousIdx = 1;
                    if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                    {
                        _link.MoveDown();
                    }
                    else if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                    {
                        _link.MoveUp();
                    }
                    else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                    {
                        _link.MoveLeft();
                    }
                    else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                    {
                        _link.MoveRight();
                    }
                    if (state.IsKeyDown(Keys.F) && !previousState.IsKeyDown(Keys.F))
                    {

                        
                            foreach (var wizzrobe in _wizzrobes)
                            {
                                if (wizzrobe.CanInteract)
                                {
                                    if (wizzrobe.chatBox != null && wizzrobe.chatBox.IsVisible)
                                    {
                                        wizzrobe.AdvanceConversation();
                                    }
                                    else
                                    {
                                        wizzrobe.StartConversation();
                                    }
                                    break; // Only interact with one Wizzrobe at a time
                                }
                            }
                     }
                        if (state.IsKeyDown(Keys.Z))
                    {
                        _link.SwordAttack();
                    }
                    if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.bow)
                    {
                        if (state.IsKeyDown(Keys.D3))
                        {
                            _link.ArrowAttack();
                        }
                           
                    } else if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.ak47)
                    {
                       
                        if (state.IsKeyDown(Keys.D3))
                        {
                            _link.ShootAk();
                        }
                    } else if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.boom)
                    {
                        if (state.IsKeyDown(Keys.D3))
                        {
                            _link.UseBomb();
                        }
                    }
                    else if (state.IsKeyDown(Keys.D2))
                    {
                        _link.UseBoomerang();
                    }
                  
                    if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                    {
                        fromPause = true;
                        // Game => Pause
                        previousIdx = 1;
                        returnVal = 5;
                    }
                    if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
                    {
                        _link.inventory.CycleSelectedItem();
                    }
                    else if (state.IsKeyDown(Keys.I) && !previousState.IsKeyDown(Keys.I))
                    {
                        _link.inventory.ToggleInventory();
                        previousIdx = 1;
                        returnVal = 3;
                    }
                    else if (state.IsKeyDown(Keys.K) && !previousState.IsKeyDown(Keys.K))
                    {
                        _link.IncrementKey();
              
                    }
                    break;

                case 2:
                    if (state.IsKeyDown(Keys.B))
                    {

                        returnVal = 0;
                    }
                    if (state.IsKeyDown(Keys.D1) && !previousState.IsKeyDown(Keys.D1))
                    {
                        previousIdx = 4;
                        returnVal = 11;
                    }
                    else if (state.IsKeyDown(Keys.D2) && !previousState.IsKeyDown(Keys.D2))
                    {
                        previousIdx = 4;
                        returnVal = 12;
                    }
                    else if (state.IsKeyDown(Keys.D3) && !previousState.IsKeyDown(Keys.D3))
                    {
                        previousIdx = 4;
                        returnVal = 13;
                    }
                    else if (state.IsKeyDown(Keys.D4) && !previousState.IsKeyDown(Keys.D4))
                    {
                        previousIdx = 4;
                        returnVal = 14;
                    }
                    break;
                // Inventory state
                case 3:
                    if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
                    {
                        _link.inventory.CycleSelectedItem();
                    }
                    if (state.IsKeyDown(Keys.Escape))
                    {
                        // Inventory => Game  
                        _link.inventory.ToggleInventory();
                        if(previousIdx == 1)
                        {
                            returnVal = 1;
                        } else
                        {
                            returnVal = 4;
                        }
                       
                    }
                    break;


                case 4: // Two PlayerMode
                    previousIdx = 4;
                    //Player2 Controls
                    if (state.IsKeyDown(Keys.Down))
                    {
                        _link2.MoveDown();
                    }
                    else if (state.IsKeyDown(Keys.Up))
                    {
                        _link2.MoveUp();
                    }
                    else if (state.IsKeyDown(Keys.Left))
                    {
                        _link2.MoveLeft();
                    }
                    else if (state.IsKeyDown(Keys.Right))
                    {
                        _link2.MoveRight();
                    }

                    if (state.IsKeyDown(Keys.S))
                    {
                        _link.MoveDown();
                    }
                    else if (state.IsKeyDown(Keys.W))
                    {
                        _link.MoveUp();
                    }
                    else if (state.IsKeyDown(Keys.A))
                    {
                        _link.MoveLeft();
                    }
                    else if (state.IsKeyDown(Keys.D))
                    {
                        _link.MoveRight();
                    }

                    if (state.IsKeyDown(Keys.F) && !previousState.IsKeyDown(Keys.F))
                    {

                            foreach (var wizzrobe in _wizzrobes)
                            {
                                if (wizzrobe.CanInteract)
                                {
                                    if (wizzrobe.chatBox != null && wizzrobe.chatBox.IsVisible)
                                    {
                                        wizzrobe.AdvanceConversation();
                                    }
                                    else
                                    {
                                        wizzrobe.StartConversation();
                                    }
                                    break; // Only interact with one Wizzrobe at a time
                                }
                        }
                    }
                    if (state.IsKeyDown(Keys.Z))
                    {
                        _link.SwordAttack();
                    }
                    if (state.IsKeyDown(Keys.NumPad1))
                    {
                        _link2.SwordAttack();
                    }

                    if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.bow)
                    {
                        if (state.IsKeyDown(Keys.D3))
                        {
                            _link.ArrowAttack();
                        }
                        if (state.IsKeyDown(Keys.Enter))
                        {
                            _link2.ArrowAttack();
                        }
                    }
                    else if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.ak47)
                    {

                        if (state.IsKeyDown(Keys.D3))
                        {
                            _link.ShootAk();
                        }
                        if (state.IsKeyDown(Keys.Enter))
                        {
                            _link2.ShootAk();
                        }

                    }
                    else if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.boom)
                    {
                        if (state.IsKeyDown(Keys.D3))
                        {
                            _link.UseBomb();
                        }
                        if (state.IsKeyDown(Keys.Enter))
                        {
                            _link2.UseBomb();
                        }

                    }

                    if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                    {
                        fromPause = true;
                        // Game => Pause
                        previousIdx = 4;
                        returnVal = 5;
                    }

                    if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
                    {
                        _link.inventory.CycleSelectedItem();
                    }
                    else if (state.IsKeyDown(Keys.I) && !previousState.IsKeyDown(Keys.I))
                    {
                        _link.inventory.ToggleInventory();
                        previousIdx = 4;
                        returnVal = 3;
                    }
                    else if (state.IsKeyDown(Keys.K) && !previousState.IsKeyDown(Keys.K))
                    {
                        _link.IncrementKey();
              //cheat code for now
                    }
                    break;
                // Pause menu
                case 5:
                    if (state.IsKeyDown(Keys.D0) && !previousState.IsKeyDown(Keys.D0))
                    {
                        if(MediaPlayer.Volume <= 0f)
                        {
                            MediaPlayer.Volume = VolumeSave;
                        } else
                        {
                            VolumeSave = MediaPlayer.Volume;
                            MediaPlayer.Volume = 0f; // mute
                        }
                        
                    }
                    if (state.IsKeyDown(Keys.OemMinus))
                    {
                        MediaPlayer.Volume = Math.Max(0.0f, MediaPlayer.Volume - 0.05f); // Decrease But not below zero
                    }
                    if (state.IsKeyDown(Keys.OemComma))
                    {
                        StageManager.Instance?.switchHitbox();
                        StageManager2.Instance?.switchHitbox();
                    }

                    if (state.IsKeyDown(Keys.OemPlus) || state.IsKeyDown(Keys.OemEnlW))
                    {
                        MediaPlayer.Volume = Math.Min(1.0f, MediaPlayer.Volume + 0.05f); // Increase volume, but not above 1.0
                    }
                    if (state.IsKeyDown(Keys.Escape))
                    {
                        fromPause = false;
                        // Pause => Game
                        if(previousIdx == 1)
                        {
                            returnVal = 1;
                        } else
                        {
                            returnVal = 4;
                        }
                        
                    }
                    // RESTART LOGIC
                    if (state.IsKeyDown(Keys.R))
                    {
                       
                        fromPause = false;
                        if(previousIdx == 1)
                        {
                            returnVal = 8;
                        }
                        if(previousIdx == 4)
                        {
                            returnVal = 9;
                        }
                    }

                    if (state.IsKeyDown(Keys.S))
                    {
                        fromPause = false;
                        returnVal = 0;
                    }

                    if (state.IsKeyDown(Keys.Q))
                    {
                        return 100;
                    }

                    if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                    {
                        
                        if(previousIdx == 1)
                        {
                            returnVal = 6;
                        } else
                        {
                            returnVal = 7;
                        }
                    }
                    break;
                case 6:
                    if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                    {
                        if (fromPause)
                        {
                            returnVal = 5;
                        } else
                        {
                            returnVal = 1;
                        }
                       
                    }
                    break;
                case 7:
                    if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
                    {
                        if (fromPause)
                        {
                            returnVal = 5;
                        }
                        else
                        {
                            returnVal = 4;
                        }
                    }
                    break;
                case -1:
                    if (state.IsKeyDown(Keys.R))
                    {
                        returnVal = 8;
                    }
                    else if (state.IsKeyDown(Keys.S))
                    {
                        returnVal = 0;
                    }
                    else if (state.IsKeyDown(Keys.Q))
                    {
                        return 100;
                    }
                    break;
                case -2:
                    if (state.IsKeyDown(Keys.R))
                    { 
                        returnVal = 9;                      
                    }

                    else if (state.IsKeyDown(Keys.S))
                    {
                        returnVal = 0;
                    }

                    else if (state.IsKeyDown(Keys.Q))
                    {
                        return 100;
                    }
                    break;
                default:
                    break;
            }
            

            previousState = state;
            return returnVal; // Return current state if no change
        }
       
    
    }

}