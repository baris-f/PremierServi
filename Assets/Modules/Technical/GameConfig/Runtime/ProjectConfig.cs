using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Technical.GameConfig.Runtime
{
    public class ProjectConfig : ScriptableObject
    {
        [Serializable]
        public enum GameSetting
        {
            Classic,
            Frogger,
            Squid,
            Random,
        }

        public enum GameLenght
        {
            Short,
            Medium,
            Long
        }
        
        [Header("Settings")]
        [SerializeField] private List<Round> availableRounds = new();
        // peut etre mettre les joycon color ici qu'elles soient pas en dur dans le code
        
        [Header("References")]
        [SerializeField] private InGameConfig config;

        public void SetupHumans(IEnumerable<Human> humans) => config.humans = humans.ToList();

        public void SetupRounds(GameLenght gameLenght, GameSetting gameSetting,
            Round.GameDifficulty gameDifficulty)
        {
            config.rounds.Clear();
            var nbRounds = gameLenght switch
            {
                GameLenght.Short => 3,
                GameLenght.Medium => 6,
                GameLenght.Long => 9
            };

            switch (gameSetting)
            {
                case GameSetting.Classic:
                    SetupClassic(nbRounds, gameDifficulty);
                    break;
                case GameSetting.Frogger:
                    break;
                case GameSetting.Squid:
                    break;
                case GameSetting.Random:
                    SetupRandom(nbRounds, gameDifficulty);
                    break;
            }
        }

        private void SetupClassic(int nbRounds, Round.GameDifficulty difficulty)
        {
            for (var i = 0; i < nbRounds; i++)
            {
                var round = availableRounds[0];
                round.difficulty = difficulty;
                config.rounds.Add(round);
            }
        }

        private void SetupRandom(int nbRounds, Round.GameDifficulty difficulty)
        {
            for (var i = 0; i < nbRounds; i++)
            {
                var roundId = Random.Range(0, availableRounds.Count);
                var round = availableRounds[roundId];
                round.difficulty = difficulty;
                config.rounds.Add(round);
            }
        }
    }
}