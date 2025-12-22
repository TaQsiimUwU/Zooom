using UnityEngine;

public class PlayerCam : MonoBehaviour
{

   public float sensX;
   public float sensY;
   public Transform orientation;
   float xRotation;
   float yRotation;
   private void Start()
   {
       // Lock cursor to center of screen
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;
   }

   public void Update()
   {
       // Get mouse input
       float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
       float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

       // Calculate rotation
       yRotation += mouseX;
       xRotation -= mouseY;
       xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

       // Apply rotation to camera and orientation
       transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
       orientation.rotation = Quaternion.Euler(0, yRotation, 0);
   }
}
