/* Implements the logic of the keyboard which is used for entering the 
 * password to get access to the IP-Keyboard.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordKeyboardScript : MonoBehaviour
{
    #region private variables
    // References to the menus:
    private GameObject applicationMenu;
    private GameObject passwordKeyboardMenu;
    private GameObject ipKeyboardMenu;
    
    private string password = "2671"; // The correct password to access the IP-Keyboard.
    #endregion

    #region properties
    // Property for the password:
    public string Password
    {
        get
        {
            return password;
        }
    }
    #endregion

    /* Start() is called upon initialization of the object.
     * It sets the references to the menus.
     */
    void Start()
    {
        // Initialize the references to the menus:
        applicationMenu = GameObject.Find("Menu/leftMenu/ApplicationMenu");
        passwordKeyboardMenu = GameObject.Find("Menu/leftMenu/PasswordKeyboardMenu");
        ipKeyboardMenu = GameObject.Find("Menu/leftMenu/IPKeyboardMenu");

        // Show an error message, if a reference could not be set.
        if (applicationMenu == null)
            Debug.LogError("PasswordKeyboardScript could not find the ApplicationMenu");
        if (passwordKeyboardMenu == null)
            Debug.LogError("PasswordKeyboardScript could not find the PasswordKeyboardMenu");
        if (ipKeyboardMenu == null)
            Debug.LogError("PasswordKeyboardScript could not find the IPKeyboardMenu");
    }

    /* Hides the password keyboard's menu and shows the IP-Keyboard's menu.
     */
    public void GoToIPKeyboardMenu()
    {
        if (passwordKeyboardMenu.active)
        {
            ipKeyboardMenu.SetActive(true);
            passwordKeyboardMenu.SetActive(false);
        }
    }

    /* Hides the password keyboard's menu and shows the application menu.
     */
    public void GoToModelMenu()
    {
        if (passwordKeyboardMenu.active)
        {
            applicationMenu.SetActive(true);
            passwordKeyboardMenu.SetActive(false);
        }
    }
}
