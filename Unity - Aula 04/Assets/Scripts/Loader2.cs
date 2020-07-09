using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Carrega o objeto (Prefab) GameManager.
public class Loader2 : MonoBehaviour
{
    public GameManager2 gameManager;

    private void Awake()
    {
        // Verifica se não existe uma instância de GameManager na cena, e se não existir, cria uma.
        if (GameManager2.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
