using System;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        [Header("Singleton Config")]
        [SerializeField] private bool forceInstance;

        private static SingletonMonoBehaviour<T> instance;

        protected void Awake()
        {
            if (forceInstance)
            {
                Destroy(instance);
                instance = this;
            }
            else if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}