using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Security.Cryptography;

// Layout the randomly generated levels each time a level starts
// based on the current level number.
public class BoardManager2 : MonoBehaviour
{
    // Uma classe para guardar um "range".
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

    // O tamanho do level.
    public int columns = 8;
    public int rows = 8;

    // Um range aleatório para o número de paredes que serão geradas no nosso level (obs, não são as que circundam o level).
    public Count wallCount = new Count(5, 9);
    // Um range aleatório para o número de comidas que serão geradas no nosso level.
    public Count foodCount = new Count(1, 5);

    // O objeto "exit" que será a saída. Nosso level terá apenas uma saída.
    public GameObject exit;

    // Nosso level terá vários "chãos", por isso vamos guardar em um array. Vamos selecionar qual spawnar de maneira aleatória.
    // A mesma lógica vale para os demais arrays abaixo.
    // Vamos inserir seus respectivos objetos no Inspector.
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    // O objeto que irá guardar o level gerado, para não encher a hierarchy de coisas.
    private Transform boardHolder;
    // Monitorar as posições possíveis de spawnar objetos no level e se algum objeto foi spawnado nela.
    private List<Vector3> gridPositions = new List<Vector3>();

    // Limpa as posições do grid e depois preenche ele.
    public void InitialiseList()
    {
        // Limpa as posições do grid, em seguida, preenche o grid com as posições possíveis.
        // (olhar Roteiro para explicação mais detalhada)
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Cria as paredes EXTERNAS e o CHÃO do level.
    void BoardSetup()
    {
        // Cria um objeto chamado Board e o atribui para boardHolder.
        boardHolder = new GameObject("Board").transform;

        // Um loop parecido com o aterior, mas para desenhar as paredes externas e o chão.
        // As paredes vão de -1 a + 1, pois serão inseridas "fora dos limites" do level.
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // O objeto a ser instanciado. Vai ser um tile qualquer que estiver dentro de floorTiles.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                
                // Verifica se estamos nas posições das paredes externas, se sim, vamos escolher um tile de parede externa.
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    // Seleciona qual objeto será instanciado.
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                // Cria uma cópia do prefab que selecionamos, dentro do jogo.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                // Coloca dentro do objeto "Board Holder" da Hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // Função para selecionarmos uma posição aleatória dentro do "grid" do nosso jogo.
    Vector3 RandomPosition()
    {
        // Um índice aleatório baseado no número de posições disponíveis no nosso grid.
        int randomIndex = Random.Range(0, gridPositions.Count);
        // A posição aleatória.
        Vector3 randomPosition = gridPositions[randomIndex];
        // Remove a posição da lista para que não seja possível colocar outra coisa nessa posição.
        gridPositions.RemoveAt(randomIndex);
        // Retorna uma posição aleatória.
        return randomPosition;
    }

    // Spawna (coloca) os objetos na posição aleatória escolhida.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // Quantos de um dado objetos serão criados.
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            // Seleciona uma posição aleatória.
            Vector3 randomPosition = RandomPosition();
            // Seleciona um tile aleatório.
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            // Instancia o objeto.
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    // A função que será chamada pelo Game Manager para inicializar (fazer o setup) da cena.
    public void SetupScene(int level)
    {
        // Cria as paredes externas.
        BoardSetup();
        // Limpa a lista de posições do grid.
        InitialiseList();
        // Instancia um número aleatório dos objetos correspondentes aos parâmetros.
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        // Determina o número de inimigos no level, baseado numa progressão logarítimica.
        int enemyCount = (int)Math.Log(level, 2f);
        // Coloca os inimigos em posições aleatórias, mas apenas a quantidade definida em enemyCount.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        // Coloca o objeto "exit".
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
