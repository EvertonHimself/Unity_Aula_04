using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 instance = null;

    public BoardManager3 boardManager;

    private int level = 9;

    public int playerFoodPoints = 100;
    // [HideInInspector] Esconde a variável no Inspector.
    [HideInInspector] public bool playersTurn = true;

    // Delay entre turnos.
    public float turnDelay = .1f;
    // A lista de inimigos presentes na fase.
    private List<Enemy3> enemies;
    // Verdadeiro se os inimigos estão se movendo.
    private bool enemiesMoving;

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

        // Inicializa nossa lista de inimigos (fazer depois da animação do inimigo).
        enemies = new List<Enemy3>();

        boardManager = GetComponent<BoardManager3>();
        InitGame();
    }

    void InitGame()
    {
        enemies.Clear();
        boardManager.SetupScene(level);
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
        if (playersTurn == true || enemiesMoving == true)
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
        enabled = false;
    }
}
