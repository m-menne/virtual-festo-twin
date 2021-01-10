/* This class implements the logic of the pump.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pump : MonoBehaviour
{
    #region private variables
    // The pump's parameters:
    private bool pumpOn = false; // Flag to indicate whether the pump is on or off.
    private float pumpPower = 0.0f; // The power of the pump. Has to be between 0 and 1.
    private bool pumpMode = false; // The mode of the pump (0/false = binary, 1/true = analog).

    // Variables for the implementation of the info bubble:
    private InfoBubble infobubble; // Reference to the pump's infobubble.
    private bool visible; // Flag to indicate whether the infobubble of the pump is visible or not.
    private string name; // The name of this GameObject. It is displayed in the infobubble.  
    private string info; // The information text which is shown in the pump's infobubble.

    // Variables used for highlighting:
    private Material standardMaterial; // The standard material of the pump.
    private Material outlineMaterial; // The outlined material of the pump.
    private float timerHighlight = 0.0f; // The timer to keep track of the time that has passed after the last call of Highlight().
    private bool highlighted = false; // Flag to indicate whether the pump is highlighted.
    private float timeUntilEndHighlight = 0.2f; // The time in seconds for which the pump stays highlighted
                                                // after you stop looking at it.

    // Variables used for the logic of the menus: 
    private GameObject pumpMenu; // Reference to the pump's menu.
    private GameObject valveLowerMenu; // Reference to the lower valve's menu.
    private GameObject valveUpperMenu; // Reference to the upper valve's menu.
    // References to the pump menu's texts:
    private Text statusText;
    private Text powerText;
    private Text modeText;
    private Text powerLabel;
    // References to the pump menu's PercentPanel, the panel's buttons, the mode button and PercentButton:
    private PercentPanel panel;
    private PercentButton percentbutton;
    private PanelButton buttonMinus10;
    private PanelButton buttonMinus1;
    private PanelButton buttonPlus1;
    private PanelButton buttonPlus10;
    private OnOffButton modeButton;

    private RequestFromZedboard webRequest; // Reference to the request handler.

    // Variables used for showing a help message:
    private float timerHelp = 0.0f; // Timer counting the time in seconds for which the valve is constantly looked at.
    private GameObject helpDisplay; // The reference to the help display.
    private float timeUntilHelpMessage; // The time in seconds after which the help message is displayed.
    private bool timerHelpRunning = false; // Flag that indicates whether timerHelp is currently running.

    // Variables used for showing the warning message:
    private string warningMessage = "\nWarning! Pump turned off."; // The warning message.
    private bool warningShown = false; // Flag that indicates if a warning message is currently shown.
    #endregion

    #region properties
    // Property for the mode of the pump:
    public bool PumpMode
    {
        get
        {
            return pumpMode;
        }
        set
        {
            // Only adjust if the mode has changed.
            if (pumpMode != value)
            {
                pumpMode = value;
                // Adjust the text in the menu.
                modeText.text = (pumpMode ? "Analog" : "Binary");
                if (pumpMode)
                {
                    // If the pump is in analog mode, activate the analog interface
                    ActivateAnalogInterface();
                    // Display the pump's power in the info bubble in analog mode.
                    info = name + "\n" + (pumpOn ? "On" : "Off") + "\nPower: " + (Math.Round(pumpPower, 2) * 100).ToString() + "%";
                    infobubble.UpdateInfo(info + (warningShown ? warningMessage : ""));
                }
                else
                {
                    // Deactivate the analog interface, if the pump is in binary mode.
                    DeactivateAnalogInterface();
                    // If the pump is in binary mode and off, disable the mode button.
                    if (!pumpOn)
                        modeButton.Deactivate();
                    // Do not display the pump's power in the info bubble in binary mode.
                    info = name + "\n" + (pumpOn ? "On" : "Off");
                }
            }
        }
    }

    // Property for the status of the pump:
    public bool PumpOn
    {
        get
        {
            return pumpOn;
        }
        set
        {
            // Only update the status if it has changed.
            if (value != pumpOn)
            {
                pumpOn = value;
                // Update the info string, the info bubble and the menu's status displaying text.
                // Display the pump's power only in analog mode.
                // If the warning flag is set, the warning message is appended to the info string.
                info = name + "\n" + (pumpOn ? "On" : "Off")
                    + (pumpMode ? "\nPower: " + (Math.Round(pumpPower,2)*100).ToString() + "%" : "");
                infobubble.UpdateInfo(info + (warningShown ? warningMessage : ""));
                statusText.text = (pumpOn ? "On" : "Off");
                // If the pump is turned on, enable the mode button.
                if (pumpOn)
                    modeButton.Activate();
                // Deactivate the mode button, if the pump is turned off in binary mode.
                else if (!pumpOn && !pumpMode)
                    modeButton.Deactivate();
            }
        }
    }

    // Property for the power of the pump:
    public float PumpPower
    {
        get
        {
            return pumpPower;
        }
        set
        {
            // The received value needs to be between 0 and 1:
            if (value >= 0 && value <= 1)
            {
                pumpPower = value;
                // Update the info string, the info bubble and the menu's power displaying text.
                // The power is only displayed in the info bubble, if the pump is in analog mode.
                info = name + "\n" + (pumpOn ? "On" : "Off")
                    + (pumpMode ? "\nPower: " + (Math.Round(pumpPower, 2) * 100).ToString() + "%" : "");
                infobubble.UpdateInfo(info + (warningShown ? warningMessage : ""));
                powerText.text = (Math.Round(pumpPower, 2) * 100).ToString() + "%";
            }
        }
    }
    #endregion

    /* Called upon initialization.
     * Sets the important variables.
     */
    void Start ()
    {
        // Get the standard material of the pump.
        standardMaterial = this.GetComponent<Renderer>().material;
        // Find the material with the outline in the Resources folder.
        outlineMaterial = Resources.Load("PumpOutlined", typeof(Material)) as Material;

        // Initialize the reference to the request handler.
        webRequest = GameObject.Find("Request").GetComponent<RequestFromZedboard>();

        // Initialize the reference to the infobubble.
        infobubble = this.GetComponentInChildren<InfoBubble>();
        // Get the visibility of the infobubble.
        visible = infobubble.IsVisible;
        // Get the name of this GameObject.
        name = this.gameObject.name;

        // Initialize the references to the menus.
        pumpMenu = GameObject.Find("Menu/rightMenu/PumpMenu");
        valveLowerMenu = GameObject.Find("Menu/rightMenu/ValveLowerMenu");
        valveUpperMenu = GameObject.Find("Menu/rightMenu/ValveUpperMenu");

        // Initialize the info string and update the infobubble.
        info = name + "\n" + (pumpOn ? "On" : "Off")
            + (pumpMode ? "\nPower: " + (Math.Round(pumpPower, 2) * 100).ToString() + "%" : "");
        infobubble.UpdateInfo(info);

        // Set the references to the pump menu's texts:
        statusText = GameObject.Find("Menu/rightMenu/PumpMenu/Display").transform.GetComponentsInChildren<Text>()[3];
        powerText = GameObject.Find("Menu/rightMenu/PumpMenu/Display").transform.GetComponentsInChildren<Text>()[5];
        modeText = GameObject.Find("Menu/rightMenu/PumpMenu/Display").transform.GetComponentsInChildren<Text>()[2];
        powerLabel = GameObject.Find("Menu/rightMenu/PumpMenu/Display").transform.GetComponentsInChildren<Text>()[4];
        // Set the reference to the panel and the buttons:
        panel = GameObject.Find("Menu/rightMenu/PumpMenu").transform.GetComponentInChildren<PercentPanel>();
        percentbutton = GameObject.Find("Menu/rightMenu/PumpMenu").transform.GetComponentInChildren<PercentButton>();
        buttonMinus10 = panel.GetComponentsInChildren<PanelButton>()[0].GetComponent<PanelButton>();
        buttonMinus1 = panel.GetComponentsInChildren<PanelButton>()[1].GetComponent<PanelButton>();
        buttonPlus1 = panel.GetComponentsInChildren<PanelButton>()[2].GetComponent<PanelButton>();
        buttonPlus10 = panel.GetComponentsInChildren<PanelButton>()[3].GetComponent<PanelButton>();
        modeButton = GameObject.Find("Menu/rightMenu/PumpMenu").transform.GetComponentsInChildren<OnOffButton>()[0];

        // Initialize the reference to the help display and get the time until the message is shown.
        helpDisplay = GameObject.Find("ScaleCamera/Main Camera/CursorCamera/HelpDisplay");
        timeUntilHelpMessage = helpDisplay.GetComponent<HelpDisplay>().TimeUntilHelp;

        // If the pump menu is shown at the beginning and the pump's mode is set to binary,
        // deactivate the analog interface.
        if (pumpMenu.active && !pumpMode)
            DeactivateAnalogInterface();
        // Deactivate the mode button at the beginning, if necessary.
        if (!pumpMode && !pumpOn)
            modeButton.Deactivate();

        // Print an error message if a reference was not set.
        if (standardMaterial == null)
            Debug.LogError("Pump could not find its standard material");
        if (outlineMaterial == null)
            Debug.LogError("Pump could not find its outlined material");
        if (webRequest == null)
            Debug.LogError("Pump could not find the Request");
        if (infobubble == null)
            Debug.LogError("Pump could not find its InfoBubble");
        if (pumpMenu == null)
            Debug.LogError("Pump could not find the PumpMenu");
        if (valveLowerMenu == null)
            Debug.LogError("Pump could not find the ValveLowerMenu");
        if (valveUpperMenu == null)
            Debug.LogError("Pump could not find its ValveUpperMenu");
        if (statusText == null)
            Debug.LogError("Pump could not find its status text");
        if (powerText == null)
            Debug.LogError("Pump could not find its power text");
        if (modeText == null)
            Debug.LogError("Pump could not find its mode text");
        if (powerLabel == null)
            Debug.LogError("Pump could not find its power label");
        if (panel == null)
            Debug.LogError("Pump could not find its PercentPanel");
        if (percentbutton == null)
            Debug.LogError("Pump could not find its PercentButton");
        if (buttonMinus10 == null)
            Debug.LogError("Pump could not find its PanelButton(-10)");
        if (buttonMinus1 == null)
            Debug.LogError("Pump could not find its PanelButton(-5)");
        if (buttonPlus1 == null)
            Debug.LogError("Pump could not find its PanelButton(+5)");
        if (buttonPlus10 == null)
            Debug.LogError("Pump could not find its PanelButton(+10)");
        if (modeButton == null)
            Debug.LogError("Pump could not find its mode button");
        if (helpDisplay == null)
            Debug.LogError("Pump could not find the HelpDisplay");
    }

    /* Update() is called once per frame.
     * It calls EndHighlight() if necessary.
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
            // If the updated time exceeds the specified timeUntilHelpMessage,
            // stop the timer and show the help display.
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

    /* Interact() is called upon interaction with the pump.    
     * It toggles the infobubble and shows the pump's menu.
     * Also resets the timer of the help message and hides the helpDisplay if it is currently shown.
     */
    public void Interact ()
    {
        // Toggle the visibility of the infobubble and hide or show
        // it accordingly.
        visible = !infobubble.IsVisible;
        infobubble.IsVisible = visible;
        if (visible)
            infobubble.ShowInfo();
        else
            infobubble.HideInfo();

        // Show the pump's menu.
        ShowMenu();
        // Hide the help display and reset the timer.
        timerHelp = 0.0f;
        helpDisplay.SendMessage("HideHelpMessage");
        // Remove the warning message when the pump is airtapped.
        warningShown = false;
    }

    /* Highlight() is called whenever the object is hit by a raycast.
	 * This happens, whenever the object is looked at with the HoloLens.
	 *
	 * It changes the object's material to the outlined material, sets the highlighted flag and
     * resets the timer.
     * It also starts the timer for the help message.
	 */
    public void Highlight ()
    {
        // Change the object's material to the outlined material.
        this.GetComponent<Renderer>().material = outlineMaterial;
        // Set the highlighted-flag.
        if (!highlighted)
            highlighted = true;
        // Reset the timer.
        timerHighlight = 0.0f;
        // Start the timer for the help message.
        timerHelpRunning = true;
    }

    /* EndHighlight() is called in Update(), if the timer exceeds the value of timeUntilEndHighlight.
     * 
     * It changes the object's material back to the default material and sets the highlighted-flag to false.
     * It also stops the timer for the help message and hides said message if necessary.
     */
    private void EndHighlight ()
    {
        // Change the object's material to the default material.
        this.GetComponent<Renderer>().material = standardMaterial;
        // Set the highlighted-flag to false.
        highlighted = false;

        // Stop the timer for the help message and reset it.
        timerHelpRunning = false;
        timerHelp = 0.0f;
        // Hide the help message.
        helpDisplay.SendMessage("HideHelpMessage");
    }

    /* Hides the valve menus and shows the pump's menu.
     */
    private void ShowMenu ()
    {
        if (!pumpMenu.active)
        {
            // Show the pump's menu.
            pumpMenu.SetActive(true);
            // Hide both valves' menus.
            valveLowerMenu.SetActive(false);
            valveUpperMenu.SetActive(false);
        }
        // If the mode of the pump is set to binary,
        // the percent button and the percent panel are deactivated.
        if (pumpMode == false)
            DeactivateAnalogInterface();
        else
            ActivateAnalogInterface();
    }

    /* Shows the warning message in the infobubble.
     * After that, the message is displayed until the
     * pump is airtapped or the warning is no longer sent.
     */
    public void ShowWarning ()
    {
        if (!warningShown)
        {
            // Set the flag.
            warningShown = true;
            // Update the infobubble's text.
            infobubble.UpdateInfo(info + warningMessage);
        }
    }

    /* Removes the warning message from the infobubble
     * and sets the warning-flag to false.
     */
    public void HideWarning ()
    {
        if(warningShown)
        {
            // Set the flag to false.
            warningShown = false;
            // Update the infobubble's text.
            infobubble.UpdateInfo(info);
        }
    }

    /* Deactivates the interface for the analog regulation.
     */
    private void DeactivateAnalogInterface ()
    {
        // Deactivate all components of the interface.
        percentbutton.Deactivate();
        panel.Deactivate();
        buttonMinus10.Deactivate();
        buttonMinus1.Deactivate();
        buttonPlus1.Deactivate();
        buttonPlus10.Deactivate();
        // Grey out the text displaying the color.
        powerText.color = Color.grey;
    }

    /* Activates the interface for the analog regulation.
     */
    private void ActivateAnalogInterface ()
    {
        // Activate all components of the interface.
        percentbutton.Activate();
        panel.Activate();
        buttonMinus10.Activate();
        buttonMinus1.Activate();
        buttonPlus1.Activate();
        buttonPlus10.Activate();
        // Set the color of the power text back to green.
        powerText.color = new Color(0.58f, 0.75f, 0.106f);
    }
}
