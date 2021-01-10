#include "VSPDataHandler.hpp"

// ---------------- Default constructor -------------------------------------------
VSPDataHandler
::
VSPDataHandler()
{
    sem_data_bin = sem_open(SEM_NAME.c_str(), O_RDWR);
    if (sem_data_bin == SEM_FAILED) {
#ifdef DEBUG        
        perror("sem_open(3) failed in VSPDataHandler-Constructor");
#endif //DEBUG
        exit(EXIT_FAILURE);
    }
}

// ---------------- Destructor ----------------------------------------------------
VSPDataHandler
::
~VSPDataHandler()
{
    if (sem_close(sem_data_bin) < 0) {
#ifdef DEBUG
        perror("sem_close(3) failed in VSPDataHandler-Destructor");
#endif //DEBUG
    }
}

// ------------------ void writeMemory(float saturation) --------------------------
// This method writes the value of the saturation into the shared memory.
void
VSPDataHandler
::
writeSaturation
(
 float saturation_a
 )
{
    // Initialize required variables for accessing the shared memory.
    uint32_t fd;                    // Filestream for /dev/mem.
    uint32_t fd_addr = 0x42000000;  // Base address of the shared memory.
    uint32_t *map;                  // Pointer to the base address of the memory.
    
    // Open /dev/mem for read and write access.
    fd = open("/dev/mem", O_RDWR);
    if (fd == -1) {
#ifdef DEBUG
        perror("Error opening file for reading/writing");
#endif //DEBUG
        exit(EXIT_FAILURE);
    }
    
    // Map the shared memory.
    map = (uint32_t*)mmap(0, 2048*sizeof(uint32_t), PROT_READ | PROT_WRITE, MAP_SHARED, fd, fd_addr);
    if (map == MAP_FAILED) {
        close(fd);
#ifdef DEBUG
        perror("Error mmapping the file");
#endif //DEBUG
        exit(EXIT_FAILURE);
    }
    
    // Write the received value of saturation into the shared memory.
    map[264] = (uint32_t)(saturation_a * 10000);
    
    // Unmap the shared memory.
    if (munmap(map, 2048*sizeof(uint32_t)) == -1) {
#ifdef DEBUG
        perror("Error un-mmapping the file");
#endif //DEBUG
    }
    
    // Close /dev/mem.
    close(fd);
    
}


// ------------------ void readData(std::vector<uint8_t> &data) -------------------
// This method reads the data values from the shared memory. It stores the values
// in the given vector.
void
VSPDataHandler
::
readData
(
 std::vector<uint8_t> &data_a
)
{
    // If the vector is not empty, clear the content of the vector.
    if(data_a.size() > 0) {
        data_a.clear();
    }
    
    if (sem_wait(sem_data_bin) < 0) {
#ifdef DEBUG
        std::cout << "readData could not get access to the file's semaphore!" << std::endl; 
#endif //DEBUG
        return;
    }

    // Open the binary file.
    std::ifstream inputFile;
    inputFile.open(bin_path, std::ios::in | std::ios::binary);
    
    if (inputFile) {
        // Get the length of the file
        inputFile.seekg(0, inputFile.end);
        uint32_t length = static_cast<uint32_t>(inputFile.tellg());
        inputFile.seekg(0, inputFile.beg);

#ifdef DEBUG
        std::cout << "\nBinary file has length: " << length << std::endl;
#endif //DEBUG

        // Read the data.
        char* input = new char[length];
        inputFile.read(input, length);
        
        // Store the data in the vector.
        for (uint32_t idx = 0; idx < length; ++idx) {
            data_a.push_back(input[idx]);
        }
        
        // Close the file.
        delete[] input;
        inputFile.close();

        if (sem_post(sem_data_bin) < 0) {

#ifdef DEBUG            
            std::cout << "readData could not release the file's semaphore!" << std::endl;
#endif //DEBUG
    }
    }
}

