using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 instance = null;

    public BoardManager2 boardScript;
    // Vamos começar testando com o level 3, pois é quando os inimigos surgem.
    private int level = 9;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager2>();
        InitGame();
    }

    // Inicializa o jogo, chamando a função SetupScene().
    void InitGame()
    {
        boardScript.SetupScene(level);
    }
    // Depois de fazer as alterações acima, volte ao editor para fazer as atribuições dos objetos.
}
