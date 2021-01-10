/* Implements the logic of the arrows that point towards the position of the model.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperArrows : MonoBehaviour
{
    public GameObject Model; // Reference to the model.
    private MeshRenderer left; // Reference to the left arrow's mesh renderer.
    private MeshRenderer right; // Reference to the right arrow's mesh renderer.
    private float minDistance = 40.0f; // The distance to the model that is required to show the arrows.

    /* Start() is used for initialization.
     */
    void Start ()
    {
        // Set the references to the mesh renderers of the arrows.
        left = GameObject.Find("Arrow_Left").GetComponent<MeshRenderer>();
        right = GameObject.Find("Arrow_Right").GetComponent<MeshRenderer>();
    }

    /* Update() is called once per frame and displays the arrows if the requirements are met.
     */
    void Update()
    {
        Vector3 forward = this.transform.forward;
        Vector3 difference = Vector3.Normalize(Model.transform.position - this.transform.position);

        Vector3 forwardYOnly = Vector3.Normalize(new Vector3(forward.x, 0, forward.z));
        Vector3 differenceYOnly = Vector3.Normalize(new Vector3(difference.x, 0, difference.z));

        float angle = Vector3.SignedAngle(forwardYOnly, differenceYOnly, Vector3.up);

        // If the user turns more than 45 degrees to the left from the model, display the left arrow,
        // if the distance is long enough.
        if (angle < -45.0f)
        {
            if (Vector3.Distance(this.transform.position, Model.transform.position) > minDistance)
                left.enabled = true;
        }
        else
            left.enabled = false;
        // If the user turns more than 45 degrees to the right from the model, display the right arrow,
        // if the distance is long enough.
        if (angle > 45.0f)
        {
            if (Vector3.Distance(this.transform.position, Model.transform.position) > minDistance)
                right.enabled = true;
        }
        else
            right.enabled = false;
    }
}

