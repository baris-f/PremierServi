using System;
using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Modules.Common.GameRunner.Runtime
{
    public class GameRunner : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int nbPlayers = 8;
        [SerializeField] private GameConfig config;
        [SerializeField] private BaseIa robotsComportment;

        [Header("Prefabs")]
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private CanonController canonPrefab;
        [SerializeField] private HumanInput humanPrefab;

        [Header("Containers")]
        [SerializeField] private Transform playersContainer;
        [SerializeField] private Transform canonsContainer;
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
            playersContainer.DestroyAllChildren();
            canonsContainer.DestroyAllChildren();
            humansContainer.DestroyAllChildren();

            var humanPlayerIds = UtilsGenerator.GenerateRandomNumbersInRange(0, nbPlayers, config.Humans.Count);

            foreach (var human in config.Humans)
            {
                if (string.IsNullOrWhiteSpace(human.deviceName)) continue;
                if (human.playerId == -1 || human.playerId >= nbPlayers)
                    human.playerId = humanPlayerIds[0];
                humanPlayerIds.RemoveAt(0);
            }

            int robotCount = 0, humanCount = 0;
            for (var playerId = 0; playerId < nbPlayers; playerId++)
            {
                var player = Instantiate(playerPrefab, playersContainer);
                player.PlayerId = playerId;
                var human = config.Humans.Find(h => h.playerId == playerId);
                if (human == null)
                {
                    player.RobotId = robotCount;
                    var robotName = $"Robot {robotCount} (player {playerId})";
                    robots.Add(new RobotInput(robotName, robotsComportment, player, gameSpeed));
                    robotCount++;
                }
                else
                {
                    player.HumanId = humanCount;
                    var canon = Instantiate(canonPrefab, canonsContainer);
                    canon.Init(playerId, humanCount);
                    var humanInput = HumanInput.Instantiate(humanPrefab, humansContainer, human, player, canon);
                    humanInput.name = $"Human {humanCount} (player {playerId}, canon {humanCount})";
                    humanCount++;
                }
            }

            Invoke(nameof(StartGame), 1);
        }

        private void StartGame()
        {
            gameStartEvent.Raise();
            gameSpeed.Value = 1f;
            foreach (var robot in robots) robot.StartGame();
        }

        private void OnDrawGizmos()
        {
            var x = goal.Value;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(x, -10), new Vector3(x, 10));
        }
    }
}