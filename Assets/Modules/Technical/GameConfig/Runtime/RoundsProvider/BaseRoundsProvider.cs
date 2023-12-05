using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime.RoundsProvider
{
    public abstract class BaseRoundsProvider : ScriptableObject
    {
        [Serializable]
        public enum GameLength
        {
            Short,
            Average,
            Long
        }

        public abstract string Name { get; }

        public abstract List<Round> GenerateRounds(GameLength length, Round.GameDifficulty difficulty);
    }
}