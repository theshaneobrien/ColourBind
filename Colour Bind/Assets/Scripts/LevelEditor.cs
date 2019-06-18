using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public Level blankLevel;

    public Tile selectedObject;
    public List<Transform> placeableObjects;

    private void Awake()
    {
        int xPos = 11;
        int ypos = 9;
        for (int i = 0; i < placeableObjects.Count; i++)
        {
            Instantiate(placeableObjects[i], new Vector3(xPos, 1, ypos), Quaternion.identity);
                xPos += 2;


        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
