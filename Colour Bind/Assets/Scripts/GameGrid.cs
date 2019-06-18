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

    private Transform playerBall;

    public void AddFilterTileToGrid(Tile tile)
    {
        filterTiles.Add(tile);
    }

    public void AddPlayer(Transform player, int position)
    {
        playerBall = player;
        gameGridTransforms[position] = player;
    }

    public void AddTileToGrid(Tile tile, int position)
    {
        gameGridTiles[position] = tile;
        gameGridTransforms[position] = tile.transform;
    }

    private void CheckAdjacentTiles()
    {
        //TODO: Needs refactor

        CheckWin();
    }

    private bool CheckWin()
    {
        foreach (Tile tile in gameGridTiles)
        {
            if (tile != null)
            {
                if (!tile.isTouchingSameColor)
                {
                    return false;
                }
            }
        }
        Debug.Log("You won!");
        return true;
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
            //If the desired position has a moveable tile, we move the player and the tile
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
                    if (filterTiles[tileDesiredPos].color == "white" || filterTiles[tileDesiredPos].color == tile.color)
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
                    if (filterTiles[tileDesiredPos].color == "white" || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x + 1, tileTransform.transform.position.y, tileTransform.transform.position.z);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTransforms[desiredPos] = playerBall;

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
        if (currentPlayerPos % 10 != 9)
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

                if (tilePos % 10 != 9 && gameGridTransforms[tileDesiredPos] == null)
                {
                    if (filterTiles[tileDesiredPos].color == "white" || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z + 1);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTransforms[desiredPos] = playerBall;

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
                    if (filterTiles[tileDesiredPos].color == "white" || filterTiles[tileDesiredPos].color == tile.color)
                    {
                        tileTransform.transform.position = new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z - 1);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTransforms[desiredPos] = playerBall;

                        CheckAdjacentTiles();
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
