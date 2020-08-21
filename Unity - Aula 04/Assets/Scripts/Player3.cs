using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class Player3 : MovingObject3
{
    // Conjunto de variáveis controladas pelo player.
    public int wallDamage = 1; // Quantidade de dano que pode causar a uma parede.
    public int pointsPerFood = 10; // Pontos que ganha ao coletar comida (cereja).
    public int pointsPerSoda = 20; // Pontos que ganha ao coletar o refrigerante.
    public float restartLevelDelay = 1f; // Tempo até carregar uma nova fase.

    private Animator animator; // O Animator do Player.
    private int food; // Guarda a quantidade total de comida que o player coletou.

    // Referência ao texto "Comida".
    public Text foodText;

    // Efeitos sonoros.
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    // Onde o jogador começou a deslizar o dedo na tela.
    private Vector2 touchOrigin = -Vector2.one;

    protected override void Start()
    {
        animator = GetComponent<Animator>(); // Aponta para o Animator contido no Player.
        food = GameManager3.instance.playerFoodPoints; // A quantidade de comida que ficou salva no GameManager.

        // Atualiza o texto da Comida na tela.
        foodText.text = "Comida: " + food;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager3.instance.playerFoodPoints = food; // Atualiza a quantidade de comida no GameManager.
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--; // A mesma coisa que food = food - 1 ou food -= 1.

        // Atualiza o texto Comida sempre que o jogador se move.
        foodText.text = "Comida: " + food;

        base.AttemptMove<T>(xDir, yDir); // Executa AttemptMove da classe base.
        RaycastHit2D hit; // O objeto que foi encontrado no caminho do Player será guardado em hit.

        if (Move (xDir, yDir, out hit))
        {
            SoundManager2.instance.RandomSfx(moveSound1, moveSound2) ;
        }

        CheckIfGameOver();
        GameManager3.instance.playersTurn = false; // Informa ao GameManager que acabou o turno do jogador.
    }

    private void Update()
    {
        if (!GameManager3.instance.playersTurn) // Mesma coisa que ...playersTurn == false.
        {
            return; // Cancela a execução do método.
        }

        Debug.Log("Updating...");

        // Guarda a direção para qual o jogador está tentando se mover.
        int horizontal = 0;
        int vertical = 0;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // Bloqueia o movimento na diagonal.
        if (horizontal != 0)
        {
            vertical = 0;
        }
#else
        if (Input.touchCount > 0)
	    {
            Touch myTouch = Input.touches[0];
        
            if (myTouch.phase == TouchPhase.Began) 
            {
                touchOrigin = myTouch.position;
            }

            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;

                if (Mathf.Abs(x) > Math.Abs(y))
                {
                    horizontal = x > 0 ? 1 : -1; // operador ternário.
                }
                else
                {
                    vertical = y > 0 ? 1 : -1;
                }
            }
	    }
#endif


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
        foodText.text = "-" + loss + " Comida: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager2.instance.PlaySingle(gameOverSound);
            SoundManager2.instance.musicSource.Stop(); // Para a música.
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
            foodText.text = "+" + pointsPerFood + " Comida: " + food;
            SoundManager2.instance.RandomSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false); // Desabilita a comida ao colidir com ela.
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Comida: " + food;
            SoundManager2.instance.RandomSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }
}
