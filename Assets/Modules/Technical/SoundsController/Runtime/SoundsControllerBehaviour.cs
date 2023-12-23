using System;
using System.Collections.Generic;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.SoundsController.Runtime
{
    public class SoundsControllerBehaviour : SingletonMonoBehaviour<SoundsControllerBehaviour>
    {
        [Header("References")]
        [SerializeField] private SoundsController soundsController;
        [SerializeField] private AudioSource backgroundMusicSource;

        private new void Awake()
        {
            base.Awake();
            soundsController.BackgroundAudioSource = backgroundMusicSource;
        }

        public void OnSetVolumeEvent(MinimalData data)
        {
            if (data is not VolumeEvent.VolumeData soundData) return;
            soundsController.SetVolume(soundData.output, soundData.mute ? -80 : soundData.value);
        }

        public void OnPlayClipEvent(MinimalData data)
        {
            if (data is not PlayClipEvent.ClipData clipData) return;
            soundsController.Play(clipData.clip, clipData.output, clipData.oneShot);
        }
    }
}