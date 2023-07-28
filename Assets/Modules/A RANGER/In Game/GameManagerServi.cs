using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using GamevrestUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class GameManagerServi : MonoBehaviour
{
    public List<Character> AIList = new List<Character>();
    public List<Character> playerList = new List<Character>();
    public GameObject playerPrefab;
    public List<Color> playerColors = new();
    public GameObject characterPrefab;
    public Transform characterStartPos;
    public Transform canonStartPos;
    public TextMeshProUGUI winText;
    public bool isSquidGame;
    public bool ISDEBUG = false;
    public SceneReference game;
    public SceneReference menu;
    private int playerCount;
    public int AINumber = 8;
    private int currentID = 1;
    public void Start()
    {
        playerCount = InputSystem.devices.OfType<Joystick>().Count() + 1;
        //if (playerCount == 0) Keyboard mode
        // foreach (var device in InputSystem.devices)
        //     Debug.Log(device.GetType());        
        for (int i = 0; i < AINumber; i++)
            AIList.Add(Instantiate(characterPrefab, characterStartPos.position -= Vector3.up, characterStartPos.rotation).GetComponent<Character>());
        for (int i = 0; i < playerCount; i++)
            AddPlayer();
    }

    public void AddPlayer()
    {
        int rand = Random.Range(0, AIList.Count);
        
        AIList[rand].isIA = false;
        Player player = Instantiate(playerPrefab, canonStartPos.position -= Vector3.up * 2, canonStartPos.rotation).GetComponent<Player>();
        player.character = AIList[rand];
        AIList[rand].player = player;
        if (ISDEBUG)
            AIList[rand].gameObject.GetComponent<SpriteRenderer>().color = playerColors[currentID - 1];
        player.color = playerColors[currentID - 1];
        player.weapon.GetComponent<SpriteRenderer>().color = playerColors[currentID - 1];
        player.weapon.GetComponent<LineRenderer>().startColor = playerColors[currentID - 1];
        if (!isSquidGame)
            player.weapon.GetComponent<LineRenderer>().endColor = playerColors[currentID - 1] - new Color(0, 0, 0, 1);
        else
            player.weapon.GetComponent<LineRenderer>().endColor = playerColors[currentID - 1];
        player.id = currentID;
        playerList.Add(AIList[rand]);
        AIList.Remove(AIList[rand]);
        currentID++;
    }

    void Update()
    {
        if (playerList.Count == 1 && !ISDEBUG)
            Win(playerList[0].player);
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }

    public void Win(Player player = null)
    {
        if (winText.text == "")
        {
            if (player == null)
                winText.text = "AI win";
            else
                winText.text = "Player " + player.id + " wins";
        }
        if (player != null)
            winText.color = player.color;
        Restart();
    }

    public List<Character> GetMovingCharacters()
    {
        var movingCharacters = new List<Character>();
        foreach(Character player in playerList)
            if (player.isRunning || player.isWalking)
                movingCharacters.Add(player);
        foreach(Character ai in AIList)
            if (ai.isRunning || ai.isWalking)
                movingCharacters.Add(ai);
        return movingCharacters;
    }
    
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        //StartCoroutine(LoadScene(game));
    }
    
    public void Quit()
    {
        SceneManager.LoadScene("0 - MainMenu");
    }
    
    public IEnumerator LoadScene(SceneReference scene)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(scene.BuildIndex);
    }

}