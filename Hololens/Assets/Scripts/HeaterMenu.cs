/* This class implements the interaction with the heater's menu. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaterMenu : MenuInteraction
{
    #region private variables
    private OnOffButton button; // The OnOffButton of the heater.
    private Text statusText; // Reference to the text displaying the heater's status.
    private Text temperatureText; // Reference to the text displaying the water temperature.
    private Text warningText; // Reference to the text displaying the heater warning.
    #endregion

    /* Start() is called when the menu is initialized.
     * It sets all required values and references.
     */
    void Start()
    {
        // Set the menu's button and the button's reference to this menu.
        button = GameObject.Find("HeaterButton").GetComponent<OnOffButton>();
        button.Menu = this;
        // Set the destination string.
        destination = "heater";
        // Set the reference to the request handler.
        request = GameObject.Find("Request").GetComponent<RequestFromZedboard>();
        // Set the reference to the menu's texts.
        statusText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[3];
        temperatureText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[5];
        warningText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[6];
        // Set the default texts.
        statusText.text = "Off";
        temperatureText.text = "0°C";
        warningText.text = string.Empty;

        // Print an error message, if a reference could not be set.
        if (request == null)
            Debug.LogError("HeaterMenu could not find the Request");
        if (statusText == null)
            Debug.LogError("HeaterMenu could not find its status text");
        if (temperatureText == null)
            Debug.LogError("HeaterMenu could not find its temperature text");
        if (warningText == null)
            Debug.LogError("HeaterMenu could not find its warning text");
    }

    /* Forms the valueString by accessing the button and sends the http-post.
     */
    public override void Send()
    {
        // Update the valueString.
        valueString = "status=" + button.ToString();
        // Send the post.
        request.PostCommand(destination, valueString);
    }

    /* Sets the texts' values to the given strings.
     * 
     * Parameters:
     * string status        The text displayed as status
     * string temperature   The text displayed as temperature
     */
    public void SetTexts (string status, string temperature)
    {
        // It is possible that this method is called although the menu has not
        // been initialized yet. In this case set the references.
        if(statusText == null)
            statusText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[3];
        if(temperatureText == null)
            temperatureText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[5];
        statusText.text = status;
        temperatureText.text = temperature;
    }

    /* Displays the warning text in the menu.
     */
    public void DisplayWarning ()
    {
        if(warningText == null)
            warningText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[6];
        warningText.text = "Heater turned off automatically.";
    }

    /* Removes the warning text in the menu.
     */ 
    public void RemoveWarning ()
    {
        if(warningText == null)
            warningText = GameObject.Find("Menu/leftMenu/HeaterMenu/Display").transform.GetComponentsInChildren<Text>()[6];
        warningText.text = string.Empty;
    }
}