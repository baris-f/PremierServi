using Modules.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.ScriptableEvents.Runtime.LocalEvents
{
    [CreateAssetMenu(fileName = "New Simple Local Event", menuName = "ScriptableEvents/Simple Local Event")]
    public class SimpleLocalEvent : LocalEvent
    {
        [Button] public void Raise() => Raise(new MinimalData());
    }
}