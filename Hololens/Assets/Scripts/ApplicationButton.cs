/* Implements the logic of the button that opens the
 * menu in which the model in the application can be controlled.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationButton : MonoBehaviour {

    #region private variables
    // References to the menus:
    private GameObject homeMenu;
    private GameObject applicationMenu;
    #endregion

    /* When initialized, the references to the menus are set.
     */
    void Start () {
        // Set the references to the menus:
        homeMenu = GameObject.Find("Menu/leftMenu/HomeMenu");
        applicationMenu = GameObject.Find("Menu/leftMenu/ApplicationMenu");

        // Print an error to the console, if a reference was not set.
        if (homeMenu == null)
            Debug.LogError("ApplicationButton could not find the HomeMenu");
        if (applicationMenu == null)
            Debug.LogError("ApplicationButton could not find the ApplicationMenu");
    }
	
    /* Upon interaction the button shows the application menu and hides
     * the home menu.
     */
	public void Interact ()
    {
        if(homeMenu.active)
        {
            homeMenu.SetActive(false);
            applicationMenu.SetActive(true);
        }
    }
}
