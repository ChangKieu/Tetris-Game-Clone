using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fallSpeed = 1f;
    private float fallTimer;
    private bool isDropping = false;

    private GridController gridManager;
    private Characters charactersManeger;
    private GameController gameController;

    private bool gameIsOver = false;
    private GameObject ghostBlock;

    private void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridController>();
        charactersManeger = GameObject.FindGameObjectWithTag("Characters").GetComponent<Characters>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        CreatGhost();
    }

    private void Update()
    {
        if (!gameIsOver)
        {
            HandleMovement();
            UpdateGhostBlock();
        }
    }
    private void CreatGhost()
    {
        ghostBlock = new GameObject("GhostBlock");
        ghostBlock.transform.position = transform.position;
        ghostBlock.transform.rotation = transform.rotation;
        foreach (Transform child in transform)
        {
            GameObject childGhost = Instantiate(child.gameObject, child.position, child.rotation);
            Renderer ghostRenderer = childGhost.GetComponent<Renderer>();
            if (ghostRenderer != null)
            {
                ghostRenderer.material.color = new Color(1f, 1f, 1f, 0.2f); 
            }
            childGhost.transform.parent = ghostBlock.transform;
        }
    }
    private void HandleMovement()
    {
        FallDown(Vector3.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveBlock(Vector3.left);
            gameController.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveBlock(Vector3.right);
            gameController.PlayMoveSound();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90);
            if (!gridManager.IsPositionValid(transform))
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                gameController.PlayMoveSound();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !isDropping)
        {
            DropBlock();
            gameController.PlayDropSound();
        }
    }

    private void DropBlock()
    {
        isDropping = true;
        while (gridManager.IsPositionValid(transform))
        {
            transform.position += Vector3.down;
        }
        transform.position -= Vector3.down;
        gridManager.UpdateGrid(transform);
        gridManager.CheckAndClearLines();
        if (gameController.IsGameOver())
        {
            GameOver();
            enabled = false;
            return;
        }
        DestroyGhost();
        charactersManeger.InstantiateBlock();
        enabled = false;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        gameIsOver = true;
    }

    private void FallDown(Vector3 moveDirection)
    {
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallSpeed)
        {
            transform.position += moveDirection;
            fallTimer = 0;
        }
        if (!gridManager.IsPositionValid(transform))
        {
            transform.position -= moveDirection;
            gridManager.UpdateGrid(transform);
            gridManager.CheckAndClearLines();
            if (gameController.IsGameOver())
            {
                GameOver();
                enabled = false;
                return;
            }
            DestroyGhost();
            charactersManeger.InstantiateBlock();
            enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isDropping = false;
        }
    }

    private void MoveBlock(Vector3 moveDirection)
    {
        transform.position += moveDirection;
        if (!gridManager.IsPositionValid(transform))
        {
            transform.position -= moveDirection;
        }
    }
    private void UpdateGhostBlock()
    {
        ghostBlock.transform.position = transform.position;
        ghostBlock.transform.rotation = transform.rotation;
        while (gridManager.IsPositionValid(ghostBlock.transform))
        {
            ghostBlock.transform.position += Vector3.down;
        }
        ghostBlock.transform.position -= Vector3.down;
    }
    private void DestroyGhost()
    {
        Destroy(ghostBlock);
    }
}
