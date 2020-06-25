using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader2 : MonoBehaviour
{
    public GameManager2 gameManager;

    private void Awake()
    {
        if (GameManager2.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
