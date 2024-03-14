using UnityEngine;

namespace Modules.Technical.SoundsController.Runtime
{
    public class AutoPlaySound : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AudioClip clipToPlay;
        [SerializeField] private bool oneShot;
        [SerializeField] private float delay;

        [Header("References")]
        [SerializeField] private PlayClipEvent clipEvent;

        private void Start() => Invoke(nameof(Play), delay);

        private void Play() => clipEvent.Raise(clipToPlay, oneShot);
    }
}