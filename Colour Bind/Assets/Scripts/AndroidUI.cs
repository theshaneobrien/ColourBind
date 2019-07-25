using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || 
            Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            gameObject.SetActive(false);
        }
    }
}
