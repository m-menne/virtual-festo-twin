#ifndef VSPDataHandler_hpp
#define VSPDataHandler_hpp

#include <iostream>
#include <string>
#include <vector>
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

class VSPDataHandler {
    
    // ---------------- Member variables ----------------------------------------------
private:
    float saturation;       // Variable for the saturation of the water.
    
    // Path of the binary file
    const std::string bin_path = "/home/ubuntu/software_zedboard/data.bin";
    const std::string SEM_NAME = "/sem_data_bin";
    sem_t *sem_data_bin;

    // ---------------- Methods -------------------------------------------------------
public:
    // Constructor of the VSPDataHandler.
    VSPDataHandler();
    
    // Destructor of the VSPDataHandler.
    ~VSPDataHandler();
    
    // Reads the data from the shared memory and stores the data in the given vector.
    void readData(std::vector<uint8_t> &data_a);
    
    // Writes the value of saturation in the shared memory between the OS and the MicroBlaze.
    void writeSaturation(float saturation_a);
};

#endif /* VSPDataHandler_hpp */

