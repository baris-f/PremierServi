using UnityEngine;

namespace Modules.Technical.ScriptableField.Implementations
{
    [CreateAssetMenu(fileName = "New Scriptable Bool", menuName = "Scriptable Fields/Bool")]
    public class ScriptableBool : ScriptableField<bool>
    {
        protected override void SaveToPlayerPrefs(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
        protected override bool LoadFomPlayerPrefs(string key) => PlayerPrefs.GetInt(key) == 1;
    }
}