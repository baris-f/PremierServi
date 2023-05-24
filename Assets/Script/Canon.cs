using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
public class Canon : MonoBehaviour
{
    public int playerID = 0;
    public float speed = 0.02f;
    private KeyCode up;
    private KeyCode down;
    private KeyCode shoot;
    void Start()
    {
        if (playerID == 1)
        {
            up = KeyCode.Joystick1Button3;
            down = KeyCode.Joystick1Button0;
            shoot = KeyCode.Joystick1Button2;
        }
        else
        {
            up = KeyCode.Joystick2Button3;
            down = KeyCode.Joystick2Button0;
            shoot = KeyCode.Joystick2Button2;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            if (Input.GetKey(up))
                transform.Translate(Vector2.up * speed);
            if (Input.GetKey(down))
                transform.Translate(Vector2.down * speed);
            if (Input.GetKey(shoot))
                transform.Translate(Vector2.left * speed * 15);
    }
}
