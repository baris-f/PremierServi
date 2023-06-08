using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public class SquidGameDoll : MonoBehaviour
{
    private bool fired;
    public LineRenderer line;
    public GameManagerServi GameManager;
    private RaycastHit2D hit;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManagerServi>();
    }

    void FixedUpdate()
    {

    }

    public void ShootLine()
    {
        var charactersList = GameManager.GetMovingCharacters();
        var lineID = 0;
        line.positionCount = charactersList.Count * 2;
        Debug.Log(line.positionCount);
        foreach (Character character in charactersList)
        {
            line.SetPosition(lineID++, transform.position);
            line.SetPosition(lineID++, character.transform.position);
            character.Die();
        }
    }

    public void Fire()
    {
        if (fired)
            return;
        ShootLine();
        fired = true;
        Invoke(nameof(DestroySelf), 1f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}