using System;
using System.Collections.Generic;
using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;
using Modules.Technical.SceneLoader.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{
    public class InGameConfig : RuntimeScriptableObject
    {
        [Header("Standard Config")]
        [SerializeField] private LoadSceneEvent endOfGameScene;

        [Header("Dynamic Config")]
        [SerializeField, SaveAtRuntime] private List<Human> humans;
        [SerializeField, SaveAtRuntime] private List<Round> rounds;

        private int curRound;
        private Round CurrentRound => rounds[curRound];
        public ModeDescriptor CurrentModeDescriptor => CurrentRound.mode;
        public List<Human> Humans => humans;

        public void SetRounds(BaseRoundsProvider provider,
            BaseRoundsProvider.GameLength length,
            Round.GameDifficulty difficulty)
            => rounds = provider.GenerateRounds(length, difficulty);

        [Button] public void LoadRound()
        {
            if (curRound >= rounds.Count)
                endOfGameScene.Raise();
            else
                CurrentRound.mode.LoadMode();
        }

        [Button] public void GoNextRound() => curRound++;

        public void AddPoints(int id, int amount) => humans.Find(h => h.playerId == id).score += amount;
        public void SortHumansByScore() => humans.Sort((a, b) => a.score.CompareTo(b.score));

        public void SetHumans(Human[] players)
        {
            curRound = 0;
            humans.Clear();
            foreach (var player in players)
                if (!string.IsNullOrWhiteSpace(player.deviceName))
                    humans.Add(player);
        }
    }
}