using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    public class RandomIa : BaseIa
    {
        protected override async Task Think(string robotName, PlayerController player)
        {
            while (State.Started)
            {
                var rndAction = Random.Range(0, 4);
                var rndTickDuration = Random.Range(1, 20);
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

                // Il doit toujours y avoir au moins un WaitForOneTick() dans chaque loop pour avoid le spam
                await WaitForTicks(rndTickDuration);
            }
        }
    }
}