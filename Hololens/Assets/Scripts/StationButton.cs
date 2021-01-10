/* Implements the logic for the button that opens the menu
 * for the station settings.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationButton : MonoBehaviour {

    #region private variables
    // The references to the required menus:
    private GameObject homeMenu;
    private GameObject stationMenu;
    #endregion

    /* Called when the object is initialized. Sets the references.
     */
    void Start () {
        // Set the references to the required menus.
        homeMenu = GameObject.Find("Menu/leftMenu/HomeMenu");
        stationMenu = GameObject.Find("Menu/leftMenu/StationMenu");

        // If a reference could not be set, display an error in the console.
        if(homeMenu == null)
            Debug.LogError("ComponentsButton could not find the HomeMenu");
        if(stationMenu == null)
            Debug.LogError("ComponentsButton could not find the StationMenu");
    }
	
    /* Upon interaction the button hides the home menu
     * and shows the station menu.
     */
	public void Interact ()
    {
        if(homeMenu.active)
        {
            homeMenu.SetActive(false);
            stationMenu.SetActive(true);
        }
    }
}
