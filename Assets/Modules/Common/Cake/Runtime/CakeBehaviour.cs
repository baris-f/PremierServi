using UnityEngine;

namespace Modules.Common.Cake.Runtime
{
    public class CakeBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Cake cake;
        [SerializeField] private CakeCollection cakeList;
        
        public Cake GetCake() => cake;

        public void SetCake(Cake newCake)
        {
            cake = newCake;
            spriteRenderer.sprite = cake.sprite;
        }

        public void Start()
        {
            SetCake(cakeList.GetRandomCake());
            spriteRenderer.sprite = cake.sprite;
        }

        // private IEnumerator ShiftShape() //pretty much useless but might be good for some menu or make videos / ads
        // {
        //     while (true)
        //     {
        //         SetCake(cakeList[Random.Range(0, cakeList.Length)], 1); 
        //
        //         yield return new WaitForSeconds(0.35f);
        //     }
        // }
    }
}
