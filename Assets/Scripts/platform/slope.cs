using UnityEngine;

public class Slope : MonoBehaviour
{
    [Header("Friction Settings")]
    [Range(0f, 1f)]
    public float dynamicFriction = 0f;
    [Range(0f, 1f)]
    public float staticFriction = 0f;

    [Tooltip("How the friction of two colliding objects is combined.")]
    public PhysicsMaterialCombine frictionCombine = PhysicsMaterialCombine.Minimum;    [Header("Force Settings")]
    [Tooltip("Force applied to objects to pull them down the slope.")]
    public float downwardForce = 50f;

    [Tooltip("Additional gravity multiplier for slopes")]
    public float gravityMultiplier = 2.5f;

    private void Awake()
    {
        Collider slopeCollider = GetComponent<Collider>();

        if (slopeCollider != null)
        {
            // Create a new PhysicsMaterial in memory
            PhysicsMaterial slipperyMaterial = new PhysicsMaterial("SlipperySlope");

            // Set low friction values
            slipperyMaterial.dynamicFriction = dynamicFriction;
            slipperyMaterial.staticFriction = staticFriction;

            // Ensure the lowest friction value is used when colliding with the player
            slipperyMaterial.frictionCombine = frictionCombine;

            // Apply to the collider
            slopeCollider.material = slipperyMaterial;
        }
        else
        {
            Debug.LogWarning($"Slope script on {gameObject.name} requires a Collider to work!");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            // Use the first contact point to determine the slope normal
            if (collision.contactCount > 0)
            {
                Vector3 normal = collision.contacts[0].normal;

                // Calculate the direction strictly down the slope
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, normal).normalized;

                // Calculate slope angle
                float slopeAngle = Vector3.Angle(Vector3.up, normal);

                // Scale force based on slope angle (steeper = faster)
                float angleMultiplier = slopeAngle / 45f; // 45 degrees = full force

                // Apply force down the slope
                rb.AddForce(slopeDirection * downwardForce * angleMultiplier, ForceMode.Acceleration);

                // Add extra gravity pull
                // rb.AddForce(Vector3.down * Physics.gravity.y * gravityMultiplier * angleMultiplier, ForceMode.Acceleration);
            }
        }
    }
}
