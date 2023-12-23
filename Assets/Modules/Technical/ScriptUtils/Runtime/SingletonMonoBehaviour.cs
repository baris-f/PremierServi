using UnityEngine;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public class SingletonMonoBehaviour : MonoBehaviour
    {
        private static SingletonMonoBehaviour instance;

        protected SingletonMonoBehaviour()
        {
            if (instance == null) instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}