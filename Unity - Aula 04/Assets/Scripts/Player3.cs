using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player3 : MovingObject3
{
    // Conjunto de variáveis controladas pelo player.
    public int wallDamage = 1; // Quantidade de dano que pode causar a uma parede.
    public int pointsPerFood = 10; // Pontos que ganha ao coletar comida (cereja).
    public int pointsPerSoda = 20; // Pontos que ganha ao coletar o refrigerante.
    public float restartLevelDelay = 1f; // Tempo até carregar uma nova fase.

    private Animator animator; // O Animator do Player.
    private int food; // Guarda a quantidade total de comida que o player coletou.

    protected override void Start()
    {
        animator = GetComponent<Animator>(); // Aponta para o Animator contido no Player.
        food = GameManager3.instance.playerFoodPoints; // A quantidade de comida que ficou salva no GameManager.

        base.Start();
    }

    private void OnDisable()
    {
        GameManager3.instance.playerFoodPoints = food; // Atualiza a quantidade de comida no GameManager.
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--; // A mesma coisa que food = food - 1 ou food -= 1.
        base.AttemptMove<T>(xDir, yDir); // Executa AttemptMove da classe base.
        RaycastHit2D hit; // O objeto que foi encontrado no caminho do Player será guardado em hit.
        CheckIfGameOver();
        GameManager3.instance.playersTurn = false; // Informa ao GameManager que acabou o turno do jogador.
    }

    private void Update()
    {
        if (!GameManager3.instance.playersTurn) // Mesma coisa que ...playersTurn == false.
        {
            return; // Cancela a execução do método.
        }

        // Guarda a direção para qual o jogador está tentando se mover.
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // Bloqueia o movimento na diagonal.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            // Tenta se mover, verificando se existe uma parede no caminho.
            AttemptMove<Wall3>(horizontal, vertical);
        }
    }

    // O que acontece se ele não puder se mover?
    protected override void OnCantMove<T>(T component)
    {
        Wall3 hitWall = component as Wall3;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    // Gera uma nova fase ao colidir com a saída.
    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    // Perde comida.
    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager3.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false); // Desabilita a comida ao colidir com ela.
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
