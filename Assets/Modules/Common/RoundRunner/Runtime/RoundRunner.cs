﻿using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.GameRunner.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Common.Status;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Common.RoundRunner.Runtime
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
        [SerializeField] private StatusController statusPrefab;

        [Header("Containers")]
        [SerializeField] private TransformLayout playersLayout;
        [SerializeField] private TransformLayout canonsLayout;
        [SerializeField] private Transform statusContainer;
        [SerializeField] private Transform humansContainer;

        [Header("Assets")]
        [SerializeField] private Sprite[] cakeList; //il y a surement moyen de faire m,ieux notament garder leur ID et faire une liste "externe"
        
        [FormerlySerializedAs("cake")]
        [Header("References")]
        [SerializeField] private Cake.Runtime.CakeBehaviour cakeBehaviour; //il y a surement moyen de faire m,ieux notament garder leur ID et faire une liste "externe"

        private int cakeId;
        
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
            statusContainer.DestroyAllChildren();
            var humanPlayerIds =
                UtilsGenerator.GenerateRandomNumbersInRange(0, modeDescriptor.NbPlayers, config.Humans.Count);

            cakeId = Random.Range(0, cakeList.Length);
            cakeBehaviour.SetCake(cakeList[cakeId], cakeId);
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
                    var status = Instantiate(statusPrefab, statusContainer.transform);
                    status.Initialize(human.playerId, human.color, modeDescriptor.NbBullets);
                    var canon = Instantiate(canonPrefab, canonsLayout.transform);
                    canon.Init(playerId, human.color, humanCount, modeDescriptor.NbBullets);
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
            gameSpeed.Value = -1;
            if (playerData.type == PlayerEvent.Type.Human) config.AddPoints(playerData.id, 1);
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