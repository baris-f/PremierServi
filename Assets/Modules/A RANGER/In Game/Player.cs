using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.A_RANGER.In_Game
{
    public class Player : MonoBehaviour
    {
        public int id;
        public Color color;
        public Character character;
        public GameObject weapon;
    
        public void Run(InputAction.CallbackContext value)
        {
            if (value.ReadValue<float>() == 1)
                character.isRunning = true;
            else
                character.isRunning = false;
        }
    
        public void Walk(InputAction.CallbackContext value)
        {
            if (value.ReadValue<float>() == 1)
                character.isWalking = true;
            else
                character.isWalking = false;
        }

    }
}
