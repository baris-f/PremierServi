using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public class OpenScenesAdditive : MonoBehaviour
    {
        private enum ActivationRule
        {
            None,
            Awake,
            Start,
            Enable,
        }

        [Header("Settings")]
        [SerializeField] private ActivationRule whenToTrigger;
        [SerializeField] private List<int> scenesToLoadFromBuildIndex;

        private void Start()
        {
            if (whenToTrigger == ActivationRule.Start) LoadScenes();
        }

        private void Awake()
        {
            if (whenToTrigger == ActivationRule.Awake) LoadScenes();
        }

        private void OnEnable()
        {
            if (whenToTrigger == ActivationRule.Enable) LoadScenes();
        }

        private void LoadScenes()
        {
            foreach (var sceneIndex in scenesToLoadFromBuildIndex)
                SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }
    }
}