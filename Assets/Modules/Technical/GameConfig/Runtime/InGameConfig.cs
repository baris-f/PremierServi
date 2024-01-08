using System.Collections.Generic;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;
using Modules.Technical.SceneLoader.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{
    public class InGameConfig : SoPresets
    {
        [Header("Standard Config")]
        [SerializeField] private LoadSceneEvent endOfGameScene;

        [Header("Dynamic Config")]
        [SerializeField, SaveInPreset] private List<Human> humans;
        [SerializeField, SaveInPreset] private List<Round> rounds;

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
        
        public void SortHumansByScore() => humans.Sort((a, b) => a.eatenCakes.Count.CompareTo(b.eatenCakes.Count));

        public Human GetHumanById(int id) => humans.Find(h => h.playerId == id);
        
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