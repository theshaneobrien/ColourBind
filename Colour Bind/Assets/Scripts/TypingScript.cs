using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypingScript : MonoBehaviour
{
    public Text lineOne;
    public Text lineTwo;
    public Text lineThree;
    public Text lineFour;
    public Text lineFive;
    public Text lineSix;
    string story1;
    string story2;
    string story6;

    public AudioSource audioSource;
    public AudioClip loadingSound;
    void Start()
    {
        story1 = lineOne.text;
        story2 = lineTwo.text;
        story6 = lineSix.text;
        lineOne.text = "";
        lineTwo.text = "";
        lineSix.text = "";
        // TODO: add optional delay when to start
        StartCoroutine("PlayLine1Text");
    }

    IEnumerator PlayLine1Text()
    {
        foreach (char c in story1)
        {
            lineOne.text += c;
            yield return new WaitForSeconds(0.125f);
        }

        StartCoroutine("PlayLine2Text");
    }
    IEnumerator PlayLine2Text()
    {
        foreach (char c in story2)
        {
            lineTwo.text += c;
            yield return new WaitForSeconds(0.125f);
        }

        lineThree.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        lineFour.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        lineFive.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        StartCoroutine("PlayLine6Text");


    }
    IEnumerator PlayLine6Text()
    {
        foreach (char c in story6)
        {
            lineSix.text += c;
            yield return new WaitForSeconds(0.125f);
        }

        //PlayLoadSound
        audioSource.PlayOneShot(loadingSound);
        yield return new WaitForSeconds(loadingSound.length);
        SceneManager.LoadScene("MainMenu");
    }
}
