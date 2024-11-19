using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToQuit()
    {
        Application.Quit();
    }
    
}
