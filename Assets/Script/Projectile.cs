
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
        if (!col || !col.CompareTag("Player"))
            return;
        if (col.GetComponent<Character>().isIA == false)
        {
            winText.SetActive(true);
            winText.GetComponent<TextMeshProUGUI>().text = "player " + col.GetComponent<Character>().id +" win !";
        }
        col.GetComponent<Character>().Die();
        //Destroy(col.gameObject);
        Destroy(gameObject);
    }
}
