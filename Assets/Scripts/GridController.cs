using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour
{
    public static int gridHeight = 20;
    public static int gridWidth = 10;

    private Transform[,] grid = new Transform[gridWidth, gridHeight];

    private GameController gameController;
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public bool IsPositionValid(Transform block)
    {
        foreach (Transform child in block)
        {
            Vector3 pos = RoundPosition(child.position);
            if (!IsInsideGrid(pos) || IsOccupied(pos))
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateGrid(Transform block)
    {
        foreach (Transform child in block)
        {
            Vector3 pos = RoundPosition(child.position);
            if (IsInsideGrid(pos))
            {
                grid[(int)pos.x, (int)pos.y] = child;
            }
            else
            {
                gameController.SetGameOver(true);
                return;
            }
        }
    }

    private bool IsInsideGrid(Vector3 position)
    {
        return (int)position.x >= 0 && (int)position.x < gridWidth && (int)position.y >= 0 && (int)position.y < gridHeight;
    }

    private bool IsOccupied(Vector3 position)
    {
        return grid[(int)position.x, (int)position.y] != null;
    }

    private Vector3 RoundPosition(Vector3 position)
    {
        float x = position.x + 4.5f;
        float y = position.y + 9.5f;
        return new Vector3 (x, y, 0);
    }

    public bool IsGameOver()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, gridHeight - 1] != null)
            {
                gameController.SetGameOver(true);
                return true;
            }
        }
        return false;
    }

    public void CheckAndClearLines()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                MoveLinesDown(y + 1);
                y--; 
            }
        }
    }

    private bool IsLineFull(int lineIndex)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, lineIndex] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearLine(int lineIndex)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Destroy(grid[x, lineIndex].gameObject);
            grid[x, lineIndex] = null;
        }
        gameController.PlayClearSound();
        gameController.AddScore(gridWidth);
    }

    private void MoveLinesDown(int startLine)
    {
        for (int y = startLine; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }
}
