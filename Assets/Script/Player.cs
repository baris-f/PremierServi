using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int id;
    public Color color;
    public Character character;
    
    public void CharacterRun(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1)
            character.isRunning = true;
        else
            character.isRunning = false;
    }
    
    public void CharacterWalk(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1)
            character.isWalking = true;
        else
            character.isWalking = false;
    }

}
