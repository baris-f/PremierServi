using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    public class SequenceIa : BaseIa
    {
        [Header("SequenceIa Config")]
        [SerializeField] private List<ActionToPerform> sequence = new();
        [SerializeField] private int actionTime = 10;
        [SerializeField] private bool loop = true;

        protected override async Task Think()
        {
            while (Started)
            {
                var queue = new Queue<ActionToPerform>(sequence);
                while (Started && queue.Count > 0)
                {
                    PerformAction(queue.Dequeue());
                    await WaitForTicks(actionTime);
                }

                if (!loop) break;
            }
        }
    }
}