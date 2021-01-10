// Class: ErrorDetector
// This class logs the water level of the left tank and the status of the upper valve.
// If there are enough values, the class starts to detect errors. The class uses the mean
// of the logged data to identify the errors. At the moment the class detects the 
// following errors:
//
// Error 1:     The class detects an error 1, if the water in the left water tank lowers, but the
//              upper valve is closed.
// 
// Error 2:     The class detects an error 2, if the water level of the left water tank is constant, but the
//              upper valve is open.

#ifndef ErrorDetector_hpp
#define ErrorDetector_hpp

#include <string>
#include <iostream>
#include <fstream>
#include <vector>
#include <cstdint>

#include "Data.hpp"


class ErrorDetector
{
    // ---------------- Member variables ------------------------------------------------
    
    private:
    
        // Vectors to store an amount of data over which the mean is calculated.
        std::vector<float> log_waterLevel_lt;       // Vector for the water level values.
        std::vector<bool> log_upperValveStatus;     // Vector for the upper valve status values.
    
        // Variables for the status of the water level of the left water tank and the status of the upper valve.
        float status_leftTank;
        float status_upperValve;
    
        // A variable that stores the maximum number of stored values.
        uint32_t max_numberOfValues;
    
        // Counter variabe of pushed elements.
        uint32_t counter = 0;
    
        // Variable for the 2-Bit error prediction flag.
        int error_prediction = 0;
    
        // Variable for the current error flag.
        uint32_t current_error = 0;
    
    // ---------------- Methods ---------------------------------------------------------
    
    private:
    
        // This method updates the status of the components (water level of the left water tank, upper valve status) the class monitors.
        void update_status();
    
    public:
    
        // Constructor
        // Creates an ErrorDetector object, which stores the given amount of values.
        ErrorDetector(uint32_t max_numberOfData_a);
    
        // Destructor of the ErrorDetector.
        ~ErrorDetector();
    
        // This method stores new values and calcuates the new status values for the water level and the upper valve status.
        void update_log(float waterlevel_a, bool upperValveStatus_a);
    
        // This method detects errors based on the stored data.
        uint32_t detect_errors(float waterlevel_a, bool pumpStatus_a);
};


#endif /* ErrorDetector_hpp */

