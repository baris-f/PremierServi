using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public class Canon : MonoBehaviour
{
    public float speed = 0.02f;
    private bool fired;
    public LineRenderer line;
    public GameObject projectile;
    private RaycastHit2D hit;
    private Vector2 movement = Vector2.zero;
/*    private KeyCode up;
    private KeyCode down;
    private KeyCode shoot;

    void Initialize()
    {
        if (!controller)
        {
            if (player.id == 1)
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
    } */



    void FixedUpdate()
    {
        line.SetPosition(0, transform.position);
        hit = Physics2D.Raycast(transform.position, Vector2.left);
        if (hit.collider != null)
        {
            line.SetPosition(1, hit.point);
        }
        else
            line.SetPosition(1, transform.position + (Vector3.left * 16));
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
        //Debug.Log("Input : " + movement);
    }
    
    public void Fire()
    {
        if (!fired)
            Instantiate(projectile, transform.position, transform.rotation);
        fired = true;
        Destroy(gameObject);
    }
}