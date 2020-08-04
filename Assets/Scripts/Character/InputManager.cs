﻿using UnityEngine;
using XboxCtrlrInput;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    /*
      private InputManager _inputManager;
      _inputManager = FindObjectOfType<InputManager>();
    */

    public XboxController controller;
    public CinemachineFreeLook cinemachine;

    [Header("Attack Button")]
    public XboxButton attackXboxKey;
    public KeyCode attackKey;

    [Header("Special Attack Button")]
    public XboxButton splAttackXboxKey;
    public KeyCode splAttackKey;

    [Header("Shield Button")]
    public XboxButton shieldXboxKey;
    public KeyCode shieldKey;

    [Header("Dash Button")]
    public XboxButton dashXboxKey;
    public KeyCode dashKey;

    [Header("Pause Button")]
    public XboxButton pauseXboxKey;
    public KeyCode pauseKey;

    private static bool _didQueryNumOfCtrlrs = false;
    private static bool isControllerConnected;

    private Vector2 _unityInputDirection = Vector2.zero;
    private Vector2 _xciInputDirection = Vector2.zero;
    private int _queriedNumberOfCtrlrs;


    // Start is called before the first frame update
    void Awake()
    {
        controller = XboxController.First;

        // Check if there is a xbox controller connected on awake
        if (!_didQueryNumOfCtrlrs)
        {
            _didQueryNumOfCtrlrs = true;

            _queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
            if (_queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
                isControllerConnected = false;
            }
            else
            {
                Debug.Log(_queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
                isControllerConnected = true;
            }

            XCI.DEBUG_LogControllerNames();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update isControllerConnected to allow the use of keyboard buttons if there is no controller connected
        if (XCI.IsPluggedIn(_queriedNumberOfCtrlrs))
            isControllerConnected = true;
        else
            isControllerConnected = false;
    }

    public Vector2 GetMovementDirectionFromInput()
    {
        if (isControllerConnected)
            return UpdateXCIInputDirection();
        else
            return UpdateUnityInputDirection();
    }

    public Vector2 UpdateUnityInputDirection()
    {
        _unityInputDirection.x = Input.GetAxisRaw("Horizontal");
        _unityInputDirection.y = Input.GetAxisRaw("Vertical");
        return _unityInputDirection;
    }

    public Vector2 UpdateXCIInputDirection()
    {
        _xciInputDirection.x = XCI.GetAxisRaw(XboxAxis.LeftStickX);
        _xciInputDirection.y = XCI.GetAxisRaw(XboxAxis.LeftStickY);
        return _xciInputDirection;
    }

    // Check for Attack button press
    public bool GetAttackButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(attackXboxKey, XboxController.First) : Input.GetKeyDown(attackKey);
    }
    
    // Check for Special Attack button press
    public bool GetSpecialAttackButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(splAttackXboxKey, XboxController.First) : Input.GetKeyDown(splAttackKey);
    }

    // Check for Shield button press
    public bool GetShieldButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(shieldXboxKey, XboxController.First) : Input.GetKeyDown(shieldKey);
    }

    // Check for Dash button press
    public bool GetDashButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(dashXboxKey, XboxController.First) : Input.GetKeyDown(dashKey);
    }

    // Check for Pause button press
    public bool GetPauseButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(pauseXboxKey, XboxController.First) : Input.GetKeyDown(pauseKey);
    }
}