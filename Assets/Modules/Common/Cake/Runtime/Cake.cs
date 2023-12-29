using UnityEngine;

namespace Modules.Common.Cake.Runtime
{
    public class Cake : ScriptableObject
    {
        public Sprite sprite;
        public string displayName;

        public void Initialize(string newName)
        {
            name = newName;
            displayName = newName;
        }

        public void ApplyNameChange() => name = displayName;
    }
}