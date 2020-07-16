using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader3 : MonoBehaviour
{
    public GameManager3 gameManager;

    private void Awake()
    {
        if (GameManager3.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
