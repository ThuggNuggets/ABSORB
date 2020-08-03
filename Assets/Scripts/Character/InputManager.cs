using UnityEngine;
using XboxCtrlrInput;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    public XboxController controller;
    public CinemachineFreeLook cinemachine;

    private static bool _didQueryNumOfCtrlrs = false;
    private static bool isControllerConnected;

    private Vector2 _unityInputDirection = Vector2.zero;
    private Vector2 _xciInputDirection = Vector2.zero;
   

    // Start is called before the first frame update
    void Awake()
    {
        controller = XboxController.First;

        if (!_didQueryNumOfCtrlrs)
        {
            _didQueryNumOfCtrlrs = true;

            int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
            if (queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
            }
            else
            {
                Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
                isControllerConnected = true;
            }

            XCI.DEBUG_LogControllerNames();
        }
    }

    // Update is called once per frame
    void Update()
    {
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

}