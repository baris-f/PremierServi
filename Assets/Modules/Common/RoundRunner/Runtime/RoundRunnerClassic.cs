using System.Collections.Generic;
using Modules.Common.Cake.Runtime;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Common.Status;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Common.RoundRunner.Runtime
{
    public class RoundRunnerClassic : RoundRunner
    {
        [Header("Config")]
        [SerializeField] protected float startGameDelay = 1;
        [SerializeField] protected InGameConfig config;
        [SerializeField] private BaseIa robotsComportment;
        [SerializeField] protected ResultsPopup results;

        [Header("Prefabs")]
        [SerializeField] private List<PlayerController> playerPrefabs = new();
        [SerializeField] private List<GameObject> backgrounds = new();
        [SerializeField] private CanonController canonPrefab;
        [SerializeField] private HumanInput humanPrefab;
        [SerializeField] private StatusController statusPrefab;

        [Header("Containers")]
        [SerializeField] private TransformLayout playersLayout;
        [SerializeField] private TransformLayout canonsLayout;
        [SerializeField] private Transform statusContainer;
        [SerializeField] private Transform humansContainer;

        [Header("References")]
        [SerializeField] private CakeBehaviour cakeBehaviour;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent gameStartEvent;

        [Header("Fields")]
        [SerializeField]
        protected ScriptableFloat gameSpeed;
        [SerializeField] private ScriptableFloat goal;

        [Header("Debug")]
        [SerializeField] private List<RobotInput> robots = new();

        protected void Start()
        {
            InitGame();
            Invoke(nameof(StartGame), startGameDelay);
        }

        protected void InitGame()
        {
            var rndBackId = Random.Range(0, backgrounds.Count);
            for (var i = 0; i < backgrounds.Count; i++)
                backgrounds[i].SetActive(i == rndBackId);
            var modeDescriptor = config.CurrentModeDescriptor;
            humansContainer.DestroyAllChildren();
            statusContainer.DestroyAllChildren();
            var humanPlayerIds =
                UtilsGenerator.GenerateRandomNumbersInRange(0, modeDescriptor.NbPlayers, config.Humans.Count);

            foreach (var human in config.Humans)
            {
                if (human.playerId == -1 || human.playerId >= modeDescriptor.NbPlayers)
                    human.playerId = humanPlayerIds[0];
                humanPlayerIds.RemoveAt(0);
            }

            int robotCount = 0, humanCount = 0;

            var randomPrefabArray = new List<PlayerController>();
            var nbToPut = modeDescriptor.NbPlayers / (playerPrefabs.Count + 1);
            foreach (var prefab in playerPrefabs)
                for (var j = 0; j < nbToPut; j++)
                    randomPrefabArray.Add(prefab);
            while(randomPrefabArray.Count < modeDescriptor.NbPlayers)
                randomPrefabArray.Add(playerPrefabs.GetRandom());

            for (var playerId = 0; playerId < modeDescriptor.NbPlayers; playerId++)
            {
                var prefab = randomPrefabArray.PickRandom();
                var player = Instantiate(prefab, playersLayout.transform);

                var human = config.Humans.Find(h => h.playerId == playerId);
                if (human == null)
                {
                    player.Init(PlayerEvent.Type.Robot, playerId, robotCount, modeDescriptor.WalkSpeed,
                        modeDescriptor.RunSpeed);
                    var robotName = $"Robot {robotCount} (player {playerId})";
                    robots.Add(new RobotInput(robotName, robotsComportment, player, gameSpeed));
                    robotCount++;
                }
                else
                {
                    player.Init(PlayerEvent.Type.Human, playerId, humanCount, modeDescriptor.WalkSpeed,
                        modeDescriptor.RunSpeed);
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
            if (playerData.type == PlayerEvent.Type.Human)
            {
                config.GetHumanById(playerData.id).eatenCakes.Add(cakeBehaviour.GetCake());
                var r = await results.Open($"{playerData.type} {playerData.typeId} ate the cake !", true);
            }
            else
            {
                var r = await results.Open($"Somebody else ate the cake !", true);
            }

            config.GoNextRound();
            config.LoadRound();
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