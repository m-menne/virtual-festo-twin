/* Implements the logic of the display that shows the help message
 * after the user continously looked at the same object for a specified time.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpDisplay : MonoBehaviour {

    #region private variables
    private string content = "Tap to interact"; // The string which is displayed in the help display.
    private Text message; // The reference to the help display's text object.
    private bool messageShown = false; // Flag to show whether the help message has already been displayed.
    private float timeUntilHelp = 4.0f; // The time in seconds, after which the message is shown.
                                        // This is passed to the components that can invoke the help message.
    #endregion

    #region properties
    // Property for the time until the help message is shown.
    public float TimeUntilHelp
    {
        get
        {
            return timeUntilHelp;
        }
    }
    #endregion

    /* Upon initialization set the reference to the text object and its content.
     */
    void Start () {
        message = GetComponentInChildren<Text>();
        message.text = string.Empty;

        if (message == null)
            Debug.LogError("HelpDisplay could not find its text");
	}

    /* Displays the help message.
     */
    public void ShowHelpMessage ()
    {
        // Only show the message if it has not been shown already.
        if (!messageShown)
        {
            message.text = content;
            // Set the flag to prevent the message from being shown again.
            messageShown = true;
        }
    }

    /* Hides the help message.
     */
    public void HideHelpMessage ()
    {
        message.text = string.Empty;
    }

}
