using System;
using System.Collections.Generic;
using System.Linq;
using Modules.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.GameConfig.Runtime
{
    public class GameConfig : ScriptableObjectSingleton<GameConfig>
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

        [Header("In game choices (don't touch)")]
        [SerializeField] private List<Player> players;
        [SerializeField] private GameMode mode;
        [SerializeField] private GameDifficulty difficulty;

        public GameMode Mode => mode;
        
        public void SetPlayersFromArray(IEnumerable<Player> value) => players = value.ToList();
        public void SetDifficultyFromString(string value) => difficulty = Enum.Parse<GameDifficulty>(value);
        public GameMode SetModeFromString(string value) => mode = Enum.Parse<GameMode>(value);
    }
}