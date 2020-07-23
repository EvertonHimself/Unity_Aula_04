using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    // Mostrar esse depois que criar a variável boardScript.
    // Publico para que seja acessível no Inspector (e outros scripts).
    // Static pois a variável irá pertencer a classe.
    public static GameManager2 instance = null;

    public BoardManager2 boardScript;

    // Criar a variável playerFoodPoints e playersTurn depois de tudo mas antes de criar o Player Script.
    // Criar também a função "Game Over".
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    
    // Vamos começar testando com o level 3 (ou 9), pois é quando os inimigos surgem.
    private int level = 3;

    // Awake é uma função da Unity que é executada antes do Start.
    private void Awake()
    {
        // Checa se instance e está vazia, e se estiver atribui a ela a própria instância do GameManager.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Não destrói o game object ao carregar uma cena.
        DontDestroyOnLoad(gameObject);

        // Referência ao componente Board Manager.
        boardScript = GetComponent<BoardManager2>();
        InitGame();
    }

    // Inicializa o jogo, chamando a função SetupScene().
    void InitGame()
    {
        boardScript.SetupScene(level);
    }
    // Depois de fazer as alterações acima, volte ao editor para fazer as atribuições dos objetos (se ainda não tiver feito).

    // Criar antes de criar o script do Player.
    public void GameOver()
    {
        enabled = false;
    }
}
