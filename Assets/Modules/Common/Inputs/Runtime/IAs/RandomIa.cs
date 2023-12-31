using System.Threading.Tasks;
using Modules.Common.Controllers.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime.IAs
{
    public class RandomIa : BaseIa
    {
        protected override async Task Think()
        {
            while (State.Started)
            {
                var rndAction = UtilsGenerator.RandomInEnum(new[] { ActionToPerform.None });
                var rndTickDuration = Random.Range(1, 20);
                PerformAction(rndAction);
                
                Debug.Log("");
                await WaitForTicks(rndTickDuration);
            }
        }
    }
}