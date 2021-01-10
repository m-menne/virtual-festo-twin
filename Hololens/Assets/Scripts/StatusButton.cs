/* Implements the logic of the button that opens
 * the station's status overview.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusButton : MonoBehaviour
{
    #region private variables
    // References to the required menus:
    private GameObject stationMenu;
    private GameObject statusMenu;
    #endregion

    /* Upon initialization the references to the menus are set.
     */
    void Start()
    {
        stationMenu = GameObject.Find("Menu/leftMenu/StationMenu");
        statusMenu = GameObject.Find("Menu/leftMenu/StatusMenu");

        // Display an error message in the console, if a reference could not be set.
        if (stationMenu == null)
            Debug.LogError("StatusButton could not find the StationMenu");
        if (statusMenu == null)
            Debug.LogError("StatusButton could not find the StatusMenu");
    }

    /* Upon interaction show the status menu and hide the station menu.
     */
    public void Interact()
    {
        if (stationMenu.active)
        {
            stationMenu.SetActive(false);
            statusMenu.SetActive(true);
        }
    }
}
