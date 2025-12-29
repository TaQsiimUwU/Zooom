using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [Header("Fall Detection")]
    [Tooltip("Y position below which the player is considered to have fallen")]
    public float fallThreshold = -10f;

    [Header("Debug")]
    public bool showDebugMessages = true;

    private bool hasFallen = false;

    private void Update()
    {
        // Check if player fell below threshold
        if (!hasFallen && transform.position.y < fallThreshold)
        {
            if (showDebugMessages)
            {
                Debug.Log($"Player fell below Y={fallThreshold}. Triggering death screen.");
            }

            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        if (hasFallen) return;

        hasFallen = true;

        // Disable player controls
        PlayerMovement pm = GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.enabled = false;
        }

        // Disable wall running
        WallRunning wr = GetComponent<WallRunning>();
        if (wr != null)
        {
            wr.enabled = false;
        }

        // Stop player physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
        }

        // Trigger lose screen
        if (LoseScreen.Instance != null)
        {
            LoseScreen.Instance.TriggerLoseScreen();
        }
        else
        {
            Debug.LogError("LoseScreen instance not found! Make sure you have a LoseScreen in your scene.");
        }
    }
}
