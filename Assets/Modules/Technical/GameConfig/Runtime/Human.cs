using System;
using UnityEngine.InputSystem;

namespace Modules.Technical.GameConfig.Runtime
{
    [Serializable]
    public class Human
    {
        public InputDevice Device;
        public string deviceName;
        public JoyConColors.ColorName color;
        public int playerId = -1; // -1 || >nbPlayers = random id on game start, else use that id to set player
    }
}