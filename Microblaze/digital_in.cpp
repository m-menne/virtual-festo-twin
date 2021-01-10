/*
 * digital_in.cpp
 *
 *  Created on: Dec 11, 2017
 *      Author: tux
 */

#include "digital_in.h"

void init_digital_inputs()
{
	// Set pointers to the direction register pf PMOD B and C
	// 1 - Input
	// 0 - Output
	volatile uint32_t *data_dir = PMOD_B_BASE + 1;
	volatile uint32_t *ctrl_dir = PMOD_C_BASE + 1;
	// Set pointer to the data register of PMOD_C
	volatile uint32_t *ctrl = PMOD_C_BASE;

	// Set PMOD_B to input mode
	*data_dir = 0x000000ff;
	// Set PMOD_C to output mode
	*ctrl_dir = 0x00000000;

	// Set the required control signals
	*ctrl = nCS | nWR | nRD;
}


uint32_t read_digital_inputs(uint32_t reg)
{
	// Set pointers to the data registers of PMOD B and C
	volatile uint32_t *data = PMOD_B_BASE;
	volatile uint32_t *ctrl = PMOD_C_BASE;
	// Set a pointer to the direction register of PMOD B
	volatile uint32_t *data_dir = PMOD_B_BASE + 1;

	// Variable to store the data word read from the digital_in board
	uint32_t reg_data;

	// Set the control signals
	*ctrl &= ~nCS;
	*ctrl |= ALE;
	// Change direction of PMOD B to write and write reg
	*data_dir = 0x00;
	*data = reg;
	*ctrl &= ~ALE;
	// Change direction of PMOD B to read and read the data
	*data_dir = 0xff;
	*ctrl &= ~nRD;
	reg_data = *data;
	// Reset the control signals
	*ctrl |= nRD;
	*ctrl |= nCS;

	return reg_data;
}

// This fuction is only for testing and not further used
/*
void write_digital_inputs (uint32_t reg, uint32_t reg_data)
{
	volatile uint32_t *data = PMOD_B_BASE;
	volatile uint32_t *ctrl = PMOD_C_BASE;
	volatile uint32_t *data_dir = PMOD_B_BASE + 1;

	*ctrl &= ~nCS;
	*ctrl |= ALE;
	*data_dir = 0x00;
	*data = reg;
	*ctrl &= ~ALE;
	*ctrl &= ~nWR;
	*data = reg_data;
	*ctrl |= nWR;
	*data_dir = 0xff;
	*ctrl |= nCS;
}
*/

uint32_t read_data() {
	// Only INPDATA and the read_digital_inputs are required to read the data
	// converted by the digital_in board

	//uint32_t DIAG;
	uint32_t INPDATA;
	//uint32_t GLERR;
	//uint32_t INTERR;

	//write_digital_inputs(0x98, 0x08);

	//DIAG = read_digital_inputs(0x00);
	INPDATA = read_digital_inputs(0x02);
	//GLERR = read_digital_inputs(0x04);
	//INTERR = read_digital_inputs(0x16);

	return INPDATA;
}


