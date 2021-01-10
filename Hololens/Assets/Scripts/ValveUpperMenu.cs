/* This class implements the interaction with the upper valve's menu. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveUpperMenu : MenuInteraction
{
    #region private variables
    private OnOffButton button; // The OnOffButton of the upper valve.
    #endregion

    /* Start() is called when the menu is initialized.
     * Sets the relationship between the menu and its button, the menu's
     * destination name and the reference to the request handler.
     */
    void Start ()
    {
        // Set the menu's button and the button's reference to this menu.
        button = GameObject.Find("ValveUpperButton").GetComponent<OnOffButton>();
        button.Menu = this;
        // Set the destination string.
        destination = "valveUpper";
        // Set the reference to the request handler.
        request = GameObject.Find("Request").GetComponent<RequestFromZedboard>();

        // If a reference could not be set, print an error to the console.
        if (button == null)
            Debug.LogError("ValveUpperMenu could not find its OnOffButton");
        if (request == null)
            Debug.LogError("ValveUpperMenu could not find the Request");
    }

    /* Forms the valueString by accessing the button and sends the http-post.
     */
    public override void Send ()
    {
        // Update the valueString.
        valueString = "status=" + button.ToString();
        // Send the post.
        request.PostCommand(destination, valueString);
    }
}