/* This class implements the lower (the left) valve's logic.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValveLower : MonoBehaviour
{
    #region private variables
    private RequestFromZedboard webRequest; // Reference to the request handler.

    private float valveStatus = 0.0f; // Flag to keep track of the status of the valve.

    // Variables for the infobubble:
    private InfoBubble infobubble; // Reference to the valve's infobubble.
    private bool infoVisible = false; // Flag to indicate whether the infoBubble of this valve is visible or not.
    private string name; // The name of this GameObject, which is displayed in the infobubble.
    private string info; // The information text which is shown in the valve's infobubble.

    // Variables used for highlighting:
    private Material standardMaterial; // The standard material of the valve.
    private Material outlineMaterial; // The outlined material of the valve. 
    private float timerHighlight = 0.0f; // The timer to keep track of the time that has passed after the last call of Highlight.
    private bool highlighted = false; // Flag to indicate whether the valve is currently highlighted.
    private float timeUntilEndHighlight = 0.2f; // The time in seconds for which the valve stays highlighted after you stop looking at it.

    // References to the menus:
    private GameObject pumpMenu;
    private GameObject valveLowerMenu;
    private GameObject valveUpperMenu;
    // Reference to the valve menu's text that displays the current power:
    private Text powerText;

    // Variables used for showing a help message:
    private float timerHelp = 0.0f; // Timer counting the time in seconds for which the valve is constantly looked at. 
    private GameObject helpDisplay; // The reference to the help display.
    private float timeUntilHelpMessage; // The time in seconds after which the help message is displayed.
    private bool timerHelpRunning = false; // Flag that indicates whether timerHelp is currently running.
    #endregion

    #region properties
    // Property for the status of the valve:
    public float ValveStatus
    {
        get
        {
            return valveStatus;
        }

        set
        {
            // The received value has to be between 0 and 1:
            if (value >= 0 && value <= 1)
            {
                valveStatus = value;
                // Update the info string, the info bubble and the menu's text.
                info = name + "\nStatus: " + (Math.Round(valveStatus,2) * 100).ToString()+"%";
                infobubble.UpdateInfo(info);
                powerText.text = (Math.Round(valveStatus,2) * 100).ToString()+"%";
            }
        }
    }
    #endregion

    /* Start() is called upon initialization of the lower valve.
     * It initializes the variables.
     */
    void Start()
    {
        // Get the standard material of the valve.
        standardMaterial = this.GetComponent<Renderer>().material;
        // Find the material with the outline in the Resources folder.
        outlineMaterial = Resources.Load("ValveLowerOutlined", typeof(Material)) as Material;

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
        info = name + "\nStatus: " + valveStatus.ToString();
        infobubble.UpdateInfo(info);
        // Set the reference to the menu's text.
        powerText = GameObject.Find("Menu/rightMenu/ValveLowerMenu/Display").transform.GetComponentsInChildren<Text>()[3];

        // Initialize the reference to the help display and get the time until the message is shown.
        helpDisplay = GameObject.Find("ScaleCamera/Main Camera/CursorCamera/HelpDisplay");
        timeUntilHelpMessage = helpDisplay.GetComponent<HelpDisplay>().TimeUntilHelp;

        // If a reference could not be set, show an error message.
        if (standardMaterial == null)
            Debug.LogError("ValveLower could not find its standard material");
        if (outlineMaterial == null)
            Debug.LogError("ValveLower could not find its outlined material");
        if (webRequest == null)
            Debug.LogError("ValveLower could not find the Request");
        if (infobubble == null)
            Debug.LogError("ValveLower could not find its InfoBubble");
        if (pumpMenu == null)
            Debug.LogError("ValveLower could not find the PumpMenu");
        if (valveLowerMenu == null)
            Debug.LogError("ValveLower could not find the ValveLowerMenu");
        if (valveUpperMenu == null)
            Debug.LogError("ValveLower could not find the ValveUpperMenu");
        if (powerText == null)
            Debug.LogError("ValveLower could not find its power text");
        if (helpDisplay == null)
            Debug.LogError("ValveLower could not find the HelpDisplay");
    }

    /* Update() is called every frame.
     * It checks if EndHighlight() is necessary.
     * Furthermore it updates the timer for the help message, if it is running
     * and if necessary displays the help message.
     */
    void Update()
    {
        // Increase the timer by the time of the last frame.
        timerHighlight += Time.deltaTime;
        // Call EndHighlight() if the object is currently highlighted and the specified time is exceeded.
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
        }
    }

    /* Called upon interaction with the lower valve.  
     *       
     * Toggles the visibility of the infobubble, which displays the information of the valve
     * and shows the valve's menu.
     * Also resets the timer of the help message and hides the helpDisplay if it is currently shown.
     */
    public void Interact()
    {
        // Toggle the visibility of the infobubble and show or hide it accordingly.
        infoVisible = !infobubble.IsVisible;
        infobubble.IsVisible = infoVisible;
        if (infoVisible)
            infobubble.ShowInfo();
        else
            infobubble.HideInfo();

        // Show this valve's menu.
        ShowMenu();

        // Reset the help display's timer and hide the display
        timerHelp = 0.0f;
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* Highlight() is called whenever the valve is hit by a raycast.
	 * In the project's context this happens if the valve is looked at with the HoloLens.
	 *
	 * It changes the object's material to the outlined material, sets the highlighted flag and
     * resets the timer.
     * It also starts the timer for the help message.
	 */
    public void Highlight()
    {
        // Change the object's material to the outlined material.
        this.GetComponent<Renderer>().material = outlineMaterial;
        // Set the highlighted-flag.
        if (!highlighted)
            highlighted = true;
        // Reset the highlight-timer.
        timerHighlight = 0.0f;
        // Start the timer for the help message.
        timerHelpRunning = true;
    }

    /* EndHighlight() is called in update, if the timer exceeds the value of timeUntilEndHighlight.
     * 
     * It changes the valve's material back to the default material and sets the highlighted-flag to false.
     * It also stops the timer for the help message and hides said message if necessary.
     */
    private void EndHighlight()
    {
        // Change the valve's material to the default material.
        this.GetComponent<Renderer>().material = standardMaterial;
        // Set the highlighted-flag to false.
        highlighted = false;

        // Stop the timer for the help message and reset it.
        timerHelpRunning = false;
        timerHelp = 0.0f;
        // Hide the help message.
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* Hides the other two menus and shows the lower valve's menu.
     */
    private void ShowMenu()
    {
        if (!valveLowerMenu.active)
        {
            valveLowerMenu.SetActive(true);
            pumpMenu.SetActive(false);
            valveUpperMenu.SetActive(false);
        }
    }
}
