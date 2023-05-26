
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public GameObject winText;

    private void Start()
    {
        winText = GameObject.Find("win");
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.left);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<CharacterController2D>().isIA == false)
        {
            winText.SetActive(true);
            if (col.GetComponent<CharacterController2D>().key == KeyCode.D
                || col.GetComponent<CharacterController2D>().key == KeyCode.Joystick1Button1)
                winText.GetComponent<TextMeshProUGUI>().text = "player 2 win !";
            else
                winText.GetComponent<TextMeshProUGUI>().text = "player 1 win !";
        }
        Destroy(col.gameObject);
        Destroy(gameObject);
    }
}
