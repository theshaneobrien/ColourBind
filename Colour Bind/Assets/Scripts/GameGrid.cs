using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private List<Transform> gameGridTransforms;
    [SerializeField]
    private List<Tile> gameGridTiles;
    private List<Tile> filterTiles = new List<Tile>();

    private List<Tile> redTiles = new List<Tile>();
    private List<Tile> greenTiles = new List<Tile>();
    private List<Tile> blueTiles = new List<Tile>();
    private List<Tile> yellowTiles = new List<Tile>();

    private List<Tile> redCheckedTiles = new List<Tile>();
    private List<Tile> greenCheckedTiles = new List<Tile>();
    private List<Tile> blueCheckedTiles = new List<Tile>();
    private List<Tile> yellowCheckedTiles = new List<Tile>();

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
        AddTileToColor(tile);
    }
    private void AddTileToColor(Tile tile)
    {
        switch (tile.color)
        {
            case "red":
                redTiles.Add(tile);
                break;
            case "green":
                greenTiles.Add(tile);
                break;
            case "blue":
                blueTiles.Add(tile);
                break;
            case "yellow":
                yellowTiles.Add(tile);
                break;
        }
    }

    private void CheckChain(Tile currentTile, List<Tile> colorToCheckAgainst, List<Tile> checkedColor)
    {
        if (checkedColor.Count < colorToCheckAgainst.Count)
        {
            int tilePos = gameGridTiles.IndexOf(currentTile);
            int upPos = tilePos + 1;
            int downPos = tilePos - 1;
            int leftPos = tilePos - 10;
            int rightPos = tilePos + 10;

            if (upPos % 10 != 0 && upPos < 100)
            {
                Tile upTile = gameGridTiles[upPos];
                if (upTile != null && !currentTile.upChecked && upTile.color == currentTile.color)
                {
                    if (!checkedColor.Contains(currentTile))
                    {
                        checkedColor.Add(currentTile);
                    }
                    currentTile.upChecked = true;
                    CheckChain(upTile, colorToCheckAgainst, checkedColor);
                }
            }

            if (downPos % 10 != 9 && downPos > 0)
            {
                Tile downTile = gameGridTiles[downPos];
                if (downTile != null && !currentTile.downChecked && downTile.color == currentTile.color)
                {
                    if (!checkedColor.Contains(currentTile))
                    {
                        checkedColor.Add(currentTile);
                    }
                    currentTile.downChecked = true;
                    CheckChain(downTile, colorToCheckAgainst, checkedColor);
                }
            }

            if (leftPos > 0)
            {
                Tile leftTile = gameGridTiles[leftPos];
                if (leftTile != null && !currentTile.leftChecked && leftTile.color == currentTile.color)
                {
                    if (!checkedColor.Contains(currentTile))
                    {
                        checkedColor.Add(currentTile);
                    }
                    currentTile.leftChecked = true;
                    CheckChain(leftTile, colorToCheckAgainst, checkedColor);
                }
            }

            if (rightPos < 100)
            {
                Tile rightTile = gameGridTiles[rightPos];
                if (rightTile != null && !currentTile.rightChecked && rightTile.color == currentTile.color)
                {
                    if (!checkedColor.Contains(currentTile))
                    {
                        checkedColor.Add(currentTile);
                    }
                    currentTile.rightChecked = true;
                    CheckChain(rightTile, colorToCheckAgainst, checkedColor);
                }
            };
        }
    }
        static MethodInfo _clearConsoleMethod;
        static MethodInfo clearConsoleMethod
        {
            get
            {
                if (_clearConsoleMethod == null)
                {
                    Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
                    Type logEntries = assembly.GetType("UnityEditor.LogEntries");
                    _clearConsoleMethod = logEntries.GetMethod("Clear");
                }
                return _clearConsoleMethod;
            }
        }

        public static void ClearLogConsole()
        {
            clearConsoleMethod.Invoke(new object(), null);
        }

        private void CheckWin()
    {
        ClearLogConsole();
        for (int i = 0; i < redCheckedTiles.Count; i++)
        {
            redCheckedTiles[i].ResetChecks();
        }
        redCheckedTiles.Clear();
        CheckChain(redTiles[0], redTiles, redCheckedTiles);

        for (int i = 0; i < greenCheckedTiles.Count; i++)
        {
            greenCheckedTiles[i].ResetChecks();
        }
        greenCheckedTiles.Clear();
        CheckChain(greenTiles[0], greenTiles, greenCheckedTiles);

        for (int i = 0; i < blueCheckedTiles.Count; i++)
        {
            blueCheckedTiles[i].ResetChecks();
        }
        blueCheckedTiles.Clear();
        CheckChain(blueTiles[0], blueTiles, blueCheckedTiles);

        for (int i = 0; i < yellowCheckedTiles.Count; i++)
        {
            yellowCheckedTiles[i].ResetChecks();
        }
        yellowCheckedTiles.Clear();
        CheckChain(yellowTiles[0], yellowTiles, yellowCheckedTiles);

        Debug.Log("Got " + redCheckedTiles.Count + " red tiles");
        Debug.Log("Got " + greenCheckedTiles.Count + " green tiles");
        Debug.Log("Got " + blueCheckedTiles.Count + " blue tiles");
        Debug.Log("Got " + yellowCheckedTiles.Count + " yellow tiles");
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
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        //Put the player in the desired position
                        gameGridTransforms[desiredPos] = playerBall;
                        //Update the GameGridTile Array to match the transform array

                        //Check if the tiles are touching tiles of the same color
                        CheckWin();
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
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        gameGridTransforms[desiredPos] = playerBall;
                        CheckWin();
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
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        gameGridTransforms[desiredPos] = playerBall;
                        CheckWin();
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
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        gameGridTransforms[desiredPos] = playerBall;
                        CheckWin();
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
