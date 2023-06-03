using System.Collections;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;

namespace Modules.SceneLoader.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        private static readonly int StartId = Animator.StringToHash("Start");

        [Header("Config")]
        [SerializeField] private Animator animator;
        [SerializeField] private int transitionTimeInMs = 1000;

        private void Awake() => animator.gameObject.SetActive(true);

        public async void LoadScene(string sceneName) => await DoTransition(sceneName);
        public async void LoadScene(SceneReference scene) => await DoTransition(scene.Name);

        private async Task DoTransition(string sceneName)
        {
            animator.SetTrigger(StartId);
            await Task.Delay(transitionTimeInMs);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}