using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDefaultLevels()
    {
        PlayerPrefs.SetInt("resume", 0);
        SceneManager.LoadScene("MainLevels");
    }

    public void ResumeGame()
    {
        PlayerPrefs.SetInt("resume", 1);
        SceneManager.LoadScene("MainLevels");
    }
}
