using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Image pauseImage;
    public Sprite pauseSprite;
    public Sprite playSprite;

    private bool gamePaused = false;
    // Start is called before the first frame update
    public void TogglePause()
    {
        Debug.Log("PAUSE");
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            pauseImage.sprite = playSprite;
        }
        else
        {
            pauseImage.sprite = pauseSprite;
        }
    }

    public void Pause()
    {
        gamePaused = true;

        pauseImage.sprite = playSprite;
    }

    public void UnPause()
    {
        gamePaused = false;

        pauseImage.sprite = pauseSprite;
    }
}
