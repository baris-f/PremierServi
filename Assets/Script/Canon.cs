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
    public bool controller;
    public LineRenderer line;
    public GameObject projectile;
    private RaycastHit2D hit;
    private KeyCode up;
    private KeyCode down;
    private KeyCode shoot;
    void Start()
    {
        if (controller)
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
        else
        {
            if (playerID == 1)
            {
                up = KeyCode.Z;
                down = KeyCode.S;
                shoot = KeyCode.Q;
            }
            else
            {
                up = KeyCode.UpArrow;
                down = KeyCode.DownArrow;
                shoot = KeyCode.LeftArrow;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        line.SetPosition(0, transform.position);
        hit = Physics2D.Raycast(transform.position, Vector2.left);
        if (hit.collider != null)
        {
            line.SetPosition(1, hit.point);
        }
        else
            line.SetPosition(1, transform.position + (Vector3.left * 50));
        if (Input.GetKey(up))
            transform.Translate(Vector2.up * speed);
        if (Input.GetKey(down))
            transform.Translate(Vector2.down * speed);
        if (Input.GetKey(shoot))
        {
            Instantiate(projectile, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void Fire()
    {
        
    }
}