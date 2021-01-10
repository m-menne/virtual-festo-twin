/* This class is used to increase the model's scale by 10 % upon interaction.
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseModelScale : MonoBehaviour {

    #region private variables
    private GameObject completeModel; // Reference to the entire model.
    private Text currentScaleValue; // Reference to the text displaying the current scale.
    #endregion

    /* Start() sets the references when this class is initialized.
     */ 
    void Start ()
    {
        completeModel = GameObject.Find("CompleteModel");
        currentScaleValue = GameObject.Find("Menu/leftMenu/ScaleMenu/PercentPanel/PanelText").GetComponent<Text>();
        
        // Print an error message if a reference could not be set.
        if (completeModel == null)
            Debug.LogError("IncreaseModelScale could not find the CompleteModel");
        if (currentScaleValue == null)
            Debug.LogError("IncreaseModelScale could not find the CurrentScaleValue");
    }

    /* Upon interaction the model's scale is increased by 10 %.
     */
    public void Interact ()
    {
        // Get the current scale value.
        float tempScaleValue = int.Parse(currentScaleValue.text.Substring(0, currentScaleValue.text.Length - 1));
        // Do not scale the model to more than 150 %.
        if (tempScaleValue < 150) {
            // Update the text displaying the current scale and the scale of the object.
            currentScaleValue.text = tempScaleValue + 10 + "%";
            completeModel.transform.localScale = new Vector3((tempScaleValue + 10)/100, (tempScaleValue + 10) / 100, (tempScaleValue + 10) / 100);
        }
    }
}
