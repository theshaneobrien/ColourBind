using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationScroll : MonoBehaviour
{
    public Text infoBox;
    private List<string> information = new List<string>();
    private Animator anim;
    private float timeToLoopMessages = 60f;
    private float timeToShowMessages = 3.15f;
    private float currentLoop = 0;
    private float currentMessageTime = 0;
    private int informationIndex = 0;
    public bool changeIndex = false;
    // Start is called before the first frame update
    void Start()
    {
        infoBox = GetComponent<Text>();
        anim = GetComponent<Animator>();
        information.Add("WELCOME TO\nCOLOUR BIND.");
        information.Add("COPYRIGHT MARK\nMAINWOOD 2019.");
        information.Add("COLOUR BIND WAS\nCONCEIVED DESIGNED");
        information.Add("AND ORIGINALLY\nPROGRAMMED BY");
        information.Add("MARK MAINWOOD.");
        information.Add("REMAKE PROGRAMMED\nBY SHANE OBRIEN");
        information.Add("ART BY\nCLARE BYRNE.");
        information.Add("WORK YOUR WAY\nTHROUGH ALL 20");
        information.Add("LEVELS BY PUSHING\nBLOCKS SO THAT ALL");
        information.Add("BLOCKS OF THE SAME\nCOLOUR ARE JOINED");
        information.Add("TOGETHER. DO NOT FALL\nDOWN HOLES OR PUSH");
        information.Add("BLOCKS INTO THEM!\nDARK BLUE BLOCKS");
        information.Add("CANNOT BE MOVED. ONLY\nBLOCKS OF THE SAME");
        information.Add("COLOUR AS A FILTER\nCAN BE PUSHED ACROSS");
        information.Add("OR ONTO IT. TIME LEFT\nIS TURNED INTO");
        information.Add("POINTS WHEN A LEVEL\nIS FINISHED.");
        information.Add("R OR THE RESTART BUTTON\nQUITS THE CURRENT");
        information.Add("LIFE. Q RETURNS YOU\nTO THE TITLE SCREEN");
        information.Add("P PAUSES THE GAME.\nNOW STOP READING");
        information.Add("THIS MESSAGE AND\nSTART PLAYING!!!");
    }

    // Update is called once per frame
    void Update()
    {
        currentMessageTime += Time.deltaTime;

        if(currentMessageTime >= timeToShowMessages)
        {

            IncrementIndex();

            currentMessageTime = 0;
        }
    }

    public void IncrementIndex()
    {
        changeIndex = false;
        informationIndex++;
        if (informationIndex >= information.Count)
        {
            informationIndex = 0;
        }
        infoBox.text = information[informationIndex];
        anim.SetTrigger("change");
    }

}
