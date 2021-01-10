/* This class implements the interaction with the
 * lower valve's menu.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveLowerMenu: MenuInteraction
{
    #region private variables
    private PercentButton button; // The reference to the menu's PercentButton.
    #endregion

    /* Start() is called when the menu is initialized.
     * Sets the relationship between the menu and its button, the menu's
     * destination name and the reference to the request handler.
     */
    void Start()
    {
        // Set the menu's button and the button's reference to this menu.
        button = GameObject.Find("ValveLowerButton").GetComponent<PercentButton>();
        button.Menu = this;
        // Set the destination string.
        destination = "valveLower";
        // Set the reference to the request handler.
        request = GameObject.Find("Request").GetComponent<RequestFromZedboard>();

        // Print an error if failed to set a reference.
        if (button == null)
            Debug.LogError("ValveLowerMenu could not find its PercentButton");
        if (request == null)
            Debug.LogError("ValveLower could not find the Request");
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
}