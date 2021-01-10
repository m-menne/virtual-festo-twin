// Class: ProcessData
// This class checks and converts a data for given component in the Model. After the conversion of the data
// it writes the correct values in corresponding part of the shared memory.
#ifndef ProcessData_hpp
#define ProcessData_hpp

#include <iostream>
#include <fstream>
#include <cstdint>
#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/mman.h>

class ProcessData {
    // ---------------- Member variables ----------------------------------------------
private:
    // Variable which defines the component to be controled.
    // 1 - Pump
    // 2 - Upper valve
    // 3 - Lower valve
    // 4 - Heater
    uint32_t component;
    
    // Variables to store received data
    bool pump_status;                           // Variable for the status of the pump.
    bool pump_mode;                             // Variable for the mode of the pump (0 = binary/1 = analog).
    float pump_power;                           // Variable for the power of the pump.
    uint32_t pump_power_festo;                  // Variable for the converted status of the pump power.
    float lowerValve_status;                    // Variable for the status of the lower valve.
    uint32_t lowerValve_status_festo;           // Variable for the converted status of the lower valve.
    bool upperValve_status;                     // Variable for the status of the upper valve.
    bool heater_status;                         // Variable for the status of the heater.
    bool filter_status;                         // Variable for the status of the filter.
    bool inflow_status;                          // Variable for the status of the inflow.
    
    // Minima and maxima that define the accepted value range
    float minimal_pump_power = 0.0;             // Variable for the minimal pump power.
    float maxmimum_pump_power = 1.0;            // Variable for the maxmimum pump power.
    float minimal_lowerValve_status = 0.0;      // Variable for the minimal lower valve.
    float maximum_lowerValve_status = 1.0;      // Variable for the maximum lower valve.
    
    
    // ---------------- Methods -------------------------------------------------------
private:
    // Checks if the data is plausible.
    bool dataOK();
    
    // Convert the received data into Festo values.
    void convertData();
    
    // Writes the converted values in the shared memory between the OS and the MicroBlaze.
    void writeMemory();
    
public:
    // Constructor for pump.
    ProcessData(bool pump_status_a, bool pump_mode_a, float pump_power_a);
    
    // Constructor for lower valve.
    ProcessData(float lowerValve_status_a);
    
    // Constructor for lower valve or heater.
    ProcessData(uint32_t comp, bool status_a);
    
    // Destructor.
    ~ProcessData();
    
    // Performs the complete filter process and writes the data into the devices files.
    void routine();
};
#endif /* ProcessData_hpp */
