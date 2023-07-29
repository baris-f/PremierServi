using Eflatun.SceneReference;
using UnityEngine;

namespace GameManagerSystem
{
    public class SceneManager : MonoBehaviour
    {
        [Header("Music Config")] public bool playMusicOnLoad;
        public AudioClip musicToPlay;
        [Header("Other Config")] public SceneReference mainMenuScene;

        private void Start()
        {
        }

        public void LoadScene(SceneReference scene) => UnityEngine.SceneManagement.SceneManager.LoadScene(scene.BuildIndex);
        public void GoMainMenu() => LoadScene(mainMenuScene);

        public void Quit()
        {
           Application.Quit();
        }
        
    }
}