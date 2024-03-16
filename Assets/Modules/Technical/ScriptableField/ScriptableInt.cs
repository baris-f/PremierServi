using UnityEngine;

namespace Modules.Technical.ScriptableField
{
    [CreateAssetMenu(fileName = "New Scriptable Int", menuName = "Scriptable Fields/Int")]
    public class ScriptableInt: ScriptableField<int>
    {
        protected override void SaveToPlayerPrefs(string key, int value) => PlayerPrefs.SetInt(key, value);
        protected override int LoadFomPlayerPrefs(string key) => PlayerPrefs.GetInt(key);
    }
}