/* This class implements the logic of the IP keyboard.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPKeyboardScript : MonoBehaviour
{
    #region private variables
    // References to the menus:
    private GameObject applicationMenu;
    private GameObject ipKeyboardMenu;
    
    private RequestFromZedboard request; // The reference to the request handler.
    private string ipAddress; // A string that represents the IP address.
    #endregion

    #region properties
    // Property for the IP address:
    public string IpAddress
    {
        get
        {
            return ipAddress;
        }

        set
        {
            ipAddress = value;
        }
    }
    #endregion

    /* Called when the keyboard is initialized.
     * Sets the required references.
     */
    void Start()
    {
        // Initialize the references to the menus:
        applicationMenu = GameObject.Find("Menu/leftMenu/ApplicationMenu");
        ipKeyboardMenu = GameObject.Find("Menu/leftMenu/IPKeyboardMenu");
        // Initialize the reference to the request-handler.
        request = GameObject.Find("Request").GetComponent<RequestFromZedboard>();

        // Get the IP from the request-handler.
        ipAddress = request.IpAddress;

        // Show an error message, if a reference could not be set.
        if (applicationMenu == null)
            Debug.LogError("IPKeyboardScript could not find the ApplicationMenu");
        if (ipKeyboardMenu == null)
            Debug.LogError("IPKeyboardScript could not find the IPKeyboardMenu");
        if (request == null)
            Debug.LogError("IPKeyboardScript could not find the Request");
    }

    /* Hides the IP-Keyboard and shows the application menu.
     */
    public void GoToModelMenu()
    {
        if (ipKeyboardMenu.active)
        {
            applicationMenu.SetActive(true);
            ipKeyboardMenu.SetActive(false);
        }
    }

    // Submits the IP to the request handler.
    public void SubmitIP ()
    {
        request.IpAddress = ipAddress;
    }
}