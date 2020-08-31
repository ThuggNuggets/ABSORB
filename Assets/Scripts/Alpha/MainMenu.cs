using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private string loadingScreen = "Loading_Scene";

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadingScreen);
    }

    public void Exit()
    {
        Application.Quit();
    }
}