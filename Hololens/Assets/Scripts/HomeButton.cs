/* Implements the logic of the home button.
 * This button shows the home menu and hides all other menus.
 * That way it can be used in every menu, as long as that menu
 * is referenced in this script.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour {

    #region private variables
    // References to the menus:
    private GameObject applicationMenu;
    private GameObject passwordKeyboardMenu;
    private GameObject ipKeyboardMenu;
    private GameObject errorMenu;
    private GameObject stationMenu;
    private GameObject homeMenu;
    private GameObject heaterMenu;
    private GameObject scaleMenu;
    private GameObject statusMenu;
    #endregion

    /* When the button is initialized, all references are set.
     */
    void Start () {
        // Initialize the references to the menus:
        applicationMenu = GameObject.Find("Menu/leftMenu/ApplicationMenu");
        passwordKeyboardMenu = GameObject.Find("Menu/leftMenu/PasswordKeyboardMenu");
        ipKeyboardMenu = GameObject.Find("Menu/leftMenu/IPKeyboardMenu");
        errorMenu = GameObject.Find("Menu/leftMenu/ErrorMenu");
        stationMenu = GameObject.Find("Menu/leftMenu/StationMenu");
        homeMenu = GameObject.Find("Menu/leftMenu/HomeMenu");
        heaterMenu = GameObject.Find("Menu/leftMenu/HeaterMenu");
        scaleMenu = GameObject.Find("Menu/leftMenu/ScaleMenu");
        statusMenu = GameObject.Find("Menu/leftMenu/StatusMenu");

        // If a reference could not be set, an error message is printed to the console.
        if (applicationMenu == null)
            Debug.LogError("HomeButton could not find the ApplicationMenu");
        if (passwordKeyboardMenu == null)
            Debug.LogError("HomeButton could not find the PasswordKeyboardMenu");
        if (ipKeyboardMenu == null)
            Debug.LogError("HomeButton could not find the IPKeyboardMenu");
        if (errorMenu == null)
            Debug.LogError("HomeButton could not find the ErrorMenu");
        if (stationMenu == null)
            Debug.LogError("HomeButton could not find the StationMenu");
        if (homeMenu == null)
            Debug.LogError("HomeButton could not find the HomeMenu");
        if (heaterMenu == null)
            Debug.LogError("HomeButton could not find the HeaterMenu");
        if (scaleMenu == null)
            Debug.LogError("HomeButton could not find the ScaleMenu");
        if (statusMenu == null)
            Debug.LogError("HomeButton could not find the StatusMenu");
    }
	
    /* Upon interaction the button shows the home menu
     * and hides all other menus.
     */
	public void Interact ()
    {
        if(!homeMenu.active)
        {
            homeMenu.SetActive(true);
            applicationMenu.SetActive(false);
            passwordKeyboardMenu.SetActive(false);
            ipKeyboardMenu.SetActive(false);
            errorMenu.SetActive(false);
            stationMenu.SetActive(false);
            heaterMenu.SetActive(false);
            scaleMenu.SetActive(false);
            statusMenu.SetActive(false);
        }
    }
}
