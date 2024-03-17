using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Common.CustomEvents.Runtime;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class CollisionDetector : MonoBehaviour
    {
        [Serializable]
        public class CollisionResponse
        {
            public PlayerEvent @event;
            public bool destroy;
            public string tag;
        }

        private PlayerEvent.PlayerData playerData;
        private Dictionary<string, CollisionResponse> responses = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!responses.TryGetValue(other.transform.tag, out var response)) return;
            if (response.destroy) Destroy(other.transform.parent.gameObject);
            response.@event.Raise(playerData);
        }

        public void Init(PlayerEvent.PlayerData newPlayerData, params CollisionResponse[] newResponses)
        {
            playerData = newPlayerData;
            foreach (var response in newResponses)
                responses.Add(response.tag, response);
        }
    }
}