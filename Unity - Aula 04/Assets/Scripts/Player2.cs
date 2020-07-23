using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MovingObject2
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    // Guarda a quantidade de comida durante o level, antes de passar para o GameManager ao trocar de level.
    private int food;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        // Pega a comida que foi salva no game manager.
        food = GameManager2.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        // Salva a comida do player no GameManager.
        GameManager2.instance.playerFoodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager2.instance.GameOver();
        }
    }

    // T = o objeto que nosso objeto em movimento espera encontrar.
    // Obs: Isso é a penas a declaração do método, ainda não é sua execução.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // Perde 1 de comida a cada movimento.
        food--;
        // Executa a função AttemptMove() da classe base.
        base.AttemptMove<T>(xDir, yDir);
        // Permite referenciar o resultado do Linecast feito em Move().
        RaycastHit2D hit;
        // Checa se deu game over, pois o jogador perde comida ao se mover.
        CheckIfGameOver();
        // Fim do turno do player.
        GameManager2.instance.playersTurn = false;
    }

    void Update()
    {
        // Cancela a execução do método caso não seja a vez do player.
        if (!GameManager2.instance.playersTurn)
        {
            return;
        }

        // Guarda a direção do movimento (1 ou -1).
        int horizontal = 0;
        int vertical = 0;
        

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // Impede o player de se mover na diagonal.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        // Se horizontal ou vertical for diferente de zero, significa que está tentando se mover.
        if (horizontal != 0 || vertical == 0)
        {
            // Testa se o player vai encontrar uma parede. Esperamos que ele encontre uma parede.
            // Parâmetros: a direção para a qual o player está tentando ir.
            AttemptMove<Wall2>(horizontal, vertical);
        }
    }

    // Implementa a OnCantMove.
    // O que acontece se não puder se mover?
    protected override void OnCantMove<T>(T component)
    {
        // No caso do player, queremos que ele faça algo se for bloqueado por uma parede.
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    // "Recarrega" o level ao colidir com o "Exit".
    public void Restart()
    {
        // Nosso jogo só tem uma scene.
        Application.LoadLevel(Application.loadedLevel);
    }

    // Perde comida.
    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    // Interage com os outros objetos no jogo.
    // Exit, Food e Soda são Triggers.
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
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
