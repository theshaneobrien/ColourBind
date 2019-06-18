using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private GameGrid gameGrid;

    public void SetGameGrid(GameGrid grid)
    {
        gameGrid = grid;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }

    public void PlayerInputs()
    {
        //TODO: Detect what platform we are on...
        KeyControls();
    }

    public void TouchControls()
    {
        //TODO: Figure out touch inputes
    }

    public void KeyControls()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
    }

    public void MoveLeft()
    {
        //TODO:Check if we can move in the desired direction
        if (gameGrid.ValidateLeftMovement(-10))
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
    }

    public void MoveRight()
    {
        if (gameGrid.ValidateRightMovement(10))
        {
            //TODO:Check if we can move in the desired direction
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
    }

    public void MoveUp()
    {
        if (gameGrid.ValidateUpMovement(1))
        {
            //TODO:Check if we can move in the desired direction
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }
    }

    public void MoveDown()
    {
        if (gameGrid.ValidateDownMovement(-1))
        {
            //TODO:Check if we can move in the desired direction
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }
    }

}
