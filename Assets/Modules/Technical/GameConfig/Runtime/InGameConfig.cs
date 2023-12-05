using System.Collections.Generic;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{
    public class InGameConfig : RuntimeScriptableObject
    {
        [Header("Dynamic Config")]
        [SerializeField, SaveAtRuntime] private int curRound;
        [SerializeField, SaveAtRuntime] private List<Human> humans;
        [SerializeField, SaveAtRuntime] private List<Round> rounds;

        [Header("Debug")]
        [SerializeField] private BaseRoundsProvider providerToTest;
        [SerializeField] private BaseRoundsProvider.GameLength testGameLength;
        [SerializeField] private Round.GameDifficulty testGameDifficulty;

        [Button] private void TestProvider()
            => rounds = providerToTest.GenerateRounds(testGameLength, testGameDifficulty);

        public Round CurrentRound => rounds[curRound];
        public List<Human> Humans
        {
            get => humans;
            set => humans = value;
        }
    }
}