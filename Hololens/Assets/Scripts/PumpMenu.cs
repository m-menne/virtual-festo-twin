/* This class implements the interaction with the pump's menu.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpMenu: MenuInteraction
{
    #region private variables
    // The references to the menu's buttons:
    private OnOffButton modebutton;
    private OnOffButton onoffbutton;
    private PercentButton percentbutton;
    #endregion

    /* Start() is called when the menu is initialized.
     * Sets the relationship between the menu and its button, the menu's
     * destination name and the reference to the request handler.
     */
    void Start()
    {
        // Set the menu's buttons and the buttons' reference to this menu.
        modebutton = GameObject.Find("PumpButtonMode").GetComponent<OnOffButton>();
        modebutton.Menu = this;
        onoffbutton = GameObject.Find("PumpButtonOnOff").GetComponent<OnOffButton>();
        onoffbutton.Menu = this;
        percentbutton = GameObject.Find("PumpButtonPercent").GetComponent<PercentButton>();
        percentbutton.Menu = this;
        // Set the destination string.
        destination = "pump";
        // Set the reference to the request object.
        request = GameObject.Find("Request").GetComponent<RequestFromZedboard>();

        // If a reference was not set correctly, display an error in the console.
        if (modebutton == null)
            Debug.LogError("PumpMenu could not find its ModeButton");
        if (onoffbutton == null)
            Debug.LogError("PumpMenu could not find its OnOffButton");
        if (percentbutton == null)
            Debug.LogError("PumpMenu could not find its PercentButton");
        if (request == null)
            Debug.LogError("PumpMenu could not find the Request");
    }

    /* Forms the valueString by accessing the buttons and sends the http-post.
     */
    public override void Send()
    {
        // Update the valueString.
        valueString = "status="+onoffbutton.ToString() + "&mode=" + modebutton.ToString() + "&power=" + percentbutton.ToString();
        // Send the post.
        request.PostCommand(destination, valueString);
    }
}