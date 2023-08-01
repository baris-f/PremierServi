using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.ScriptableEvents.Runtime.LocalEvents
{
    [CreateAssetMenu(fileName = "New Simple Local Event", menuName = "ScriptableEvents/Simple Local Event")]
    public class SimpleLocalEvent : LocalEvent<MinimalData>
    {
        private readonly MinimalData data = new();
        [Button] public void Raise() => Raise(data);
    }
}