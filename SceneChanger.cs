using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public string sceneName; //Publicly set string name of scene
void Start()
{

}

void Update()
{

}

public void changeScene()
{
    SceneManager.LoadScene(sceneName); // Pass scene name string to unity scene manager/Loadscene Function
Debug.Log("Welcome to level one"); //Debug logs to console if function completes successfully
}
    
}
