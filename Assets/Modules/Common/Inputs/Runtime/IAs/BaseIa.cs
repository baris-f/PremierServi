using System;
using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    // toutes les ia doivent heriter de celle la
    // ce serait mieux avec une interface mais... unity c code avec le cul (bon jabuse mais voila)
    public abstract class BaseIa : ScriptableObject
    {
        protected enum ActionToPerform
        {
            None,
            Walk,
            Run,
            Taunt,
            Stop
        }

        [SerializeField] public int tickDelayInMs = 100;
        [SerializeField] private bool verbose;
        protected RobotInput.GameState State;
        private PlayerController playerController;

        public async Task StartThinking(RobotInput.GameState newState, PlayerController newPlayer)
        {
            State = newState;
            await Think();
            playerController = newPlayer;
        }

        protected abstract Task Think();

        protected async Task WaitForTicks(int nbTicks)
        {
            if (nbTicks == 1)
            {
                await WaitForOneTick();
                return;
            }

            var elapsedTicks = 0;
            while (elapsedTicks <= nbTicks)
            {
                await Task.Delay(tickDelayInMs);
                if (!State.Paused) elapsedTicks++;
                if (!State.Started) return;
            }
        }

        protected void PerformAction(ActionToPerform action)
        {
            if (verbose)
                Debug.Log($"{playerController.name} performs action {action.ToString()}");
            switch (action)
            {
                case ActionToPerform.None:
                default:
                    break;
                case ActionToPerform.Walk:
                    playerController.StartWalking();
                    break;
                case ActionToPerform.Run:
                    playerController.StartRunning();
                    break;
                case ActionToPerform.Taunt:
                    playerController.StartTaunt();
                    break;
                case ActionToPerform.Stop:
                    playerController.Stop();
                    break;
            }
        }

        protected async Task WaitForOneTick() => await Task.Delay(tickDelayInMs);
    }
}