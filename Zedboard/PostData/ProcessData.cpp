// Implementation of the ProcessData class
#include "ProcessData.hpp"

// ---------------- Constructor for the pump --------------------------------------
// Intializes an object that process the data for the pump.
//
// It gets the values for the status and the power of the pump. It also sets component to 1, so the class
// can call the correct process for the pump.
//
// pump_status_a        Status of the pump. Either true or false.
// Type: bool
//
// pump_mode_a          Mode of the pump. False/0 = Binary or True/1 = Analog.
// Type: bool
//
// pump_power_a         Power of the pump. This is required only, if the pump should run in the analog mode.
// Type: float
ProcessData
::
ProcessData
(
 bool pump_status_a,
 bool pump_mode_a,
 float pump_power_a
 ):
component(1),
pump_status(pump_status_a),
pump_mode(pump_mode_a),
pump_power(pump_power_a)
{
}

// ------------------ Constructor for the lower valve -----------------------------
// Intializes an object that process the data for the lowervalve.
//
// It gets the value for the status of the lower valve. It also sets component to 3, so the class
// can process the request for the lower valve correctly.
//
// lowerValve_status:   The status of the lower valve. It represents the opening of the valve.
// Type: float
ProcessData
::
ProcessData
(
 float lowerValve_status_a
 ):
component(3),
lowerValve_status(lowerValve_status_a)
{
}

// ---------------- Constructor for the other actuators ---------------------------
// Initializes an object that either processes the value for the upper valve,
// the heater, the filter or the inflow based on the value of the given component.
//
// It gets the component and a status. Based on the component it intializes the object for the upper valve
// or the heater.
//
// comp_a               The given component. Either 2 for the upper valve or 4 for the heater.
// Type: uint32_t
//
// status_a             The status of the component. Either true or false.
// Type: bool
ProcessData
::
ProcessData
(
 uint32_t comp_a,
 bool status_a
 ):
component(comp_a)
{
    // If the upper valve (component 2) is addressed, this sets the value for updating the status of the upper valve.
    if(comp_a == 2){
        upperValve_status = status_a;
    }
    // If the heater (component 4) is addressed, this sets the value for updating the status of the heater.
    else if(comp_a == 4) {
        heater_status = status_a;
    }
    // If the filter (component 5) is addressed, this sets the value for updating the status of the filter.
    else if(comp_a == 5) {
        filter_status = status_a;
    }
    // If the inflow (component 6) is addressed, this sets the value for updating the status of the inflow.
    else if(comp_a == 6) {
        inflow_status = status_a;
    }
}

// ---------------- Destructor ----------------------------------------------------
ProcessData
::
~ProcessData
(
)
{
}

// ------------------ bool ProcessData () -----------------------------------------
// Checks if the data is plausible.
//
// The float values have to be between 0.0 and 1.0.
// If the data is correct the function returns true, if not the function returns false.
bool
ProcessData
::
dataOK()
{
    // Checks the received data based on the specified component.
    switch (component) {
            // Component: pump
        case 1:
            if (pump_power < minimal_pump_power || pump_power > maxmimum_pump_power) {
                return false;
            } else return true;
            break;
            
            // Component: upper valve
            // This always true because a bool is always either true or false.
        case 2:
            return true;
            break;
            
            // Component: lower valve
        case 3:
            if (lowerValve_status < minimal_lowerValve_status || lowerValve_status > maximum_lowerValve_status) {
                return false;
            } else return true;
            break;
            
            // Component: heater
            // This always true because a bool is always either true or false.
        case 4:
        case 5:
        case 6:
            return true;
            break;
            
            // Default case:
        default:
            return false;
            break;
    }
}

// ------------------ void convertData () -----------------------------------------
// Converts the received and checked data into the values required by the Festo-Machine.
//
// Based on the component the function does the procedure to convert the correct values.
void
ProcessData
::
convertData
(
)
{
    // Check which values have to converted depending on the specified component.
    switch (component) {
            // If the values are for the pump (component 1).
        case 1:
            // Calculate the pump_power.
            if(pump_mode)
                pump_power_festo = (uint32_t)((pump_power+0.06398252)*(25632*2));
            
            // If the status of the component is 3 (upper valve).
        case 3:
            // Calculate the status of the lowervalve.
            lowerValve_status_festo = (uint32_t)((lowerValve_status+0.06398252)*(25632*2));
            
            // Default case:
        default:
            break;
    }
}

// ------------------ void writeMemory() ------------------------------------------
// Writes the converted values in the shared memory based on the specified component.
void
ProcessData
::
writeMemory()
{
    // Initialize required variables for accessing the shared memory.
    uint32_t fd;                    // Filestream for /dev/mem.
    uint32_t fd_addr = 0x42000000;  // Base address of the shared memory.
    uint32_t *map;                  // Pointer to the base address of the memory.
    
    // Open /dev/mem for read and write access.
    fd = open("/dev/mem", O_RDWR);
    if (fd == -1) {
        perror("Error opening file for reading/writing");
        exit(EXIT_FAILURE);
    }
    
    // Map the shared memory.
    map = (uint32_t*)mmap(0, 2048*sizeof(uint32_t), PROT_READ | PROT_WRITE, MAP_SHARED, fd, fd_addr);
    if (map == MAP_FAILED) {
        close(fd);
        perror("Error mmapping the file");
        exit(EXIT_FAILURE);
    }
    
    switch (component) {
            // Write the received and converted data in the shared memory that corresponds with the pump.
        case 1:
        {
            // Turn the pump off, if either the minswitch or the pump status is 0.
            if(!((map[0] >> 3) & 0x01) || pump_status == 0){
                map[512] ^= (-0 ^ map[512]) & (1UL << 3);
                map[512] ^= (-0 ^ map[512]) & (1UL << 2);
            }
            // Otherwise turn the pump on.
            else{
                // Write the pump mode in the shared memory.
                map[512] ^= (-pump_mode ^ map[512]) & (1UL << 2);
                // If analog mode is chosen, write the new power to the shared memory.
                if(pump_mode) map[768] = pump_power_festo;
                // Write the pump status in the memory.
                map[512] ^= (-pump_status ^ map[512]) & (1UL << 3);
            }
            
            break;
        }
            
            // Write the received and converted data in the shared memory that corresponds with the upper valve.
        case 2:
        {
            // Write new upperValve_status to the shared memory.
            map[512] ^= (-upperValve_status ^ map[512]) & (1UL << 0);
            
            break;
        }
            
            // Write the received and converted data in the shared memory that corresponds with the lower valve.
        case 3:
        {
            // Write new lower valve status to the shared memory.
            map[768+1] = lowerValve_status_festo;
            
            // Set the lower valve only to active, if the input is larger than 0.009 to ensure
            // that at least the valve has an opening of one percent.
            if(lowerValve_status > 0.009){
                map[512] ^= (-1 ^ map[512]) & (1UL << 4);
            }
            // Otherwise deactivate the lower valve
            else{
                map[512] ^= (-0 ^ map[512]) & (1UL << 4);
            }
            
            break;
        }
            
            // Write the received and converted data in the shared memory that corresponds with the heater.
        case 4:
        {
            // Write new upperValve_status to the shared memory if the minswitch is 1 and the temperature is under 38 degrees.
            if(map[258] > 20500 || heater_status == 0 || map[256] > 10937)
                map[512] ^= (-heater_status ^ map[512]) & (1UL << 1);
            
            break;
        }
            // Write the received and converted data in the shared memory that corresponds with the filter.
        case 5:
        {
            // Write the new status of the filter to the shared memory.
            map[512] ^= (-filter_status ^ map[512]) & (1UL << 5);
            
            break;
        }
            // Write the received and converted data in the shared memory that corresponds with the inlfow.
        case 6:
        {
            // Write the new status of the inflow to the shared memory.
            map[512] ^= (-inflow_status ^ map[512]) & (1UL << 6);
            
            break;
        }
    }
    
    // Unmap the shared memory.
    if (munmap(map, 2048*sizeof(uint32_t)) == -1) {
        perror("Error un-mmapping the file");
    }
    // Close /dev/mem.
    close(fd);
}

// ------------------ void routine () ---------------------------------------------
// Performs the complete filter process and writes the data into the shared memory.
void
ProcessData
::
routine()
{
    // If the received data is ok, convert it to Festo data format and write it into the shared memory.
    if(dataOK()){
        // Convert the checked data.
        convertData();
        // Write converted data into the shared memory.
        writeMemory();
    }
}
