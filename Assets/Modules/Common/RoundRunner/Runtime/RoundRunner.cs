using System.Collections.Generic;
using Modules.Common.Cake.Runtime;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Common.Status;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.GameConfig.Runtime.Modes;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptableField.Implementations;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.RoundRunner.Runtime
{
    public abstract class RoundRunner : MonoBehaviour
    {
        [Header("Robots")]
        [SerializeField] private BaseIa robotsComportment;

        [Header("Players")]
        [SerializeField] private List<PlayerController> playerPrefabs = new();
        [SerializeField] protected TransformLayout playersLayout;

        [Header("Humans")]
        [SerializeField] protected HumanInput humanPrefab;
        [SerializeField] protected Transform humansContainer;

        [Header("Status")]
        [SerializeField] private StatusController statusPrefab;
        [SerializeField] private Transform statusContainer;

        [Header("References")]
        [SerializeField] private CakeBehaviour cakeBehaviour;
        [SerializeField] protected InGameConfig config;
        [SerializeField] protected ResultsPopup results;

        [Header("Scriptables")]
        [SerializeField] private SimpleLocalEvent gameStartEvent;
        [SerializeField] protected ScriptableFloat gameSpeed;
        [SerializeField] private ScriptableFloat goal;

        [Header("Debug")]
        private readonly List<RobotInput> robots = new();
        [SerializeField] private int nbPlayersDead;
        [SerializeField] private int nbHumansDead;

        protected void Reset()
        {
            humansContainer.DestroyAllChildren();
            statusContainer.DestroyAllChildren();
            playersLayout.Clear();
            robots.Clear();
            nbHumansDead = 0;
            nbPlayersDead = 0;
        }

        protected void RefreshLayouts()
        {
            playersLayout.RefreshLayout();
        }

        protected void InitHumans(int nbPlayers)
        {
            var humanPlayerIds =
                UtilsGenerator.GenerateRandomNumbersInRange(0, nbPlayers, config.Humans.Count);
            foreach (var human in config.Humans)
            {
                if (human.playerId == -1 || human.playerId >= nbPlayers)
                    human.playerId = humanPlayerIds[0];
                humanPlayerIds.RemoveAt(0);
            }
        }

        protected List<PlayerController> CreatePlayerPrefabArray(int nbPlayers)
        {
            var randomPrefabArray = new List<PlayerController>();
            var nbToPut = nbPlayers / (playerPrefabs.Count + 1);
            foreach (var prefab in playerPrefabs)
                for (var j = 0; j < nbToPut; j++)
                    randomPrefabArray.Add(prefab);
            while (randomPrefabArray.Count < nbPlayers)
                randomPrefabArray.Add(playerPrefabs.GetRandom());
            return randomPrefabArray;
        }

        protected HumanInput SetupHuman(PlayerController player, int playerId, ModeDescriptor modeDescriptor,
            Human human)
        {
            player.Init(PlayerEvent.Type.Human, playerId, human.humanId, modeDescriptor.WalkSpeed,
                modeDescriptor.RunSpeed);
            var status = Instantiate(statusPrefab, statusContainer.transform);
            status.Initialize(human.playerId, human.color, human.humanId, modeDescriptor.NbBullets);
            var humanInput = HumanInput.Instantiate(humanPrefab, humansContainer, human);
            humanInput.name = $"Human {human.humanId} (player {playerId}, canon {human.humanId})";
            return humanInput;
        }

        protected void SetupRobot(PlayerController player, int playerId, int robotCount, ModeDescriptor modeDescriptor)
        {
            player.Init(PlayerEvent.Type.Robot, playerId, robotCount, modeDescriptor.WalkSpeed,
                modeDescriptor.RunSpeed);
            var robotName = $"Robot {robotCount} (player {playerId})";
            robots.Add(new RobotInput(robotName, robotsComportment, player, gameSpeed));
        }

        protected void StartGame()
        {
            gameStartEvent.Raise();
            gameSpeed.Value = 1f;
            foreach (var robot in robots) robot.StartGame();
        }

        public async void OnPlayerWin(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData) return;
            gameSpeed.Value = -1;
            if (config.Humans is { Count: > 0 })
            {
                if (playerData.type == PlayerEvent.Type.Human)
                {
                    config.GetHumanById(playerData.id).eatenCakes.Add(cakeBehaviour.GetCake());
                    await results.Open($"Player {playerData.typeId} ate the cake !", true);
                }
                else
                    await results.Open($"Somebody else ate the cake !", true);
            }

            config.GoNextRound();
            config.LoadRound();
        }

        public async void OnPlayerDeath(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData) return;
            nbPlayersDead++;
            if (playerData.type == PlayerEvent.Type.Human) nbHumansDead++;

            if (nbHumansDead >= config.Humans.Count)
            {
                gameSpeed.Value = -1;
                await results.Open($"Nobody could eat the cake !", true);
                config.GoNextRound();
                config.LoadRound();
            }

            if (config.LastOneWins && config.Humans.Count > 1 && nbHumansDead >= config.Humans.Count - 1)
            {
                gameSpeed.Value = -1;
                await results.Open($"Player {playerData.typeId} is the last player standing ! That's a win.", true);
                config.GoNextRound();
                config.LoadRound();
            }
        }

        private void OnDrawGizmos()
        {
            var x = goal.Value;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(x, -10), new Vector3(x, 10));
        }

        protected void OnEnable() => gameSpeed.Value = -1;
        protected void OnDisable() => gameSpeed.Value = -1;
    }
}