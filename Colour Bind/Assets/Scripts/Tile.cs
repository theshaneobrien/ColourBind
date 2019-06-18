using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string color = "red";
    public bool upChecked = false;
    public bool downChecked = false;
    public bool leftChecked = false;
    public bool rightChecked = false;
    public int pos = 0;

    public void ResetChecks()
    {
        upChecked = false;
        downChecked = false;
        leftChecked = false;
        rightChecked = false;
    }
}
