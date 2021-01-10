/* This abstract class serves as a base for the buttons
 * of the menus for the model's components.
 * It is NOT used for the other buttons in the projects, such
 * as those used to navigate through the menus.
 * 
 * The classes that inherit from this class are:
 * OnOffButton, PercentButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Button: MonoBehaviour
{
    #region protected variables
    // The menu this button is assigned to. This reference
    // is set by the menu, not the button itself.
    protected MenuInteraction menu;
    #endregion

    #region properties
    // Property for the button's menu:
    public MenuInteraction Menu
    {
        get
        {
            return this.menu;
        }

        set
        {
            menu = value;
        }
    }
    #endregion

    /* Called upon interaction with the button. In this project
     * this is usually an air tap.
     */
    public abstract void Interact();

    /* This method notifies the menu of a change.
     */
    public void NotifyMenu ()
    {
        menu.Send();
    }

    /* Sets the button's text color to grey and disables its box collider.
     */
    public void Deactivate()
    {
        this.GetComponentInChildren<Text>().color = Color.grey;
        this.GetComponent<BoxCollider>().enabled = false;
    }

    /* Sets the button's text color to black and enables its box collider.
     */
    public void Activate()
    {
        this.GetComponentInChildren<Text>().color = Color.black;
        this.GetComponent<BoxCollider>().enabled = true;
    }
}