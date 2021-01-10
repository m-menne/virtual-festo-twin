/* Implements the logic of the button that opens the menu
 * for the error explanations.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorExplanationButton : MonoBehaviour {

    #region private variables
    // References to the menus:
    private GameObject homeMenu;
    private GameObject errorMenu;
    #endregion

    /* Upon initialization the references to the menus are set.
     */
    void Start ()
    {
        // Initialize the references to the menus.
        homeMenu = GameObject.Find("Menu/leftMenu/HomeMenu");
        errorMenu = GameObject.Find("Menu/leftMenu/ErrorMenu");

        // Display an error message in the console, if a reference could not be set.
        if (homeMenu == null)
            Debug.LogError("ErrorExplanationButton could not find the HomeMenu");
        if (errorMenu == null)
            Debug.LogError("ErrorExplanationButton could not find the ErrorMenu");
    }

    /* Upon interaction show the error menu and hide the home menu.
     */
    public void Interact()
    {
        if (homeMenu.active)
        {
            errorMenu.SetActive(true);
            homeMenu.SetActive(false);
        }
    }
}
