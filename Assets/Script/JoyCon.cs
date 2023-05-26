using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JCKey // 20 difference in the keycode between each joystick controller (0 button : 330 generic | 350 joystick 1 | 370 joystick 2 ...)
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
}

public class JoyCon
{
    public static KeyCode GetKeyCode(JCKey key, int id = 0)
    {
        return (KeyCode)(key + id * 20);
    }
}
