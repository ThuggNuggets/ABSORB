using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public Slider volumeSlider;
    private ReadWriteText readWrite;
    private InputManager _inputManager;
    private bool Paused;
    private MainMenu _mainMenu;

    #region Camera shizniz

    private CameraManager _cameraManager;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        readWrite = GetComponent<ReadWriteText>();
        _inputManager = FindObjectOfType<InputManager>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _mainMenu = GetComponent<MainMenu>();
        volumeSlider.value = readWrite.volume;
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure you can't pause when you die or the game is over
        if (Input.GetKeyDown(KeyCode.Escape) && !_mainMenu.inMainMenu)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (Paused) // Game unpaused
        {
            Paused = false;
            _inputManager.EnableInput();
            pauseMenu.SetActive(false);
            readWrite.volume = volumeSlider.value;
            readWrite.overrideControls = _inputManager.GetOverrideController();
            readWrite.OverwriteData();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cameraManager.EnableCameraMovement();
        }
        else // Game is paused
        {
            Paused = true;
            _inputManager.DisableInput();
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cameraManager.DisableCameraMovement();
        }
    }

    public void ResumeGame()
    {
        Pause();
    }

    public void QuitGame()
    {
        // ADD A "ARE YOU SURE?" FIRST
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ToggleOverrideControls()
    {
        _inputManager.SetOverrideController(!_inputManager.GetOverrideController());
    }
}
