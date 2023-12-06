using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.SceneLoader.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Scenes.GameResult.Runtime
{
    public class GameResult : MonoBehaviour
    {
        [Header("UI Refs")]
        [SerializeField] private RectTransform resultsContainer;
        [SerializeField] private TextMeshProUGUI resultsPrefab;

        [Header("References")]
        [SerializeField] private InGameConfig inGameConfig;
        [SerializeField] private PlayerInput input;

        [Header("Events")]
        [SerializeField] private LoadSceneEvent mainMenu;

        #region Input Setup

        private InputAction submit;
        private void Awake() => submit = input.actions["Submit"];
        private void OnEnable() => submit.performed += OnSubmit;
        private void OnDisable() => submit.performed -= OnSubmit;
        private void OnSubmit(InputAction.CallbackContext obj) => OnContinue();

        #endregion

        private void Start()
        {
            resultsContainer.DestroyAllChildren();
            foreach (var entry in inGameConfig.Scores)
            {
                var text = Instantiate(resultsPrefab, resultsContainer);
                text.text = $"{entry.Key} - {entry.Value} points";
            }
        }

        public void OnContinue() => mainMenu.Raise();
    }
}