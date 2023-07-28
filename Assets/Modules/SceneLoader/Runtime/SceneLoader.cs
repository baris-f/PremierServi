using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modules.ScriptableEvents.Runtime;
using UnityEngine;

namespace Modules.SceneLoader.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        private static readonly int StartId = Animator.StringToHash("Start");

        [Header("Config")]
        [SerializeField] private Animator animator;
        [SerializeField] private int transitionTimeInMs = 1000;
        [SerializeField] private InspectorEventListener inspectorEventListener;

        [SerializeField] private List<LoadSceneEvent> sceneEvents;

        private void Awake()
        {
            animator.gameObject.SetActive(true);
            foreach (var sceneEvent in sceneEvents) inspectorEventListener.AddCallback(sceneEvent, LoadSceneCallback);
        }

        private void LoadSceneCallback(MinimalData data)
        {
            if (data is not LoadSceneEvent.LoadSceneData sceneData)
                return;
            LoadScene(sceneData.sceneName);
        }

        public async void LoadScene(string sceneName) => await DoTransition(sceneName);

        private async Task DoTransition(string sceneName)
        {
            animator.SetTrigger(StartId);
            await Task.Delay(transitionTimeInMs);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}