using System.Runtime.CompilerServices;
using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Common.Status
{
    public class StatusController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image[] elemToColor;
        [SerializeField] private LayoutGroup shotLeft;
        [SerializeField] private GameObject shotContainer;
        private Image[] shotLeftRefs;

        [Header("Color settings")]
        [SerializeField] private float desaturationAmount = 0.2f;
        [SerializeField] private float darkenAmount = 0.25f;

        private JoyConColors color;
        private int playerID;
        private int numberOfShotLeft;

        public void Initialize(int newPlayerID, JoyConColors newColor, int ammo = 3)
        {
            playerID = newPlayerID;
            color = newColor;
            numberOfShotLeft = ammo;
            shotLeftRefs = new Image[ammo];
            SetColor(color.BodyColor);
            for (int i = 0; i < numberOfShotLeft; i++) // a changer ofc
            {
                var shot = Instantiate(shotContainer, shotLeft.transform);
                Debug.Log(shot.transform.GetChild(0).name);
                shotLeftRefs[i] = shot.transform.GetChild(0).transform.GetComponent<Image>();
                //(shot.transform.GetComponentInChildren<Image>());
            }
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

        public void OnPlayerFire(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData || playerData.id != playerID ||
                numberOfShotLeft <= 0) return;
            shotLeftRefs[numberOfShotLeft - 1].color = Color.clear;
            numberOfShotLeft--;
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