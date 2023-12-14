using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Common.GameRunner.Runtime
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private Image[] elemToColor;
        [SerializeField] private int playerID;

        public void Initialize(int newPlayerID, Color newColor)
        {
            playerID = newPlayerID;
            color = newColor;
        }

        void SetColor(Color newColor)
        {
            foreach (var image in elemToColor)
            {
                image.color = newColor;
            }
        }

        void Start()
        {
            SetColor(color);
        }

        public void ListenDeath(MinimalData data) //(PlayerEvent.PlayerData playerData)
        {
            PlayerEvent.PlayerData playerData = data as PlayerEvent.PlayerData;
            if (playerData.id == playerID)
                Die();
        }

        void Die()
        {
            float desaturationAmount = 0.2f;
            float darkenAmount = 0.25f;

            Color.RGBToHSV(color, out float h, out float s, out float v);

            s -= desaturationAmount;
            s = Mathf.Clamp(s, 0f, 1f);

            SetColor(Color.HSVToRGB(h, s, v) - new Color(darkenAmount, darkenAmount, darkenAmount, 0));
        }

        void Update()
        {

        }
    }
}
