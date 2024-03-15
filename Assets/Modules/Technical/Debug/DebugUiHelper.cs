using IngameDebugConsole;
using RuntimeInspectorNamespace;
using UnityEngine;

namespace Modules.Technical.Debug
{
    public class DebugUiHelper : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DebugLogPopup debugLogPopup;
        [SerializeField] private RuntimeHierarchy runtimeHierarchy;
        [SerializeField] private RuntimeInspector runtimeInspector;

        [Header("Debug")]
        [SerializeField] private int pressCounter;
        [SerializeField] private bool canOpenHierarchy;
        [SerializeField] private bool hierarchyOpened;

        private void Start()
        {
            canOpenHierarchy = UnityEngine.Debug.isDebugBuild;
            canOpenHierarchy = false;
            ToggleHierarchy(true);
            CheckPopup();
        }

        private void Update()
        {
            if (canOpenHierarchy && Input.GetKeyUp(KeyCode.O)) ToggleHierarchy();
            if (Input.GetKeyUp(KeyCode.P)) pressCounter++;
            if (pressCounter < 5) return;
            pressCounter = 0;
            canOpenHierarchy = !canOpenHierarchy;
            CheckPopup();
        }

        private void CheckPopup() => debugLogPopup.gameObject.SetActive(canOpenHierarchy);

        private void ToggleHierarchy(bool forceClosed = false)
        {
            hierarchyOpened = !hierarchyOpened;
            if (forceClosed) hierarchyOpened = false;
            runtimeHierarchy.gameObject.SetActive(hierarchyOpened);
            runtimeInspector.gameObject.SetActive(hierarchyOpened);
        }
    }
}