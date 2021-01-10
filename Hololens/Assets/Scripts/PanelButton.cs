/* This class represents a button which, when pressed,
 * changes the value of the referenced panel by its own
 * value.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelButton: MonoBehaviour
{
    #region private variables
    private int buttonValue = 0; // The value of this button.
    private PercentPanel panel; // The reference to the panel, this button is part of.
    #endregion

    #region properties
    // Property for the button's value.
    public int ButtonValue
    {
        get
        {
            return buttonValue;
        }

        set
        {
            buttonValue = value;
        }
    }
    // Property for the panel.
    public PercentPanel Panel
    {
        set
        {
            panel = value;
        }
    }
    #endregion

    /* Called upon interaction.
     * Adds the button's value to the panel's current value.
     */
    void Interact ()
    {
        panel.CurrentValue += buttonValue;
    }

    /* Sets the button's text color to grey and disables its box collider.
     */
    public void Deactivate ()
    {
        this.GetComponentInChildren<Text>().color = Color.grey;
        this.GetComponent<BoxCollider>().enabled = false;
    }

    /* Sets the button's text color to black and enables its box collider.
     */
    public void Activate ()
    {
        this.GetComponentInChildren<Text>().color = Color.black;
        this.GetComponent<BoxCollider>().enabled = true;
    }
}