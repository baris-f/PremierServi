using UnityEngine;
using UnityEngine.Audio;

namespace Modules.Technical.SoundsController.Runtime
{
    public class SoundsController : ScriptableObject
    {
        public enum VolumeType
        {
            None,
            Music,
            Effects,
            UiEffects,
            Voices
        }

        [Header("References")]
        [SerializeField] private AudioMixer audioMixer;

        public AudioSource BackgroundAudioSource { private get; set; }

        public void SetVolume(VolumeType type, float value) => audioMixer.SetFloat($"{type}Volume", value);

        public void Play(AudioClip clip, VolumeType output = VolumeType.None, bool oneShot = true)
        {
            if (output != VolumeType.None)
            {
                var groups = audioMixer.FindMatchingGroups($"{output}");
                if (groups is { Length: > 0 })
                    BackgroundAudioSource.outputAudioMixerGroup = groups[0];
            }

            BackgroundAudioSource.clip = clip;
            BackgroundAudioSource.loop = !oneShot;
            BackgroundAudioSource.Play();
        }
    }
}