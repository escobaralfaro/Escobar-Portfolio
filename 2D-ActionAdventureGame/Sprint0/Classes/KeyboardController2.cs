using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprint0.Player;
using Sprint2.Enemy;
using Sprint2.GameStates;
using Sprint2.Map;
using Sprint2.TwoPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sprint2.Classes.Iitem;

namespace Sprint0.Classes
{
    public class KeyboardController2
    {
        KeyboardState previousState;
        private Link _link;
        private Link _link2;
        private StageManager _stageManager;
        private StageManager2 _stageManager2;

        private List<Wizzrobe> _wizzrobes;
        int previousIdx;
        float VolumeSave;
        bool fromPause;
        public KeyboardController2(Link link, Link link2)
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
            int newGameStateIndex = GameStateIndex;
            switch (GameStateIndex)
            {
                case 0: 
                    newGameStateIndex = UpdateStartMenu(state, GameStateIndex);
                    break;
                case 1:
                    previousIdx = 1;
                    newGameStateIndex = UpdateSinglePlayer(state, GameStateIndex);
                    break;
                case 2:  
                    newGameStateIndex = UpdateTwoPlayerMenu(state, GameStateIndex);
                    break;
                case 3: 
                    newGameStateIndex = UpdateInventory(state, GameStateIndex);
                    break;
                case 4: 
                    previousIdx = 4;
                    newGameStateIndex = UpdateTwoPlayer(state, GameStateIndex);
                    break;
                case 5:
                    newGameStateIndex = UpdatePauseMenu(state, GameStateIndex);
                    break;
                case 7:
                    newGameStateIndex = UpdateControls(state, GameStateIndex);
                    break;
                case -1:
                    newGameStateIndex = UpdateGameOver1(state, GameStateIndex);
                    break;
                case -2:
                    newGameStateIndex= UpdateGameOver2(state, GameStateIndex);
                    break;
                default:
                    break;
            }
            previousState = state;
            return newGameStateIndex; // Return current state if no change
        }
        public int UpdateStartMenu(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.D1))
            { // Start => Single Player
                return 1; // MAY HAVE TO CHANGE TO 6 not sure
            }
            else if (state.IsKeyDown(Keys.D2))
            { // Start => Two player
                return 2;
            }
            // State Remains Unchanged
            return OriginalIndex;
        }

        public int UpdatePauseMenu(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.D0) && !previousState.IsKeyDown(Keys.D0)) // MUTE CONTROLS
            {
                previousState = state;
                if (MediaPlayer.Volume <= 0f)
                {
                    MediaPlayer.Volume = VolumeSave;
                }
                else
                {
                    VolumeSave = MediaPlayer.Volume;
                    MediaPlayer.Volume = 0f;
                }
            }
            else if (state.IsKeyDown(Keys.OemMinus)) // REDUCE VOlUME
            {
                MediaPlayer.Volume = Math.Max(0.0f, MediaPlayer.Volume - 0.05f);
            }
            else if (state.IsKeyDown(Keys.OemPlus) || state.IsKeyDown(Keys.OemEnlW)) // INCREASE VOLUME
            {
                MediaPlayer.Volume = Math.Min(1.0f, MediaPlayer.Volume + 0.05f);
            }
            else if (state.IsKeyDown(Keys.OemComma)) //TOGGLE HITBOX
            {
                if (previousIdx == 1)
                {
                    StageManager.Instance?.switchHitbox();
                }
                else
                {
                    StageManager2.Instance?.switchHitbox();
                }
            }
            else if (state.IsKeyDown(Keys.Escape)) // Pause => Game
            {
                fromPause = false;
                if (previousIdx == 1) // BACK TO SINGLE PLAYER
                {
                    return 1;
                }
                else // BACK TO TWO PLAYER
                {
                    return 4;
                }
            }
            else if (state.IsKeyDown(Keys.R)) // RESTART LOGIC
            {
                fromPause = false;
                if (previousIdx == 1)
                {
                    return 8;
                }
                if (previousIdx == 4)
                {
                    return 9;
                }
            }
            else if (state.IsKeyDown(Keys.S)) // START MENU
            {
                fromPause = false;
                return 0;
            }
            else if (state.IsKeyDown(Keys.Q)) // QUIT
            {
                return 100;
            }
            else if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space)) // INSTRUCTIONS
            {
                previousState = state;
                if (previousIdx == 1)
                {
                    return 6;
                }
                else
                {
                    return 7;
                }
            }
            return OriginalIndex;
        }

        public int UpdateSinglePlayer(KeyboardState state, int OriginalIndex)
        {
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
            else if (state.IsKeyDown(Keys.F) && !previousState.IsKeyDown(Keys.F))
            {
                previousState = state;
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

            }
            else if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.ak47)
            {

                if (state.IsKeyDown(Keys.D3))
                {
                    _link.ShootAk();
                }
            }
            else if (_link.inventory?.SelectedItem?.CurrentItemType == ItemType.boom)
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
                return 5;
            }
            if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
            {
                previousState = state;
                _link.inventory.CycleSelectedItem();
            }
            else if (state.IsKeyDown(Keys.I) && !previousState.IsKeyDown(Keys.I))
            {
                previousState = state;
                _link.inventory.ToggleInventory();
                return 3;
            }
            return OriginalIndex;
        }

        public int UpdateTwoPlayerMenu(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.B)) // BACK TO START
            {
                return 0;
            }
            if (state.IsKeyDown(Keys.D1) && !previousState.IsKeyDown(Keys.D1)) 
            {
                previousState = state;
                return 11;
            }
            else if (state.IsKeyDown(Keys.D2) && !previousState.IsKeyDown(Keys.D2))
            {
                previousState = state;
                return 12;
            }
            else if (state.IsKeyDown(Keys.D3) && !previousState.IsKeyDown(Keys.D3))
            {
                previousState = state;
                return 13;
            }
            else if (state.IsKeyDown(Keys.D4) && !previousState.IsKeyDown(Keys.D4))
            {
                previousState = state;
                return 14;
            }
            return OriginalIndex;
        }

        public int UpdateInventory(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P)) // CYCLE ITEM
            {
                previousState = state;
                _link.inventory.CycleSelectedItem();
            }
            if (state.IsKeyDown(Keys.Escape))
            {
                _link.inventory.ToggleInventory();
                if (previousIdx == 1)
                {
                    return 1; // BACK TO SINGLE PLAYER
                }
                else
                {
                    return 4; // BACK TO TWO PLAYER
                }
            }
            return OriginalIndex;
        }
        public int UpdateTwoPlayer(KeyboardState state, int OriginalIndex)
        {
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
                previousState = state;
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
                previousState = state;
                fromPause = true;
                // Game => Pause
                previousIdx = 4;
                return 5;
            }

            if (state.IsKeyDown(Keys.P) && !previousState.IsKeyDown(Keys.P))
            {
                previousState = state;
                _link.inventory.CycleSelectedItem();
            }
            else if (state.IsKeyDown(Keys.I) && !previousState.IsKeyDown(Keys.I))
            {
                previousState = state;
                _link.inventory.ToggleInventory();
                previousIdx = 4;
                return 3;
            }
            return OriginalIndex;
        }
        public int UpdateControls(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
            {
                previousState = state;
                if (fromPause)
                {
                    return 5;
                }
                else
                {
                    return 4;
                }
            }
            return OriginalIndex;
        }

        public int UpdateGameOver1(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.R)) // RESTART
            {
                return 8;
            }
            else if (state.IsKeyDown(Keys.S)) // START
            {
                return 0;
            }
            else if (state.IsKeyDown(Keys.Q)) // QUIT
            {
                return 100;
            }
            return OriginalIndex;
        }
        public int UpdateGameOver2(KeyboardState state, int OriginalIndex)
        {
            if (state.IsKeyDown(Keys.R))
            {
                return 9;
            }

            else if (state.IsKeyDown(Keys.S))
            {
                return 0;
            }

            else if (state.IsKeyDown(Keys.Q))
            {
                return 100;
            }
            return OriginalIndex;
        }
    }
}
