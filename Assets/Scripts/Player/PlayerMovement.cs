using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Assingables
    [Header("References")]
    public Transform playerCam;
    public Transform orientation;

    // Other
    private Rigidbody rb;

    // Rotation and look
    [Header("Look Settings")]
    private float xRotation;
    [SerializeField] private float sensitivity = 50f;
    [SerializeField] private float sensMultiplier = 1f;

    // Movement
    [Header("Movement")]
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    private WallRunning wallRunning;

    // Crouch & Slide
    [Header("Crouching & Sliding")]
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    // Jumping
    [Header("Jumping")]
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    // Input
    [Header("Input")]
    public float x, y;
    public bool jumping, sprinting, crouching;

    private Vector3 normalVector = Vector3.up;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        wallRunning = GetComponent<WallRunning>();
    }

    void Start() {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Update() {
        MyInput();
        Look();
    }

    private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }    private void Movement() {
        if (wallRunning != null && wallRunning.isWallRunning) {
            // Wall running logic is now handled in WallRunning.cs
        } else {
            rb.useGravity = true;
            rb.AddForce(Vector3.down * Time.deltaTime * 10f);
        }

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        if (readyToJump && jumping) Jump();

        float maxS = maxSpeed;

        if (crouching && grounded && readyToJump) {
            // When crouching on ground, add extra downward force and increase max speed
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);

            // Don't clamp velocity as strictly when sliding - allow acceleration
            if (rb.linearVelocity.magnitude > maxSpeed) {
                maxS = maxSpeed * 2.5f; // Allow sliding to go faster
            }
        }

        if (x > 0 && xMag > maxS) x = 0;
        if (x < 0 && xMag < -maxS) x = 0;
        if (y > 0 && yMag > maxS) y = 0;
        if (y < 0 && yMag < -maxS) y = 0;

        float multiplier = 1f, multiplierV = 1f;

        if (!grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (grounded && crouching) multiplierV = 0f;

        if (wallRunning == null || !wallRunning.isWallRunning) {
            rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
        }
    }

    private void Jump() {
        if (grounded && readyToJump) {
            readyToJump = false;

            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            Vector3 vel = rb.linearVelocity;
            if (rb.linearVelocity.y < 0.5f)
                rb.linearVelocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.linearVelocity.y > 0)
                rb.linearVelocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (wallRunning != null && wallRunning.isWallRunning && readyToJump) {
            readyToJump = false;

            // Kill wallrun state immediately so physics doesn't fight the jump
            wallRunning.isWallRunning = false;
            wallRunning.lastWallNormal = wallRunning.wallNormal;

            // Clear current downward glide velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            // 1. Vertical: 1.5x the normal grounded jump
            rb.AddForce(Vector3.up * (jumpForce * 2.25f));

            // 2. Side: Kick away from the wall
            rb.AddForce(wallRunning.wallNormal * wallRunning.walljumpSideForce);

            // 3. Forward: Speed burst in the look direction
            rb.AddForce(orientation.forward * wallRunning.walljumpForwardForce);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump() {
        readyToJump = true;
    }

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        float desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        float tilt = 0;
        if (wallRunning != null) tilt = wallRunning.wallRunCameraTilt;

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping || (wallRunning != null && wallRunning.isWallRunning)) return;

        if (crouching) {
            // When crouching/sliding, use much less counter movement to allow acceleration
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.linearVelocity.normalized * slideCounterMovement * 0.3f);
            return;
        }

        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(rb.linearVelocity.x, 2) + Mathf.Pow(rb.linearVelocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.linearVelocity.y;
            Vector3 n = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.linearVelocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    private void OnCollisionStay(Collision other) {
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
                if (wallRunning != null) wallRunning.lastWallNormal = Vector3.zero;
            }
        }

        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }


}
