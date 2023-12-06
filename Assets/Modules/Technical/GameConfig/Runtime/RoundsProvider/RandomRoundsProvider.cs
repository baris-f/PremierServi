using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Technical.GameConfig.Runtime.RoundsProvider
{
    public class RandomRoundsProvider : BaseRoundsProvider
    {
        [Header("Config ")]
        [SerializeField] private List<ModeDescriptor> modesToProvide = new();

        public override string Name => "Random";

        public override List<Round> GenerateRounds(GameLength length, Round.GameDifficulty difficulty)
        {
            var rounds = new List<Round>();
            var amount = GetDefaultLength(length);
            for (var i = 0; i < amount; i++)
            {
                var id = Random.Range(0, modesToProvide.Count);
                var mode = modesToProvide[id];
                var round = new Round(mode, difficulty);
                rounds.Add(round);
            }

            return rounds;
        }
    }
}