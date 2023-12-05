using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime.RoundsProvider
{
    public class SimpleRoundsProvider : BaseRoundsProvider
    {
        [Header("Config ")]
        [SerializeField] private string modeName;
        [SerializeField] private ModeDescriptor modeToProvide;

        public override string Name => modeName;

        public override List<Round> GenerateRounds(GameLength length, Round.GameDifficulty difficulty)
        {
            var rounds = new List<Round>();
            var amount = length switch
            {
                GameLength.Short => 3,
                GameLength.Average => 6,
                GameLength.Long => 9,
                _ => throw new ArgumentOutOfRangeException(nameof(length), length, null)
            };
            for (var i = 0; i < amount; i++)
            {
                var round = new Round(modeToProvide, difficulty);
                rounds.Add(round);
            }

            return rounds;
        }
    }
}