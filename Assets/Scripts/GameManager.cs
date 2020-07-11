using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    private int level = 100;



    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame();

    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

     //Update is called once per frame
    void Update()
    {
        
    }
}
