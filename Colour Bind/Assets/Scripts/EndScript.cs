using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public Text lineOne;
    public Text lineTwo;
    string story1;
    string story2;
    void Start()
    {
        story1 = lineOne.text;
        story2 = lineTwo.text;
        lineOne.text = "";
        lineTwo.text = "";
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

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("MainMenu");

    }
}
