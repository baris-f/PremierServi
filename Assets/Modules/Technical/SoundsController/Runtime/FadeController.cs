using UnityEngine;
using UnityEngine.Audio;

namespace Modules.Technical.SoundsController.Runtime
{
    public class FadeController : MonoBehaviour
    {
        private const string MasterVolumeParamName = "MasterVolume";

        [Header("Settings")]
        [SerializeField] private float volume;
        [SerializeField] private bool monitorVolume;

        [Header("References")]
        [SerializeField] private AudioMixer audioMixer;

        private void FixedUpdate()
        {
            if (!monitorVolume) return;
            var newVol = volume * -80f;
            audioMixer.SetFloat(MasterVolumeParamName, newVol);
        }
    }
}