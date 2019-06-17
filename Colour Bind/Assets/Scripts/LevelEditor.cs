using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public Level level1;

    // Start is called before the first frame update
    void Start()
    {
        level1.SaveLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
