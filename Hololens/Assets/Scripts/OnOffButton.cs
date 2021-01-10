/* This class represents a button which toggles between
 * two values, usually on and off.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffButton: Button
{
    #region private variables
    // The current status of the OnOffButton:
    private bool status = false;
    #endregion

    /* Called upon interaction. 
     * Toggles the status and notifies the menu of the change.
     */
    public override void Interact ()
    {
        status = !status;
        NotifyMenu();
    }

    /* This method returns a string "1" if the 
     * button's status is true and "0" if it is false.
     */
    public override string ToString ()
    {
        return status ? "1" : "0";
    }
}