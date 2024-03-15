using System.Threading.Tasks;
using Modules.Technical.ScriptableEvents.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.Technical.SceneLoader.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        private static readonly int StartId = Animator.StringToHash("Start");

        [Header("Config")]
        [SerializeField] private Animator animator;
        [SerializeField] private float transitionTime = 1;

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
            await Task.Delay((int)(transitionTime * 1000));
            SceneManager.LoadScene(sceneName);
        }
    }
}