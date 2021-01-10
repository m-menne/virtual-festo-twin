/* This class implements the logic of the button that
 * leads to the IP keyboard. It opens the password keyboard
 * which is set before the IP keyboard 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeIPButtonScript : MonoBehaviour
{
    #region private variables
    // References to the menus:
    private GameObject applicationMenu;
    private GameObject passwordKeyboardMenu;
    #endregion

    /* Called when the object is initialized. Sets the references to the menus.
     */
    void Start()
    {
        // Initialize the references to the menus:
        applicationMenu = GameObject.Find("Menu/leftMenu/ApplicationMenu");
        passwordKeyboardMenu = GameObject.Find("Menu/leftMenu/PasswordKeyboardMenu");
        
        // If a reference could not be set, print an error message.
        if (applicationMenu == null)
            Debug.LogError("ChangeIPButtonScript could not find the ApplicationMenu");
        if (passwordKeyboardMenu == null)
            Debug.LogError("ChangeIPButtonScript could not find the PasswordKeyboardMenu");
    }

    /* Called upon interaction.
     * Shows the password keyboard's menu and hides the application menu.
     */
    public void Interact()
    {
        if (applicationMenu.active)
        {
            passwordKeyboardMenu.SetActive(true);
            applicationMenu.SetActive(false);
        }
    }
}
