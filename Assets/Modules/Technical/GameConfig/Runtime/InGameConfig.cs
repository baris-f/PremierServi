﻿using System;
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
        public SortedDictionary<string, int> Scores { get; } = new();
        private Round CurrentRound => rounds[curRound];
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

        public void AddPoints(PlayerEvent.PlayerData data, int amount)
        {
            var key = $"{data.type} {data.id}";
            if (Scores.ContainsKey(key))
                Scores[key] += amount;
            else
                Scores.Add(key, amount);
        }

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