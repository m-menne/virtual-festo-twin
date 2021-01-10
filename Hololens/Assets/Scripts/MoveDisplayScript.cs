/* This class implements the logic of the display that shows up if
 * the model is moving. Upon interaction with this display the model
 * is placed at its current position.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisplayScript : MonoBehaviour {

    #region private variables
    private bool isShowing = false; // The status of the display.
    private GameObject moveButton; // Reference to the button that enables the relocation of the model.
    #endregion

    #region properties
    // Property for the displaying flag:
    public bool IsShowing
    {
        get
        {
            return isShowing;
        }

        set
        {
            isShowing = value;
        }
    }
    #endregion

    /* Start() is called, when the object is initialized.
     * It sets the reference to the button.
     */
    void Start () {
        moveButton = GameObject.Find("Menu/leftMenu/ApplicationMenu/MoveButton");
        // Print an error message if the button could not be found.
        if (moveButton == null)
            Debug.LogError("MoveDisplayScript could not find the MoveButton");
	}

    /* Called upon interaction.
     * Places the model.
     */
    public void Interact()
    {
        isShowing = false;
        moveButton.GetComponent<MoveModelScript>().PlaceModel();
    }

    /* Disables the display's canvas and box collider.
     */
    public void Deactivate ()
    {
        this.GetComponent<Canvas>().enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;
    }

    /* Enables the display's canvas and box collider.
     */
    public void Activate ()
    {
        this.GetComponent<Canvas>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;
    }
}