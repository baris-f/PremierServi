using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.GameRunner.Runtime
{
    public class RoundRunner : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private InGameConfig config;
        [SerializeField] private BaseIa robotsComportment;
        [SerializeField] private ResultsPopup results;

        [Header("Prefabs")]
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private CanonController canonPrefab;
        [SerializeField] private HumanInput humanPrefab;

        [Header("Containers")]
        [SerializeField] private TransformLayout playersLayout;
        [SerializeField] private TransformLayout canonsLayout;
        [SerializeField] private Transform humansContainer;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent gameStartEvent;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;
        [SerializeField] private ScriptableFloat goal;

        [Header("Debug")]
        [SerializeField] private List<RobotInput> robots = new();

        private void Start()
        {
            var modeDescriptor = config.CurrentModeDescriptor;
            humansContainer.DestroyAllChildren();
            var humanPlayerIds =
                UtilsGenerator.GenerateRandomNumbersInRange(0, modeDescriptor.NbPlayers, config.Humans.Count);

            foreach (var human in config.Humans)
            {
                if (human.playerId == -1 || human.playerId >= modeDescriptor.NbPlayers)
                    human.playerId = humanPlayerIds[0];
                humanPlayerIds.RemoveAt(0);
            }

            int robotCount = 0, humanCount = 0;
            for (var playerId = 0; playerId < modeDescriptor.NbPlayers; playerId++)
            {
                var player = Instantiate(playerPrefab, playersLayout.transform);
                var human = config.Humans.Find(h => h.playerId == playerId);
                if (human == null)
                {
                    player.Init(PlayerEvent.Type.Robot, playerId, robotCount);
                    var robotName = $"Robot {robotCount} (player {playerId})";
                    robots.Add(new RobotInput(robotName, robotsComportment, player, gameSpeed));
                    robotCount++;
                }
                else
                {
                    player.Init(PlayerEvent.Type.Human, playerId, humanCount);
                    var canon = Instantiate(canonPrefab, canonsLayout.transform);
                    canon.Init(playerId, humanCount, modeDescriptor.NbBullets);
                    var humanInput = HumanInput.Instantiate(humanPrefab, humansContainer, human, player, canon);
                    humanInput.name = $"Human {humanCount} (player {playerId}, canon {humanCount})";
                    humanCount++;
                }
            }

            playersLayout.RefreshLayout();
            canonsLayout.RefreshLayout();

            Invoke(nameof(StartGame), 1);
        }

        private void StartGame()
        {
            gameStartEvent.Raise();
            gameSpeed.Value = 1f;
            foreach (var robot in robots) robot.StartGame();
        }

        public async void OnPlayerWin(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData) return;
            config.AddPoints(playerData, 1);
            gameSpeed.Value = -1;
            await results.Open($"{playerData.type} {playerData.id} has won", true);
            config.GoNextRound();
            config.LoadRound();
        }

        private void OnDrawGizmos()
        {
            var x = goal.Value;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(x, -10), new Vector3(x, 10));
        }

        private void OnEnable() => gameSpeed.Value = -1;
        private void OnDisable() => gameSpeed.Value = -1;
    }
}