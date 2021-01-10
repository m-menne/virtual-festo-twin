/*
 * analog_in.h
 *
 *  Created on: Dec 11, 2017
 *      Author: tux
 */

#ifndef SRC_ANALOG_IN_H_
#define SRC_ANALOG_IN_H_

#include "io_defs.h"

void init_analog_inputs();
void read_analog_inputs(uint32_t* data_a);

#endif /* SRC_ANALOG_IN_H_ */
