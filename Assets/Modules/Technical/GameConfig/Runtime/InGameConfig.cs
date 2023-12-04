using System.Collections.Generic;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{
    public class InGameConfig : RuntimeScriptableObject
    {
        [Header("Dynamic Config")]
        [SaveAtRuntime] public List<Human> humans;
        [SaveAtRuntime] public List<Round> rounds;
    }
}