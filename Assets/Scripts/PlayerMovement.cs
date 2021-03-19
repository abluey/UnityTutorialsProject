using UnityEngine;

public enum PlayerMovementState
{
    NORMAL,
    SPRINT,
    CROUCH
}

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody;

    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpImpulse;
    private bool shouldJump = false;

    [SerializeField] private KeyCode crouchKey;
    [SerializeField] private float crouchingSpeed;
    private bool shouldCrouch = false;

    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private float sprintingSpeed;
    private bool shouldSprint = false;

    [SerializeField] private Grounder playerGrounder;
    private bool isGrounded = false;

    [SerializeField] private float baseSpeed;
    private float xInput = 0, zInput = 0;

    //magic methods
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(jumpKey))
        {
            shouldJump = true;
        }
        else if (Input.GetKeyUp(jumpKey))
        {
            shouldJump = false;
        }

        if (Input.GetKeyDown(crouchKey))
        {
            shouldCrouch = true;
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            shouldCrouch = false;
        }

        if (Input.GetKeyDown(sprintKey))
        {
            shouldSprint = true;
        }
        else if (Input.GetKeyUp(sprintKey))
        {
            shouldSprint = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 relativePositionDelta = (xInput * transform.right) + (zInput * transform.forward);

        switch (State)
        {
            case PlayerMovementState.CROUCH:
                relativePositionDelta *= crouchingSpeed;
                break;

            case PlayerMovementState.SPRINT:
                relativePositionDelta *= sprintingSpeed;
                break;

            default:
                relativePositionDelta *= baseSpeed;
                break;
        }

        relativePositionDelta *= Time.fixedDeltaTime;
        playerRigidbody.MovePosition(transform.position + relativePositionDelta);

        if (shouldJump && isGrounded)
        {
            playerRigidbody.AddForce(new Vector3(0, jumpImpulse, 0), ForceMode.Impulse);
            shouldJump = false;
        }
    }

    //ground colliders, jumping
    private void OnTouchedGround(Collider other)
    {
        isGrounded = true;
    }

    private void OnLeftGround(Collider other)
    {
        isGrounded = false;
    }

    private void OnEnable()
    {
        playerGrounder.TouchedGround += OnTouchedGround;
        playerGrounder.LeftGround += OnLeftGround;
    }

    private void OnDisable()
    {
        playerGrounder.TouchedGround -= OnTouchedGround;
        playerGrounder.LeftGround -= OnLeftGround;
    }

    private void OnDestroy()
    {
        playerGrounder.TouchedGround -= OnTouchedGround;
        playerGrounder.LeftGround -= OnLeftGround;
    }

    public PlayerMovementState State
    {
        get
        {
            if (shouldSprint)
                return PlayerMovementState.SPRINT;
            if (shouldCrouch)
                return PlayerMovementState.CROUCH;
            return PlayerMovementState.NORMAL;
        }
    }
}
