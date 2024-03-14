using System;
using UnityEngine;

namespace Modules.Common.Cake.Runtime
{
    public class Cake : ScriptableObject
    {
        public enum CakeType
        {
            None,
            All,
            Classic,
            Eclair,
            Pain,
        }
        
        public Sprite sprite;
        public string displayName;
        public CakeType type;
        
        public void Initialize(string newName, CakeType newType = CakeType.Classic)
        {
            name = newName;
            displayName = newName;
            type = newType;
        }

        public void ApplyNameChange() => name = displayName;

        public bool CompareType(CakeType other) => other == CakeType.All || type == other;
    }
}