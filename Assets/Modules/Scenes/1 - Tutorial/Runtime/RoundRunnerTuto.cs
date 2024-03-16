using Modules.Common.CustomEvents.Runtime;
using Modules.Scenes._2._1___Classic.Runtime;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.SceneLoader.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.Playables;

namespace Modules.Scenes._1___Tutorial.Runtime
{
    public class RoundRunnerTuto : RoundRunnerClassic
    {
        [Header("Tuto Config")]
        [SerializeField] private PlayableDirector timeline;
        [SerializeField] private JoyConColors.ColorName forcedColor = JoyConColors.ColorName.PastelGreen;
        [SerializeField] private LoadSceneEvent mainMenu;

        private new void Start() => StartTuto(true);

        private void StartTuto(bool startTimeline)
        {
            var robotCount = 0;
            var modeDescriptor = config.CurrentModeDescriptor;
            var randomPrefabArray = CreatePlayerPrefabArray(modeDescriptor.NbPlayers);

            Reset();

            for (var playerId = 0; playerId < modeDescriptor.NbPlayers; playerId++)
            {
                var player = Instantiate(randomPrefabArray.PickRandom(), playersLayout.transform);
                if (playerId == modeDescriptor.NbPlayers / 2)
                {
                    var human = new Human
                    {
                        color = JoyConColors.Colors[forcedColor],
                        playerId = playerId,
                    };
                    var input = SetupHuman(player, playerId, 0, modeDescriptor, human);
                    var canon = Instantiate(canonPrefab, canonsLayout.transform);
                    canon.Init(playerId, human.color, 0, modeDescriptor.NbBullets);
                    input.Init(player, canon);
                }
                else
                {
                    SetupRobot(player, playerId, robotCount, modeDescriptor);
                    robotCount++;
                }
            }

            Invoke(nameof(RefreshLayouts), .1f);
            if (startTimeline)
                timeline.Play();
            else
                StartGame();
        }

        public void StartGameFromTimeline()
        {
            gameSpeed.OnValueChanged -= OnGameSpeedChange;
            StartGame();
        }

        private void OnGameSpeedChange(float value)
        {
            if (timeline == null) return;
            if (value <= 0)
                timeline.Pause();
            else
                timeline.Play();
        }

        public new async void OnPlayerWin(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData) return;
            gameSpeed.Value = -1;
            if (playerData.type == PlayerEvent.Type.Human)
            {
                await results.Open($"You ate the cake !", true);
                mainMenu.Raise();
            }
            else
            {
                await results.Open($"Somebody else ate the cake ! Try Again.", true);
                StartTuto(false);
            }
        }

        public new async void OnPlayerDeath(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData) return;
            if (playerData.type != PlayerEvent.Type.Human) return;
            gameSpeed.Value = -1;
            await results.Open($"You shot yourself ! Be careful..", true);
            StartTuto(false);
        }

        private new void OnEnable()
        {
            base.OnEnable();
            gameSpeed.OnValueChanged += OnGameSpeedChange;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            gameSpeed.OnValueChanged -= OnGameSpeedChange;
        }
    }
}