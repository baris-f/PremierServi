using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    public class TestingIa : BaseIa
    {
        private enum ActionToPerform
        {
            None,
            Walk,
            Run,
            Taunt,
            Stop
        }

        [SerializeField] private List<ActionToPerform> sequence = new();
        [SerializeField] private int actionTime = 10;
        [SerializeField] private bool loop = true;

        protected override async Task Think(PlayerController player)
        {
            while (State.Started)
            {
                var queue = new Queue<ActionToPerform>(sequence);
                while (State.Started && queue.Count > 0)
                {
                    switch (queue.Dequeue())
                    {
                        case ActionToPerform.None:
                            break;
                        case ActionToPerform.Walk:
                            player.StartWalking();
                            break;
                        case ActionToPerform.Run:
                            player.StartRunning();
                            break;
                        case ActionToPerform.Taunt:
                            player.StartTaunt();
                            break;
                        case ActionToPerform.Stop:
                            player.Stop();
                            break;
                    }

                    await WaitForTicks(actionTime);
                }

                if (!loop) break;
            }
        }
    }
}