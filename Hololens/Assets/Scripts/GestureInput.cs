/* Script used to handle the gesture input of the HoloLens.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureInput : MonoBehaviour {
    #region private variables
    private GestureRecognizer gestures; // Reference to the gesture recognizer.
    private GameObject cam; // Reference to the camera object.
    #endregion

    /* Called upon initialization.
     * References the camera and starts the gesture capturing.
     */
    void Start ()
    {
        // Set the reference to the camera.
        cam = GameObject.Find("Main Camera");
        // Configure the GestureRecognizer.
        gestures = new GestureRecognizer();
        gestures.SetRecognizableGestures(GestureSettings.Tap);
        // Add the event callback function for the Air Tap.
        gestures.TappedEvent += Recognizer_TappedEvent;
		// Start the gesture capturing.
        gestures.StartCapturingGestures();
    }

    /* Update() is called once per frame.
     * It casts an interaction ray to highlight the object currently looked at.
     */
    void Update () {
        // Create a new RaycastHit.
        RaycastHit rayHit = new RaycastHit();
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, Mathf.Infinity))
        {
            // If possible, highlight the hit object.
            if (rayHit.transform.tag == "Highlightable")
                rayHit.transform.gameObject.SendMessage("Highlight");
        }
    }

    /* Callback event for the Air Tap.
     */
    void Recognizer_TappedEvent (UnityEngine.VR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay)
    {
        // Everytime the Air Tap is recognized, a virtual and infinite Ray is cast out from the camera's position
        // in the direction the camera is facing.
        headRay.origin = cam.transform.position;
        ShootInteractionRay(headRay);        
    }

    /* Raycast function for shooting the infinite ray.
     */
    void ShootInteractionRay (Ray ray)
    {
		// Create new RaycastHit.
        RaycastHit rayHit = new RaycastHit();
        if(Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            // Detect if the ray hits an interactable or highlightable object.
            // If that's the case, execute the function Interact() on this GameObject.
            if(rayHit.transform.tag == "Interactable" || rayHit.transform.tag == "Highlightable")
                rayHit.transform.gameObject.SendMessage("Interact");
        }
    }
}
