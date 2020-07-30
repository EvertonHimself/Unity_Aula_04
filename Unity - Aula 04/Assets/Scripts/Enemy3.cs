using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy3 é filho de MovingObject3. Ou seja, tem tudo que está na classe pai.
public class Enemy3 : MovingObject3
{
    // O dano que causa no player.
    public int playerDamage;
    // Referência ao componente Animator do inimigo.
    private Animator animator;
    // Referência a posição do jogador.
    private Transform target;
    // Faz o inimigo alternar turnos.
    private bool skipMove;

    // Sobrescreve o método "Start()" da classe pai.
    protected override void Start()
    {
        // Se adiciona a lista de inimigos na instância do GameManager3.
        GameManager3.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        // A posição do Player.
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove == true)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

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

        AttemptMove<Player3>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player3 hitPlayer = component as Player3;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }
}
