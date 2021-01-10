/* This script assures that the model's menus are always faced towards the camera.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    #region private variables
    private Transform cam; // Reference to the main camera.
    #endregion

    /* Called upon initalization. Sets the reference to the camera.
     */
    void Start () {
        // Set the reference to the main camera:
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (cam == null)
            Debug.LogError("MenuScript could not find the MainCamera");
    }
	
	/* Update() is called once per frame.
     * It faces the menu towards the camera.
     */
	void Update () {
        // The menu looks at the camera.
        this.transform.LookAt(new Vector3(cam.position.x, this.transform.position.y, cam.position.z));
    }
}
