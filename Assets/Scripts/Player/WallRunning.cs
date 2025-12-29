using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce = 200f;
    public float wallClimbSpeed = 1.5f;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance = 0.7f;
    public float minJumpHeight = 2.0f;
    public float minWallNormalAngle = 70f; // Slope threshold to be considered a wall
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Forces (Jumping)")]
    public float walljumpSideForce = 600f;
    public float walljumpForwardForce = 600f;

    [Header("Camera")]
    public float maxWallRunCameraTilt = 15f;

    [HideInInspector] public float wallRunCameraTilt;
    [HideInInspector] public bool isWallRunning;
    [HideInInspector] public Vector3 wallNormal;
    [HideInInspector] public Vector3 lastWallNormal;

    private PlayerMovement pm;
    private Rigidbody rb;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        // Try to get ground layer from PM if not set
        if (whatIsGround == 0) whatIsGround = pm.whatIsGround;
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        CalculateCameraTilt();
    }

    private void FixedUpdate()
    {
        if (isWallRunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, pm.orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -pm.orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // State Logic
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !pm.grounded)
        {
            Vector3 currentWallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

            // Check wall angle
            float wallAngle = Vector3.Angle(currentWallNormal, Vector3.up);
            if (wallAngle >= minWallNormalAngle && wallAngle <= 180f - minWallNormalAngle)
            {
                if (!isWallRunning)
                {
                    // Only start if it's a new wall or we reset the last wall
                    if (currentWallNormal != lastWallNormal)
                    {
                        StartWallRun();
                        wallNormal = currentWallNormal;
                    }
                }
                else
                {
                    // If already wallrunning, just update the normal
                    wallNormal = currentWallNormal;
                }
            }
            else
            {
                if (isWallRunning) StopWallRun();
            }
        }
        else
        {
            if (isWallRunning) StopWallRun();
        }
    }

    private void StartWallRun()
    {
        isWallRunning = true;

        // Reset Y velocity to prevent immediate sliding/jumping momentum issues
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }

    private void StopWallRun()
    {
        isWallRunning = false;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((pm.orientation.forward - wallForward).magnitude > (pm.orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // Forward Force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // Vertical Movement
        if (upwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        if (downwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);

        // Push to wall force (stickiness)
        // Only push if not actively steering away
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void CalculateCameraTilt()
    {
        if (isWallRunning)
        {
            float tiltTarget = 0;
            if (wallRight) tiltTarget = maxWallRunCameraTilt;
            else if (wallLeft) tiltTarget = -maxWallRunCameraTilt;

            wallRunCameraTilt = Mathf.Lerp(wallRunCameraTilt, tiltTarget, Time.deltaTime * 5f);
        }
        else
        {
            wallRunCameraTilt = Mathf.Lerp(wallRunCameraTilt, 0, Time.deltaTime * 5f);
        }
    }
}
