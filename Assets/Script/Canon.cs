using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public class Canon : MonoBehaviour
{
    public int playerID = 0;
    public float speed = 0.02f;
    public bool controller;
    public LineRenderer line;
    public GameObject projectile;
    private RaycastHit2D hit;
    private Vector2 movement = Vector2.zero;
    private KeyCode up;
    private KeyCode down;
    private KeyCode shoot;
    void Start()
    {
        if (controller)
        {
            up = JoyCon.GetKeyCode(JCKey.Y, playerID);
            down = JoyCon.GetKeyCode(JCKey.A, playerID);
            shoot = JoyCon.GetKeyCode(JCKey.B, playerID);
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
        Debug.Log(playerID + " - up : " + up + " down : " + down + " left: " + shoot);
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
        if (movement != Vector2.zero)
            transform.Translate(movement * Vector2.up * speed);
        /*if (Input.GetKey(up))
            MoveUp();
        if (Input.GetKey(down))
            MoveDown();
        if (Input.GetKey(shoot))
            Fire();*/
    }

    public void MoveUp()
    {
        transform.Translate(Vector2.up * speed);
    }
    
    public void MoveDown()
    {
        transform.Translate(Vector2.down * speed);
    }

    public void Move(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
        Debug.Log("Input : " + movement);
    }
    
    public void Fire()
    {
        Instantiate(projectile, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}