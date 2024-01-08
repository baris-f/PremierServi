using System;
using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
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

        [Header("Base Config")]
        [SerializeField] public int tickDelayInMs = 100;
        [SerializeField] private bool verbose;

        private ScriptableFloat gameSpeed;
        private PlayerController playerController;

        protected bool Paused => gameSpeed.Value == 0;
        protected bool Started => gameSpeed.Value >= 0;

        public async Task StartThinking(ScriptableFloat newGameSpeed, PlayerController newPlayer)
        {
            if (verbose)
                Debug.Log($"{newPlayer.name} starts Thinking");
            gameSpeed = newGameSpeed;
            playerController = newPlayer;
            try
            {
                await Think();
                playerController.Stop();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        protected abstract Task Think();

        protected async Task WaitForTicks(int nbTicks)
        {
            if (verbose)
                Debug.Log($"Wait {nbTicks} ticks ({nbTicks * tickDelayInMs / 1000})");
            if (nbTicks == 1)
            {
                await WaitForOneTick();
                return;
            }

            var elapsedTicks = 0;
            while (elapsedTicks <= nbTicks)
            {
                await WaitForOneTick();
                if (!Paused) elapsedTicks++;
                if (!Started) return;
            }
        }

        protected void PerformAction(ActionToPerform action)
        {
            if (playerController == null) return;
            if (verbose)
                Debug.Log($"{playerController.name} performs action {action.ToString()}");
            switch (action)
            {
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
                case ActionToPerform.None:
                default:
                    break;
            }
        }

        protected async Task WaitForOneTick() => await Task.Delay(tickDelayInMs);
    }
}