#include <iostream>
#include <cstdlib>
#include <cmath>

#define FESTO_IMG_WIDTH 141
#define FESTO_IMG_HEIGHT 42

#define PUMP_ROW 35
#define PUMP_COL 55

#define HEATER_ROW 31
#define HEATER_COL 127

#define FILTER_ROW 35
#define FILTER_COL 17

#define INFLOW_ROW 2
#define INFLOW_COL 32

#define UPPER_VALVE_ROW 17
#define UPPER_VALVE_COL 67

#define TANK1_ROW 27
#define TANK1_COL 102

#define TANK2_ROW 14
#define TANK2_COL 24

#define CONTAMINATION_ROW 24
#define CONTAMINATION_COL 1

#define FLOW_ROW 29
#define FLOW_COL 2

extern char festoImg[FESTO_IMG_HEIGHT][FESTO_IMG_WIDTH];

void printImage();
void writeState(char festoImg[FESTO_IMG_HEIGHT][FESTO_IMG_WIDTH], char * state, int stateLenght, int row, int column);

void writeUpperValveState(bool state);
void writePumpState(bool state);
void writeHeaterState(bool state);
void writeFilterState(bool state);
void writeInflowState(bool state);
void writeTank1Level(float level);
void writeTank2Level(float level);
void writeContaminationLevel(float level);
void writeFlowValue(float value);
