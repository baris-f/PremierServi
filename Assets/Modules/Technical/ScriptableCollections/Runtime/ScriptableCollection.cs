using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.ScriptableCollections.Runtime
{
    public abstract class ScriptableCollection<T> : ScriptableObject where T : ScriptableObject
    {
        public List<T> collection = new();
        [HideInInspector] public string path;
    }
}