/* This class implements the movement of the model.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveModelScript : MonoBehaviour
{
    #region private variables
    // References to the required objects:
    private GameObject festoModel;
    private GameObject completeModel;
    private GameObject cursor;
    private GameObject movePosition;
    private GameObject menu;
    private MoveDisplayScript moveDisplay;
  
    private bool isMoving; // The current status of the model.
    private GameObject cam; // Reference to the main camera.
    private GameObject[] infoBubbles; // A list of all info bubbles in the model.
    #endregion

    #region properties
    // Property for the status:
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        set
        {
            isMoving = value;
        }
    }
    #endregion

    /* Start() is called upon initialization.
     * It sets all the required references.
     */
    void Start()
    {
        // Set the required references:
        festoModel = GameObject.Find("FestoModel");
        completeModel = GameObject.Find("CompleteModel");
        cursor = GameObject.Find("Circle");
        movePosition = GameObject.Find("MovePosition");
        menu = GameObject.Find("Menu");
        moveDisplay = GameObject.Find("MoveDisplay").GetComponent<MoveDisplayScript>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        infoBubbles = GameObject.FindGameObjectsWithTag("Infobubble");
        // The Model is currently not moving.
        isMoving = false;
        // The Move Display is deactivated at the beginning.
        moveDisplay.Deactivate();
        
        // If a reference could not be set, show an error message.
        if (festoModel == null)
            Debug.LogError("MoveModelScript could not find the FestoModel");
        if (completeModel == null)
            Debug.LogError("MoveModelScript could not find the CompleteModel");
        if (cursor == null)
            Debug.LogError("MoveModelScript could not find the Cursor");
        if (movePosition == null)
            Debug.LogError("MoveModelScript could not find the MovePosition");
        if (menu == null)
            Debug.LogError("MoveModelScript could not find the Menu");
        if (moveDisplay == null)
            Debug.LogError("MoveModelScript could not find the MoveDisplay");
        if (cam == null)
            Debug.LogError("MoveModelScript could not find the MainCamera");
        if (infoBubbles == null)
            Debug.LogError("MoveModelScript could not find the InfoBubbles");
    }

    /* Upon interaction the model starts moving.
     */
    public void Interact()
    {
        if (!isMoving)
            MoveModel();
    }

    /* Binds the model to the MovePosition.
     * All infobubbles are closed and the menus hidden if the model starts to move.
     * Shows the MoveDisplay which is used for placing the model.
     */
    public void MoveModel()
    {
        // Bind the model to the MovePosition.
        completeModel.transform.rotation = movePosition.transform.rotation;
        completeModel.transform.parent = movePosition.transform;
        completeModel.transform.localPosition = new Vector3(0.03f, -0.3f, 1.5f);
        // Do not show the menu.
        menu.GetComponent<Canvas>().enabled = false;

        // Disable all infobubbles:
        foreach(GameObject infoBubble in infoBubbles)
            infoBubble.GetComponent<InfoBubble>().HideInfo();

        isMoving = true;
        // Activate the move display and show it.
        moveDisplay.Activate();
        moveDisplay.IsShowing = true;
    }

    /* Binds the model to its original place in the hierarchy.
     */
    public void PlaceModel()
    {
        // Bind the model to its original place.
        completeModel.transform.parent = null;
        completeModel.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        // Show the menus again.
        menu.GetComponent<Canvas>().enabled = true;

        isMoving = false;
        // Deactivate the Move Display.
        moveDisplay.Deactivate();
    }
}