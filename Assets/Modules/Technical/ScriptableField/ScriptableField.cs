using System;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.ScriptableField
{
    public class ScriptableField<T> : ScriptableObject
    {
        [SerializeField] private T value;

        public T Value
        {
            get => value;
            set
            {
                value = Value;
                NotifyChange();
            }
        }

        public event Action<T> OnValueChanged;

        [Button] private void NotifyChange() => OnValueChanged?.Invoke(value);
        public void ChangeSilently(T newValue) => value = newValue;
    }
}