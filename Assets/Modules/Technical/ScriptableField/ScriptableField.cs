using System;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.ScriptableField
{
    public abstract class ScriptableField<T> : ScriptableObject
    {
        [SerializeField] private T value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                NotifyChange();
            }
        }

        public event Action<T> OnValueChanged;

        [Button] protected void NotifyChange() => OnValueChanged?.Invoke(value);
        public void ChangeSilently(T newValue) => value = newValue;
    }
}