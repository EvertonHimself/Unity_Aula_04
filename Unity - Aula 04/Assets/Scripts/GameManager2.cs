using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Namespace da UI
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    // O delay entre turnos (fazer depois da animação do inimigo).
    public float turnDelay = .1f;
    // Um array de inimigos (fazer depois da animação do inimigo).
    private List<Enemy2> enemies;
    // Fazer depois da animação do inimigo.
    private bool enemiesMoving;

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
    private int level = 1;

    /* Configurações da UI */
    // Tempo antes de iniciar o Level.
    public float levelStartDelay = 2f;
    // Texto do level.
    private Text levelText;
    // Imagem do level (a imagem preta).
    private GameObject levelImage;
    // Prevenir que o player se mova antes de iniciar o jogo.
    private bool doingSetup;

    // Evento lançado quando um level é carregado.
    private void OnLevelWasLoaded(int index)
    {
        level++;
        Debug.Log(level);
        InitGame(); // Depois de fazer essa parte, colocar o doingSetup = true no InitGame().
    }

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

        // Inicializa nossa lista de inimigos (fazer depois da animação do inimigo).
        enemies = new List<Enemy2>();

        // Referência ao componente Board Manager.
        boardScript = GetComponent<BoardManager2>();
        InitGame();
    }

    // Inicializa o jogo, chamando a função SetupScene().
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true); // Depois disso, criar a função HideLevelImage()
        Invoke("HideLevelImage", levelStartDelay); // Invoke chama uma funções depois de um determinado tempo em segundos.
        // Vá para o update configurar o doingSetup.

        // Limpa a lista de inimigos ao carregar uma fase (fazer depois da animação do inimigo).
        enemies.Clear();
        boardScript.SetupScene(level);
    }
    // Depois de fazer as alterações acima, volte ao editor para fazer as atribuições dos objetos (se ainda não tiver feito).

    
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
        
    // Criar antes de criar o script do Player.
    public void GameOver()
    {
        // Mostra quantos dias o jogador sobreviveu e liga a tela preta.
        levelText.text = "After " + level + " you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    // Move os inimigos, um de cada vez, em sequência (fazer depois da animação do inimigo).
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    // (fazer depois da animação do inimigo).
    private void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup) // Vai impedir o movimento se estiver fazendo o setup. Configure o GameOver.
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    // Função usada para registrar os inimigos no GameManager para que o GameManager possa controlá-los.
    public void AddEnemyToList(Enemy2 script)
    {
        enemies.Add(script);
    }

    // Agora volte para o script Enemy2.
}
