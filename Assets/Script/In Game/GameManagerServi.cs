using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamevrestUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.XR;
using Random = UnityEngine.Random;


public class GameManagerServi : MonoBehaviour
{
    public List<Character> list = new List<Character>();
    public GameObject playerPrefab;
    public List<Color> playerColors = new();
    public GameObject characterPrefab;
    public Transform characterStartPos;
    public Transform canonStartPos;
    public bool ISDEBUG = false;
    public SceneReference game;
    public SceneReference menu;
    private int playerCount;
    public int AINumber = 8;
    private int currentID = 1;
    public void Start()
    {
        playerCount = InputSystem.devices.OfType<Joystick>().Count();
        //if (playerCount == 0) Keyboard mode
        foreach (var device in InputSystem.devices)
            Debug.Log(device.GetType());        
        for (int i = 0; i < AINumber; i++)
            list.Add(Instantiate(characterPrefab, characterStartPos.position -= Vector3.up, characterStartPos.rotation).GetComponent<Character>());
        for (int i = 0; i < playerCount; i++)
            AddPlayer();
    }

    public void AddPlayer()
    {
        int rand = Random.Range(0, list.Count);
        
        list[rand].isIA = false;
        list[rand].id = currentID;
        Player player = Instantiate(playerPrefab, canonStartPos.position -= Vector3.up * 2, canonStartPos.rotation).GetComponent<Player>();
        player.character = list[rand];
        list[rand].player = player;
        if (ISDEBUG)
            list[rand].gameObject.GetComponent<SpriteRenderer>().color = playerColors[currentID - 1];
        player.color = playerColors[currentID - 1];
        player.canon.GetComponent<SpriteRenderer>().color = playerColors[currentID - 1];
        player.canon.GetComponent<LineRenderer>().startColor = playerColors[currentID - 1];
        player.canon.GetComponent<LineRenderer>().endColor = playerColors[currentID - 1] - new Color(0, 0, 0, 1);
        list.Remove(list[rand]);
        currentID++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }

    public void Restart()
    {
        StartCoroutine(LoadScene(game));
    }
    
    public void Quit()
    {
        StartCoroutine(LoadScene(menu));
    }
    
    public IEnumerator LoadScene(SceneReference scene)
    {
        yield return new WaitForSeconds(3f);
        scene.Load();
    }

}