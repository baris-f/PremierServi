using GameManagerSystem;
using GamevrestUtils;
using UnityEngine;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("General")] public GameObject creditsCanvas;
        public SceneManager SceneManager;
        [Header("Scenes")] public SceneReference newGameScene;

        public void ShowCredits() => creditsCanvas.SetActive(true);
        public void HideCredits() => creditsCanvas.SetActive(false);

        public void NewGame()
        {
            SceneManager.LoadScene(newGameScene);
        }

    }
}