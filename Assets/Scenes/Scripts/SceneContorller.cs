using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelScene()
    {
        SceneManager.LoadScene(2);
    }

    public void HelpScene()
    {
        SceneManager.LoadScene(3);
    }

    public void NextLevelScene()
    {
        SceneManager.LoadScene(4);
    }

    public void MainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LastScene()
    {
        //Time.timeScale = 0;
        SceneManager.LoadScene(5);
    }
 
    public void EndGame()
    {
        Application.Quit();
    }

}
