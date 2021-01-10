/* This class implements a PercentPanel.
 * A PercentPanel is an object which consists of an integer value
 * between 0 and 100 and four buttons to change this value in four ways:
 * Subtract 5, subtract 10, add 5 and add 10.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentPanel: MonoBehaviour
{
    #region private variables 
    private int currentValue; // The panel's current value.

    // References to the panel's buttons:
    private PanelButton buttonMinus10;
    private PanelButton buttonMinus5;
    private PanelButton buttonPlus5;
    private PanelButton buttonPlus10;
    
    private Text panelText; // The reference to the text that is changed by the panel.
    #endregion

    #region properties
    // Property for the current value
    public int CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            currentValue = value;
            // Set the current value to 100, if the received value is too large,
            // and to 0, if it is too small.
            if (currentValue > 100)
                currentValue = 100;
            else if (currentValue < 0)
                currentValue = 0;
            // Update the text of the panel.
            panelText.text = currentValue.ToString() + "%";
        }
    }
    #endregion

    void Start()
    {
        // Set the references to the buttons, the references from the buttons to this panel
        // and the buttons values.
        buttonMinus10 = GetComponentsInChildren<PanelButton>()[0].GetComponent<PanelButton>();
        buttonMinus10.Panel = this;
        buttonMinus10.ButtonValue = -10;
        buttonMinus5 = GetComponentsInChildren<PanelButton>()[1].GetComponent<PanelButton>();
        buttonMinus5.Panel = this;
        buttonMinus5.ButtonValue = -5;
        buttonPlus5 = GetComponentsInChildren<PanelButton>()[2].GetComponent<PanelButton>();
        buttonPlus5.Panel = this;
        buttonPlus5.ButtonValue = 5;
        buttonPlus10 = GetComponentsInChildren<PanelButton>()[3].GetComponent<PanelButton>();
        buttonPlus10.Panel = this;
        buttonPlus10.ButtonValue = 10;
        // Set the reference to the panel text.
        panelText = GetComponentInChildren<Text>();

        // If a reference could not be set correctly, show an error message in the console.
        if (buttonMinus10 == null)
            Debug.LogError("PercentPanel could not find its Button(-10)");
        if (buttonMinus5 == null)
            Debug.LogError("PercentPanel could not find its Button(-5)");
        if (buttonPlus5 == null)
            Debug.LogError("PercentPanel could not find its Button(+5)");
        if (buttonPlus10 == null)
            Debug.LogError("PercentPanel could not find its Button(+10)");
        if (panelText == null)
            Debug.LogError("PercentPanel could not find its text");
    }

    /* Sets the panel's text color to grey.
     */
    public void Deactivate ()
    {
        this.GetComponentInChildren<Text>().color = Color.grey;
    }

    /* Sets the panel's text color to black.
     */
    public void Activate ()
    {
        this.GetComponentInChildren<Text>().color = new Color(0.58f, 0.75f, 0.106f);
    }
}