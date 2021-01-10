#include <iostream>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <stdlib.h>
#include <unistd.h>
#include <vector>
#include <bitset>
#include <signal.h>

#include "VSPDataHandler.hpp"

#define DEBUG 1

static volatile sig_atomic_t got_sig_int = 0;
static volatile sig_atomic_t got_sig_pipe = 0;

static void sig_handler_sigint(int signo) {
#ifdef DEBUG
    std::cout << "Signal handler called! (SIGINT)" << std::endl;
#endif //DEBUG
    got_sig_int = 1;
}

static void sig_handler_sigterm(int signo) {
#ifdef DEBUG
    std::cout << "Signal handler called! (SIGTERM)" << std::endl;
#endif //DEBUG
    got_sig_int = 1;
}

// Handle the SIGPIPE signal when sending fails
static void sig_handler_sigpipe(int signo) {
#ifdef DEBUG
    std::cout << "Signal handler called! (SIGPIPE)" << std::endl;
#endif //DEBUG
    got_sig_pipe = 1;
}

int main()
{
        // Register the signal handler for SIGPIPE
        struct sigaction sa_pipe;
        memset(&sa_pipe, 0, sizeof(struct sigaction));
        sa_pipe.sa_handler = &sig_handler_sigpipe;
        if (sigaction(SIGPIPE, &sa_pipe, NULL) == -1) {
#ifdef DEBUG
            perror("Problem registering signal handler! (SIGPIPE");
#endif //DEBUG
            return EXIT_FAILURE;
        }

    VSPDataHandler DataHandler;
    std::vector<uint8_t> data;
    
    // Initialize variables.
    int client_socket, server_socket;        // File descriptors to store the values returned by the socket system call and the accept system call.
    int portNum = 1500;
    
    constexpr int bufsize = 1024;
    char buffer[bufsize];
    int time = 500000;
    
    struct sockaddr_in server_addr;
    socklen_t size;
    auto retVal = -1;
    do {
        // Establishing socket connection.
        server_socket = socket(AF_INET, SOCK_STREAM, 0);
        
        if (server_socket < 0) {
#ifdef DEBUG
            std::cout << "\nError establishing socket..." << std::endl;
#endif //DEBUG
            exit(EXIT_FAILURE);
        }

#ifdef DEBUG
        std::cout << "\nSocket server has been created..." << std::endl;
#endif //DEBUG        
        server_addr.sin_family = AF_INET; // Contains code for the address family.
        server_addr.sin_addr.s_addr = htons(INADDR_ANY); // Contains the ip address.
        server_addr.sin_port = htons(portNum); // Contains the port number.
        
        // Binding the socket.
        retVal = bind(server_socket, (struct sockaddr*)&server_addr, sizeof(server_addr));
        if (retVal < 0) { 
#ifdef DEBUG
            std::cout << "Error binding connection, the socket has already been established..." << std::endl;
#endif //DEBUG
            close(server_socket);
#ifdef DEBUG
            std::cout << "Socket closed. Retrying to bind..." << std::endl;
#endif //DEBUG            
            usleep(2000000);
        }
    } while (retVal < 0);

#ifdef DEBUG
    std::cout << "Looking for clients..." << std::endl;
#endif //DEBUG
    do {
        // Listening call allows the process to listen on the socket for connections.
        listen(server_socket, 1);
        
        size = sizeof(server_addr);
        client_socket = accept(server_socket, (struct sockaddr *)&server_addr, &size);
        
        struct sockaddr_in client_addr;
        socklen_t addr_size = sizeof(struct sockaddr_in);
        int res = getpeername(client_socket, (struct sockaddr *)&client_addr, &addr_size);
        char *clientip = new char[20];
        strcpy(clientip, inet_ntoa(client_addr.sin_addr));

#ifdef DEBUG
        std::cout << "Connected with the client on VSP at address " << clientip << std::endl;
        std::cout << std::endl;
#endif //DEBUG

        // Register the signal handler for SIGINT (CTRL+C)
        struct sigaction sa;
        memset(&sa, 0, sizeof(struct sigaction));
        sa.sa_handler = &sig_handler_sigint;
        if (sigaction(SIGINT, &sa, NULL) == -1) {
#ifdef DEBUG
            perror("Problem registering signal handler!");
#endif //DEBUG
            return EXIT_FAILURE;
        }

        // Register the signal handler for SIGTERM
        struct sigaction sa_term;
        memset(&sa_term, 0, sizeof(struct sigaction));
        sa_term.sa_handler = &sig_handler_sigterm;
        if (sigaction(SIGTERM, &sa_term, NULL) == -1) {
#ifdef DEBUG
            perror("Problem registering signal handler!");
#endif //DEBUG
            return EXIT_FAILURE;
        }

        while  (got_sig_int == 0) 
        {   
            // Control the cycle time
            usleep(time);

            // Read the current data from the binary file.
            DataHandler.readData(data);
            
            // Sometimes no data is returned, in this case -> Try again ;-)
            if (data.size() == 0) continue;

            // Prepare the data to send it.
#ifdef DEBUG
            std::cout << "Data to be sent\n";
#endif //DEBUG
            for(uint32_t idx = 0; idx < data.size(); ++idx){
                buffer[idx] = data[idx];
                if (idx % 4 == 0 && idx != 0) std::cout << " | ";
#ifdef DEBUG
                std::cout << std::hex << static_cast<int>(data[idx]) << " ";
#endif //DEBUG
            }
#ifdef DEBUG
            std::cout << std::dec << std::endl;

            std::cout << "Sending " << data.size() << " Bytes of data\n";
#endif //DEBUG
                        
            // Send the data.
            auto sentBytes = send(client_socket, buffer, data.size(), 0);
#ifdef DEBUG
            std::cout << sentBytes << " Bytes of Data have been sent" << std::endl;
#endif //DEBUG
            if (sentBytes < 0) {
#ifdef DEBUG
                std::cout << "Socket error, not able to send data to client!" << std::endl;
#endif //DEBUG                
                break;
            }

            // Receive the current data of the VSP.
            auto recvBytes = recv(client_socket, buffer, 4, 0);
#ifdef DEBUG
            std::cout << recvBytes << " Bytes of Data have been received" << std::endl;
#endif //DEBUG
            
            // Write the measured saturation to the shared memory.
#ifdef DEBUG
            std::cout << "Writing the value " << *((float*)(buffer)) << " to memory" << std::endl;
#endif //DEBUG
            DataHandler.writeSaturation(*((float*)(buffer)));
            
        }
        
        // Close connection.
#ifdef DEBUG
        std::cout << "\n\nConnection terminated with IP " << inet_ntoa(server_addr.sin_addr) << std::endl;
#endif //DEBUG

        close(client_socket);
    } while  (got_sig_int == 0);

    close(server_socket);
    
    return 0;
}
