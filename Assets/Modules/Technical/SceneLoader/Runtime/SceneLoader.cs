using System.Threading.Tasks;
using Modules.Technical.ScriptableEvents.Runtime;
using UnityEngine;

namespace Modules.Technical.SceneLoader.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        private static readonly int StartId = Animator.StringToHash("Start");

        [Header("Config")]
        [SerializeField] private Animator animator;
        [SerializeField] private int transitionTimeInMs = 1000;

        private void Awake() => animator.gameObject.SetActive(true);

        public void LoadSceneCallback(MinimalData data)
        {
            if (data is not LoadSceneEvent.LoadSceneData sceneData) return;
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