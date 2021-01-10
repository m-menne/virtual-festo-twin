#include <iostream>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <stdlib.h>
#include <unistd.h>
#include <netdb.h>


int main()
{

    // Initialize variables. 
    int client;		// File descriptor to store the values returned by the socket system call and the accept system call.
    int portNum = 1500;

    int bufsize = 1024;
    char buffer[bufsize];

    char* ip = "127.0.0.1";

    struct sockaddr_in server_addr;

    // Establishing socket connection.
    client = socket(AF_INET, SOCK_STREAM, 0);
	
    if (client < 0) 
    {
        std::cout << "\nError establishing socket..." << std::endl;
        exit(1);
    }

    std::cout << "\nSocket client has been created..." << std::endl;
    
    server_addr.sin_family = AF_INET; // Contains code for the address family.
    server_addr.sin_port = htons(portNum); // Contains the port number.

    // Connect the socket.
    if (connect(client,(struct sockaddr *)&server_addr, sizeof(server_addr)) == 0)
        std::cout << "=> Connection to the server port number: " << portNum << std::endl;

    std::cout << "Awaiting confirmation from the server..." << std::endl;
    recv(client, buffer, bufsize, 0);
    std::cout << "Connection confirmed...\n"<<std::endl;

    for(int i = 0;i < 10;++i){

    // Receive the current data of the Festo Workstation.
    recv(client, buffer, bufsize, 0);
    std::cout << buffer[60] << std::endl;
    
    // Send the current data of the VSP.
    strcpy(buffer, "128909");
    send(client, buffer, bufsize, 0);

    }

    // Close connection.
    std::cout << "\nConnection terminated."<<std::endl;

    close(client);
    return 0;
}
