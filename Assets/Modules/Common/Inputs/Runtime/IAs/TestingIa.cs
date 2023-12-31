using System.Collections.Generic;
using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    public class TestingIa : BaseIa
    {
        [SerializeField] private List<ActionToPerform> sequence = new();
        [SerializeField] private int actionTime = 10;
        [SerializeField] private bool loop = true;

        protected override async Task Think()
        {
            while (State.Started)
            {
                var queue = new Queue<ActionToPerform>(sequence);
                while (State.Started && queue.Count > 0)
                {
                    PerformAction(queue.Dequeue());
                    await WaitForTicks(actionTime);
                }

                if (!loop) break;
            }
        }
    }
}