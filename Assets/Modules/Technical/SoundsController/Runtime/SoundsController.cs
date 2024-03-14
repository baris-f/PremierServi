using System.Collections;
using System.Collections.Generic;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptUtils.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Modules.Technical.SoundsController.Runtime
{
    public class SoundsController : SingletonMonoBehaviour<SoundsController>
    {
        public enum Output
        {
            None,
            Music,
            Effects,
            UiEffects,
            Voices
        }

        [Header("References")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private List<ScriptableFloat> volumeFields = new();

        private void Start()
        {
            foreach (var volumeField in volumeFields)
            {
                if (volumeField == null) continue;
                var success = audioMixer.SetFloat(volumeField.name, volumeField.Value);
                if (!success) continue;
                volumeField.OnValueChanged += value => audioMixer.SetFloat(volumeField.name, value);
            }
        }

        public void OnPlayBackgroundMusicEvent(MinimalData data)
        {
            if (data is not PlayClipEvent.ClipData clipData) return;
            PlayClip(clipData.clip, backgroundMusicSource, clipData.output, oneShot: clipData.oneShot);
        }

        private void PlayClip(AudioClip clip, AudioSource source, Output output = Output.None, bool oneShot = true)
        {
            if (output != Output.None)
            {
                var groups = audioMixer.FindMatchingGroups($"{output}");
                if (groups is { Length: > 0 })
                    source.outputAudioMixerGroup = groups[0];
            }

            source.clip = clip;
            source.loop = !oneShot;
            source.Play();
        }
    }
}