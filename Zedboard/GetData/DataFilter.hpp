// Class: DataFilter
// This class reads the measured data from the shared memory and checks its validity.

#ifndef DataFilter_hpp
#define DataFilter_hpp

#include <fstream>
#include <string>
#include <iostream>
#include <cstdint>

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/mman.h>

#include "Data.hpp"

class DataFilter
{
    // ---------------- Member variables ------------------------------------------------
    
    private:
    
        // Data object to store the current data.
        Data* data;
    
        // Minima and maxima that define the accepted value range.
        uint32_t minimal_leftTank_waterlevel;       // Variable for the minimal waterlevel.
        uint32_t maximum_leftTank_waterlevel;       // Variable for the maximum waterlevel.
        uint32_t minimal_pipeline_waterflow;        // Variable for the minimal waterflow in pipeline.
        uint32_t maximum_pipeline_waterflow;        // Variable for the maximum waterflow in pipeline.
        uint32_t minimal_pipeline_pressure;         // Variable for the minimal pressure in pipeline.
        uint32_t maximum_pipeline_pressure;         // Variable for the maximum pressure in pipeline.
        uint32_t minimal_rightTank_temperature;     // Variable for the minimal temperature of water in the right watertank.
        uint32_t maximum_rightTank_temperature;     // Variable for the maximum temperature of water in the right watertank.
        uint32_t minimal_pipeline_saturation;       // Variable for the minimal saturation of water in pipeline.
        uint32_t maximum_pipeline_saturation;       // Variable for the maximum saturation of water in pipeline.
        uint32_t minimal_pump_power;                // Variable for the minimal pump power.
        uint32_t maximum_pump_power;                // Variable for the maxmimum pump power.
        uint32_t minimal_lowerValve_flowrate;       // Variable for the minimal flow rate of the lower valve.
        uint32_t maximum_lowerValve_flowrate;       // Variable for the maximum flow rate of the lower valve.
    
    // ---------------- Methods ---------------------------------------------------------
    
    private:
    
        // This method reads the measured values from the shared memory and stores it into the data object.
        void readSharedMemory();
    
        // This method checks the data for their validity.
        bool dataCheck();
    
    
    public:
    
        // Constructor
        // Creates a DataFilter object with maximum and minimum values and the given data object.
        DataFilter(uint32_t minimal_leftTank_waterlevel_a, uint32_t maximum_leftTank_waterlevel_a, uint32_t minimal_pipeline_waterflow_a, uint32_t maximum_pipeline_waterflow_a, uint32_t minimal_pipeline_pressure_a, uint32_t maximum_pipeline_pressure_a, uint32_t minimal_rightTank_temperature_a, uint32_t maximum_rightTank_temperature_a, uint32_t minimal_pipeline_saturation_a, uint32_t maximum_pipeline_saturation_a, uint32_t minimal_pump_power_a, uint32_t maximum_pump_power_a, uint32_t minimal_lowerValve_flowrate_a, uint32_t maximum_lowerValve_flowrate_a, Data* data_a);
    
        // Destructor of the DataFilter.
        ~DataFilter();
    
        // This method performs the complete filter process.
        bool routine();
};


#endif /* DataFilter_hpp */
