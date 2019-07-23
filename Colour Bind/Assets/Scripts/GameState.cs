using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    private LevelSpawner levelSpawner;

    private int currentScore = 0;
    private int currentLives = 2;
    private float currentLevelTime = 0;
    private string currentLevel = "";

    private int previousScore;
    private int finishedTime;

    public Text scoreText;
    public Text livesText;
    public Text timeText;
    public Text levelText;

    public bool playerStarted = false;
    public AudioSource audioSource;
    public AudioClip push;
    public AudioClip teleportIn;
    public AudioClip teleportOut;
    public AudioClip hurryUp;
    public AudioClip death;
    public AudioClip gameOver;
    public AudioClip scoreCount;
    
    void Update()
    {
        CountDownTimer();
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Death());
        }
    }

    private void CountDownTimer()
    {
        if (playerStarted)
        {
            //Weird time multiplier to make it the same speed as the  C64 version
            currentLevelTime -= Time.deltaTime * 1.735f;

            timeText.text = currentLevelTime.ToString("0000");
            if (currentLevelTime <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    public IEnumerator Death()
    {
        UpDateLives(-1);
        //Play Death Sound and Animation
        audioSource.PlayOneShot(death);
        yield return new WaitForSeconds(death.length);
        levelSpawner.ReloadLevel();
    }

    private void UpDateLives(int lifeChange)
    {
        currentLives += lifeChange;
        livesText.text = currentLives.ToString("00");
        playerStarted = false;
    }

    public IEnumerator SetUpGameState(LevelSpawner lSpawner, int levelTime, string levelName)
    {
        playerStarted = false;
        levelSpawner = lSpawner;
        currentLevelTime = levelTime;
        currentLevel = levelName;
        timeText.text = levelTime.ToString("0000");
        livesText.text = currentLives.ToString("00");
        //Play Teleport In Animation + Sound
        audioSource.PlayOneShot(teleportIn);
        yield return new WaitForSeconds(teleportIn.length);
        playerStarted = true;
    }

    public void SetUpFinalTally()
    {
        playerStarted = false;
        previousScore = currentScore;
        finishedTime = (int)currentLevelTime;
    }

    public IEnumerator CountScore()
    {
        for (int i = finishedTime; i > -1; i--)
        {
            currentScore++;
            currentLevelTime--;
            scoreText.text = currentScore.ToString("000000");
            timeText.text = currentLevelTime.ToString("0000");
            audioSource.PlayOneShot(scoreCount);
            yield return new WaitForSeconds(0.005f);
        }

        //Teleport
        audioSource.PlayOneShot(teleportOut);
        yield return new WaitForSeconds(teleportOut.length);
        levelSpawner.LoadNextLevel();
    }

    public void playMoveSound()
    {
        audioSource.PlayOneShot(push);
    }
}
