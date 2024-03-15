using System;
using Modules.Technical.GameConfig.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Scenes._0___MainMenu.Runtime
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
        [SerializeField] private TextMeshProUGUI deviceDescription;

        private void Awake() => Connected(false);

        public void Connected(bool value, string deviceName = "unknown")
        {
            connected.SetActive(value);
            notConnected.SetActive(!value);
            deviceDescription.text = $"{deviceName} connected";
        }
        
        public JoyConColors Color => JoyConColors.Colors[color];

        private void Start() => background.color = JoyConColors.Colors[color].BodyColor;
    }
}