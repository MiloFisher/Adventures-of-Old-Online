using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void NewGameScene()
    {
        SceneManager.LoadScene("New Game");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Load Game");

    }
    public void JoinGameScene()
    {
        SceneManager.LoadScene("Join Game");
    }

    public void QuitScene()
    {
        Debug.Log("Quit Program");
        Application.Quit();
    }
}
