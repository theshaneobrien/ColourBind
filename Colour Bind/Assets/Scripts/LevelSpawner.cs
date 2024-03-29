﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{

    public GameObject[] gameGridTilePrefabs;
    public GameObject[] filterTilePrefabs;
    public GameObject player;
    private BallMovement playerBall;

    public GameGrid gameGrid;
    public GameState gameState;

    public Level currentLevel;
    public List<Level> levels;


    public void Awake()
    {
        int resume = PlayerPrefs.GetInt("resume", 0);
        if (resume == 1)
        {
            Debug.Log("Resuming level: " + PlayerPrefs.GetInt("currentLevel", 0));
            currentLevel = levels[PlayerPrefs.GetInt("currentLevel", 0)];
            SpawnLevel(currentLevel);
            SpawnGameGridTiles(currentLevel);
            StartCoroutine(gameState.SetUpGameState(this, currentLevel.levelTime, currentLevel.levelName, playerBall));
        }
        else
        {
            currentLevel = levels[0];
            SpawnLevel(currentLevel);
            SpawnGameGridTiles(currentLevel);
            StartCoroutine(gameState.SetUpGameState(this, currentLevel.levelTime, currentLevel.levelName, playerBall));
        }
        
    }

    public void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                LoadNextLevel();
            }
        }
    }

    public void SpawnLevel(Level levelToSpawn)
    {
        int xPos = -1;
        int yPos = 0;
        for (int i = 0; i < levelToSpawn.filterTileIds.Count; i++)
        {
            if (i % 10 == 0)
            {
                xPos++;
                yPos = 0;
            }
            yPos++;

            Tile tile = Instantiate(filterTilePrefabs[levelToSpawn.filterTileIds[i]], new Vector3(xPos, 0, yPos - 1), Quaternion.identity).GetComponent<Tile>();
            gameGrid.AddFilterTileToGrid(tile);
        }
    }

    public void SpawnGameGridTiles(Level levelToSpawn)
    {
        int xPos = -1;
        int yPos = 0;
        for (int i = 0; i < levelToSpawn.gameGridTileIds.Count; i++)
        {
            if (i % 10 == 0)
            {
                xPos++;
                yPos = 0;
            }
            yPos++;
            if (levelToSpawn.gameGridTileIds[i] != 0)
            {
                Tile tile = Instantiate(gameGridTilePrefabs[levelToSpawn.gameGridTileIds[i]], new Vector3(xPos, 1, yPos - 1), Quaternion.identity).GetComponent<Tile>();
                gameGrid.AddTileToGrid(tile, i);

            }
            else
            {
                gameGrid.AddTileToGrid(null, i);
            }
            if (i == levelToSpawn.playerPos)
            {
                playerBall = Instantiate(player, new Vector3(xPos, 1, yPos - 1), Quaternion.Euler(-25f, 45, 0)).GetComponent<BallMovement>();
                playerBall.SetGameGrid(gameGrid);
                gameGrid.AddPlayer(playerBall.transform, i);
            }
        }
    }

    public void LoadNextLevel()
    {
        if (levels.IndexOf(currentLevel) != levels.Count - 1)
        {
            currentLevel = levels[levels.IndexOf(currentLevel) + 1];

            PlayerPrefs.SetInt("currentLevel", levels.IndexOf(currentLevel));
            gameGrid.CleanUpLevel();
            SpawnLevel(currentLevel);
            SpawnGameGridTiles(currentLevel);
            StartCoroutine(gameState.SetUpGameState(this, currentLevel.levelTime, currentLevel.levelName, playerBall));
        }
        else
        {
            SceneManager.LoadScene("WIN");
        }
    }

    public void ReloadLevel()
    {
        gameGrid.CleanUpLevel();
        SpawnLevel(currentLevel);
        SpawnGameGridTiles(currentLevel);
        StartCoroutine(gameState.SetUpGameState(this, currentLevel.levelTime, currentLevel.levelName,playerBall));
    }
}
