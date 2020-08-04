using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // Player movement script
    public Movement playerMovement;

    // Player cameras
    public CinemachineFreeLook controllerCamera;
    public CinemachineFreeLook mouseCamera;

    // Input Manager script
    public InputManager inputManager;

    private bool controllerUpdated = false;

    // Only show on menu when controller is connected?
    public bool overrideController = false;

    // Start is called before the first frame update
    void Start()
    {
        // If controller is connected and not overridden
        if (inputManager.GetControllerConnected()/* && !overrideController*/)
            SetControllerCamera();
        else if (!inputManager.GetControllerConnected()/* || overrideController*/) // If no controller is connected or player has selected to use keyboard controls
            SetMouseCamera();

        controllerUpdated = inputManager.GetControllerConnected();
    }

    // Update is called once per frame
    void Update()
    {
        // Run when the inputManager and local var dont match
        // (Basically this should on run once when a controller is connected/disconnected)
        if (inputManager.GetControllerConnected() != controllerUpdated)
        {
            if (inputManager.GetControllerConnected())
                SetControllerCamera();
            else if (!inputManager.GetControllerConnected())
                SetMouseCamera();

            controllerUpdated = inputManager.GetControllerConnected();
            Debug.LogWarning("Controller changed");
        }
    }

    void SetControllerCamera()
    {
        // Enable/disable correct camera
        controllerCamera.gameObject.SetActive(true);
        mouseCamera.gameObject.SetActive(false);

        // Set player movement cameras
        playerMovement.freeLookCamera = controllerCamera;
        playerMovement.cameraTransform = controllerCamera.transform;

        // Set InputManager camera
        inputManager.cinemachine = controllerCamera;
    }

    void SetMouseCamera()
    {
        // Enable/disable correct camera
        mouseCamera.gameObject.SetActive(true);
        controllerCamera.gameObject.SetActive(false);

        // Set player movement cameras
        playerMovement.freeLookCamera = mouseCamera;
        playerMovement.cameraTransform = mouseCamera.transform;

        // Set InputManager camera
        inputManager.cinemachine = mouseCamera;
    }
}
