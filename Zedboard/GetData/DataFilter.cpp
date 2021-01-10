// Implementation of the DataFilter class
#include "DataFilter.hpp"

// ---------------- Constructor of the DataFilter -----------------------------------
// Creates a DataFilter object with maximum and minimum values and the given data object.
//
// minimal_leftTank_waterlevel_a        Minimal and maximal water level of the left water tank.
// maximum_leftTank_waterlevel_a
// Type: uint32_t
//
// minimal_pipeline_waterflow_a         Minimal and maximal water flow in the pipeline.
// maximum_pipeline_waterflow_a
// Type: uint32_t
//
// minimal_pipeline_pressure_a          Minimal and maximal water pressure in the pipeline.
// maximum_pipeline_pressure_a
// Type: uint32_t
//
// minimal_rightTank_temperature_a      Minimal and maximal temperature of the water in the right water tank.
// maximal_rightTank_temperature_a
// Type: uint32_t
//
// minimal_pipeline_saturation_a        Minimal and maximal saturation of the water in the pipeline.
// maximum_pipeline_saturation_a
// Type: uint32_t
//
// minimal_pump_power_a                 Minimal and maximal power of the pump.
// maximal_pump_power_a
// Type: uint32_t
//
// minimal_lowerValve_flowrate_a        Minimal and maximal flow rate of the lower valve.
// maximal_lowerValve_flowrate_a
// Type: uint32_t
//
// data_a                               Given data object in which the data will be stored.
// Type: Data
DataFilter
::
DataFilter(uint32_t minimal_leftTank_waterlevel_a, uint32_t maximum_leftTank_waterlevel_a, uint32_t minimal_pipeline_waterflow_a, uint32_t maximum_pipeline_waterflow_a, uint32_t minimal_pipeline_pressure_a, uint32_t maximum_pipeline_pressure_a, uint32_t minimal_rightTank_temperature_a, uint32_t maximum_rightTank_temperature_a, uint32_t minimal_pipeline_saturation_a, uint32_t maximum_pipeline_saturation_a, uint32_t minimal_pump_power_a, uint32_t maximum_pump_power_a, uint32_t minimal_lowerValve_flowrate_a, uint32_t maximum_lowerValve_flowrate_a, Data* data_a)
:
minimal_leftTank_waterlevel(minimal_leftTank_waterlevel_a),
maximum_leftTank_waterlevel(maximum_leftTank_waterlevel_a),
minimal_pipeline_waterflow(minimal_pipeline_waterflow_a),
maximum_pipeline_waterflow(maximum_pipeline_waterflow_a),
minimal_pipeline_pressure(minimal_pipeline_pressure_a),
maximum_pipeline_pressure(maximum_pipeline_pressure_a),
minimal_rightTank_temperature(minimal_rightTank_temperature_a),
maximum_rightTank_temperature(maximum_rightTank_temperature_a),
minimal_pipeline_saturation(minimal_pipeline_saturation_a),
maximum_pipeline_saturation(maximum_pipeline_saturation_a),
minimal_pump_power(minimal_pump_power_a),
maximum_pump_power(maximum_pump_power_a),
minimal_lowerValve_flowrate(minimal_lowerValve_flowrate_a),
maximum_lowerValve_flowrate(maximum_lowerValve_flowrate_a),
data(data_a)
{
}

// ---------------- Destructor ------------------------------------------------------
DataFilter
::
~DataFilter()
{
}

// ---------------- void readSharedMemory() -----------------------------------------
// Reads the values of the shared memory and stores it into the data object
void
DataFilter
::
readSharedMemory(){
    
    // Declare arrays and variables to store the values read from the shared memory.
    uint32_t digital_in;            // Variable for the digital input values.
    uint32_t analog_in[5];          // Array for the analog input values.
    uint32_t digital_out;           // Variable for the digital output values.
    uint32_t analog_out[2];         // Array for the analog output values.
    
    // Initialize required variables for accessing the shared memory.
    uint32_t fd;                    // Filestream for /dev/mem.
    uint32_t fd_addr = 0x42000000;  // Base address of the shared memory.
    uint32_t *map;                  // Pointer to the base address of the memory.
    
    // Open /dev/mem for read and write access.
    fd = open("/dev/mem", O_RDONLY);
    if (fd == -1) {
        perror("Error opening file for reading/writing");
        exit(EXIT_FAILURE);
    }
    
    // Map the shared memory.
    map = (uint32_t*)mmap(0, 2048*sizeof(uint32_t), PROT_READ, MAP_SHARED, fd, fd_addr);
    if (map == MAP_FAILED) {
        close(fd);
        perror("Error mmapping the file");
        exit(EXIT_FAILURE);
    }
    
    // Read the digital input values from the shared memory.
    digital_in = map[0];
    
    // Read the analog input values from the shared memory.
    for(uint32_t i = 0; i < 8; ++i){
        if(i%2 == 0) analog_in[i/2] = map[256+i];
    }

    // Read the saturation value from the shared memory.
    analog_in[4] = map[264]; 
    
    // Read the digital output values from the shared memory.
    digital_out = map[512];
    
    // Read the analog output values from the shared memory.
    for(uint32_t i = 0; i < 2; ++i){
        analog_out[i] = map[768+i];
    }
    
    // Unmap the shared memory.
    if (munmap(map, 2048*sizeof(uint32_t)) == -1) {
        perror("Error un-mmapping the file");
    }
    
    // Close /dev/mem.
    close(fd);
    
    
    // Sets the absolute maximal water level of the right water tank.
    data->setRightTank_overflow(((digital_in >> 1) & 0x01));
    // Sets the status of the float switch of the left water tank.
    data->setLeftTank_floatswitch(((digital_in >> 2) & 0x01));
    // Sets the minimal water level of the right water tank.
    data->setRightTank_minswitch(((digital_in >> 3) & 0x01));
    // Sets the maximal water level of the right water tank.
    data->setRightTank_maxswitch(((digital_in >> 4) & 0x01));
    // Sets the status of the upper valve.
    data->setUpperValve_closed(((digital_in >> 5) & 0x01));
    data->setUpperValve_open(((digital_in >> 6) & 0x01));
    
    // Sets the water level of the left water tank.
    data->setLeftTank_waterlevel(analog_in[0]);
    // Sets the water flow in pipeline.
    data->setPipeline_waterflow(analog_in[2]);
    // Sets the pressure in pipeline.
    data->setPipeline_pressure(analog_in[3]);
    // Sets the temperature of water in the right water tank.
    data->setRightTank_temperature(analog_in[1]);
    // Sets the saturation of water in pipeline.
    data->setPipeline_saturation(analog_in[4]);
    
    // Sets the heater status.
    data->setHeater_status(((digital_out >> 1) & 0x01));
    // Sets the mode of the pump.
    data->setPump_mode(((digital_out >> 2) & 0x01));
    // Sets the status of the pump.
    data->setPump_status(((digital_out >> 3) & 0x01));
    // Sets the status of the lower valve.
    data->setLowerValve_status(((digital_out >> 4) & 0x01));
    // Sets the status of the filter.
    data->setFilter_status(((digital_out >> 5) & 0x01));
    // Sets the status of the inflow.
    data->setInflow_status(((digital_out >> 6) & 0x01));
    
    // Sets the power of the pump.
    data->setPump_power(analog_out[0]);
    // Sets the flow rate of the lower valve.
    data->setLowerValve_flowrate(analog_out[1]);
}

// ---------------- bool dataCheck() ------------------------------------------------
// This method checks if the measured data is valid. This is fulfilled if the data lies in the value range.
// If the data is correct the function returns true, if not the function returns false.
bool
DataFilter
::
dataCheck(){
    
    // Checks the measured water level.
    if (data->getLeftTank_waterlevel() < minimal_leftTank_waterlevel || data->getLeftTank_waterlevel() > maximum_leftTank_waterlevel) {
	return false;
    }
    
    // Checks the measured water flow in the pipeline.
    else if (data->getPipeline_waterflow() < minimal_pipeline_waterflow || data->getPipeline_waterflow() > maximum_pipeline_waterflow) {
	return false;
    }
    
    // Checks the measured pressure in the pipeline.
    else if (data->getPipeline_pressure() < minimal_pipeline_pressure || data->getPipeline_pressure() > maximum_pipeline_pressure) {
	return false;
    }
    
    // Checks the measured temperature of the water in the right water tank.
    else if (data->getRightTank_temperature() < minimal_rightTank_temperature || data->getRightTank_temperature() > maximum_rightTank_temperature) {
	return false;
    }
    
    // Checks the measured pump power.
    else if (data->getPump_power() < minimal_pump_power || data->getPump_power() > maximum_pump_power) {
	return false;
    }
    
    // Checks the measured flow rate of the lower valve.
    else if (data->getLowerValve_flowrate() < minimal_lowerValve_flowrate || data->getLowerValve_flowrate() > maximum_lowerValve_flowrate) {
	return false;
    }
    
    // Checks the measured saturation of the water.
    else if (data->getPipeline_saturation() < minimal_pipeline_saturation || data->getPipeline_saturation() > maximum_pipeline_saturation) {
    return false;
    }
    
    // If all inspections passed, the validity of the data is proven and the method returns true.
    else return true;
}

// ---------------- bool routine() --------------------------------------------------
// This method performs the complete filter process and returns true if the data is valid.
bool
DataFilter
::
routine(){
    readSharedMemory();
    return dataCheck();
}
