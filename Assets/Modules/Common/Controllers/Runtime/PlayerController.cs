using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class PlayerController : Controller
    {
        [Header("Config")]
        [SerializeField] private RobotInput defaultInput;

        [Header("Events")]
        [SerializeField] private PlayerWinEvent playerWin;

        private int playerId;
        private Transform goal;

        public void Setup(int newPlayerId, Transform newGoal)
        {
            playerId = newPlayerId;
            goal = newGoal;
        }

        private void Awake()
        {
            Input = defaultInput;
            name = $"{defaultInput.name}";
        }

        protected override void OnUpdate()
        {
            if (transform.position.x > goal.position.x)
            {
                OnGamePause();
                Debug.Log("win yay");
                playerWin.Raise(playerId);
            }
        }
    }
}