using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public enum PlayerAnimatorState
    {
        IDLE,
        ATTACK,
        SHIELD,
        DASH,
        ABSORB,
        DEATH,
    }
    private PlayerAnimatorState _currentState;

    // Attributes
    [Header("Attributes")]
    public int maxHealth = 10;
    public float respawnFlyingTime = 5.0f;
    public float respawnFlyingHeight = 20.0f;
    private int currentHealth = 10;
    public float offset = 10.0f;

    // References
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private LocomotionHandler _locomotionHandler;
    private CombatHandler _combatHandler;
    private InputManager _inputManager;
    private CheckPoint _checkpoints;
    private Vector3 _respawnPosition;
    private CapsuleCollider _capsule;

    Vector3 point = Vector3.zero;
    Vector3 point2 = Vector3.zero;

    // Flags
    private bool isAlive = true;

    void Awake()
    {
        // Temporarily setting cursor locked state within this script.
        Cursor.lockState = CursorLockMode.Locked;

        // Getting the required components
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        _transform = this.GetComponent<Transform>();
        _locomotionHandler = this.GetComponent<LocomotionHandler>();
        _combatHandler = this.GetComponent<CombatHandler>();
        _inputManager = FindObjectOfType<InputManager>();
        _checkpoints = FindObjectOfType<CheckPoint>();
        _capsule = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        // Set the player position as a respawn point
        SetRespawnPosition(this._transform.position); // Might have to rewrite to be Vector3 instead of transform
    }

    #region Player Respawn

    public void RespawnPlayer()
    {
        // if (!isAlive)
        // {
        isAlive = true;
        currentHealth = maxHealth;
        StartCoroutine(MoveOverSeconds(this.gameObject, GetRespawnPosition(), respawnFlyingHeight, respawnFlyingTime));
        //_transform.rotation = GetRespawnPosition().rotation;
        //}
    }

    // Move the player back to checkpoint position over a certain number of seconds
    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float height, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        _capsule.enabled = false;
        _locomotionHandler.enabled = false;
        _rigidbody.useGravity = false;

        while (elapsedTime < seconds)
        {
            //objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            objectToMove.transform.position = MathParabola.Parabola(startingPos, end, 20.0f, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        _locomotionHandler.enabled = true;
        _capsule.enabled = true;
        _rigidbody.useGravity = true;
    }

    private Vector3 GetPoint(Vector3 object1, Vector3 object2)
    {
        //get the positions of our transforms
        Vector3 pos1 = object1;
        Vector3 pos2 = object2;

        //get the direction between the two transforms -->
        Vector3 dir = (pos2 - pos1).normalized;

        //get a direction that crosses our [dir] direction
        //NOTE! : this can be any of a buhgillion directions that cross our [dir] in 3D space
        //To alter which direction we're crossing in, assign another directional value to the 2nd parameter
        Vector3 perpDir = Vector3.Cross(dir, Vector3.right);

        //get our midway point
        Vector3 midPoint = (pos1 + pos2) / 2f;

        //get the offset point
        //This is the point you're looking for.
        Vector3 offsetPoint = midPoint + (perpDir * offset);

        return offsetPoint;
    }

    // void OnDrawGizmos()
    // {
    //     Vector3 point = GetPoint(this.transform.position, GetRespawnPosition());
    //     Vector3 point2 = GetPoint(point, GetRespawnPosition());

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawCube(point, new Vector3(1.0f, 1.0f));
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawCube(point2, new Vector3(1.0f, 1.0f));
    // }

    #endregion

    #region Getters and setters

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public bool SetIsAlive(bool aliveState)
    {
        return isAlive = aliveState;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetState(PlayerAnimatorState state)
    {
        _currentState = state;
    }

    public PlayerAnimatorState GetState()
    {
        return _currentState;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public InputManager GetInputManager()
    {
        return _inputManager;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public LocomotionHandler GetLocomotionHandler()
    {
        return _locomotionHandler;
    }

    public CombatHandler GetCombatHandler()
    {
        return _combatHandler;
    }

    public CheckPoint GetCheckPoint()
    {
        return _checkpoints;
    }

    public Vector3 GetRespawnPosition()
    {
        return _respawnPosition;
    }

    public Vector3 SetRespawnPosition(Vector3 checkpointPosition)
    {
        Debug.Log("Set new respawn position at: (" + checkpointPosition.x + ", " +
        checkpointPosition.y + ", " + checkpointPosition.z + ")");
        return _respawnPosition = checkpointPosition;
    }

    #endregion
}