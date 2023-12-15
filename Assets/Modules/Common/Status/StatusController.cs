using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Common.Status
{
    public class StatusController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image[] elemToColor;

        [Header("Color settings")]
        [SerializeField] private float desaturationAmount = 0.2f;
        [SerializeField] private float darkenAmount = 0.25f;

        private JoyConColors color;
        private int playerID;

        public void Initialize(int newPlayerID, JoyConColors newColor)
        {
            playerID = newPlayerID;
            color = newColor;
            SetColor(color.BodyColor);
        }

        private void SetColor(Color newColor)
        {
            foreach (var image in elemToColor)
                image.color = newColor;
        }

        public void OnPlayerDeath(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData) return;
            if (playerData.id == playerID)
                Die();
        }

        private void Die()
        {
            Color.RGBToHSV(color.BodyColor, out var h, out var s, out var v);
            s -= desaturationAmount;
            s = Mathf.Clamp(s, 0f, 1f);
            var subtractColor = new Color(darkenAmount, darkenAmount, darkenAmount, 0);
            var newColor = Color.HSVToRGB(h, s, v) - subtractColor;
            SetColor(newColor);
        }
    }
}