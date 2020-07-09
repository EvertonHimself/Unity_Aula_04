using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

// Cria uma fase aleatória cada vez que um nova se inicia, baseada no número da fase.
public class BoardManager3 : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit;

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

    // Preenche a lista com posições possíveis de se spawnar objetos.
    public void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Cria as paredes externas do nosso level e o chão.
    public void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // Cria um "chão" a cada iteração do laço.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // Verifica se estamos na posição necessária para criar uma parede externa.
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // Seleciona uma posição aleatória dentro da fase/level/grid.
    Vector3 RandomPosition()
    {
        // Um número aleatório que servirá como índice.
        int randomIndex = Random.Range(0, gridPositions.Count);
        // A posição aleatória.
        Vector3 randomPosition = gridPositions[randomIndex];
        // Remove a posição da lista para evitar criar objetos em posições iguais.
        gridPositions.RemoveAt(randomIndex);
        // Retorna a posição aleatória selecionada.
        return randomPosition;
    }

    // Cria os objetos "interativos" do jogo.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // QUantos de um dado objeto serão criados.
        int objectCount = Random.Range(minimum, maximum + 1);
        
        for (int i = 0; i < objectCount; i++)
        {
            // Seleciona uma posição aleatória.
            Vector3 randomPosition = RandomPosition();
            // Seleciona um tile (bloco/sprite) aleatório.
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            // Cria o tile (bloco/sprite).
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // A função que fará a inicialização do level.
    public void SetupScene(int level) 
    {
        // Cria as paredes externas.
        BoardSetup();
        // Limpa a lista de posições no level.
        InitialiseList();
        // Cria os objetos interativos;
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        // Determina o número de inimigos por fase.
        int enemyCount = (int)Math.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
