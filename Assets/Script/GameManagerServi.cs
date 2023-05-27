using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class GameManagerServi : MonoBehaviour
{
    public List<Character> list = new List<Character>();
    public GameObject playerPrefab;
    public GameObject characterPrefab;
    public bool iscontroller;
    public Transform characterStartPos;
    public Transform canonStartPos;
    public int playerNumber;
    public int AINumber = 8;
    private int currentID = 1;
    public void Start()
    {

        for (int i = 0; i < AINumber; i++)
            list.Add(Instantiate(characterPrefab, characterStartPos.position -= Vector3.up, characterStartPos.rotation).GetComponent<Character>());
        for (int i = 0; i < playerNumber; i++)
            AddPlayer();
    }

    public void AddPlayer()
    {
        int rand = Random.Range(0, list.Count);
        
        list[rand].isIA = false;
        list[rand].id = currentID++;
        Player player = Instantiate(playerPrefab, canonStartPos.position -= Vector3.up * 2, canonStartPos.rotation).GetComponent<Player>();
        player.character = list[rand];
        list[rand].gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        list.Remove(list[rand]);
    }
    
    void Update()
    {
        /*if (Input.anyKey)
            Debug.Log(Input.inputString);
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            Debug.Log("Joycon1 X");
        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
            Debug.Log("Joycon2 X");
        if (Input.GetKeyDown(KeyCode.Joystick3Button1))
            Debug.Log("Joycon3 X");
        if (Input.GetKeyDown(KeyCode.Joystick4Button1))
            Debug.Log("Joycon4 X");*/
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