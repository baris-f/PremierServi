using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.ScriptableEvents.Runtime.LocalEvents
{
    [CreateAssetMenu(fileName = "New Simple Local Event", menuName = "Scriptable Events/Simple Local")]
    public class SimpleLocalEvent : LocalEvent<MinimalData>
    {
        private readonly MinimalData data = new();
        [Button] public void Raise() => Raise(data);
    }
}