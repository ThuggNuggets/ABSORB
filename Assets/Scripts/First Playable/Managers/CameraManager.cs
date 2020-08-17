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

    // Only show on menu when controller is connected?
    public bool overrideController = false;

    private bool controllerUpdated = false;
    private bool overrideUpdated = false;

    // Start is called before the first frame update
    void Start()
    {
        // If controller is connected and not overridden
        if (inputManager.GetControllerConnected() && !overrideController)
            SetControllerCamera();
        // If no controller is connected or player has selected to use keyboard controls
        else if (!inputManager.GetControllerConnected() || overrideController)
            SetMouseCamera();

        controllerUpdated = inputManager.GetControllerConnected();
        overrideUpdated = overrideController;
    }

    // Update is called once per frame
    void Update()
    {
        // Run when the inputManager and controllerUpdated dont match
        // (Basically this should on run once when a controller is connected/disconnected)
        // Also run if overrideController has been turned on/off
        if (inputManager.GetControllerConnected() != controllerUpdated || overrideController != overrideUpdated)
        {
            if (inputManager.GetControllerConnected() && !overrideController)
            {
                //controllerCamera.transform.position = mouseCamera.transform.position;
                controllerCamera.m_YAxis.Value = mouseCamera.m_YAxis.Value;
                controllerCamera.m_XAxis.Value = mouseCamera.m_XAxis.Value;
                SetControllerCamera();
            }
            else if (!inputManager.GetControllerConnected() || overrideController)
            {
                //mouseCamera.transform.position = controllerCamera.transform.position;
                mouseCamera.m_YAxis.Value = controllerCamera.m_YAxis.Value;
                mouseCamera.m_XAxis.Value = controllerCamera.m_XAxis.Value;
                SetMouseCamera();
            }

            overrideUpdated = overrideController;
            controllerUpdated = inputManager.GetControllerConnected();
            Debug.LogWarning("Controller changed");
        }
    }

    void SetControllerCamera()
    {
        // Enable/disable correct camera
        //controllerCamera.gameObject.SetActive(true);
        //mouseCamera.gameObject.SetActive(false);
        controllerCamera.Priority = 1;
        mouseCamera.Priority = 0;

        // Set player movement cameras
        playerMovement.freeLookCamera = controllerCamera;
        playerMovement.cameraTransform = controllerCamera.transform;

        // Set InputManager camera
        inputManager.cinemachine = controllerCamera;
    }

    void SetMouseCamera()
    {
        // Enable/disable correct camera
        //mouseCamera.gameObject.SetActive(true);
        //controllerCamera.gameObject.SetActive(false);
        mouseCamera.Priority = 1;
        controllerCamera.Priority = 0;

        // Set player movement cameras
        playerMovement.freeLookCamera = mouseCamera;
        playerMovement.cameraTransform = mouseCamera.transform;

        // Set InputManager camera
        inputManager.cinemachine = mouseCamera;
    }
}
