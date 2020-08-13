using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Vamos herdar de MovingObject2 para utilizar os métodos de movimento sem precisar reescrevê-los.
public class Enemy2 : MovingObject2
{
    // O tanto de dano que o inimigo causa no player.
    public int playerDamage;
    // Referência ao animator do inimigo.
    private Animator animator;
    // A posição do jogador, para onde o inimigo irá se mover.
    private Transform target;
    // Faz o inimigo se mover em turnos alternados.
    private bool skipMove;

    // Áudios do inimigo.
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;
    // Vá para OnCantMove.

    protected override void Start()
    {
        // Fazer isso depois das adições no GameManager.
        // O inimigo se adiciona a lista de inimigos do GameManager.
        GameManager2.instance.AddEnemyToList(this);

        animator = GetComponent<Animator>();
        // O transform do player.
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    // Será executado pelo Game Manager.
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Math.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player2>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player2 hitPlayer = component as Player2;
        // Fazer isso depois das alterações do GameManager2.
        // Ativa a animação do personagem.
        animator.SetTrigger("enemyAttack");

        // Randomiza os sons do inimigo.
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);

        hitPlayer.LoseFood(playerDamage);
    }
}
