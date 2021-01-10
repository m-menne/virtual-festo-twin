/*
 * analog_out.h
 *
 *  Created on: Dec 11, 2017
 *      Author: tux
 */

#ifndef SRC_ANALOG_OUT_H_
#define SRC_ANALOG_OUT_H_

#include "io_defs.h"

// Define bitmask of the control signals for SPI bit-banging
#define SPI_CS1_POS 	0x01 // Chip select 1
#define SPI_MOSI_POS 	0x02 // Master -> Output, Slave -> Input
#define SPI_MISO_POS	0x04 // Master -> Input,  Slave -> Output
#define SPI_CLOCK_POS	0x08 // Clock used for sync purposes
#define SPI_INT_POS		0x10 // Interrupt
#define SPI_CS2_POS		0x20 // Chip select 2

void init_analog_outputs();
void write_analog_outputs(uint32_t* data);

#endif /* SRC_ANALOG_OUT_H_ */
