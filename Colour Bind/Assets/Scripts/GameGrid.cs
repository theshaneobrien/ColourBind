using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private List<Transform> gameGridTransforms = new List<Transform>();
    [SerializeField]
    private List<Tile> gameGridTiles = new List<Tile>();
    private List<Tile> filterTiles = new List<Tile>();

    private List<Tile> redTiles = new List<Tile>();
    private List<Tile> greenTiles = new List<Tile>();
    private List<Tile> blueTiles = new List<Tile>();
    private List<Tile> yellowTiles = new List<Tile>();

    private List<Tile> redCheckedTiles = new List<Tile>();
    private List<Tile> greenCheckedTiles = new List<Tile>();
    private List<Tile> blueCheckedTiles = new List<Tile>();
    private List<Tile> yellowCheckedTiles = new List<Tile>();

    [SerializeField]
    private LevelSpawner levelSpawner;
    [SerializeField]
    private GameState gameState;

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
        if (tile != null)
        {
            gameGridTiles.Add(tile);
            gameGridTransforms.Add(tile.transform);
            AddTileToColor(tile);
        }
        else
        {
            gameGridTiles.Add(null);
            gameGridTransforms.Add(null);
        }
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

            if (downPos % 10 != 9 && downPos >= 0)
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

            if (leftPos >= 0)
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

        private void CheckWin()
    {
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

        if(redCheckedTiles.Count == redTiles.Count &&
            greenCheckedTiles.Count == greenTiles.Count &&
            blueCheckedTiles.Count == blueTiles.Count &&
            yellowCheckedTiles.Count == yellowTiles.Count)
        {
            gameState.SetUpFinalTally();
            StartCoroutine(gameState.CountScore());
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

                if(filterTiles[desiredPos].tag == "blackHole")
                {
                   StartCoroutine(gameState.Death());
                }
                return true;
            }
            //If the desired position has a moveable tile, we move the player and the tile
            //DUNNO WHY THIS ? is required to prevent the null
            else if (gameGridTransforms[desiredPos]?.tag == "moveableTile")
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
                        gameState.playMoveSound();
                        //Remove the player from the previous position
                        gameGridTransforms[currentPlayerPos] = null;
                        //Put the tile in it's desired position
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        //Put the player in the desired position
                        gameGridTransforms[desiredPos] = playerBall;
                        //Update the GameGridTile Array to match the transform array

                        StartCoroutine(MoveTileToPos(tileTransform, new Vector3(tileTransform.transform.position.x - 1, tileTransform.transform.position.y, tileTransform.transform.position.z)));
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

                if (filterTiles[desiredPos].tag == "blackHole")
                {
                    StartCoroutine(gameState.Death());
                }
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
                        gameState.playMoveSound();
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        gameGridTransforms[desiredPos] = playerBall;
                        StartCoroutine(MoveTileToPos(tileTransform, new Vector3(tileTransform.transform.position.x + 1, tileTransform.transform.position.y, tileTransform.transform.position.z)));
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

                if (filterTiles[desiredPos].tag == "blackHole")
                {
                    StartCoroutine(gameState.Death());
                }
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
                        gameState.playMoveSound();
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        gameGridTransforms[desiredPos] = playerBall;
                        StartCoroutine(MoveTileToPos(tileTransform, new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z + 1)));
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

                if (filterTiles[desiredPos].tag == "blackHole")
                {
                    StartCoroutine(gameState.Death());
                }
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
                        gameState.playMoveSound();
                        //tileTransform.transform.position = new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z - 1);
                        gameGridTransforms[currentPlayerPos] = null;
                        gameGridTiles[desiredPos] = null;
                        gameGridTransforms[tileDesiredPos] = tileTransform;
                        gameGridTiles[tileDesiredPos] = tile;
                        gameGridTransforms[desiredPos] = playerBall;
                        StartCoroutine(MoveTileToPos(tileTransform, new Vector3(tileTransform.transform.position.x, tileTransform.transform.position.y, tileTransform.transform.position.z - 1)));
                        CheckWin();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator MoveTileToPos(Transform tileToMove, Vector3 desiredPos)
    {
        float startTime = Time.time;

        float journeyLength;

        journeyLength = Vector3.Distance(tileToMove.transform.position, desiredPos);
        while (Vector3.Distance(tileToMove.transform.position, desiredPos) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * 1.5f;
            float fracJourney = distCovered / journeyLength;
            tileToMove.transform.position = Vector3.Lerp(tileToMove.transform.position, desiredPos, fracJourney);
            yield return null;
        }
        tileToMove.transform.position = desiredPos;
    }

        public void CleanUpLevel()
    {
        //remove all tiles, reset all arrays
        for (int i = 0; i < gameGridTiles.Count; i++)
        {
            if (gameGridTiles[i] != null)
            {
                Destroy(gameGridTiles[i].gameObject);
            }
        }
        gameGridTiles.Clear();
        gameGridTransforms.Clear();
        redTiles.Clear();
        blueTiles.Clear();
        greenTiles.Clear();
        yellowTiles.Clear();
        for (int i = 0; i < filterTiles.Count; i++)
        {
            Destroy(filterTiles[i].gameObject);
        }
        filterTiles.Clear();
        Destroy(playerBall.gameObject);
    }
}
