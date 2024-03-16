using UnityEngine;

namespace Modules.Technical.ScriptableField
{
    [CreateAssetMenu(fileName = "New Scriptable Float", menuName = "Scriptable Fields/Float")]
    public class ScriptableFloat : ScriptableField<float>
    {
        protected override void SaveToPlayerPrefs(string key, float value) => PlayerPrefs.SetFloat(key, value);
        protected override float LoadFomPlayerPrefs(string key) => PlayerPrefs.GetFloat(key);
    }
}