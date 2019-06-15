using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private List<Transform> gameGridTransforms;
    [SerializeField]
    private List<Tile> gameGridTiles;
    [SerializeField]
    private List<Tile> floorGridTiles;

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
        #region Checking Regular Tiles
        foreach (List<Tile> tileList in allTiles)
        {
            if (tileList != null)
            {
                foreach (Tile tile in tileList)
                {
                    tile.isTouchingSameColor = false;
                    //Get the color we are checking for
                    string tileColor = tile.color;
                    //The current position of the tile we are checking
                    ///POTENTIAL BUG HERE?! As FloorTile pos not updated every move...
                    int gameGridPos = tile.pos;
                    //The List indexes of the tiles left, right, up and down from us
                    int leftPlace = gameGridPos - 10;
                    int rightPlace = gameGridPos + 10;
                    int upPlace = gameGridPos - 1;
                    int downPlace = gameGridPos + 1;

                    //If we are in the top row, don't check the next array index up
                    if (gameGridPos % 10 == 0)
                    {
                        if (gameGridTiles[downPlace]?.color == tile.color ||
                            gameGridTiles[leftPlace]?.color == tile.color ||
                            gameGridTiles[rightPlace]?.color == tile.color ||
                            floorGridTiles[gameGridPos]?.color == tile.color)
                        {
                            if (gameGridTiles[upPlace] != tile && gameGridTiles[leftPlace] != tile)
                            {
                                tile.isTouchingSameColor = true;
                            }
                        }
                    }
                    //If we are in the bottom row, don't check the previous array index up
                    else if (gameGridPos % 10 == 9)
                    {
                        if (gameGridTiles[upPlace]?.color == tile.color ||
                            gameGridTiles[leftPlace]?.color == tile.color ||
                            gameGridTiles[rightPlace]?.color == tile.color ||
                            floorGridTiles[gameGridPos]?.color == tile.color)
                        {
                            //Debug.Log("test bottom row");
                            if (gameGridTiles[downPlace] != tile && gameGridTiles[rightPlace] != tile)
                            {
                                tile.isTouchingSameColor = true;
                            }
                        }
                    }
                    else
                    //Otherwise check all directions
                    {
                        //Debug.Log("test anything else");
                        if (gameGridTiles[upPlace]?.color == tile.color ||
                            gameGridTiles[downPlace]?.color == tile.color ||
                            gameGridTiles[leftPlace]?.color == tile.color ||
                            gameGridTiles[rightPlace]?.color == tile.color ||
                            floorGridTiles[gameGridPos]?.color == tile.color)
                        {
                            tile.isTouchingSameColor = true;
                        }
                    }

                    if (gameGridPos == 0)
                    {
                        if (gameGridTiles[downPlace]?.color == tile.color ||
                            gameGridTiles[rightPlace]?.color == tile.color)
                        {
                            //Debug.Log("test 0 pos");
                            tile.isTouchingSameColor = true;
                        }
                    }
                    else if (gameGridPos == 99)
                    {
                        if (gameGridTiles[upPlace]?.color == tile.color ||
                            gameGridTiles[leftPlace]?.color == tile.color)
                        {
                            //Debug.Log("test 99 pos");
                            tile.isTouchingSameColor = true;
                        }
                    }
                    
                }
            }
        }
        #endregion

        #region Checking Floor Tiles
        foreach (Tile floorTile in floorGridTiles)
        {
            if (floorTile != null)
            {
                floorTile.isTouchingSameColor = false;
                //Get the color we are checking for
                string tileColor = floorTile.color;
                //The current position of the tile we are checking
                int floorGridPos = floorGridTiles.IndexOf(floorTile);

                if (gameGridTiles[floorGridPos]?.color == tileColor)
                {
                    floorTile.isTouchingSameColor = true;
                    gameGridTiles[floorGridPos].isTouchingSameColor = true;
                }
            }
        }
        #endregion

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
        int currentPlayerPos = gameGridTransforms.IndexOf(playerBall);
        int desiredPos = currentPlayerPos + playerMovement;

        if (desiredPos >= 0)
        {
            if (gameGridTransforms[desiredPos] == null && desiredPos >= 0 && desiredPos < 100)
            {
                gameGridTransforms[currentPlayerPos] = null;
                gameGridTransforms[desiredPos] = playerBall;
                return true;
            }
            else if (gameGridTransforms[desiredPos].tag == "moveableTile")
            {
                Transform tile = gameGridTransforms[desiredPos];
                int tileDesiredPos = desiredPos + playerMovement;

                if (tileDesiredPos >= 0 && gameGridTransforms[tileDesiredPos] == null)
                {
                    tile.transform.position = new Vector3(tile.transform.position.x - 1, tile.transform.position.y, tile.transform.position.z);
                    gameGridTransforms[currentPlayerPos] = null;
                    gameGridTransforms[tileDesiredPos] = tile;
                    gameGridTransforms[desiredPos] = playerBall;
                    UpdateGameGridTiles();
                    CheckAdjacentTiles();
                    return true;
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
                Transform tile = gameGridTransforms[desiredPos];
                int tileDesiredPos = desiredPos + playerMovement;

                if (tileDesiredPos < 100 && gameGridTransforms[tileDesiredPos] == null)
                {
                    tile.transform.position = new Vector3(tile.transform.position.x + 1, tile.transform.position.y, tile.transform.position.z);
                    gameGridTransforms[currentPlayerPos] = null;
                    gameGridTransforms[tileDesiredPos] = tile;
                    gameGridTransforms[desiredPos] = playerBall;
                    UpdateGameGridTiles();
                    CheckAdjacentTiles();
                    return true;
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
                Transform tile = gameGridTransforms[desiredPos];
                int tilePos = desiredPos;
                int tileDesiredPos = desiredPos + playerMovement;

                if (tilePos % 10 != 0 && gameGridTransforms[tileDesiredPos] == null)
                {
                    tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
                    gameGridTransforms[currentPlayerPos] = null;
                    gameGridTransforms[tileDesiredPos] = tile;
                    gameGridTransforms[desiredPos] = playerBall;
                    UpdateGameGridTiles();
                    CheckAdjacentTiles();
                    return true;
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
                Transform tile = gameGridTransforms[desiredPos];
                int tilePos = desiredPos;
                int tileDesiredPos = desiredPos + playerMovement;

                if (tileDesiredPos % 10 != 0 && gameGridTransforms[tileDesiredPos] == null)
                {
                    tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z - 1);
                    gameGridTransforms[currentPlayerPos] = null;
                    gameGridTransforms[tileDesiredPos] = tile;
                    gameGridTransforms[desiredPos] = playerBall;
                    UpdateGameGridTiles();
                    CheckAdjacentTiles();
                    return true;
                }
            }
        }
        return false;
    }
}
