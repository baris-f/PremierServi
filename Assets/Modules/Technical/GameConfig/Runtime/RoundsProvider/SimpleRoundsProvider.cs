using System.Collections.Generic;
using Modules.Technical.GameConfig.Runtime.Modes;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime.RoundsProvider
{
    public class SimpleRoundsProvider : BaseRoundsProvider
    {
        [Header("Config")]
        [SerializeField] private string modeName;
        [SerializeField] private ModeDescriptor modeToProvide;

        public override string Name => modeName;

        public override List<Round> GenerateRounds(GameLength length, Round.GameDifficulty difficulty)
        {
            var rounds = new List<Round>();
            var amount = GetDefaultLength(length);
            for (var i = 0; i < amount; i++)
            {
                var round = new Round(modeToProvide, difficulty);
                rounds.Add(round);
            }

            return rounds;
        }
    }
}