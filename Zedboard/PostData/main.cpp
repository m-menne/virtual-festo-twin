#include <iostream>
#include "ProcessData.hpp"

int main(int argc, const char * argv[]) {
    
    // Error Handling.
    if (argc < 2){
        std::cout<<"Wrong input parameters!"<<std::endl;
        return -1;
    }
    
    ProcessData *data;
    
    
    // Create a ProcessData object for the given component.
    // argv[0] - Component
    // 1       - Pump (bool status, bool mode, float power)
    // 2       - Upper valve (bool status)
    // 3       - Lower valve (float status)
    // 4       - Heater (bool status)
    // 5       - Filter (bool status)
    // 6       - Inflow (bool status)
    
    // Create the required object to control the given component.
    switch (std::atoi(argv[1])) {
        // Pump:
        case 1:
        {
            data = new ProcessData((bool)std::atoi(argv[2]), (bool)std::atoi(argv[3]), std::atof(argv[4]));
            break;
        }
        // Upper valve, heater, filter or inflow:
        case 2:
        case 4:
        case 5:
        case 6:
        {
            data = new ProcessData(std::atoi(argv[1]), (bool)std::atoi(argv[2]));
            break;
        }
        // Lower valve:
        case 3:
        {
            data = new ProcessData(std::atof(argv[2]));
            break;
        }
        // Default case:
        default:
            return -1;
            break;
    }
    
    // Perform the complete filter process and write the data into the shared memory.
    data->routine();
    
    return 0;
}

