using System;
using Modules.Technical.GameConfig.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Scenes.MainMenu.Runtime
{
    [Serializable]
    public class PlayerCard : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private JoyConColors.ColorName color;

        [Header("Ui refs")]
        [SerializeField] private GameObject notConnected;
        [SerializeField] private GameObject connected;
        [SerializeField] private Image background;

        public bool Connected
        {
            set
            {
                connected.SetActive(value);
                notConnected.SetActive(!value);
            }
        }
        public JoyConColors Color => JoyConColors.Colors[color];

        private void Start() => background.color = JoyConColors.Colors[color].BodyColor;
    }
}