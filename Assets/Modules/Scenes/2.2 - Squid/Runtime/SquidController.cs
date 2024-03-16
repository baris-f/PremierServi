using Modules.Common.Controllers.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._2___Squid.Runtime
{
    public class SquidController : ShootController
    {
        public new void Init(int newPlayerId, int humanId, int maxBullets)
        {
            base.Init(newPlayerId, humanId, maxBullets);
            name = $"Squid controller {PlayerData.typeId} (player {PlayerData.id}, human {PlayerData.typeId})";
        }

        public override void Move(Vector2 amount)
        {
        }

        public override void Fire() => FireInternal();

        protected override void DisableShooter() => DisableShooterInternal();
    }
}