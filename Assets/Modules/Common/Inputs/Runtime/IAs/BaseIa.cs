using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    // toutes les ia doivent heriter de celle la
    // ce serait mieux avec une interface mais... unity c code avec le cul (bon jabuse mais voila)
    public class BaseIa : ScriptableObject
    {
        [SerializeField] public int tickDelay = 100;

        public async Task Think(RobotInput.GameState state, PlayerController player)
        {
            while (state.Started)
            {
                await Task.Delay(tickDelay);
                if (state.Paused || !state.Started) continue;
                var rndAction = Random.Range(0, 4);
                var rndDuration = Random.Range(100, 2000);
                switch (rndAction)
                {
                    case 0:
                        player.StartWalking();
                        break;
                    case 1:
                        player.StartRunning();
                        break;
                    case 2:
                        player.Stop();
                        break;
                    case 3:
                        player.Taunt();
                        break;
                }
                
                await Task.Delay(rndDuration);
            }
        }
    }
}