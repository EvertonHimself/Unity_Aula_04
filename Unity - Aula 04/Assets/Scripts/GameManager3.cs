using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 instance = null;

    public BoardManager3 boardManager;

    private int level = 3;

    public int playerFoodPoints = 100;
    // Esconde a variável no Inspector.
    [HideInInspector] public bool playersTurn = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardManager = GetComponent<BoardManager3>();
        InitGame();
    }

    void InitGame()
    {
        boardManager.SetupScene(level);
    }

    // Desabilita o game object.
    public void GameOver()
    {
        enabled = false;
    }
}
