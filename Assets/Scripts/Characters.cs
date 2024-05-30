using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private GameObject nextGameObject;
    private GameObject ghostBlock;
    private int[][,] matrices;

    private void Start()
    {
        InitializeMatrices();
        InstantiateBlock();

    }
    public void InitializeMatrices()
    {
        matrices = new int[7][,];

        // L
        matrices[0] = new int[,] {
            { 0, 1, 0},
            { 0, 1, 0},
            { 0, 1, 1}
        };

        // S
        matrices[1] = new int[,] {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 1 },
        };


        // J
        matrices[2] = new int[,] {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        };

        // O
        matrices[3] = new int[,] {
            { 1, 1 },
            { 1, 1 }
        };

        // T
        matrices[4] = new int[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 },
        };

        // Z
        matrices[5] = new int[,] {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 0 }
        };
        //I
        matrices[6] = new int[,] {
            {0, 1, 0},
            {0, 1, 0},
            {0, 1, 0},
         };
    }

    public void InstantiateBlock()
    {
        int randomIndex = Random.Range(0, matrices.Length);
        Vector3 position = new Vector3(0, 10, 0);
        GameObject blockContainer = new GameObject("BlockContainer");
        int matrixHeight = matrices[randomIndex].GetLength(0);
        int matrixWidth = matrices[randomIndex].GetLength(1);
        Vector3 matrixCenter = new Vector3(matrixWidth / 2.0f - 1f, -matrixHeight / 2.0f, 0);
        blockContainer.transform.position = position + matrixCenter;
        for (int i = 0; i < matrices[randomIndex].GetLength(0); i++)
        {
            for (int j = 0; j < matrices[randomIndex].GetLength(1); j++)
            {
                if (matrices[randomIndex][i, j] == 1)
                {
                    Vector3 spawnPosition = position + new Vector3(j - 0.5f, -i - 0.5f, 0);
                    GameObject block = Instantiate(blockPrefabs[randomIndex], spawnPosition, Quaternion.identity);
                    block.transform.parent = blockContainer.transform;
                }
            }
        }
        blockContainer.AddComponent<BlockController>();
    }
}
