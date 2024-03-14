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
            Infinite,
            Single,
            Short,
            Average,
            Long
        }

        public abstract string Name { get; }

        protected static int GetDefaultLength(GameLength length) => length switch
        {
            GameLength.Infinite => 999,
            GameLength.Single => 1,
            GameLength.Short => 3,
            GameLength.Average => 6,
            GameLength.Long => 9,
            _ => throw new ArgumentOutOfRangeException(nameof(length), length, null)
        };
        public abstract List<Round> GenerateRounds(GameLength length, Round.GameDifficulty difficulty);
    }
}