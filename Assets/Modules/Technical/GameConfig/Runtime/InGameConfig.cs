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

        public Round CurrentRound => rounds[curRound];
        public List<Human> Humans
        {
            get => humans;
            set => humans = value;
        }

        public void SetRounds(BaseRoundsProvider provider,
            BaseRoundsProvider.GameLength length,
            Round.GameDifficulty difficulty)
            => rounds = provider.GenerateRounds(length, difficulty);

        [Button] public void LoadRound() => CurrentRound.mode.LoadMode();

        [Button] private void NextRound() => curRound++;
    }
}