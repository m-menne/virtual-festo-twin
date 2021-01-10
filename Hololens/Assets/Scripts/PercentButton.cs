/* This class represents a button which applies
 * an int value between 0 and 100, which it receives from a PercentPanel,
 * to the menu.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentButton: Button
{
    #region private variables
    private float value; // The value of the PercentButton.
    private PercentPanel panel; // The reference to the PercentPanel.
    #endregion

    /* Upon initialization the reference to the panel is set.
     */
    void Start()
    {
        // Set the reference to the panel:
        panel = this.transform.parent.GetComponentInChildren<PercentPanel>().GetComponent<PercentPanel>();

        // Print an error message if the panel could not be referenced.
        if (panel == null)
            Debug.LogError("PercentButton could not find its panel");
    }

    /* Called upon interaction.
     * Updates the button's value and notifies the menu.
     */
    public override void Interact()
    {
        // Get the value from the panel.
        value = panel.CurrentValue;
        // Notify the menu of the change.
        NotifyMenu();
    }

    /* This method returns the button's value as a string. 
     */
    public override string ToString()
    {
        return (value / 100).ToString();
    }
}