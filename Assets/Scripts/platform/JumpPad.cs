using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Settings")]
    public float launchForce = 15f; // How hard to throw the player
    public bool resetVelocity = true; // "True" gives consistent height every time

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object hitting the pad is the Player
        if (other.CompareTag("Player"))
        {
            // 2. Get the Player's Rigidbody (Physics Brain)
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // 3. (Optional) Reset current vertical speed so the jump is always the same height
                // If we don't do this, gravity might cancel out the jump force.
                if (resetVelocity)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                }

                // 4. Add Force in the direction the PAD is facing (Green Arrow)
                // ForceMode.Impulse is for instant explosions/jumps
                rb.AddForce(transform.up * launchForce, ForceMode.Impulse);
            }
        }
    }
}
