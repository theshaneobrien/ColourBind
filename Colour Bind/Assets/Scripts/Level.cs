using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "New Level", order = 1)]
public class Level : ScriptableObject
{
    public string levelName;
    public string levelAuthor;
    public string levelTime;
    public List<int> gameGridTileIds;
    public List<int> filterTileIds;
    public int playerPos;
    // Start is called before the first frame update
    void Start()
    {

    }
        
    public bool SaveLevel()
    {
        if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/My Games/Colour Bind/"))
        {
            string serializedData = JsonUtility.ToJson(this);
            string filePathName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/My Games/Colour Bind/" + levelName + ".json";
            System.IO.File.WriteAllText(filePathName, serializedData);
            Debug.Log(serializedData);
        }
        else
        {
            System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/My Games/Colour Bind/");
            string serializedData = JsonUtility.ToJson(this);
            string filePathName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/My Games/Colour Bind/" + levelName + ".json";
            System.IO.File.WriteAllText(filePathName, serializedData);
            Debug.Log(serializedData);
        }

        return false;
    }
}
