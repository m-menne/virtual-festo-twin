/*
 * analog_in.cpp
 *
 *  Created on: Dec 11, 2017
 *      Author: tux
 */

#include "analog_in.h"

// Initialize pointer to required HW-addresses
uint32_t *MAXIM_CS = MAXIM_CS_Adr;
uint32_t *MAXIM_RD = MAXIM_RD_Adr;
uint32_t *MAXIM_SHDN = MAXIM_SHDN_Adr;
uint32_t *MAXIM_CONVST = MAXIM_CONVST_Adr;
uint32_t *MAXIM_WR = MAXIM_WR_Adr;
uint32_t *MAXIM_EOC = MAXIM_EOC_Adr;
int *MAXIM_DATA = MAXIM_DATA_Adr;

// Funtion to initialize the analog_in board
void init_analog_inputs() {
	// Set CS, RD & WR to high
	*MAXIM_CS = 1;
	*MAXIM_RD = 1;
	*MAXIM_WR = 1;

	// Set data
	*MAXIM_DATA = 0x8;

	// Set CS and WR to low
	*MAXIM_CS = 0;
	*MAXIM_WR = 0;
	// Set CS and WR to high
	*MAXIM_WR = 1;
	*MAXIM_CS = 1;

	// Set Conversion start to high
	*MAXIM_CONVST = 1;
}

// Function to read the data
// !!! The function samples 8500 measured values to graduate the values
// Parameters:
// *data_a 			- Array to store the data read from the board
void read_analog_inputs(uint32_t* data_a){

	// Set the amount of values which are used to sample the data
	uint32_t amount_of_values = 8500;

	uint32_t average_data[8] = { 0 };

	// Sample the data
	for(uint32_t range = 0; range < amount_of_values; ++range){
		// Start conversion
		*MAXIM_CONVST = 1;

		// Wait for the end of conversion
		while(*MAXIM_EOC);

		// Set CONVST and CS to low
		*MAXIM_CONVST = 0;
		*MAXIM_CS = 0;

		// Read all values
		for(uint32_t ChId = 0; ChId < 8; ++ChId){
			*MAXIM_RD = 0;
			average_data[ChId] += *MAXIM_DATA;
			*MAXIM_RD = 1;
		}

		// Set CS to high
		*MAXIM_CS = 1;
	}

	// Calculate the mean
	for(uint32_t i = 0; i < 8; ++i)
		data_a[i] = average_data[i]/amount_of_values;
}



