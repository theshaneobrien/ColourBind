﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    private LevelSpawner levelSpawner;

    private int currentScore = 0;
    private int currentLives = 2;
    private float currentLevelTime = 0.0f;
    private string currentLevel = "";

    private int previousScore;
    private int finishedTime;

    private float warningInterval = 1.0f;
    private float trackedWarningTime = 0.0f;

    public Text scoreText;
    public Text livesText;
    public Text timeText;
    public Text levelText;
    public Text gameOverText;

    public bool playerStarted = false;
    public AudioSource audioSource;
    public AudioClip push;
    public AudioClip teleportIn;
    public AudioClip teleportOut;
    public AudioClip hurryUp;
    public AudioClip death;
    public AudioClip gameOver;
    public AudioClip scoreCount;

    private BallMovement playerBall;
    private Animator playerAnim;

    private bool gamePaused;

    public PauseButton pauseButton;

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        pauseButton.TogglePause();
        playerBall.gamePaused = !playerBall.gamePaused;
    }

    public void Retry()
    {
        StartCoroutine(Death());
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    void Update()
    {
        CountDownTimer();
        if (!gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Death());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                BackToMainMenu();
            }

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseButton.TogglePause();
            gamePaused = !gamePaused;
        }
    }

    private void CountDownTimer()
    {
        if (playerStarted && !gamePaused)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer ||
                  Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
            {
                //Weird time multiplier to make it the same speed as the  C64 version
                currentLevelTime -= Time.deltaTime * 1.735f;
            }
            else
            {
                currentLevelTime -= Time.deltaTime * 1.2f;
            }
            timeText.text = currentLevelTime.ToString("0000");
            if(currentLevelTime <= 60)
            {
                trackedWarningTime += Time.deltaTime * 1.735f;

                if (trackedWarningTime >= warningInterval)
                {
                    audioSource.PlayOneShot(hurryUp);
                    trackedWarningTime = 0.0f;

                    if (timeText.color == new Color(0.749f, 0.749f, 0.749f))
                    {
                        timeText.color = new Color(0.3098f, 0.16078f, 0.117647f);
                    }
                    else
                    {
                        timeText.color = new Color(0.749f, 0.749f, 0.749f);
                    }
                }
            }
            if (currentLevelTime <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    public IEnumerator Death()
    {
        gamePaused = true;
        playerBall.gamePaused = true;
        pauseButton.Pause();
        if (currentLives > 0)
        {
            //Play Death Sound and Animation
            UpDateLives(-1);
            audioSource.PlayOneShot(death);
            playerAnim.Play("teleportOut");
            levelSpawner.gameGrid.AnimateTileDeath();
            yield return new WaitForSeconds(death.length + 0.90f);
            levelSpawner.ReloadLevel();
        }
        else
        {
            //GameOver
            //Fade in GameOver Text!
            //Load back to the Main Menu
            gameOverText.gameObject.SetActive(true);
            gameOverText.gameObject.GetComponent<Animator>().Play("gameOver");
            audioSource.PlayOneShot(gameOver);
            playerAnim.Play("teleportOut");
            levelSpawner.gameGrid.AnimateTileDeath();
            yield return new WaitForSeconds(gameOver.length + 0.90f);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public IEnumerator DeathByTileFall(Animator tileAnimator)
    {
        gamePaused = true;
        playerBall.gamePaused = true;
        pauseButton.Pause();
        if (currentLives > 0)
        {
            //Play Death Sound and Animation
            UpDateLives(-1);
            audioSource.PlayOneShot(death);
            tileAnimator.Play("tileFall");
            yield return new WaitForSeconds(death.length + 0.90f);
            levelSpawner.ReloadLevel();
        }
        else
        {
            //GameOver
            //Fade in GameOver Text!
            //Load back to the Main Menu
            gameOverText.gameObject.SetActive(true);
            gameOverText.gameObject.GetComponent<Animator>().Play("gameOver");

            tileAnimator.Play("tileFall");
            audioSource.PlayOneShot(gameOver);
            yield return new WaitForSeconds(gameOver.length + 0.90f);
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void UpDateLives(int lifeChange)
    {
        currentLives += lifeChange;
        livesText.text = currentLives.ToString("00");
        playerStarted = false;
    }

    public IEnumerator SetUpGameState(LevelSpawner lSpawner, int levelTime, string levelName, BallMovement playerB)
    {
        playerBall = playerB;
        gamePaused = true;
        playerBall.gamePaused = true;
        pauseButton.Pause();
        playerStarted = false;
        timeText.color = new Color(0.749f, 0.749f, 0.749f);
        levelSpawner = lSpawner;
        currentLevelTime = levelTime;
        currentLevel = levelName;
        timeText.text = levelTime.ToString("0000");
        livesText.text = currentLives.ToString("00");
        //Play Teleport In Animation + Sound
        playerAnim = playerBall.GetComponent<Animator>();
        audioSource.PlayOneShot(teleportIn);
        playerAnim.Play("teleportIn");
        
        yield return new WaitForSeconds(teleportIn.length);
        playerStarted = true;
        gamePaused = false;
        playerBall.gamePaused = false;

        pauseButton.UnPause();
    }

    public void SetUpFinalTally()
    {
        playerStarted = false;
        gamePaused = true;
        playerBall.gamePaused = true;
        pauseButton.Pause();
        previousScore = currentScore;
        finishedTime = (int)currentLevelTime;
    }

    public IEnumerator CountScore()
    {
        for (int i = finishedTime; i > 0; i--)
        {
            currentScore++;
            currentLevelTime--;
            scoreText.text = currentScore.ToString("000000");
            timeText.text = currentLevelTime.ToString("0000");
            audioSource.PlayOneShot(scoreCount);
            yield return new WaitForSeconds(0.005f);
        }

        //Teleport
        timeText.text = "000000";
        audioSource.PlayOneShot(teleportOut);
        playerAnim.Play("teleportOut");
        levelSpawner.gameGrid.AnimateTileTeleport();
        yield return new WaitForSeconds(teleportOut.length);
        levelSpawner.LoadNextLevel();
    }

    public void playMoveSound()
    {
        audioSource.PlayOneShot(push);
    }
}
