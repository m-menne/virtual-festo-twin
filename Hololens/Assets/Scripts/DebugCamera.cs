/* This script implements functions to simulate the camera of the HoloLens.
 * This includes the movement of the camera and the simulation of the Air tap via mouseclick.
 * It is used to test the application without the HoloLens.
 * It has to be disabled for the application to run correctly on the HoloLens!
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour 
{ 
	#region private variables
    private Camera cam; // Reference to the camera.

    // Necessary variables to simulate the Hololens' movements and interactions:
    private float movementSpeed;
    private float acceleration;
    private bool moving;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    #endregion

    /* Start() is called when the script is initialized.
     * It is used to initialize the required variables.
     */
    private void Start ()
    {
        // Set the reference to the camera.
        cam = this.GetComponent<Camera>();
        if (cam == null)
            Debug.LogError("Camera not found!");
        // Set the cursor's variables.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	/* Update() is called once every frame. It simulates the HoloLens' functions.
     */
	private void Update ()
    {  
        // The left mouse button simulates the HoloLens' Air tap.
        if (Input.GetMouseButtonDown(0))
        {
            // Create a new RaycastHit.
            RaycastHit rayHit = new RaycastHit();
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, Mathf.Infinity))
            {
                // If the ray hits an interactable or hightlightable object, call its Interact()-method.
                if (rayHit.transform.tag == "Interactable" || rayHit.transform.tag == "Highlightable")
                    rayHit.transform.gameObject.SendMessage("Interact");
            }
        }

        // Simulate the Hololens' movement with the 'WASD'-Keys.
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.right * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * movementSpeed);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        // If the camera moves, set its acceleration and speed.
        if (moving)
        {
            acceleration += Time.deltaTime * 2;
            acceleration = Mathf.Clamp(acceleration, 0.0f, 1.0f);
            movementSpeed = Mathf.Lerp(0.0f, 1.0f, acceleration);
        }
        // If the camera does not move, reset its acceleration and speed.
        else
        {
            acceleration -= Time.deltaTime * 2;
            acceleration = Mathf.Clamp(acceleration, 0.0f, 1.0f);
            movementSpeed = Mathf.Lerp(0.0f, 1.0f, acceleration);
        }

        // Simulate the Hololens's Head rotation.
        mouseX = Mathf.Lerp(mouseX, Input.GetAxis("Mouse X"), Time.deltaTime * 20);
        mouseY = Mathf.Lerp(mouseY, Input.GetAxis("Mouse Y"), Time.deltaTime * 20);
        rotationX += mouseX * 2;
        rotationY += mouseY * 2;

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        this.transform.localRotation = Quaternion.identity * xQuaternion * yQuaternion;
    }

}
