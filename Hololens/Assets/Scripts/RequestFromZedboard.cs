/* This class provides the communication between the HoloLens and
 * the server which is implemented on the ZedBoard.
 * 
 * It receives the data from the Zedboard and updates the corresponding
 * components of the model.
 * It also provides the functions to send data from the components of the model
 * to the Zedboard.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestFromZedboard : MonoBehaviour {

    #region private variables
    private string ipAddress = "192.168.25.100"; // The IP-Address to which the requests are sent.
    private bool validIP = false; // Flag that is set if the current IP has been validated.
    private float timer = 0.0f; // Timer used for updating the model.
    private float updateInterval = 0.1f; // The interval (in seconds) in which this class requests the data
                                         // from the workstation and updates the model.

    // References:
    private WaterTank tankL;
    private WaterTank tankR;
    private Pump pump;
    private ValveLower valveLower;
    private ValveUpper valveUpper;
    private GameObject homeMenu;
    private HeaterMenu heaterMenu;
    private GameObject errorMenu;
    private GameObject ipButton;
    // Reference to the status overview's textfield:
    private Text statusOverview;
    // Reference to the error menu's textfield:
    private Text errorText;
    // The last received error and warning code:
    private int lastErrorCode = 0;
    private int lastWarningCode = 0;
    #endregion

    #region properties
    // Property for the ip address:
    public string IpAddress
    {
        get
        {
            return ipAddress;
        }

        set
        {
            ipAddress = value;
            // After changing the IP, the new IP is validated.
            // While ValidateIP() is running, the IP is marked as invalid
            // and the button to change the IP is deactivated to prevent errors.
            validIP = false;
            DeactivateIPButton();
            StartCoroutine(ValidateIP());
        }
    }

    // Property for the update interval:
    public float UpdateInterval
    {
        get
        {
            return updateInterval;
        }

        set
        {
            updateInterval = value;
        }
    }
    #endregion

    /* Called upon initialization.
     * Sets all the required references.
     */
    void Start ()
    {
        // Set the required references.
        // Watertanks:
        tankL = GameObject.Find("TankLeft").GetComponent<WaterTank>();
        tankR = GameObject.Find("TankRight").GetComponent<WaterTank>();
        // Pump:
        pump = GameObject.Find("Pump").GetComponent<Pump>();
        // Valves:
        valveLower = GameObject.Find("ValveLower").GetComponent<ValveLower>();
        valveUpper = GameObject.Find("ValveUpper").GetComponent<ValveUpper>();
        // Menus:
        homeMenu = GameObject.Find("Menu/leftMenu/HomeMenu");
        heaterMenu = GameObject.Find("Menu/leftMenu/HeaterMenu").GetComponent<HeaterMenu>();
        errorMenu = GameObject.Find("Menu/leftMenu/ErrorMenu");
        // IP Button:
        ipButton = GameObject.Find("Menu/leftMenu/ApplicationMenu/ChangeIPButton");
        // Status overview (has to be the third child of type 'Text'. Keep in mind, that the
        // display itself has a text too, so you need the fourth text in the list):
        statusOverview = GameObject.Find("Menu/leftMenu/StatusMenu/Display").GetComponentsInChildren<Text>()[3];
        // Error text (has to be the second child of type 'Text'. Keep in mind, that the
        // display itself has a text too, so you need the third text in the list):
        errorText = GameObject.Find("Menu/leftMenu/ErrorMenu/Display").GetComponentsInChildren<Text>()[2];

        // Validate the initial IP.
        StartCoroutine(ValidateIP());

        // Print an error message to the console, if a reference was not set.
        if (tankL == null)
            Debug.LogError("RequestFromZedboard could not find TankLeft");
        if (tankR == null)
            Debug.LogError("RequestFromZedboard could not find TankRight");
        if (pump == null)
            Debug.LogError("RequestFromZedboard could not find Pump");
        if (valveLower == null)
            Debug.LogError("RequestFromZedboard could not find ValveLower");
        if (valveUpper == null)
            Debug.LogError("RequestFromZedboard could not find ValveUpper");
        if (homeMenu == null)
            Debug.LogError("RequestFromZedboard could not find the HomeMenu");
        if (heaterMenu == null)
            Debug.LogError("RequestFromZedboard could not find the HeaterMenu");
        if (errorMenu == null)
            Debug.LogError("RequestFromZedboard could not find the ErrorMenu");
        if (ipButton == null)
            Debug.LogError("RequestFromZedboard could not find the IPButton");
        if (statusOverview == null)
            Debug.LogError("RequestFromZedboard could not find the status overview");
        if (errorText == null)
            Debug.LogError("RequestFromZedboard could not find the error text");
    }

    /* Update is called once per frame.
     * It updates the model in the specified interval
     * by requesting the data from the workstation.
     */
    void Update ()
    {
        // Add the duration of the last frame to the timer.
        timer += Time.deltaTime;
        // After the specified interval reset the timer and request the data from the workstation.
        if (timer > updateInterval)
        {
            timer = 0.0f;
            // Only request the data if the current IP has been validated.
            if (validIP)
                StartCoroutine(this.RequestData());
        }
    }

    /* This method is called by the components of the model
     * and posts a command with the data of the component to the workstation.
     * It will have no effect, if the IP is invalid.
     * 
     * Parameters:
     * string destObject:   The name of the object you want to control
     *                      (depending on the name of the .php-script).
     *                      
     * string value:        The information the component sends, formatted in a string.
     */
    public void PostCommand (string destObject, string value)
    {
        // If the IP is valid, start the Coroutine to post the command to the server.
        if(validIP)
            StartCoroutine(SendPost(destObject, value));
    }

    /* Requests the data from the workstation via http-get
     * and updates the model.
     */
    private IEnumerator RequestData ()
    {
        // Create and send the request.
        UnityWebRequest www = UnityWebRequest.Get("http://" + ipAddress + "/get_data.php");
        yield return www.Send();

        // Print the error if one occured.
        if (www.isNetworkError)
            Debug.LogError(www.error);
        // If the request was successful, adjust the data of the model.
        else
            this.UpdateModel(www.downloadHandler.text);
    }

    /* Sends a post-request with the data of the component to the workstation.
     * 
     * Parameters:
     * string destObject:   The name of the object you want to control
     *                      (depending on the name of the .php-script).
     *                      
     * string value:        The information the component sends, formatted in a string.
     */
    private IEnumerator SendPost (string destObject, string value)
    {
        // Create the request and send it.
        // The resulting url looks like this: http://ipAddress/post_destObject.php?value
        print("http://" + ipAddress + "/" + "post_" + destObject + ".php?" + value);
        UnityWebRequest www = UnityWebRequest.Post("http://" + ipAddress + "/" + "post_" + destObject + ".php?" + value, new WWWForm());
        yield return www.Send();

        // Print the error if one occured.
        if (www.isNetworkError || www.isHttpError)
            Debug.LogError(www.error);
    }

    /* Parses the received data and updates the model accordingly.
     * 
     * Parameters:
     * string receivedData      The received data formatted as a Json-string.
     */
    private void UpdateModel(string receivedData)
    {
        // Make sure that the received string is not empty.
        if (receivedData != "")
        {
            // Splits the received string into an array of substrings, containing every name
            // and every value of receivedData.
            char[] delimiterChars = { '{', '}', '"', ':', ',' };
            string[] splitData = receivedData.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

            // The required variables to update the model with the received values:
            string overflowText = "NaN";
            string floatSwitchText = "NaN";
            string minSwitchText = "NaN";
            string maxSwitchText = "NaN";
            float leftTankWaterlevel;
            float rightTankWaterlevel;
            float waterflow;
            float pressure;
            string waterflowText = "NaN";
            string pressureText = "NaN";
            float temperature;
            bool heater = false;
            float pumpPower;
            float lowerValveFlowrate;
            int errorcode;
            int warningcode;

            // Now try to parse every string in splitData to the required data type.
            // If this does not work, prompt an error message. If it works, save the value in the variable.

            // Overflow:
            if (splitData[1] != "0" && splitData[1] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[0]);
            else
                overflowText = (splitData[1] == "1") ? "1" : "0";
            // FloatSwitch:
            if (splitData[3] != "0" && splitData[3] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[2]);
            else
                floatSwitchText = (splitData[3] == "1") ? "1" : "0";
            // MinSwitch:
            if (splitData[5] != "0" && splitData[5] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[4]);
            else
                minSwitchText = (splitData[5] == "1") ? "1" : "0";
            // MaxSwitch:
            if (splitData[7] != "0" && splitData[7] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[6]);
            else
                maxSwitchText = (splitData[7] == "1") ? "1" : "0";
            // UpperValve:
            if (splitData[9] != "0" && splitData[9] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[8]);
            else
                valveUpper.ValveOpen = (splitData[9] == "1");
            // LeftTankWaterLevel:
            if (!float.TryParse(splitData[11], out leftTankWaterlevel))
                Debug.LogError("Did not receive a float: " + splitData[10]);
            else
                tankL.WaterLevel = leftTankWaterlevel;
            // RightTankWaterLevel:
            if (!float.TryParse(splitData[13], out rightTankWaterlevel))
                Debug.LogError("Did not receive a float: " + splitData[12]);
            else
                tankR.WaterLevel = rightTankWaterlevel;
            // Waterflow:
            if (!float.TryParse(splitData[15], out waterflow))
                Debug.LogError("Did not receive a float: " + splitData[14]);
            else
                waterflowText = (Math.Round(waterflow,1)).ToString();
            // Pressure:
            if (!float.TryParse(splitData[17], out pressure))
                Debug.LogError("Did not receive a float: " + splitData[16]);
            else
                pressureText = (Math.Round(pressure,0)).ToString();
            // Temperature:
            if (!float.TryParse(splitData[19], out temperature))
                Debug.LogError("Did not receive a float: " + splitData[18]);
            // Heater:
            if (splitData[21] != "0" && splitData[21] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[20]);
            else
                heater = (splitData[21] == "1");
            // PumpMode:
            if (splitData[23] != "0" && splitData[23] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[22]);
            else
                pump.PumpMode = (splitData[23] == "1");
            // PumpStatus:
            if (splitData[25] != "0" && splitData[25] != "1")
                Debug.LogError("Did not receive a \"0\" or \"1\": " + splitData[24]);
            else
                pump.PumpOn = (splitData[25] == "1");
            // PumpPower:
            if (!float.TryParse(splitData[29], out pumpPower))
                Debug.LogError("Did not receive a float: " + splitData[28]);
            else
                pump.PumpPower = pumpPower;
            // LowerValveFlowrate:
            if (!float.TryParse(splitData[31], out lowerValveFlowrate))
                Debug.LogError("Did not receive a float: " + splitData[30]);
            else
                valveLower.ValveStatus = lowerValveFlowrate;
            // Errorcode:
            if (!int.TryParse(splitData[33], out errorcode))
                Debug.LogError("Did not receive an int: " + splitData[32]);
            // Warningcode:
            if (!int.TryParse(splitData[35], out warningcode))
                Debug.LogError("Did not receive an int: " + splitData[34]);

            // Update the HeaterMenu:
            heaterMenu.SetTexts((heater ? "On" : "Off"), (Math.Round(temperature,0)).ToString() + "°C");
            // Update the status overview:
            statusOverview.text = waterflowText + " l/min\n"
                                + pressureText + " mbar\n"
                                + minSwitchText + "\n"
                                + maxSwitchText + "\n"
                                + floatSwitchText + "\n"
                                + overflowText;

            // Evaluate the error code if it has changed.
            if (errorcode != lastErrorCode)
            {
                lastErrorCode = errorcode;
                switch (errorcode)
                {
                    case 0:
                        {
                            // Error code 0: OK.
                            // Hide all previously shown errors.
                            tankL.HideError();
                            valveUpper.HideError();
                            pump.HideWarning();
                            break;
                        }
                    case 1:
                        {
                            // Error code 1: 
                            // Show error 1 at the left tank and the upper valve.
                            print("Error 1 received!");
                            tankL.ShowError(1);
                            valveUpper.ShowError(1);
                            break;
                        }
                    case 2:
                        {
                            // Error code 2: 
                            // Show error 2 at the left tank and the upper valve.
                            print("Error 2 received!");
                            tankL.ShowError(2);
                            valveUpper.ShowError(2);
                            break;
                        }
                    default:
                        {
                            print("Unknown error code received!");
                            break;
                        }
                } // end switch
                // Update the text in the error menu.
                UpdateErrorText(errorcode);
                // Open the error menu, if no other menu is open:
                if (homeMenu.active)
                {
                    homeMenu.SetActive(false);
                    errorMenu.SetActive(true);
                }
            } // end if

            // Evaluate the warning code if it has changed.
            if (warningcode != lastWarningCode)
            {
                lastWarningCode = warningcode;
                switch (warningcode)
                {
                    case 0:
                        {
                            // Warning code 0: No warning.
                            // Hide previous warnings.
                            pump.HideWarning();
                            heaterMenu.RemoveWarning();
                            break;
                        }
                    case 1:
                        {
                            // Warning code 1: Heater turned off.
                            print("Warning 1 received!");
                            heaterMenu.DisplayWarning();
                            pump.HideWarning();
                            break;
                        }
                    case 2:
                        {
                            // Warning code 2: Pump turned off.
                            print("Warning 2 received!");
                            pump.ShowWarning();
                            heaterMenu.RemoveWarning();
                            break;
                        }
                    case 3:
                        {
                            // Warning code 3: Pump and heater turned off.
                            print("Warning 3 received!");
                            pump.ShowWarning();
                            heaterMenu.DisplayWarning();
                            break;
                        }
                    default:
                        {
                            print("Unknown warning code received!");
                            break;
                        }
                } // end switch
            } // end if
        } // end if
    }

    /* This method changes the referenced error text
     * according to the new received error code.
     * 
     * Parameters:
     * int code     The new received error code
     */
    private void UpdateErrorText (int code)
    {
        // Depending on the received code, choose the corresponding
        // error text:
        switch(code)
        {
            case 0:
                 errorText.text = "0\nEverything is working.";
                break;
            case 1:
                errorText.text = "1\nThe water level in the left tank goes down, although the upper valve is closed.";
                break;
            case 2:
                errorText.text = "2\nThe water level in the left tank stays constant, although the upper valve is open.";
                break;
            default:
                errorText.text = "Unrecognized error! There might be a problem regarding the communication with the embedded board.";
                break;
        } // end switch
    }

    /* Requests the data from the workstation via http-get
     * but does not update the model. Depending on the success
     * of the request, the flag validIP is set.
     */
    private IEnumerator ValidateIP()
    {
        // Split the IP at every '.' and check if this results in 4 substrings that are not empty.
        // If this is not the case, the IP is invalid.
        char[] seperator = { '.' };
        if (ipAddress.Split(seperator).Length != 4 || ipAddress.Split(seperator, System.StringSplitOptions.RemoveEmptyEntries).Length != 4)
        {
            validIP = false;
            print("IP is invalid!");
        }
        else
        {
            // Create and send the request.
            UnityWebRequest www = UnityWebRequest.Get("http://" + ipAddress + "/get_data.php");
            yield return www.Send();

            // If an error occured, the IP is invalid.
            if (www.isNetworkError)
            {
                validIP = false;
                print("IP is invalid!");
            }
            // If the request was successful, the IP is valid.
            else
            {
                validIP = true;
                print("IP is valid!");
            }
        }
        // Activate the button for the IP keyboard.
        ActivateIPButton();
    }

    /* Deactivates the button for the IP keyboard, by disabling its
     * box collider and changing its text.
     */
    private void DeactivateIPButton ()
    {
        ipButton.GetComponentInChildren<Text>().text = "Validating IP";
        ipButton.GetComponentInChildren<Text>().color = Color.grey;
        ipButton.GetComponent<BoxCollider>().enabled = false;
    }

    /* Activates the button for the IP keyboard, by enabling its
     * box collider. Also changes its text back.
     */
    private void ActivateIPButton ()
    {
        ipButton.GetComponentInChildren<Text>().text = "Change IP";
        ipButton.GetComponentInChildren<Text>().color = Color.black;
        ipButton.GetComponent<BoxCollider>().enabled = true;
    }
}
