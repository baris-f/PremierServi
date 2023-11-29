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
        public class Human
        {
            public InputDevice Device;
            public string deviceName;
            public JoyConColors.ColorName color;
            public int playerId = -1; // -1 || >nbPlayers = random id on game start else, random at game start
        }

        [SerializeField, SaveAtRuntime] private List<Human> humans;
        [SerializeField, SaveAtRuntime] private GameMode mode;
        [SerializeField, SaveAtRuntime] private GameDifficulty difficulty;

        public GameMode Mode => mode;
        public List<Human> Humans => humans;

        public void SetPlayersFromArray(IEnumerable<Human> value) => humans = value.ToList();
        public void SetDifficultyFromString(string value) => difficulty = Enum.Parse<GameDifficulty>(value);
        public GameMode SetModeFromString(string value) => mode = Enum.Parse<GameMode>(value);
    }
}