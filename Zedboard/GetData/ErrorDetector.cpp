// Implementation of the ErrorDetector class
#include "ErrorDetector.hpp"

// ---------------- Constructor of the ErrorDetector --------------------------------
// Initializes an ErrorDetector object. It sets the status values to 0.0 and the number
// of data values to be stored by the class.
//
// max_numberOfValues_a             The number of values to be stored by the object.
// typ: uint32_t
ErrorDetector
::
ErrorDetector
(
uint32_t max_numberOfValues_a
)
{
    // If the amount of stored values is smaller than 10, set it to 10.
    if (max_numberOfValues_a < 10) this->max_numberOfValues = 10;
    // If the amount of stored values is larger than 50, set it to 50.
    else if (max_numberOfValues_a > 50) this->max_numberOfValues = 50;
    // Else store the number of values that should be stored.
    else this->max_numberOfValues = max_numberOfValues_a;
    
    // Initialize the status variables.
    status_leftTank = 0.0;
    status_upperValve = 0.0;
}

// ---------------- Destructor ------------------------------------------------------
ErrorDetector
::
~ErrorDetector
(
)
{
}

// ---------------- void update_log (float waterlevel_a, float upperValveStatus_a) --
// This method takes the waterlevel of the left tank and the status of the upper valve and
// stores them in the correct vector. If there is more than one value, the method
// calls the update_status() method to update the status.
//
// waterlevel_a          The waterlevel of the left tank.
// Typ: float
//
// upperValveStatus_a    The status of the upper valve.
// Typ: bool
void
ErrorDetector
::
update_log
(
float waterlevelA,
bool upperValveStatusA
)
{
    // Insert the new values at the front of the vector.
    log_waterLevel_lt.insert(log_waterLevel_lt.begin(), waterlevelA);
    log_upperValveStatus.insert(log_upperValveStatus.begin(), upperValveStatusA);
    // Increment the counter for the number of stored elements, if the maximum is not reached.
    if (counter <= max_numberOfValues)++counter;
    // Update the status of each component, if there are at least two values.
    if (counter > 1) update_status();
}

// ---------------- void update_status () -------------------------------------------
// This method updates the status variables. It adds the new value divided by the maximum
// number of values and subtracts the last value, if the counter reaches the maximum number
// of values + 1. After the last value is subtracted, the function deletes the last value.
void
ErrorDetector
::
update_status
(
)
{
    // Add the new value to the status of the left tank variable.
    status_leftTank += (log_waterLevel_lt[0] - log_waterLevel_lt[1]) * 10 / max_numberOfValues;
    // Delete the oldest value in the vector and update the status of the left tank variable.
    if(counter == max_numberOfValues+1) {
        status_leftTank -= (log_waterLevel_lt[max_numberOfValues-1] - log_waterLevel_lt[max_numberOfValues]) * 10 / max_numberOfValues; //
        log_waterLevel_lt.pop_back();
    }
    // Add the new value to the status of the upper valve
    status_upperValve += (float)log_upperValveStatus[0] / max_numberOfValues;
    // Delete the oldest value in the vector and update the status of the left tank variable.
    if(counter == max_numberOfValues+1) {
        status_upperValve -= (float)log_upperValveStatus[max_numberOfValues] / max_numberOfValues;
        log_upperValveStatus.pop_back();
    }
}

// ---------------- int detect_errors (float waterlevel_a, bool pumpStatus_a) -------
// This method
uint32_t
ErrorDetector
::
detect_errors
(
float waterlevel_a,
bool pumpStatus_a
)
{

    // If there are not enough logged values, this method returns 0.
    if (counter != max_numberOfValues+1){
        return 0;
    }
    // If there are enough logged values, the error detection runs.
    else{
        
        // This case deals with the error code 1.
        // The upper valve is closed (so the status_upperValve should be below 0.008) and the water level in left tank lowers (so the status_leftTank should be below -0.0018 (these are empirical values)).
        if(status_leftTank < -0.0018 && status_upperValve < 0.008){
            
            // If there is currently no other error run the error prediction.
            if(current_error == 0 || current_error == 1) {
                // This error prediction should avoid sending a wrong error status.
                // Return error 1, if the class detected previously more than one error 1.
                if(error_prediction == 2){
                    return 1;
                }
                // Return error 1, if the class detected previously an error 1.
                else if(error_prediction == 1){
                    error_prediction++;
                    current_error = 1;
                    return 1;
                }
                // Return 0, if the class detects the error for the first time.
                else{
                    error_prediction++;
                    return 0;
                }
            }
            // If there is currently an other error, send this error.
            else {
                return current_error;
            }
        }
        
        // This case deals with the error code 2.
        // The upper valve is open (so the status_upperValve should be over 0.008 (this is an empirical value) and the water level in left tank does not change (so the status_leftTank should be near zero), the pump is off and the water level in the left tank is over 23 percent.
        else if((status_leftTank < 0.0000001 && status_leftTank >-0.0000001) && status_upperValve > 0.008 && !pumpStatus_a && waterlevel_a > 0.23){
            
            // If there is currently no other error run the error prediction.
            if(current_error == 0 || current_error == 2) {
                // This error prediction should avoid sending a wrong error status.
                // Return error 1, if the class detected previously more than one error 2.
                if(error_prediction == 2){
                    return 2;
                }
                // Return error 2, if the class detected previously an error 2.
                else if(error_prediction == 1){
                    error_prediction++;
                    current_error = 2;
                    return 2;
                }
                // Return 0, if the class detects the error for the first time.
                else{
                    error_prediction++;
                    return 0;
                }
            }
            // If there is currently an other error, send this error.
            else {
                return current_error;
            }
        }
        
        // This case deals with the variables for the error prediction, if there is currently no error.
        else{
            // Return 0, if there was no error before.
            if(error_prediction == -1){
                return 0;
            }
            // If there was an error before, but the error prediction flag is 0, return 0.
            else if(error_prediction == 0){
                current_error = 0;
                error_prediction--;
                return 0;
            }
            // If there was an error before, but the error prediction flag is larger than 0, decrement the error prediction flag and return the current error.
            else{
                error_prediction--;
                return current_error;
            }
        }
        
    }
    
}
