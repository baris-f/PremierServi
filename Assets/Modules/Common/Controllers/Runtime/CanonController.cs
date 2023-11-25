using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class CanonController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int nbShots;

        private int curShot;
        
        // gestion des animations
        protected void Update()
        {
            if (curShot >= nbShots)
            {
                Debug.Log("Canon dead");
            }
        }
    }
}