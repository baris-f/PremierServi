using System.Collections;
using UnityEngine;

namespace Modules.Common.Cake
{
    public class Cake : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] cakeList;
    
        [Header("Infos")]
        [SerializeField] private int cakeId;
    
        public void SetCake(Sprite cakeSprite, int newCakeId)
        {
            spriteRenderer.sprite = cakeSprite;
            cakeId = newCakeId;
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
