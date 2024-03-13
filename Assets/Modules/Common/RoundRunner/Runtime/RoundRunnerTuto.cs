using UnityEngine;
using UnityEngine.Playables;

namespace Modules.Common.RoundRunner.Runtime
{
    public class RoundRunnerTuto : RoundRunnerClassic
    {
        [Header("Tuto Config")]
        [SerializeField] protected float startTutoDelay = 1;
        [SerializeField] private PlayableDirector timeline;

        private new void Start()
        {
            InitGame();
            Invoke(nameof(StartTuto), startTutoDelay);
        }

        private void StartTuto() => timeline.Play();

        public void StartGameFromTimeline() => StartGame();

        private void OnGameSpeedChange(float value)
        {
            if (value <= 0)
                timeline.Pause();
            else
                timeline.Play();
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