using System.Threading.Tasks;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    public class RandomIa : BaseIa
    {
        [Header("RandomIa Config")]
        [SerializeField] private ActionToPerform[] allowedActions =
            { ActionToPerform.Walk, ActionToPerform.Run, ActionToPerform.Stop };

        protected override async Task Think()
        {
            while (Started)
            {
                var rndAction = allowedActions.GetRandom();
                var rndTickDuration = Random.Range(1, 20);
                PerformAction(rndAction);

                await WaitForTicks(rndTickDuration);
            }
        }
    }
}