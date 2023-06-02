using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Car : MonoBehaviour
{
    public float refSpeed = 0.02f;
    private float speed = 0.02f;
    private bool hasStarted;

    private void Start()
    {
        randomize();
        hasStarted = true;
    }

    void randomize()
    {
        speed = refSpeed + Random.Range(-0.01f, 0.01f);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * -speed);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col || !col.CompareTag("Player"))
            return;
        col.GetComponent<Character>().Die();
        //Destroy(col.gameObject);
        //Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        if (!hasStarted)
            return;
        Invoke("changeDir", Random.Range(1f, 2.5f));
    }

    void changeDir()
    {
        transform.Rotate(Vector3.forward, 180);
    }
}
