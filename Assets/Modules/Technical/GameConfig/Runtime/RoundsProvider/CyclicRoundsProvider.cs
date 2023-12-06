using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime.RoundsProvider
{
    public class CyclicRoundsProvider : BaseRoundsProvider
    {
        [Header("Config ")]
        [SerializeField] private List<ModeDescriptor> modesToProvide = new();

        public override string Name => "Cyclic";

        public override List<Round> GenerateRounds(GameLength length, Round.GameDifficulty difficulty)
        {
            var rounds = new List<Round>();
            var amount = length switch
            {
                GameLength.Single => 1,
                GameLength.Short => modesToProvide.Count,
                GameLength.Average => modesToProvide.Count * 2,
                GameLength.Long => modesToProvide.Count * 3,
                _ => throw new ArgumentOutOfRangeException(nameof(length), length, null)
            };
            for (var i = 0; i < amount; i++)
            {
                var mode = modesToProvide[i % modesToProvide.Count];
                var round = new Round(mode, difficulty);
                rounds.Add(round);
            }

            return rounds;
        }
    }
}