using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class CanonController : Controller
    {
        [Header("Config")]
        [SerializeField] private int nbShots;

        private int curShot;
        
        // gestion des animations
        protected override void OnUpdate()
        {
            if (curShot >= nbShots)
            {
                Debug.Log("Canon dead");
            }
        }
    }
}