using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionHandler : MonoBehaviour
{
    /*
        This class will be used to control the "Locomotion" state machine within the 
        player controller.  
    */

    // References
    [Header("References")]
    public Transform mainPlayerCamera;
    private PlayerHandler _playerHandler;
    private InputManager _inputManager;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;

    [Header("Movement Attributes")]
    public float acceleration = 100.0f;
    public float maxVelocity = 10.0f;
    public float turnSpeed = 0.20f;
    private float _currentAcceleration = 0.0f;

    // Awake is called on initialise
    private void Awake()
    {
        // Setting the current acceleration
        _currentAcceleration = acceleration;
        _tempPlayerAcceleration = acceleration;
    }

    // Start is called before first frame
    private void Start()
    {
        // Getting the required references
        _playerHandler = this.GetComponent<PlayerHandler>();
        _rigidbody = _playerHandler.GetRigidbody();
        _transform = _playerHandler.GetTransform();
        _animator = _playerHandler.GetAnimator();
        _inputManager = _playerHandler.GetInputManager();
    }

    // Update is called once per frame
    private void Update()
    {
        // Updates the players slow down FSM
        UpdateSlowdownFSM();

        // Checks if the player is moving, and sets the animator accordingly
        if (_rigidbody.velocity.magnitude > 0.1F)
            _animator.SetBool("Movement", true);
        else if (_animator.GetBool("Movement"))
            _animator.SetBool("Movement", false);
    }

    // Fixed update is called every physics
    private void FixedUpdate()
    {
        // Updating the general movement
        FixedUpdateGeneralMovement();

        // Updating Dash
        UpdateDash();
    }

    #region General Movement

    private void FixedUpdateGeneralMovement()
    {
        // Updates the input direction from the scenes input manager
        Vector3 calculatedDirection = GetInputDirection().normalized;

        // Move player via forces
        if (_rigidbody.velocity.magnitude < maxVelocity)
            _rigidbody.AddForce(calculatedDirection * _currentAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);

        // Rotate player to face direction
        if (calculatedDirection.magnitude > 0.1F)
            _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(calculatedDirection), turnSpeed);
    }

    // Returns the forward direction of the camera.
    private Vector3 GetCalculatedForward()
    {
        // Get the camera forward
        Vector3 camPos = mainPlayerCamera.position;
        camPos.y = _transform.position.y;
        Vector3 cameraForward = (_transform.position - camPos).normalized;

        // Get the ground forward
        RaycastHit hit;
        if (Physics.SphereCast(_transform.position + Vector3.up, 0.2f, Vector3.down, out hit, 1.0f))
        {
            // Return calculated forward
            Vector3 groundForward = Quaternion.AngleAxis(90, mainPlayerCamera.right) * hit.normal;
            return cameraForward + groundForward;
        }

        // Return cameras forward
        return cameraForward;
    }

    private Vector3 GetInputDirection()
    {
        // Calculate the forward direction with a bunch of shitty vector math
        Vector3 inputDirection = _inputManager.GetMovementDirectionFromInput();
        Vector3 forward = GetCalculatedForward();
        Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;
        return (forward * inputDirection.y) + (right * inputDirection.x);
    }

    #endregion

    #region Slowdown   

    public enum SlowState
    {
        Default,    // Default to chill on until another state is called
        Slowdown,   // Slow down the player
        SpeedUp     // Speed up the player
    }
    [HideInInspector]
    public SlowState slowState;

    [Header("Slowdown Properties")]
    public float maxSlowAcceleration = 10.0f;  // Lowest speed the player will be slowed down to
    public float timeToAcceleration = 0.2f;    // Approx time for the player acceleration to reach the slow/default amount

    private float _speedSmoothVelocity;
    private float _tempPlayerAcceleration;
    private bool _resetPlayerAcceleration = false;


    // Updates the slowdown
    public void UpdateSlowdownFSM()
    {
        switch (slowState)
        {
            case SlowState.Default:
                ResetPlayer();
                break;
            case SlowState.Slowdown:
                SlowdownPlayer();
                break;
            case SlowState.SpeedUp:
                SpeedUpPlayer();
                break;
        }
    }

    private void SlowdownPlayer()
    {
        _currentAcceleration = Mathf.SmoothDamp(_currentAcceleration, maxSlowAcceleration, ref _speedSmoothVelocity, timeToAcceleration);
    }

    private void SpeedUpPlayer()
    {
        _currentAcceleration = Mathf.SmoothDamp(_currentAcceleration, _tempPlayerAcceleration, ref _speedSmoothVelocity, timeToAcceleration);
        //if (!_resetPlayerAcceleration)

        // Failsafe to set back to default state so the slowdown state can be called again
        // It just makes sense that it goes back to normal once you reach normal speed again
        if (_currentAcceleration >= (_tempPlayerAcceleration * 0.95)) // When it's reached above 95% of the normal speed
        {
            _resetPlayerAcceleration = true;
            slowState = SlowState.Default;
        }
    }

    private void ResetPlayer()
    {
        if (_resetPlayerAcceleration)
        {
            _currentAcceleration = _tempPlayerAcceleration;
            _resetPlayerAcceleration = false;
        }
    }

    // Slows the player down to shield and absorb speed
    public void Key_ActivateSlowdown()
    {
        slowState = SlowState.Slowdown;
    }

    // Speeds the player back up to normal acceleration
    public void Key_DeactivateSlowdown()
    {
        slowState = SlowState.SpeedUp;
    }

    #endregion

    #region Dash

    [Header("Dash Attributes")]
    public float force = 50.0f;
    public float cooldownTime = 5.0f;
    public float distance = 20.0f;
    public float smoothTime = 0.5f;
    public bool lerp = false;
    [Header("Debug Optionals")]
    public bool disableVelocityReset = false;
    public bool smoothDamp = false;
    private bool _canDash = true;
    private Vector3 _initialVelocity = Vector3.zero;
    private Vector3 _initialPosition = Vector3.zero;
    private bool _haveReset = false;

    private void UpdateDash()
    {
        // if (_inputManager.GetDashButtonPress() && _canDash && _playerHandler.GetCombatHandler().shieldState != CombatHandler.ShieldState.Shielding)
        // {
        //     _initialVelocity = _rigidbody.velocity;
        //     _initialPosition = transform.position;
        //     _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        //     _animator.SetBool("Dash", true);
        //     _canDash = false;
        //     StartCoroutine(CoolDownSequence());
        // }

        // if (!_canDash)
        // {
        //     if (Vector3.Distance(transform.position, _initialPosition) > distance && !_haveReset)
        //     {
        //         _rigidbody.velocity = _initialVelocity;
        //         _initialVelocity = Vector3.zero;
        //         _initialPosition = Vector3.zero;
        //         _animator.SetBool("Dash", false);
        //         _haveReset = true;
        //     }
        // }

        if (_inputManager.GetDashButtonPress() && _canDash && _playerHandler.GetCombatHandler().shieldState != CombatHandler.ShieldState.Shielding)
        {
            _initialVelocity = _rigidbody.velocity;
            _initialPosition = transform.position;
            _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
            _animator.SetBool("Dash", true);
            _canDash = false;
            StartCoroutine(CoolDownSequence());
        }

        if (!_canDash)
        {
            if (Vector3.Distance(transform.position, _initialPosition) > distance && !_haveReset)
            {
                if (!disableVelocityReset)
                {
                    //_rigidbody.velocity = _initialVelocity;
                    if (smoothDamp)
                        _rigidbody.velocity = new Vector3(Mathf.SmoothDamp(_rigidbody.velocity.x, _initialVelocity.x, ref _speedSmoothVelocity, smoothTime), 0,
                        Mathf.SmoothDamp(_rigidbody.velocity.z, _initialVelocity.z, ref _speedSmoothVelocity, smoothTime));
                    else if (lerp)
                        _rigidbody.velocity = new Vector3(Mathf.LerpUnclamped(_rigidbody.velocity.x, _initialVelocity.x, smoothTime), 0,
                        Mathf.LerpUnclamped(_rigidbody.velocity.z, _initialVelocity.z, smoothTime));
                    else
                        _rigidbody.velocity = _initialVelocity;
                }
                _initialVelocity = Vector3.zero;
                _initialPosition = Vector3.zero;
                _animator.SetBool("Dash", false);
                _haveReset = true;
            }
        }
    }

    private IEnumerator CoolDownSequence()
    {
        yield return new WaitForSecondsRealtime(cooldownTime);
        _canDash = true;
        _haveReset = false;
    }

    #endregion
}
