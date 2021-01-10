/*
 * digital_in.h
 *
 *  Created on: Dec 11, 2017
 *      Author: tux
 */

#ifndef SRC_DIGITAL_IN_H_
#define SRC_DIGITAL_IN_H_

#include "io_defs.h"

// Define bitmask of the control signals
#define nCS 0x10 //	not-chip-select-bit
#define ALE 0x20 // address-latch-enable-bit
#define nWR 0x40 //	not-write-bit
#define nRD 0x80 // not-read-bit

void init_digital_inputs();
uint32_t read_digital_inputs(uint32_t reg);
// void write_digital_inputs(uint32_t reg, uint32_t reg_data);
uint32_t read_data();

#endif /* SRC_DIGITAL_IN_H_ */