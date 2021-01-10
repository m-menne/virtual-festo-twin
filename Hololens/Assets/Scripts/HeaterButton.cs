/* Implements the logic of the button that opens
 * the heater's control menu.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaterButton : MonoBehaviour {

    #region private variables
    // References to the required menus:
    private GameObject stationMenu;
    private GameObject heaterMenu;
    #endregion

    /* Upon initialization the references to the menus are set.
     */
    void Start () {
        stationMenu = GameObject.Find("Menu/leftMenu/StationMenu");
        heaterMenu = GameObject.Find("Menu/leftMenu/HeaterMenu");

        // Print an error to the console, if a reference was not set.
        if(stationMenu == null)
            Debug.LogError("HeaterButton could not find the StationMenu");
        if (heaterMenu == null)
            Debug.LogError("HeaterButton could not find the HeaterMenu");
    }
	
	/* Upon interaction show the heater's menu and hide the station menu.
     */
	public void Interact () {
        if (stationMenu.active)
        {
            stationMenu.SetActive(false);
            heaterMenu.SetActive(true);
        }
    }
}
