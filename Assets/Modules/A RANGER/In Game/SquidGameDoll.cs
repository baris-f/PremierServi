using UnityEngine;

namespace Modules.A_RANGER.In_Game
{
    public class SquidGameDoll : MonoBehaviour, IWeapon
    {
        private bool fired;
        public LineRenderer line;
        public GameManagerServi GameManager;
        public Animator animator;
        public Transform eyes;
        private RaycastHit2D hit;

        private void Start()
        {
            GameManager = GameObject.Find("GameManager").GetComponent<GameManagerServi>();
        }

        public void ShootLine()
        {
            var charactersList = GameManager.GetMovingCharacters();
            var lineID = 0;
            line.positionCount = charactersList.Count * 2;
            Debug.Log(line.positionCount);
            foreach (Character character in charactersList)
            {
                if (!character)
                    continue;
                line.SetPosition(lineID++, eyes.position);
                line.SetPosition(lineID++, character.transform.position);
                character.Die();
            }
        }

        public void Fire()
        {
            if (fired)
                return;
            //ShootLine(); triggered by animation
            animator.SetTrigger("Fire");
            Invoke(nameof(ReallyDestroySelf), 2f);
            fired = true;
        }

        public void DestroySelf()
        {
            if (!fired)
                ReallyDestroySelf();
        }

        void ReallyDestroySelf()
        {
            Destroy(gameObject);
        }
    }
}