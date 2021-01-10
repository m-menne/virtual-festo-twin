/* This class represents an infobubble attached to a component of the model.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBubble : MonoBehaviour
{
    #region private variables
    private Text infoBubbleText; // Reference to the text object that displays the information. 
    private Image infoBubbleImage; // Reference to the bubble's background image.
    private Transform cam; // Reference to the main camera.
    private string infotext = string.Empty; // The information text this bubble is supposed to display.
    private bool isVisible = false; // Flag to indicate whether the infoBubble is visible or not.
    #endregion

    #region properties
    public bool IsVisible
    {
        get
        {
            return isVisible;
        }

        set
        {
            isVisible = value;
        }
    }
    #endregion

    /* Called upon the initialization of the bubble.
     * Sets the references of the bubble.
     */
    private void Start ()
    {
        // Set the reference to the corresponding image.
        infoBubbleImage = this.transform.GetChild(0).GetComponent<Image>();
        // Set the reference to the corresponding text object.
        infoBubbleText = this.transform.GetChild(1).GetComponent<Text>();

        // Set the reference to the main camera.
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // Print an error message to the console, if a reference was not set.
        if (infoBubbleText == null)
            Debug.LogError("InfoBubble could not find the Text");
        if (infoBubbleImage == null)
            Debug.LogError("InfoBubble could not find the Image");
        if (cam == null)
            Debug.LogError("InfoBubble could not find the MainCamera");
    }

    /* Update() is called once every frame.
     * This function makes sure that the
     * bubble is always faced towards the main camera.
     */
    private void Update ()
    {
        // If the bubble is visible, face it towards the camera.
        if (isVisible)
            this.transform.LookAt(new Vector3(cam.position.x, this.transform.position.y, cam.position.z));
    }

    /* Called by the GameObject this infobubble is assigned to, to display a text.
     * The function enables the image, assigns the infotext to the text object
     * and sets the visibility to true.
     */
    public void ShowInfo()
    {
        // Enable the image.
        infoBubbleImage.enabled = true;
        // Set the text of the text object to the bubble's infotext.
        infoBubbleText.text = infotext;
        // Set the this bubble's visibility to true.
        isVisible = true;
    }


    /* Called by the GameObject this infobubble is assigned to, to toggle the text off.
     * This function disables the image, clears the text object and sets the visibility to false.
     */
    public void HideInfo()
    {
        // Disable the image.
        infoBubbleImage.enabled = false;
        // Clear the text object.
        infoBubbleText.text = string.Empty;
        // Set the visibility to false.
        isVisible = false;
    }

    /* Sets the infotext of the bubble to the given string.
     * 
     * Parameters:
     * string info      The new infotext of the bubble.
     */
    public void UpdateInfo(string info)
    {
        infotext = info;
        // If the bubble is visible when this method is called, the text object's content is also changed.
        if (isVisible)
            infoBubbleText.text = infotext;
    }

}
