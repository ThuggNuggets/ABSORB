using UnityEngine;
using XboxCtrlrInput;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    public XboxController controller;
    public CinemachineFreeLook cinemachine;

    [Header("Shield Button")]
    public XboxButton shieldXboxKey;
    public KeyCode shieldKey;

    [Header("Dash Button")]
    public XboxButton dashXboxKey;
    public KeyCode dashKey;

    private static bool _didQueryNumOfCtrlrs = false;
    private static bool isControllerConnected;

    private Vector2 _unityInputDirection = Vector2.zero;
    private Vector2 _xciInputDirection = Vector2.zero;
    private int _queriedNumberOfCtrlrs;


    // Start is called before the first frame update
    void Awake()
    {
        controller = XboxController.First;

        if (!_didQueryNumOfCtrlrs)
        {
            _didQueryNumOfCtrlrs = true;

            _queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
            if (_queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
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
        Debug.Log("Xbox Controller plugged in = " + XCI.IsPluggedIn(_queriedNumberOfCtrlrs));
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

    public bool GetShieldButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(shieldXboxKey, XboxController.First) : Input.GetKeyDown(shieldKey);
    }

    public bool GetDashButtonPress()
    {
        return (isControllerConnected) ? XCI.GetButtonDown(dashXboxKey, XboxController.First) : Input.GetKeyDown(dashKey);
    }

}