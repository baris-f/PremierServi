using System;
using System.Collections.Generic;
using Modules.Common.Cake.Runtime;
using UnityEngine.InputSystem;

namespace Modules.Technical.GameConfig.Runtime
{
    [Serializable]
    public class Human
    {
        public InputDevice Device;
        public string deviceName;
        public JoyConColors color;
        public int playerId = -1; // -1 || >nbPlayers = random id on game start, else use that id to set player
        public int humanId = -1;
        public List<Cake> eatenCakes = new();
    }
}