// Class: ConvertAndPrepareData
// This class gets clean and cleared up data from dataFilter. It converts the data in the format required
// by the HoloLens and creates by using the data a string. This string looks like a JSON-String and is 
// ready  to be send to the HoloLens using a HTTP-Request.


#ifndef ConvertAndPrepareData_hpp
#define ConvertAndPrepareData_hpp

#include "Data.hpp"
#include <string>
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
#include <semaphore.h>


#include "ErrorDetector.hpp"


class ConvertAndPrepareData
{
    // --------------------- Member variables -------------------------------------------
    
    private:
    
        // Data Object that contains the measured Data by the Festo-Machine.
        Data *data;
        // ErrorDetector Object
        ErrorDetector *detective;
    
        // JSON string
        std::string JSONString = " ";
        // Path of the txt the object should write.
        std::string dest_path = "/home/ubuntu/software_zedboard/json_data.txt";
        // Path of the binary file
        std::string bin_path = "/home/ubuntu/software_zedboard/data.bin";
    
        // Variables used to store the warning for a given time.
        uint32_t counter = 0;
        uint32_t counterLimit = 500;
        uint32_t currentWarning = 0;
        sem_t *sem_data_bin;

        const std::string SEM_NAME = "/sem_data_bin";
        const int SEM_PERMS = (S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP);
        const int SEM_INITIAL_VALUE = 1;
    
    // --------------------- Methods ----------------------------------------------------
    
    private:
    
        // Writes the JSON-String in a txt-file.
        void writeData();
        // Converts the data.
        void convertData();
        // Monitors the binary output signals.
        uint32_t monitoringUnit();
        // Creates a string in the JSON string with the given data.
        void createJSONString(uint32_t error_a, uint32_t warning_a);
        // Writes the data to a given binary file.
        void writeBinaryData();
        // Writes a single value in a given file stream.
        template <typename T> void writeToBinaryFile(T inData, std::ofstream &outStream);
        
    public:
    
        // Constructor.
        ConvertAndPrepareData(Data *data, ErrorDetector *detective);
        // Destructor.
        ~ConvertAndPrepareData();
    
        // Function running the complete routine.
        int runRoutine(bool dataIsCorrect_a);
};

#endif /* ConvertAndPrepareData_hpp */
