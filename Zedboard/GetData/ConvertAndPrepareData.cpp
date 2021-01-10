// Implementation of the ConvertAndPrepareData class
#include "ConvertAndPrepareData.hpp"

// -------------- Constructor -------------------------------------------------------
// Initializes a ConvertAndPrepareData object.
//
// Data *data:                  A pointer to object that contains valid data measured 
//                              by the Festo-Machine.
// ErrorDetector *detective     A pointer to the ErrorDector object which is used by 
//                              the class to detect errors
ConvertAndPrepareData
::
ConvertAndPrepareData
(
 Data *data,
 ErrorDetector *detective
 ):
data(data),
detective(detective)
{
    /* We initialize the semaphore counter to 1 (INITIAL_VALUE) */
    sem_data_bin = sem_open(SEM_NAME.c_str(), O_CREAT | O_EXCL, SEM_PERMS, SEM_INITIAL_VALUE);

    if (sem_data_bin == SEM_FAILED) {
        perror("sem_open(3) error in ConvertAndPrepareData-Constructor");
        exit(EXIT_FAILURE);
    }
}

// -------------- Destructor --------------------------------------------------------
ConvertAndPrepareData::
~ConvertAndPrepareData
(
)
{
    if (sem_close(sem_data_bin) < 0)
        perror("sem_close(3) failed in ConvertAndPrepareData-Destructor");
}

//---------------- void writeData () ------------------------------------------------
// The method writes the JSON-String in a given txt.
void
ConvertAndPrepareData
::
writeData
(
)
{
    std::ofstream outputFile;
    // Open the specified file.
    outputFile.open(this->dest_path);
    // Write the string into the file.
    outputFile << this->JSONString;
    // Close the file.
    outputFile.close();
}

// --------------- void convertData () ----------------------------------------------
// This method converts the data to units required by the HoloLens.
//
// It takes the values from the Data object, calculates the correct values in corresponding
// units and save them again in the Data object.
void
ConvertAndPrepareData
::
convertData
(
)
{
    // Convert the value of the overflow sensor from Festo values to Hololens values.
    data->setRightTank_overflow(!data->getRightTank_overflow());
    
    // Convert the waterlevel of the left watertank from Festo values to percent.
    data->setLeftTank_waterlevel((uint32_t)data->getLeftTank_waterlevel() & 0xFFFFFFC0);
    data->setLeftTank_waterlevel((((-9.0)/30328)*data->getLeftTank_waterlevel()+9.52136639)/10.0);
    
    // Convert the waterflow in the pipeline from Festo values to litre/min.
    data->setPipeline_waterflow(((-3.3)/10539)*data->getPipeline_waterflow()+10.2578992);
    
    // Convert the pressure in the pipeline from Festo values to millibar.
    data->setPipeline_pressure(((((-3.6)/11814)*data->getPipeline_pressure()+9.97328593)/10)*400);
    
    // Convert the temperature of the water in right watertank from Festo values to degree centigrade.
    data->setRightTank_temperature(((-9.0)/2385)*data->getRightTank_temperature()+116.630189);
    
    // Convert the saturation of the water from VSP values to percent.
    data->setPipeline_saturation(data->getPipeline_saturation()/10000.0);
    
    // Convert the power of the pump from Festo values to percent.
    data->setPump_power(((0.5)/25632)*data->getPump_power()-0.06398252);
    
    // Convert the flow rate of the lower valve from Festo values to percent.
    data->setLowerValve_flowrate(((0.5)/25632)*data->getLowerValve_flowrate()-0.06398252);
    
    // If any value is negative, set it to zero.
    if(data->getLeftTank_waterlevel() < 0.0) data->setLeftTank_waterlevel(0.0);
    if(data->getPipeline_waterflow() < 0.0) data->setPipeline_waterflow(0.0);
    if(data->getPipeline_pressure() < 0.0) data->setPipeline_pressure(0.0);
    if(data->getRightTank_temperature() < 0.0) data->setRightTank_temperature(0.0);
    if(data->getPipeline_saturation() < 0.0) data->setPipeline_saturation(0.0);
    if(data->getPump_power() < 0.0) data->setPump_power(0.0);
    if(data->getLowerValve_flowrate() < 0.0) data->setLowerValve_flowrate(0.0);
}

// --------------- uint32_t monitoringUnit () ---------------------------------------
// This method observes the temperature and the water level in the right tank. The method
// handles three cases.
// If the water level is lower than the minswitch, it turns off the pump.
// If the water level is lower than 0.3, it turns off the pump.
// If the temperature is higher than 38 degree centigrade, it turns off the heater.
// In all three cases the function returns a warning after turning off the specified component.
// This warning persist a given time.
uint32_t
ConvertAndPrepareData
::
monitoringUnit
(
)
{
    // Warning Status - Definition
    // 0              - No Warning
    // 1              - Heater Warning
    // 2              - Pump Warning
    // 3              - Heater and Pump Warning
    
    // Variable that contains the warning.
    uint32_t warning = 0;
    
    // Check, whether the method has to turn off a component.
    if(!data->getRightTank_minswitch() || data->getRightTank_temperature() > 38.0 || data->getLeftTank_waterlevel() > 0.7){
        // If the heater has to be turned off.
        if(data->getHeater_status()){
            data->setHeater_status(0);
            // Set the warning to 1.
            warning = 1;
        }
        // Check, whether the minswitch is 0.
        if(!data->getRightTank_minswitch()){
            // If the pump is turned on, turn the pump off.
            if(data->getPump_status()){
                data->setPump_status(0);
                data->setPump_mode(0);
                // Increase the warning by 2 to get either the warning code 2 or warning code 3.
                warning += currentWarning + 2;
            }
        }
        
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
        map = (uint32_t*)mmap(0, 1*sizeof(uint32_t), PROT_READ | PROT_WRITE, MAP_SHARED, fd, fd_addr);
        if (map == MAP_FAILED) {
            close(fd);
            perror("Error mmapping the file");
            exit(EXIT_FAILURE);
        }
        
        // Writes the new values into the shared memory
        map[512] ^= (-data->getPump_status() ^ map[512]) & (1UL << 3);
        map[512] ^= (-data->getPump_mode() ^ map[512]) & (1UL << 2);
        map[512] ^= (-data->getHeater_status() ^ map[512]) & (1UL << 1);
        
        // Unmap the shared memory.
        if (munmap(map, 1*sizeof(uint32_t)) == -1) {
            perror("Error un-mmapping the file");
        }
        
        // Close /dev/mem.
        close(fd);
        
    }
    
    // The following code ensures that the warning persists for given time.
    // If there is no new warning and the counter is between 0 and the counter limit,
    // increment the counter and return the current warning.
    if((warning == 0 || warning == currentWarning) && (counter > 0 && counter < counterLimit)){
        ++counter;
        return currentWarning;
    }
    // If there is a new warning and the last warning was not 0, reset the counter (set the counter to 1),
    // set the currentwarning to the new warning and return that warning.
    else if(warning != 0){
        counter = 1;
        currentWarning = warning;
        return warning;
    }
    // If the counter is larger than its limit, set the counter to 0, set the currentwarning to the
    // new warning and return that warning.
    else{
        counter = 0;
        currentWarning = warning;
        return warning;
    }
}

//---------------- void createJSONString (uint32_t error_a, uint32_t warning_a) -----
// This method uses the given values and uses these values to create a string. The 
// string looks like a JSON-String.
// The JSON-String looks like this:
//     " {"Overflow":bool,"Floatswitch":bool,"Minswitch":bool,"MaxSwitch":bool,
//        "UpperValve":bool,"LeftTankWaterlevel":float,"RightTankWaterlevel":float,
//        "Waterflow":float,"Pressure":float,"Temperature":float,"Heater":bool,
//        "PumpMode":bool,"PumpStatus":bool,"LowerValve"bool,"PumpPower":float,
//        "LowerValveFlowrate":float,"ErrorCode":uint32_t,"WarningCode":uint32_t,
//        "Saturation":float,"Filter":bool,"Inflow":bool} "
void
ConvertAndPrepareData
::
createJSONString
(
 uint32_t error_a,
 uint32_t warning_a
)
{
    // Create a string to use it for the append.
    std::string dataForString;
    // Start the JSON string.
    JSONString = "{";
    
    // Add the status of the overflow sensor of the right watertank.
    dataForString = std::to_string(data->getRightTank_overflow());
    JSONString.append("\"Overflow\":");
    JSONString.append(dataForString);
    
    // Add the status of the float switch of the left watertank.
    dataForString = std::to_string(data->getLeftTank_floatswitch());
    JSONString.append(",\"FloatSwitch\":");
    JSONString.append(dataForString);
    
    // Add the status of the minimal switch of the right watertank.
    dataForString = std::to_string(data->getRightTank_minswitch());
    JSONString.append(",\"MinSwitch\":");
    JSONString.append(dataForString);
    
    // Add the status of the maxmimal switch of the right watertank.
    dataForString = std::to_string(data->getRightTank_maxswitch());
    JSONString.append(",\"MaxSwitch\":");
    JSONString.append(dataForString);
    
    // Add the status of the upper valve.
    dataForString = std::to_string(data->getUpperValve_open());
    JSONString.append(",\"UpperValve\":");
    JSONString.append(dataForString);
    
    // Add the waterlevel of the left watertank.
    dataForString = std::to_string(data->getLeftTank_waterlevel());
    JSONString.append(",\"LeftTankWaterlevel\":");
    JSONString.append(dataForString);
    
    // Add the waterlevel of the right watertank.
    dataForString = std::to_string(1.0-data->getLeftTank_waterlevel());
    JSONString.append(",\"RightTankWaterlevel\":");
    JSONString.append(dataForString);
    
    // Add the waterflow in pipeline.
    dataForString = std::to_string(data->getPipeline_waterflow());
    JSONString.append(",\"Waterflow\":");
    JSONString.append(dataForString);
    
    // Add the pressure in pipeline.
    dataForString = std::to_string(data->getPipeline_pressure());
    JSONString.append(",\"Pressure\":");
    JSONString.append(dataForString);
    
    // Add the temperature of the water in the right watertank.
    dataForString = std::to_string(data->getRightTank_temperature());
    JSONString.append(",\"Temperature\":");
    JSONString.append(dataForString);
    
    // Add the status of the heater.
    dataForString = std::to_string(data->getHeater_status());
    JSONString.append(",\"Heater\":");
    JSONString.append(dataForString);
    
    // Add the mode of the pump.
    dataForString = std::to_string(data->getPump_mode());
    JSONString.append(",\"PumpMode\":");
    JSONString.append(dataForString);
    
    // Add the status of the pump.
    dataForString = std::to_string(data->getPump_status());
    JSONString.append(",\"PumpStatus\":");
    JSONString.append(dataForString);
    
    // Add the status of the lower valve.
    dataForString = std::to_string(data->getLowerValve_status());
    JSONString.append(",\"LowerValve\":");
    JSONString.append(dataForString);
    
    // Add the power of the pump.
    dataForString = std::to_string(data->getPump_power());
    JSONString.append(",\"PumpPower\":");
    JSONString.append(dataForString);
    
    // Add the flowrate of the lower valve.
    dataForString = std::to_string(data->getLowerValve_flowrate());
    JSONString.append(",\"LowerValveFlowrate\":");
    JSONString.append(dataForString);
    
    // Add the errorcode.
    dataForString = std::to_string(error_a);
    JSONString.append(",\"ErrorCode\":");
    JSONString.append(dataForString);
    
    // Add the warningcode.
    dataForString = std::to_string(warning_a);
    JSONString.append(",\"WarningCode\":");
    JSONString.append(dataForString);
    
    // Add the saturation of the water.
    dataForString = std::to_string(data->getPipeline_saturation());
    JSONString.append(",\"Saturation\":");
    JSONString.append(dataForString);
    
    // Add the status of the filter.
    dataForString = std::to_string(data->getFilter_status());
    JSONString.append(",\"Filter\":");
    JSONString.append(dataForString);
    
    // Add the status of the inflow.
    dataForString = std::to_string(data->getInflow_status());
    JSONString.append(",\"Inflow\":");
    JSONString.append(dataForString);
    
    // Complete the JSON string.
    JSONString.append("}");
}


//---------------- void writeBinaryData() -------------------------------------------
// This methods writes the converted data into a binary file. First of all the method
// writes all analog values into the file. Then it prepares the digital values and 
// writes them also into the binary file.
void
ConvertAndPrepareData
::
writeBinaryData
(
)
{
    if (sem_wait(sem_data_bin) < 0) {
        std::cout << "writeBinaryData could not get access to the file's semaphore!" << std::endl; 
        return;
    }

    // Prepare the output file.
    std::ofstream outFile (bin_path, std::ios::binary | std::ios::out);

    // Write all anlog values.
    // Left tank water level.
    writeToBinaryFile(data->getLeftTank_waterlevel(), outFile);
    // Water flow in the pipeline.
    writeToBinaryFile(data->getPipeline_waterflow(), outFile);
    // Pressure in the pipeline.
    writeToBinaryFile(data->getPipeline_pressure(), outFile);
    // Temperature in the right tank.
    writeToBinaryFile(data->getRightTank_temperature(), outFile);
    // Saturation in the pipeline.
    writeToBinaryFile(data->getPipeline_saturation(), outFile);
    // Pump power.
    writeToBinaryFile(data->getPump_power(), outFile);
    // Flow rate of the lower valve.
    writeToBinaryFile(data->getLowerValve_flowrate(), outFile);
    
    
    // Prepare the digital values.
    uint32_t digitalValues = 0;
    
    // Digital inputs.
    // Right tank overflow.
    digitalValues |= data->getRightTank_overflow() ? (0x01 << 1) : 0x00;
    // Left tank floatswitch.
    digitalValues |= data->getLeftTank_floatswitch() ? (0x01 << 2) : 0x00;
    // Right tank minswitch.
    digitalValues |= data->getRightTank_minswitch() ? (0x01 << 3) : 0x00;
    // Right tank maxswitch.
    digitalValues |= data->getRightTank_maxswitch() ? (0x01 << 4) : 0x00;
    // Upper valve closed value.
    digitalValues |= data->getUpperValve_closed() ? (0x01 << 5) : 0x00;
    // Upper valve open value.
    digitalValues |= data->getUpperValve_open() ? (0x01 << 6) : 0x00;
    
    // Digital outputs
    // Status of the heater.
    digitalValues |= data->getHeater_status() ? (0x01 << 15) : 0x00;
    // Pump mode.
    digitalValues |= data->getPump_mode() ? (0x01 << 16) : 0x00;
    // Status of the pump.
    digitalValues |= data->getPump_status() ? (0x01 << 17) : 0x00;
    // Status of the lower valve.
    digitalValues |= data->getLowerValve_status() ? ( 0x01 << 18) : 0x00;
    // Status of the filter.
    digitalValues |= data->getFilter_status() ? (0x01 << 19) : 0x00;
    // Status of the inflow.
    digitalValues |= data->getInflow_status() ? (0x01 << 20) : 0x00;
    
    // Write the digital values into the given file.
    writeToBinaryFile(digitalValues, outFile);
    outFile.close();

    if (sem_post(sem_data_bin) < 0) {
        std::cout << "writeBinaryData could not release the file's semaphore!" << std::endl;
    }

}


//---------------- void writeToBinaryFile(T inData, std::ofstream &outStream) -------
// This methods writes a single value in a given file stream.
template <typename T>
void 
ConvertAndPrepareData
::
writeToBinaryFile
(
T inData,
std::ofstream &outStream
)
{
    outStream.write(((char*)&inData), sizeof(T));
}

//---------------- void runRoutine (bool dataIsCorrect_a) ---------------------------
// This methods executes the routine processing the data of the Festo-Machine.
//
// First of all the function calculate the values the HoloLens needs to show the data.
// Then it creates the JSON-String and at least writes this string in a given txt.
int
ConvertAndPrepareData
::
runRoutine
(
 bool dataIsCorrect_a
 )
{
    // Proceed if the data is correct.
    if (dataIsCorrect_a) {
        // Convert the data and aproximate the waterlevel of the right tank.
        convertData();
        // Monitorung the data.
        uint32_t warning = monitoringUnit();
        // Update the log of the ErrorDetector.
        detective->update_log(data->getLeftTank_waterlevel(), data->getUpperValve_open());
        // Check for errors.
        uint32_t error = detective->detect_errors(data->getLeftTank_waterlevel(), data->getPump_status());
        // Create the JSON-String
        createJSONString(error, warning);
        // Write the data.
        writeData();
        writeBinaryData();
        // Return 0 if the operations succeded.
        return 0;
    }
    // In case the data is not correct skip the provision.
    else
        return -1;
}
