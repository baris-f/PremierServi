using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Technical.GameConfig.Runtime
{
    public class GameConfig : RuntimeScriptableObject
    {
        public enum GameMode
        {
            Classic,
            Frogger,
            Squid
        }

        public enum GameDifficulty
        {
            Easy,
            Normal,
            Hard
        }

        [Serializable]
        public class Player
        {
            public InputDevice Device;
            public string deviceName;
        }

        [SerializeField, SaveAtRuntime] private List<Player> players;
        [SerializeField, SaveAtRuntime] private GameMode mode;
        [SerializeField, SaveAtRuntime] private GameDifficulty difficulty;

        public GameMode Mode => mode;
        public List<Player> Players => players;

        public void SetPlayersFromArray(IEnumerable<Player> value) => players = value.ToList();
        public void SetDifficultyFromString(string value) => difficulty = Enum.Parse<GameDifficulty>(value);
        public GameMode SetModeFromString(string value) => mode = Enum.Parse<GameMode>(value);
    }
}