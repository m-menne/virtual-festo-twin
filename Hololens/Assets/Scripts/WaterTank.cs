/* This class implements the logic for the model's water tanks.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank : MonoBehaviour {
    #region private variables
    // Parameters of the tank:
    private float waterLevel = 0.5f; // Current waterlevel (by default both tanks are half full)
    private float waterSpeed = 0.1f; // The speed at which the waterlevel changes.

    private GameObject water; // Reference to the water object which is stored in the tank.

    // Variables used for the logic of the info bubble: 
    private InfoBubble infobubble; // Reference to the InfoBubble which shows the information of this component. 
    new private string name; // The name of this GameObject. This is displayed in the infobubble. 
    private string info; // The information text which is shown in the tank's infobubble. 
    private bool infoVisible = false; // Flag to indicate whether the infoBubble of this tank is visible or not.

    // Variables used for the implementation of the highlighting:
    private Material standardMaterial; // The standard material of the water tank.	
	private Material outlineMaterial; // The outline material of the water tank.                               
    private float timerHighlight = 0.0f; // The timer to keep track of the time that has passed after the last call of Highlight().                                     
    private bool highlighted = false; // Flag to indicate whether the tank is highlighted.                                  
    private float timeUntilEndHighlight = 0.2f; // The time in seconds for which the tank stays highlighted
                                                // after you stop looking at it.

    // Variables used for the display of the errors:
    private bool errorShown = false; // Flag to indicate whether the tank shows an error at the moment.
    private Material errorMaterial; // The error material of the water tank.
    private string errorMessage = string.Empty; // The current error message of the tank.

    // Variables used for showing a help message:
    private float timerHelp = 0.0f; // Timer counting the time in seconds for which the tank is constantly looked at.   
    private GameObject helpDisplay; // The reference to the help display.
    private float timeUntilHelpMessage; // The time in seconds after which the help message is displayed.
    private bool timerHelpRunning = false; // Flag that indicates whether timerHelp is currently running.
    #endregion

    #region Properties
    // Property for the water level:
    public float WaterLevel
    {
        get
        {
            return waterLevel;
        }

        set
        {
            // The new value needs to be between 0 and 1.
            if (value >= 0 && value <= 1)
            {
                waterLevel = value;
                // Setting the water level also updates the tank's info text.
                info = name + "\n" + "Waterlevel: " + (Math.Round(waterLevel,2)*100).ToString() + "%" + errorMessage;
                infobubble.UpdateInfo(info);
            }
            // Print a warning if this is violated.
            else
                Debug.LogWarning("Error! The waterlevel needs to be between 0 and 1.");
        }
    }
    #endregion

    /* Called at the initialization of the object.
     * Sets the important variables.
     */
    void Start ()
    {
        // The water in the tank is the first child of the tank-object.
        water = this.transform.GetChild(0).gameObject;
        // The InfoBubble is a child of the tank.
        infobubble = this.GetComponentInChildren<InfoBubble>();

        // Set the name of this GameObject.
        name = "Tank";
		
		// Get the standard material of the water tank.
		standardMaterial = this.GetComponent<Renderer>().material;
		// Find the material with the outline in the Resources folder.
		outlineMaterial = Resources.Load("TankOutlined", typeof(Material)) as Material;
        // Find the error-material in the Resources folder.
        errorMaterial = Resources.Load("TankError", typeof(Material)) as Material;

        // Initialize the info string and update the infobubble.
        info = name + "\n" + "Waterlevel: " + (waterLevel * 100).ToString() + "%";
        infobubble.UpdateInfo(info);

        // Initialize the reference to the help display and get the time until the message is shown.
        helpDisplay = GameObject.Find("ScaleCamera/Main Camera/CursorCamera/HelpDisplay");
        timeUntilHelpMessage = helpDisplay.GetComponent<HelpDisplay>().TimeUntilHelp;

        // If a reference could not be set, send an error.
        if (water == null)
            Debug.LogError("WaterTank could not find its water object");
        if (infobubble == null)
            Debug.LogError("WaterTank could not find its InfoBubble");
        if (standardMaterial == null)
            Debug.LogError("WaterTank could not find its standard material");
        if (outlineMaterial == null)
            Debug.LogError("WaterTank could not find its outlined material");
        if (errorMaterial == null)
            Debug.LogError("WaterTank could not find its error material");
    }
	
    /* Update() is called every frame.
     * It adjusts the water level and checks if EndHighlight() is necessary.
     * Furthermore it updates the timer for the help message
     * and if necessary displays the help message.
     */
	void Update ()
    {
        // Adjust the waterlevel.
        AdjustWaterLevel();

		// Increase the timer by the time of the last frame.
		timerHighlight += Time.deltaTime;
        // Call EndHighlight if the object is highlighted and the specified time is exceeded.
        if (timerHighlight > timeUntilEndHighlight && highlighted)
			EndHighlight();

        // If the timer for the help message is running, update it.
        if(timerHelpRunning)
        {
            timerHelp += Time.deltaTime;
            // If the updated time exceeds the specified time until the help message
            // is supposed to be shown, stop the timer and show the help display.
            if(timerHelp > timeUntilHelpMessage)
            {
                // Stop and reset the timer and show the help display.
                timerHelpRunning = false;
                timerHelp = 0.0f;
                helpDisplay.SendMessage("ShowHelpMessage");
            }
        }
    }

    /* Called upon interaction with the tank. 
     * Toggles the visibility of the infobubble which displays the waterlevel of the tank.
     * Also resets the timer of the help message and hides the helpDisplay if it is currently shown.
     */
    public void Interact ()
    {
        // Toggle the visibility of the infobubble and show or hide the infobubble accordingly.
        infoVisible = !infobubble.IsVisible;
        infobubble.IsVisible = infoVisible;
        if (infoVisible)
            infobubble.ShowInfo();
        else
            infobubble.HideInfo();

        // Reset the timer for the help display and hide the display.
        timerHelp = 0.0f;
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* Adjusts the height of the tank's water-object to the current waterlevel.
     * The time in which the object's height changes depends on the variable waterSpeed.
     */
    private void AdjustWaterLevel ()
    {
        // If the water-object is higher than the current water level decrease its height.
        if (water.transform.localScale.y > waterLevel)
        {
            water.transform.localScale -= new Vector3(0, waterSpeed * Time.deltaTime, 0);
        }
        // If the water-object is lower than the current water level increase its height.
        if (water.transform.localScale.y < waterLevel)
        {
            water.transform.localScale += new Vector3(0, waterSpeed * Time.deltaTime, 0);
        }
    }
	
	/* Highlight() is called whenever the object is hit by a ray cast.
	 * In other words it is called whenever the object is looked at with the HoloLens.
	 *
	 * It changes the object's material to the outlined material, sets the highlighted flag and
     * resets the timer.
     * It also starts the timer for the help message.
	 */
	public void Highlight ()
	{
        // Do not overwrite the error shader.
        if (!errorShown)
        {
            // Change the tank's material to the outlined material.
            this.GetComponent<Renderer>().material = outlineMaterial;
        }
		// Set the highlighted-flag.
		if(!highlighted)
			highlighted = true;
		// Reset the timer.
		timerHighlight = 0.0f;
        // Start the timer for the help message.
        timerHelpRunning = true;
	}
	
    /* EndHighlight() is called in update, if the timer exceeds the value of timeUntilEndHighlight.
     * 
     * It changes the object's material back to the default material and sets the highlighted-flag to false. 
     * It also stops the timer for the help message and hides said message if necessary.
     */
	private void EndHighlight ()
	{
        // Do not remove the error shader.
        if (!errorShown)
        {
            // Change the tank's material to the default material.
            this.GetComponent<Renderer>().material = standardMaterial;
        }
		// Set the highlighted-flag to false.
		highlighted = false;

        // Stop the timer for the help message, reset it and hide the help message.
        timerHelpRunning = false;
        timerHelp = 0.0f;
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* ShowError() changes the tank's material to the errorMaterial and changes
     * the tank's error message so that it displays the error code.
     */
    public void ShowError (int code)
    {
        if(!errorShown)
        {
            // Change the material to the errorMaterial.
            this.GetComponent<Renderer>().material = errorMaterial;
            // Set the flag.
            errorShown = true;
            // Create the error message and send it to the info bubble, together
            // with the info text. After this, the 'new' info text is used in the info bubble
            // until HideError() is called, because the error message is appended whenever the
            // water level is set (see the property WaterLevel).
            errorMessage = "\nError: Code " + code;
            infobubble.UpdateInfo(info + errorMessage);
        }
    }

    /* Changes the tank's material back to the correct material
     * and clears the error message.
     */
    public void HideError ()
    {
        if(errorShown)
        {
            // Set the tank's material to the default material.
            this.GetComponent<Renderer>().material = standardMaterial;
            // Set the flag to false.
            errorShown = false;
            // If the error is no longer shown and the tank is currently looked at,
            // it has to be highlighted.
            if (highlighted)
                Highlight();
            // Clear the error message, reset the info string and update the infobubble.
            errorMessage = string.Empty;
            info = name + "\n" + "Waterlevel: " + (Math.Round(waterLevel, 2) * 100).ToString() + "%";
            infobubble.UpdateInfo(info);
        }
    }
}
