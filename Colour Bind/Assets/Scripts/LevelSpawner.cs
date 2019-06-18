using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    public GameObject[] gameGridTilePrefabs;
    public GameObject[] filterTilePrefabs;
    public GameObject player;

    public GameGrid gameGrid;

    public Level currentLevel;


    public void Awake()
    {
        SpawnInitialLevel();
        SpawnGameGridTiles();
    }

    public void SpawnInitialLevel()
    {
        int xPos = -1;
        int yPos = 0;
        for (int i = 0; i < currentLevel.filterTileIds.Count; i++)
        {
            if (i % 10 == 0)
            {
                xPos++;
                yPos = 0;
            }
            yPos++;

            Tile tile = Instantiate(filterTilePrefabs[currentLevel.filterTileIds[i]], new Vector3(xPos, 0, yPos - 1), Quaternion.identity).GetComponent<Tile>();
            gameGrid.AddFilterTileToGrid(tile);
            if (i == currentLevel.playerPos)
            {
                BallMovement playerBall = Instantiate(player, new Vector3(xPos, 1, yPos - 1), Quaternion.identity).GetComponent<BallMovement>();
                gameGrid.AddPlayer(playerBall.transform, i);
                playerBall.SetGameGrid(gameGrid);
            }
        }
    }

    public void SpawnGameGridTiles()
    {
        int xPos = -1;
        int yPos = 0;
        for (int i = 0; i < currentLevel.gameGridTileIds.Count; i++)
        {
            if (i % 10 == 0)
            {
                xPos++;
                yPos = 0;
            }
            yPos++;
            if (currentLevel.gameGridTileIds[i] != 0)
            {
                Tile tile = Instantiate(gameGridTilePrefabs[currentLevel.gameGridTileIds[i]], new Vector3(xPos, 1, yPos - 1), Quaternion.identity).GetComponent<Tile>();
                gameGrid.AddTileToGrid(tile, i);
            }
        }
    }
}
