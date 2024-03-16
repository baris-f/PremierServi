using System;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.ScriptableField
{
    public abstract class ScriptableField<T> : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private T value;
        [SerializeField] private T defaultValue;

        public event Action<T> OnValueChanged;
        private string key;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                NotifyChange();
            }
        }

        private void OnValidate() => key = $"{name}_field";

        public void ChangeSilently(T newValue) => value = newValue;

        protected abstract void SaveToPlayerPrefs(string key, T value);
        protected abstract T LoadFomPlayerPrefs(string key);

        [Button(header: "Callback")]
        private void NotifyChange() => OnValueChanged?.Invoke(value);

        [Button(horizontal: true, header: "Player Prefs")]
        public void SaveToPlayerPrefs() => SaveToPlayerPrefs(key, Value);

        [Button(horizontal: true)]
        public void LoadFromPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey(key))
                SaveToPlayerPrefs(key, defaultValue);
            Value = LoadFomPlayerPrefs(key);
        }

        [Button]
        public void ClearPlayerPrefs() => PlayerPrefs.DeleteKey(key);
    }
}