using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.SceneLoader.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Modules.Scenes._3___GameResult.Runtime
{
    public class GameResult : MonoBehaviour
    {
        [Header("UI Refs")]
        [SerializeField] private Image background;
        [SerializeField] private RectTransform resultsContainer;
        [SerializeField] private TextMeshProUGUI resultsPrefab;
        [SerializeField] private Button buttonToSelect;

        [Header("Prefab")]
        [SerializeField] private Image cakeIconPrefab;

        [Header("References")]
        [SerializeField] private Sprite[] backgrounds;
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

            background.sprite = backgrounds.GetRandom();
            foreach (var human in inGameConfig.Humans)
            {
                var text = Instantiate(resultsPrefab, resultsContainer);
                text.text = $"Player {human.humanId}";
                text.color = human.color.BodyColor;
                human.eatenCakes.ForEach(cake =>
                {
                    var cakeIcon = Instantiate(cakeIconPrefab, text.transform.GetChild(0));
                    cakeIcon.sprite = cake.sprite;
                });
            }

            if (buttonToSelect != null) buttonToSelect.Select();
        }

        public void OnContinue() => mainMenu.Raise();
    }
}