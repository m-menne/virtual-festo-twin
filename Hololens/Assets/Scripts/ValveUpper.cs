/* This class implements the logic of the upper (the right) valve.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValveUpper : MonoBehaviour
{
    #region private variables
    private RequestFromZedboard webRequest; // Reference to the request handler.

    private bool valveOpen = false; // Flag to keep track of the status of the valve.

    // Variables for the infobubble:
    private InfoBubble infobubble; // Reference to the valve's infobubble.
    private bool infoVisible = false; // Flag to indicate whether the infobubble of this valve is visible or not.
    private string name; // The name of this GameObject. This is displayed in the infobubble.
    private string info; // The information text which is shown in the valve's infobubble.

    // Variables for implementing the highlighting functions:
    private Material standardMaterial; // The standard material of the valve.
    private Material outlineMaterial; // The outlined material of the valve.
    private float timerHighlight = 0.0f; // The timer to keep track of the time that has passed after the last call of Highlight().
    private bool highlighted = false; // Flag to indicate whether the valve is currently highlighted.
    private float timeUntilEndHighlight = 0.2f; // The time in seconds for which the valve stays highlighted after you stop looking at it.

    // Variables for the error implementation:
    private bool errorShown = false; // Flag to indicate whether the pump currently shows an error.
    private Material errorMaterial; // The error material of the upper valve.
    private string errorMessage = string.Empty; // The current error message of the valve.

    // References to the menus:
    private GameObject pumpMenu;
    private GameObject valveLowerMenu;
    private GameObject valveUpperMenu;
    // Reference to the valve menu's status text:
    private Text statusText;

    // Variables used for showing a help message:
    private float timerHelp = 0.0f; // Timer for counting the time in seconds for which the valve is constantly looked at.
    private GameObject helpDisplay; // The reference to the help display.
    private float timeUntilHelpMessage; // The time in seconds after which the help message is displayed.
    private bool timerHelpRunning = false; // Flag that indicates whether timerHelp is currently running.
    #endregion

    #region properties
    // Property for the status of the valve:
    public bool ValveOpen
    {
        get
        {
            return valveOpen;
        }

        set
        {
            // Only update the status if it has changed.
            if (value != valveOpen)
            {
                valveOpen = value;
                // Update the information.
                info = name + "\n" + (valveOpen ? "Open" : "Closed") + errorMessage;
                infobubble.UpdateInfo(info);
                statusText.text = (valveOpen ? "Open" : "Closed");
            }
        }
    }
    #endregion

    /* Start() is called, when the valve is initialized.
     * It mainly initializes the valve's variables.
     */
    void Start ()
    {
        // Get the standard material of the valve.
        standardMaterial = this.GetComponent<Renderer>().material;
        // Find the material with the outline in the Resources folder.
        outlineMaterial = Resources.Load("ValveUpperOutlined", typeof(Material)) as Material;
        // Find the error material in the Resources folder.
        errorMaterial = Resources.Load("ValveLowerError", typeof(Material)) as Material;

        // Set the reference to the request handler.
        webRequest = GameObject.Find("Request").GetComponent<RequestFromZedboard>();

        // Initialize the reference to the info bubble.
        infobubble = this.GetComponentInChildren<InfoBubble>();
        // Set the name of this GameObject.
        name = this.gameObject.name;

        // Initialize the references to the menus:
        pumpMenu = GameObject.Find("Menu/rightMenu/PumpMenu");
        valveLowerMenu = GameObject.Find("Menu/rightMenu/ValveLowerMenu");
        valveUpperMenu = GameObject.Find("Menu/rightMenu/ValveUpperMenu");

        // Initialize the info string and update the infobubble.
        info = name + "\n" + (valveOpen ? "Open" : "Closed");
        infobubble.UpdateInfo(info);
        // Set the reference to the status text.
        statusText = GameObject.Find("Menu/rightMenu/ValveUpperMenu/Display").transform.GetComponentsInChildren<Text>()[3];

        // Initialize the reference to the help display and get the time until the message is shown.
        helpDisplay = GameObject.Find("ScaleCamera/Main Camera/CursorCamera/HelpDisplay");
        timeUntilHelpMessage = helpDisplay.GetComponent<HelpDisplay>().TimeUntilHelp;

        // Print an error message to the console, if a reference was not set successfully.
        if (standardMaterial == null)
            Debug.LogError("ValveUpper could not find its standard material");
        if (outlineMaterial == null)
            Debug.LogError("ValveUpper could not find its outlined material");
        if (errorMaterial == null)
            Debug.LogError("ValveLower could not find its error material");
        if (webRequest == null)
            Debug.LogError("ValveUpper could not find the Request");
        if (infobubble == null)
            Debug.LogError("ValveUpper could not find its InfoBubble");
        if (pumpMenu == null)
            Debug.LogError("ValveUpper could not find the PumpMenu");
        if (valveLowerMenu == null)
            Debug.LogError("ValveUpper could not find the ValveLowerMenu");
        if (valveUpperMenu == null)
            Debug.LogError("ValveUpper could not find the ValveUpperMenu");
        if (statusText == null)
            Debug.LogError("ValveUpper could not find its status text");
        if (helpDisplay == null)
            Debug.LogError("ValveUpper could not find the HelpDisplay");
    }

    /* Update() is called once every frame.
     * It checks if EndHighlight() is necessary.
     * Furthermore it updates the timer for the help message, if it is running
     * and if necessary displays the help message.
     */
    void Update ()
    {
        // Increase the timer by the time of the last frame.
        timerHighlight += Time.deltaTime;
        // Call EndHighlight() if the object is highlighted and the specified time is exceeded.
        if (timerHighlight > timeUntilEndHighlight && highlighted)
            EndHighlight();

        // If the timer for the help message is running, update it.
        if (timerHelpRunning)
        {
            timerHelp += Time.deltaTime;
            // If the updated time exceeds the specified time until the help message
            // is supposed to be shown, stop the timer and show the help display.
            if (timerHelp > timeUntilHelpMessage)
            {
                // Stop and reset the timer.
                timerHelpRunning = false;
                timerHelp = 0.0f;
                // Show the help display.
                helpDisplay.SendMessage("ShowHelpMessage");
            }
        } // end if
    }

    /* Called upon interaction.     
     * Toggles the visibility of the infobubble, which displays the information of the valve,
     * and shows the menu.
     * Also resets the timer of the help message and hides the helpDisplay if it is currently shown.
     */
    public void Interact ()
    {
        // Toggle the visibility of the infobubble and hide or show it accordingly.
        infoVisible = !infobubble.IsVisible;
        infobubble.IsVisible = infoVisible;
        if (infoVisible)
            infobubble.ShowInfo();
        else
            infobubble.HideInfo();

        ShowMenu();

        // Hide the help display and reset the timer.
        timerHelp = 0.0f;
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* Toggles the valve between open and closed.
     */
    public void Toggle ()
    {
        valveOpen = !valveOpen;
    }

    /* Highlight() is called whenever the object is hit by a ray cast.
	 * This happens if the valve is looked at with the HoloLens.
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
            // Change the object's material to the outlined material.
            this.GetComponent<Renderer>().material = outlineMaterial;
        }
        // Set the highlighted-flag.
        if (!highlighted)
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
            // Change the object's material to the default material.
            this.GetComponent<Renderer>().material = standardMaterial;
        }
        // Set the highlighted-flag to false.
        highlighted = false;

        // Stop the timer for the help message and reset it.
        timerHelpRunning = false;
        timerHelp = 0.0f;
        // Hide the help message.
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* Hide the other two menus and show this valve's menu.
     */
    private void ShowMenu ()
    {
        if (!valveUpperMenu.active)
        {
            // Show the upper valve's menu.
            valveUpperMenu.SetActive(true);
            // Hide the pump's menu and the lower valve's menu.
            pumpMenu.SetActive(false);
            valveLowerMenu.SetActive(false);
        }
    }

    /* Sets the valve's material to the error material
     * and sets the error message. 
     * 
     * Parameters:
     * int code:        The errorcode which should be displayed.
     */
    public void ShowError (int code)
    {
        // Check if no Error is shown at the moment.
        if (!errorShown)
        {
            // Set the valve's material to the error material.
            this.GetComponent<Renderer>().material = errorMaterial;
            // Now an error is shown.
            errorShown = true;
            // Create the error message and send it to the info bubble, together
            // with the info text. After this, the 'new' info text is used in the info bubble
            // until HideError() is called, because the error message is appended whenever the
            // valve's status is set (See property ValveOpen).
            errorMessage = "\nError: Code " + code;
            infobubble.UpdateInfo(info + errorMessage);
        }
    }

    /* Sets the valve's material back to the correct material
     * and clears the error message.
     */
    public void HideError ()
    {
        // Check if an error is shown at the moment.
        if (errorShown)
        {
            // Set the valve's material to the default material.
            this.GetComponent<Renderer>().material = standardMaterial;
            errorShown = false;
            // If the error is no longer shown and the valve is currently
            // looked at, highlight it.
            if (highlighted)
                Highlight();
            // Clear the error message, reset the info string and update the infobubble.
            errorMessage = string.Empty;
            info = name + "\n" + (valveOpen ? "Open" : "Closed");
            infobubble.UpdateInfo(info);
        }
    }
}
