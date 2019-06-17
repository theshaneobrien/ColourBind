using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private List<Transform> gameGridTransforms;
    [SerializeField]
    private List<Tile> gameGridTiles;
    [SerializeField]
    private List<Tile> filterTiles;

    [SerializeField]
    private Transform playerBall;

    [SerializeField]
    private List<Tile> redTiles;
    [SerializeField]
    private List<Tile> blueTiles;
    [SerializeField]
    private List<Tile> greenTiles;
    [SerializeField]
    private List<Tile> yellowTiles;

    private List<List<Tile>> allTiles = new List<List<Tile>>();

    public void Awake()
    {
        UpdateGameGridTiles();
        gameGridTransforms[0] = playerBall;

        allTiles.Add(redTiles);
        allTiles.Add(blueTiles);
        allTiles.Add(greenTiles);
        allTiles.Add(yellowTiles);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckAdjacentTiles()
    {
        //TODO: Needs refactor

        CheckWin();
    }

    private bool CheckWin()
    {
        foreach (List<Tile> tileList in allTiles)
        {
            if (tileList != null)
            {
                foreach (Tile tile in tileList)
                {
                    if (!tile.isTouchingSameColor)
                    {
                        return false;
                    }
                }
            }
        }
        Debug.Log("You won!");
        return true;
    }

    /// <summary>
    /// Mirrors the GameGridTransforms List with the GameGridTiles List
    /// </summary>
    private void UpdateGameGridTiles()
    {
        for (int i = 0; i < gameGridTransforms.Count; i++)
        {
            if (gameGridTransforms[i] != null)
            {
                if (gameGridTransforms[i].tag == "moveableTile")
                {
                    gameGridTiles[i] = gameGridTransforms[i].GetComponent<Tile>();
                    gameGridTiles[i].pos = i;
                }
                else
                {
                    gameGridTiles[i] = null;
                }
            }
        }
    }

    public bool ValidateLeftMovement(int playerMovement)
    {
        //Current player array location
        int currentPlayerPos = gameGridTransforms.IndexOf(playerBall);
        //Desired player array location
        int desiredPos = currentPlayerPos + playerMovement;

        //If we are not in the very first pos of the array
        if (desiredPos >= 0)
        {
            //Check if the desired position in the array is free, and within the bounds
            if (gameGridTransforms[desiredPos] == null && desiredPos >= 0 && desiredPos < 100)
            {
                //Remove player from the position they were in
                gameGridTransforms[currentPlayerPos] = null;
                //Put the player in the position they want to be in
                gameGridTransforms[desiredPos] = playerBall;
                return true;
            }
            //If the desired position has a moveable time, we move the player and the tile
            else if (gameGridTransforms[desiredPos].tag == "moveableTile")
            {
                //Cache the specific tile we will be affecting
                Transform tileTransform = gameGridTransforms[desiredPos];
                Tile tile = gameGridTiles[desiredPos];
                //Calculate the tiles new potential position
                int tileDesiredPos = desiredPos + playerMovement;

                //If the desired position of the tile is within bounds and empty, move it
                //TODO: Add in filter checks!
                if (tileDesiredPos >= 0 && gameGridTransforms[tileDesiredPos] == null)
                {
                    if (filterTiles[tileDesiredPos] == null || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        //Update the tile's position in the scene
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x - 1, tileTransform.transform.position.y, tileTransform.transform.position.z);
                        //Remove the player from the previous position
                        gameGridTransforms[currentPlayerPos] = null;
                        //Put the tile in it's desired position
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        //Put the player in the desired position
                        gameGridTransforms[desiredPos] = playerBall;
                        //Update the GameGridTile Array to match the transform array
                        UpdateGameGridTiles();
                        //Check if the tiles are touching tiles of the same color
                        CheckAdjacentTiles();
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool ValidateRightMovement(int playerMovement)
    {
        int currentPlayerPos = gameGridTransforms.IndexOf(playerBall);
        int desiredPos = currentPlayerPos + playerMovement;

        if (desiredPos < 100)
        {
            if (gameGridTransforms[desiredPos] == null)
            {
                gameGridTransforms[currentPlayerPos] = null;
                gameGridTransforms[desiredPos] = playerBall;
                return true;
            }
            else if(gameGridTransforms[desiredPos].tag == "moveableTile")
            {
                Transform tileTransform = gameGridTransforms[desiredPos];
                Tile tile = gameGridTiles[desiredPos];
                int tileDesiredPos = desiredPos + playerMovement;

                if (tileDesiredPos < 100 && gameGridTransforms[tileDesiredPos] == null)
                {
                    if (filterTiles[tileDesiredPos] == null || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x + 1, tileTransform.transform.position.y, tileTransform.transform.position.z);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTransforms[desiredPos] = playerBall;
                        UpdateGameGridTiles();
                        CheckAdjacentTiles();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool ValidateUpMovement(int playerMovement)
    {
        int currentPlayerPos = gameGridTransforms.IndexOf(playerBall);
        int desiredPos = currentPlayerPos + playerMovement;
        if (currentPlayerPos % 10 != 0)
        {
            if (gameGridTransforms[desiredPos] == null)
            {
                gameGridTransforms[currentPlayerPos] = null;
                gameGridTransforms[desiredPos] = playerBall;
                return true;
            }
            else if (gameGridTransforms[desiredPos].tag == "moveableTile")
            {
                Transform tileTransform = gameGridTransforms[desiredPos];
                Tile tile = gameGridTiles[desiredPos];
                int tilePos = desiredPos;
                int tileDesiredPos = desiredPos + playerMovement;

                if (tilePos % 10 != 0 && gameGridTransforms[tileDesiredPos] == null)
                {
                    if (filterTiles[tileDesiredPos] == null || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z + 1);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTransforms[desiredPos] = playerBall;
                        UpdateGameGridTiles();
                        CheckAdjacentTiles();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool ValidateDownMovement(int playerMovement)
    {
        int currentPlayerPos = gameGridTransforms.IndexOf(playerBall);
        int desiredPos = currentPlayerPos + playerMovement;
        if (desiredPos % 10 != 0)
        {
            if (gameGridTransforms[desiredPos] == null)
            {
                gameGridTransforms[currentPlayerPos] = null;
                gameGridTransforms[desiredPos] = playerBall;
                return true;
            }
            else if (gameGridTransforms[desiredPos].tag == "moveableTile")
            {
                Transform tileTransform = gameGridTransforms[desiredPos];
                Tile tile = gameGridTiles[desiredPos];
                int tilePos = desiredPos;
                int tileDesiredPos = desiredPos + playerMovement;

                if (tileDesiredPos % 10 != 0 && gameGridTransforms[tileDesiredPos] == null)
                {
                    if (filterTiles[tileDesiredPos] == null || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z - 1);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTransforms[desiredPos] = playerBall;
                        UpdateGameGridTiles();
                        CheckAdjacentTiles();
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
