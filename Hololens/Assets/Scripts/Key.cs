/* This implements the function of the keys of the keyboards.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key: MonoBehaviour {

    #region private variables
    // Reference to current password's and IP's texts:
    private Text currentPassword;
    private Text currentIPAddress;
    
    private string correctPassword; // The correct password.
    private string keyValue; // The value of the key.
    // References to the required menus:
    private GameObject passwordKeyboardMenu;
    private GameObject ipKeyboardMenu;
    #endregion

    /* Called when the key is initialized.
     * Sets all required variables.
     */
    void Start()
    {
        // Set the references to the texts:
        currentPassword = GameObject.Find("Menu/leftMenu/PasswordKeyboardMenu/Display/CurrentPassword").GetComponent<Text>();
        currentIPAddress = GameObject.Find("Menu/leftMenu/IPKeyboardMenu/Display/CurrentIP").GetComponent<Text>();
        // Set the key's value to its text in the model.
        keyValue = this.gameObject.transform.GetChild(0).GetComponent<Text>().text;
        // Set the references to the menus:
        passwordKeyboardMenu = GameObject.Find("Menu/leftMenu/PasswordKeyboardMenu");
        ipKeyboardMenu = GameObject.Find("Menu/leftMenu/IPKeyboardMenu");
        // Get the correct password from the password keyboard.
        correctPassword = passwordKeyboardMenu.GetComponent<PasswordKeyboardScript>().Password;
        // Set the current IP and display it in the menu.
        currentIPAddress.text = ipKeyboardMenu.GetComponent<IPKeyboardScript>().IpAddress;
        
        // If a reference could not be set, print an error message.
        if (currentPassword == null)
            Debug.LogError("KeyOfKeyboard could not find the CurrentPassword");
        if (currentIPAddress == null)
            Debug.LogError("KeyOfKeyboard could not find the CurrentIPAddress");
        if (passwordKeyboardMenu == null)
            Debug.LogError("KeyOfKeyboard could not find the PasswordKeyboardMenu");
        if (ipKeyboardMenu == null)
            Debug.LogError("KeyOfKeyboard could not find the IPKeyboardMenu");
    }

    /* Called upon interaction with the key.
     */
    public void Interact()
    {
        if (keyValue == "OK")
        {
            // If the key is part of the PasswordKeyboard,
            // check if the entered password equals the correct password.
            if (this.transform.parent.gameObject == passwordKeyboardMenu)
            {
                // Password correct: go to the IP-Keyboard.
                if (correctPassword == currentPassword.text)
                {
                    currentPassword.text = "";
                    passwordKeyboardMenu.GetComponent<PasswordKeyboardScript>().GoToIPKeyboardMenu();
                }
                // Password incorrect: clear it.
                else
                {
                     currentPassword.text = "";
                }
            } // end if
            // If the key is part of the IPKeyboard, set the IP, submit it and return to the 
            // model menu.
            if (this.transform.parent.gameObject == ipKeyboardMenu)
            {
                ipKeyboardMenu.GetComponent<IPKeyboardScript>().IpAddress = currentIPAddress.text;
                ipKeyboardMenu.GetComponent<IPKeyboardScript>().SubmitIP();
                ipKeyboardMenu.GetComponent<IPKeyboardScript>().GoToModelMenu();
                }
        } // end if
        else if (keyValue == "Cancel")
        {
            // The cancel button leads back to the model menu in both keyboards.
            if (this.transform.parent.gameObject == passwordKeyboardMenu)
            {
                // Clear the current password.
                currentPassword.text = "";
                passwordKeyboardMenu.GetComponent<PasswordKeyboardScript>().GoToModelMenu();
            }
            if (this.transform.parent.gameObject == ipKeyboardMenu)
            {
                currentIPAddress.text = ipKeyboardMenu.GetComponent<IPKeyboardScript>().IpAddress;
                ipKeyboardMenu.GetComponent<IPKeyboardScript>().GoToModelMenu();
            }
        } // end else if
        else if (keyValue == "Del")
        {
            // Delete the last character of the keyboard's text. This is the same for both keyboards.
            if (this.transform.parent.gameObject == passwordKeyboardMenu)
            {
                if (currentPassword.text.Length > 0)
                    currentPassword.text = currentPassword.text.Substring(0, currentPassword.text.Length - 1);
            }
            if (this.transform.parent.gameObject == ipKeyboardMenu)
            {
                if (currentIPAddress.text.Length > 0)
                    currentIPAddress.text = currentIPAddress.text.Substring(0, currentIPAddress.text.Length - 1);
            }
        } // end else if
        else
        {
            // If the key's value is a number (or a dot), append this character to the
            // keyboard's text. This is the same for both keyboards.
            if (this.transform.parent.gameObject == passwordKeyboardMenu)
            {
                currentPassword.text += keyValue;
            }
            if (this.transform.parent.gameObject == ipKeyboardMenu)
            {
                currentIPAddress.text += keyValue;
            }
        } // end else
    }
}
