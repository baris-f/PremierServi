using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Vector3 = System.Numerics.Vector3;


public class AIManager : MonoBehaviour
{
    public List<CharacterController2D> list = new List<CharacterController2D>();
    public bool iscontroller;
    public void Start()
    {
        int rand = Random.Range(0, list.Count);
        int rand2 = rand;

        while (rand2 == rand)
            rand2 = Random.Range(0, list.Count);
        list[rand].isIA = false;
        if (iscontroller)
            list[rand].key = KeyCode.Joystick1Button1;
        else
            list[rand].key = KeyCode.D;
        list[rand2].isIA = false;
        if (iscontroller)
            list[rand2].key = KeyCode.Joystick2Button1;
        else
            list[rand2].key = KeyCode.RightArrow;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            Debug.Log("Joycon1 X");
        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
            Debug.Log("Joycon2 X");
    }
}

/* public enum JoyConRKey
{
    None = KeyCode.None,

    A = KeyCode.JoystickButton0,
    B = KeyCode.JoystickButton2,
    X = KeyCode.JoystickButton1,
    Y = KeyCode.JoystickButton3,
    Plus = KeyCode.JoystickButton9,
    Home = KeyCode.JoystickButton12,

    R = KeyCode.JoystickButton14,
    ZR = KeyCode.JoystickButton15,

    SR = KeyCode.JoystickButton5,
    SL = KeyCode.JoystickButton4,

    // Joystick Horizontal = Joystick Axis 10.
    // Joystick Vertical = Joystick Axis 9.
}*/