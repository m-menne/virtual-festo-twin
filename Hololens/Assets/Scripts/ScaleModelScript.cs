/* This class implements the logic of the button which leads
 * to the menu used to scale the model in the application.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModelScript : MonoBehaviour
{
    #region private variables
    // References to the menus:
    private GameObject applicationMenu;
    private GameObject scaleMenu;
    #endregion

    /* When initialized, the references to the menus are set.
     */
    void Start () {
        // Set the references to the menus:
        applicationMenu = GameObject.Find("Menu/leftMenu/ApplicationMenu");
        scaleMenu = GameObject.Find("Menu/leftMenu/ScaleMenu");
        
        // If a reference could not be set, print an error message.
        if (applicationMenu == null)
            Debug.LogError("ModelControlButton could not find the ApplicationMenu");
        if (scaleMenu == null)
            Debug.LogError("ModelControlButton could not find the ScaleMenu");
    }

    /* Called upon interaction with the button.
     * Hides the application menu and shows the scaling menu.
     */
    public void Interact()
    {
        if (applicationMenu.active)
        {
            applicationMenu.SetActive(false);
            scaleMenu.SetActive(true);
        }
    }
}
