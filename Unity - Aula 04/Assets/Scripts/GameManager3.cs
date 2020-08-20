using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 instance = null;

    public BoardManager3 boardManager;

    private int level = 1;

    public int playerFoodPoints = 100;
    // [HideInInspector] Esconde a variável no Inspector.
    [HideInInspector] public bool playersTurn = true;

    // Delay entre turnos.
    public float turnDelay = .1f;
    // A lista de inimigos presentes na fase.
    private List<Enemy3> enemies;
    // Verdadeiro se os inimigos estão se movendo.
    private bool enemiesMoving;

    /* Referências aos GameObjects da UI. */
    // Tempo antes de iniciar o jogo
    public float levelStartDelay = 2f;
    // Texto do level.
    private Text levelText;
    // Imagem de background preta.
    private GameObject levelImage;
    // Impede o personagem de se mover enquanto é exibida a tela "Dia X".
    private bool doingSetup;

    private void OnLevelWasLoaded(int index)
    {
        level++;
        Debug.Log(level);
        InitGame();
    }

    private void Awake()
    {
        // Singleton pattern (padrão singleton).
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Inicializa nossa lista de inimigos (fazer depois da animação do inimigo).
        enemies = new List<Enemy3>();

        boardManager = GetComponent<BoardManager3>();
        InitGame();
    }

    // Inicializa o jogo (limpa a lista de inimigos e gera a fase).
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Dia " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardManager.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    // Move os inimigos, um de cada vez em sequência.
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

    private void Update()
    {
        if (playersTurn == true || enemiesMoving == true || doingSetup == true)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    // Usado pelo Inimigo para se colocar na lista de inimigos desse Game Manager.
    public void AddEnemyToList(Enemy3 enemy)
    {
        enemies.Add(enemy);
    }

    // Desabilita o game object.
    public void GameOver()
    {
        // Mostra quantos dias o jogador durou.
        levelText.text = "Depois de " + level + " dias você morreu.";
        levelImage.SetActive(true);

        enabled = false;
    }
}
