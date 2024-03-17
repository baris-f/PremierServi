using System;
using Modules.Technical.ScriptUtils.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.ScriptableField
{
    public abstract class ScriptableArrays<T> : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private T[] values;

        public event Action<T[]> OnValueChanged;

        public T[] Values
        {
            get => values;
            set
            {
                this.values = value;
                NotifyChange();
            }
        }

        public T GetRandom() => Values.GetRandom();
        
        public void ChangeSilently(T[] newValue) => values = newValue;

        [Button(header: "Callback")]
        private void NotifyChange() => OnValueChanged?.Invoke(values);
    }
}