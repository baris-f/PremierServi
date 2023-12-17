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

            inGameConfig.SortHumansByScore();
            
            foreach (var human in inGameConfig.Humans)
            {
                var text = Instantiate(resultsPrefab, resultsContainer);
                text.text = $"Player {human.playerId} - {human.score} points";
                text.color = JoyConColors.Colors[human.color].BodyColor; // todo remplacer par meilleur facon de store color (branche status)
            }
        }

        public void OnContinue() => mainMenu.Raise();
    }
}