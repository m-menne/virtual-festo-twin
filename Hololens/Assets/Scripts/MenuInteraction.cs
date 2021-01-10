/* This abstract class serves as a base for the
 * interaction with the menus which control the model's components.
 * 
 * The classes that inherit from this class are: PumpMenu, ValveLowerMenu,
 * ValveUpperMenu, HeaterMenu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuInteraction : MonoBehaviour
{
    #region protected variables
    protected RequestFromZedboard request; // The reference to the script that manages the http-communication.
    protected string destination; // The destination of this menu's post.
    protected string valueString; // The string which represents the value that is sent with the post.
    #endregion

    #region properties
    // Property for the menu's value:
    public string ValueString
    {
        get
        {
            return valueString;
        }

        set
        {
            valueString = value;
        }
    }
    #endregion

    /* Send is supposed to access all of the menu's buttons to form the valueString.
     * It then calls PostCommand from RequestFromZedboard with the menu's destination and
     * valueString.
     */
    public abstract void Send();
}