/*
 * Empty C++ Application
 */
#include <cstdlib>

#include "digital_out.h"
#include "digital_in.h"
#include "analog_out.h"
#include "analog_in.h"

// Function to write digital and analog data measured by the input boards into the shared memory
// and read the data written in the shared memory for the analog and digital output signals
// Parameters:
// digital_in_a 	- Byte of digital input values
// analog_in_a 		- Array of analog input values
// digital_out_a	- Byte of digital output values
// analog_out_a		- Array of analog output values
void memory_update(uint32_t &digital_in_a, uint32_t* analog_in_a, uint32_t &digital_out_a, uint32_t* analog_out_a){

	// Pointer that points to the start address of the shared memory
	uint32_t* bram_ptr = ((uint32_t*)(0x42000000));

	// Start addresses of the different values
	uint32_t analog_in_start = 256;
	uint32_t digital_out_start = 512;
	uint32_t analog_out_start = 768;

	// Write measured data from the board into the shared memory
	// Digital input values
	*bram_ptr = digital_in_a;
	// Analog input values
	for(int index = 0; index < 8; ++index){
		bram_ptr[index+analog_in_start] = analog_in_a[index];
	}

	// Read data from the shared memory written by the OS
	// Digital output values
	digital_out_a = bram_ptr[digital_out_start];
	// Analog output values
	for(int index = 0; index < 4; ++index){
		analog_out_a[index] = bram_ptr[index+analog_out_start];
	}

}

int main() {

	// Call init routines of the I/O-boards
	init_digital_outputs();
	init_digital_inputs();
	init_analog_outputs();
	init_analog_inputs();

	// Initializes the local variables for the I/O data
	uint32_t digital_in = 0;
	uint32_t analog_in[8] = { 0 };
	uint32_t digital_out = 0;
	uint32_t analog_out[4] = { 0 };

	// Variables for the timer
	uint32_t* clock = ((uint32_t*)(0x41C00000));
	uint32_t time = 0;

	while(true) {

		// Read Data
		digital_in = read_data();
		//*clock = 0x80; // start timer
		read_analog_inputs(analog_in);
		//*clock = 0x00; // stop timer
		//time = clock[2]; // safe timer

		// Update data
		memory_update(digital_in, analog_in, digital_out, analog_out);

		// Write Data
		write_digital_outputs(digital_out);
		write_analog_outputs(analog_out);

	}

	return EXIT_SUCCESS;
}

