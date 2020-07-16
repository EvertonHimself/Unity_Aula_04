using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MovingObject2
{
    public int wallDamage = 10;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager2.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager2.instance.playerFoodPoints = food;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager2.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager2.instance.GameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager2.instance.GameOver();
        }
    }
}
