#include <iostream>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <stdlib.h>
#include <unistd.h>
#include <netdb.h>
#include <cstdio>
#include <bitset>
#include <signal.h>
#include <cmath>

#include "festoImg.h"

static volatile sig_atomic_t got_signal = 0;

static void sig_handler_sigint(int signo) {
#ifdef DEBUG
	std::cout << "Signal handler called!" << std::endl;
#endif
	got_signal = 1;
}


int main()
{
    struct sigaction sa;

    // Register the signal handler for SIGINT (CTRL+C)
    memset(&sa, 0, sizeof(struct sigaction));
    sa.sa_handler = &sig_handler_sigint;
    if (sigaction(SIGINT, &sa, NULL) == -1) {
#ifdef DEBUG
    	perror("Problem registering signal handler!");
#endif
        return EXIT_FAILURE;
    }

    // Initialize variables.
    int client;		// File descriptor to store the values returned by the socket system call and the accept system call.
    int portNum = 1500;

    const int bufsize = 1024;
    char buffer[bufsize]={0};

    const char* ip = "192.168.25.25";

    while (true) {
		struct sockaddr_in server_addr;

		// Establishing socket connection.
		client =  socket(AF_INET, SOCK_STREAM, 0);

		if (client < 0)
		{
#ifdef DEBUG
			std::cout << "\nError establishing socket..." << std::endl;
#endif
			exit(1);
		}
#ifdef DEBUG
		std::cout << "\nSocket client has been created..." << std::endl;
#endif

		server_addr.sin_family = AF_INET; // Contains code for the address family.
		inet_pton(AF_INET, ip, &(server_addr.sin_addr));
		server_addr.sin_port = htons(portNum); // Contains the port number.

		// Connect the socket.
		if (connect(client,(struct sockaddr *)&server_addr, sizeof(server_addr)) == 0) {
#ifdef DEBUG
			std::cout << "=> Connection to the server port number: " << portNum << std::endl;
#endif
		}
		else {
#ifdef DEBUG
			std::cout << "Could not connect to server, retrying in 2 seconds!" << std::endl;
#endif
			usleep(2000000);
			if (got_signal == 0) {
#ifdef DEBUG
				std::cout << "No signal waiting!" << std::endl;
#endif
				continue;
			}
			else {
#ifdef DEBUG
				std::cout << "Received SIGINT from user! Terminating!" << std::endl;
#endif
				break;
			}
		}

#ifdef DEBUG
		std::cout << "Awaiting confirmation from the server..." << std::endl;
		std::cout << "Connection confirmed...\n"<<std::endl;
#endif
		while(true) {
			// Receive the current data of the Festo Workstation.
			auto no_of_Bytes = recv(client, buffer, 32, 0);
#ifdef DEBUG
			std::cout << "Received " << no_of_Bytes << " Bytes"<< std::endl;
#endif

			if (got_signal == 0) {
#ifdef DEBUG
				std::cout << "No signal waiting in inner loop!" << std::endl;
#endif
			}
			else {
#ifdef DEBUG
				std::cout << "Received SIGINT from user!" << std::endl;
#endif
				close(client);
				exit(0);
			}

			if ((no_of_Bytes != 32)) {
#ifdef DEBUG
				std::cout << "Error, Server doing weird things :-(" << std::endl;
#endif
				close(client);
				break;
			}

			// Read the float-values from the received data
//			// Pump power.
//			float Pump_power = 0.0f;
//			memcpy(&Pump_power, &buffer[5*4], sizeof(float));
//			// Flow rate of the lower valve.
//			float LowerValve_flowrate = 0.0f;
//			memcpy(&LowerValve_flowrate, &buffer[6*4], sizeof(float));
//			// Pressure in the pipeline.
//			float Pipeline_pressure = 0.0f;
//			memcpy(&Pipeline_pressure, &buffer[2*4], sizeof(float));
//			// Temperature in the right tank.
//			float RightTank_temperature = 0.0f;
//			memcpy(&RightTank_temperature, &buffer[3*4], sizeof(float));
//			// Saturation in the pipeline.

			// Waterlevel in the upper tank in percent
			float LeftTank_waterlevel = 0.0f;
			memcpy(&LeftTank_waterlevel, buffer, sizeof(float));

			// Water flow in the pipeline in l/min
			float Pipeline_waterflow = 0.0f;
			memcpy(&Pipeline_waterflow, &buffer[1*4], sizeof(float));

			// Saturation of the contamination in the pipeline in percent
			float Pipeline_saturation = 0.0f;
			memcpy(&Pipeline_saturation, &buffer[4*4], sizeof(float));

			// Digital values for the inputs and outputs
			uint32_t digitalValues = 0;
			memcpy(&digitalValues, &buffer[7*4], sizeof(uint32_t));

			// Convert the digital values from the int to single bool-vars
			bool UpperValve_status = digitalValues & (0x1 << 6);
			bool Heater_status = digitalValues & (0x1 << 15);
			bool Pump_status = digitalValues & (0x1 << 17);
			bool Filter_status = digitalValues & (0x1 << 19);
			bool Inflow_status = digitalValues & (0x1 << 20);

			// Update the values within the visualization
		    writeUpperValveState(UpperValve_status);
		    writePumpState(Pump_status);
		    writeHeaterState(Heater_status);
		    writeFilterState(Filter_status);
		    writeInflowState(Inflow_status);
		    writeTank1Level(1.0f - LeftTank_waterlevel);
		    writeTank2Level(LeftTank_waterlevel);
		    writeContaminationLevel(Pipeline_saturation);
		    writeFlowValue(Pipeline_waterflow);
		    printImage();

			// Calculate the current saturation
#ifdef DEBUG
		    std::cout << "Binary data: " << std::bitset<32>(digitalValues) << std::endl;
			std::cout << "Binary data: " << std::hex << digitalValues << std::dec << std::endl;
			std::cout << "Saturation received: " << Pipeline_saturation << std::endl;
#endif
			if (true == Inflow_status) {
#ifdef DEBUG
				std::cout << "Inflow active!\n";
#endif
				Pipeline_saturation += 0.01;
			}

			if (true == Filter_status) {
#ifdef DEBUG
				std::cout << "Filter active!\n";
#endif
				float factor = powf(0.98f, Pipeline_waterflow);
				Pipeline_saturation *= factor;
			}

			Pipeline_saturation = Pipeline_saturation <= 1.0 ? Pipeline_saturation : 1.0f;

			// Send the current data of the VSP.
#ifdef DEBUG
			std::cout << "Saturation: " << Pipeline_saturation << std::endl;
#endif
			no_of_Bytes = send(client, &Pipeline_saturation, sizeof(float), 0);
#ifdef DEBUG
			std::cout << "Sent " << no_of_Bytes << " Bytes" << std::endl;
#endif
		}
    }

    // Close connection.
#ifdef DEBUG
    std::cout << "\nConnection terminated."<<std::endl;
#endif
    return 0;
}

