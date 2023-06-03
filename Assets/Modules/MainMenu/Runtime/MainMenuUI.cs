using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.MainMenu.Runtime
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> states = new();

        public void SelectState(Object target) => SelectState(target.name);

        public void SelectState(string targetName)
        {
            foreach (var state in states)
                state.SetActive(state.name == targetName);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}