using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    // toutes les ia doivent heriter de celle la
    // ce serait mieux avec une interface mais... unity c code avec le cul (bon jabuse mais voila)
    public abstract class BaseIa : ScriptableObject
    {
        [SerializeField] public int tickDelay = 100;
        protected RobotInput.GameState State;

        public async Task StartThinking(RobotInput.GameState newState, PlayerController newPlayer)
        {
            State = newState;
            await Think(newPlayer);
        }

        protected abstract Task Think(PlayerController player);

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
                await Task.Delay(tickDelay);
                if (!State.Paused) elapsedTicks++;
                if (!State.Started) return;
            }
        }

        protected async Task WaitForOneTick() => await Task.Delay(tickDelay);
    }
}