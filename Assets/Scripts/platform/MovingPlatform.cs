using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Setup")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 3.0f;

    [Header("Internal Logic")]
    private Vector3 targetPos;
    private Rigidbody playerRb; // We will store the player here when they touch us

    void Start()
    {
        targetPos = pointB.position;
    }

    void FixedUpdate()
    {
        // 1. Calculate the movement for this specific physics frame
        Vector3 moveStep = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);

        // 2. Figure out the "Delta" (The difference between where we are and where we are going)
        Vector3 platformMovement = moveStep - transform.position;

        // 3. Move the Platform
        transform.position = moveStep;

        // 4. CRITICAL: If the player is on us, Move them the exact same amount
        if (playerRb != null)
        {
            // MovePosition is safer than AddForce for platforms because it doesn't build up infinite speed
            playerRb.MovePosition(playerRb.position + platformMovement);
        }

        // 5. Check if we reached the target and switch
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            targetPos = (targetPos == pointB.position) ? pointA.position : pointB.position;
        }
    }

    // --- DETECTING THE PLAYER ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = null;
        }
    }
}
