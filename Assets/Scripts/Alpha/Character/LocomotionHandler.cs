using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionHandler : MonoBehaviour
{
    /*
        This class will be used to control the "Loco" state machine within the 
        player controller.  
    */

    PlayerHandler _playerHandler;
    InputManager _inputManager;

    void Awake()
    {
        _playerHandler  = this.GetComponent<PlayerHandler>();
        _inputManager   = _playerHandler.GetInputManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateDash()
    {

    }
}
