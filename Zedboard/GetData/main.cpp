#include <iostream>
#include <unistd.h>

#include "Data.hpp"
#include "DataFilter.hpp"
#include "ConvertAndPrepareData.hpp"
#include "ErrorDetector.hpp"

int main(int argc, const char * argv[]) {

    // Minima and maxima that define the accepted value range. These values were determined empirically.
    uint32_t minimal_leftTank_waterlevel = 0x0;             // Variable for the minimal water level.
    uint32_t maximum_leftTank_waterlevel = 0x80e8;          // Variable for the maximum water level.
    uint32_t minimal_pipeline_waterflow = 0x0;              // Variable for the minimal water flow in pipeline.
    uint32_t maximum_pipeline_waterflow = 0x80e8;           // Variable for the maximum water flow in pipeline.
    uint32_t minimal_pipeline_pressure = 0x0;               // Variable for the minimal pressure in pipeline.
    uint32_t maximum_pipeline_pressure = 0x80e8;            // Variable for the maximum pressure in pipeline.
    uint32_t minimal_rightTank_temperature = 0x0;           // Variable for the minimal temperature of water in the right water tank.
    uint32_t maximum_rightTank_temperature = 0x7918;        // Variable for the maximum temperature of water in the right water tank.
    uint32_t minimal_pipeline_saturation = 0x0;             // Variable for the minimal saturation of water in pipeline.
    uint32_t maximum_pipeline_saturation = 0x80e8;          // Variable for the maximal saturation of water in pipeline.
    uint32_t minimal_pump_power = 0x0;                      // Variable for the minimal pump power.
    uint32_t maximum_pump_power = 0xd9cf;                   // Variable for the maxmimum pump power.
    uint32_t minimal_lowerValve_flowrate = 0x0;             // Variable for the minimal flow rate of the lower valve.
    uint32_t maximum_lowerValve_flowrate = 0xd9cf;          // Variable for the maximum flow rate of the lower valve.

    // Initialze an ErrorDetector object that stores 15 values.
    ErrorDetector detective(15);
    
    // Time between read accesses to shared memory in microseconds.
    uint32_t time = 100000;
    
    // Data object to store the measured data.
    Data data;
    
    // Create a DataFilter object with the minima and maxima values from above and the data object.
    DataFilter filter(minimal_leftTank_waterlevel, maximum_leftTank_waterlevel, minimal_pipeline_waterflow, maximum_pipeline_waterflow, minimal_pipeline_pressure, maximum_pipeline_pressure, minimal_rightTank_temperature, maximum_rightTank_temperature, minimal_pipeline_saturation, maximum_pipeline_saturation, minimal_pump_power, maximum_pump_power, minimal_lowerValve_flowrate, maximum_lowerValve_flowrate, &data);
    
    // Create a ConvertAndPrepareData object and hand over the data object and the ErrorDector object.
    ConvertAndPrepareData capd(&data, &detective);
    
    // Flag that contains if the data is valid or not.
    bool dataOK;
    
    // Run the programm.
    while(true){
        
        // Read the data and check the validity of the data.
        dataOK = filter.routine();
        // Conversion and parsing of the data.
        // Write the data into "json_data.txt".
        capd.runRoutine(dataOK);

        // Wait until the next read access.
        usleep(time);
    }
    
    return 0;
}
