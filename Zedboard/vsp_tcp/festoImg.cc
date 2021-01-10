#include "festoImg.h"

char festoImg[FESTO_IMG_HEIGHT][FESTO_IMG_WIDTH] = {
{"                                INFLOW                                                                                                      "},
{"                              +---------+                                                                                                   "},
{"                              |         |                                                                                                   "},
{"                              +---+ +---+                                                                                                   "},
{"                                  | |                                                                                                       "},
{"                                  | |                                                                                                       "},
{"                                  | |                                                                                                       "},
{"PRESSURE             TANK2        | |                                                                                                       "},
{"+-----+       +-------------------+ +----+                                                                                                  "},
{"| | | |       |                          +------------------------------------------------------------------------+                         "},
{"| | | |       |                          +----------------------------------------------------------------------+ |                         "},
{"| | | |       |                          |                                                                      | |                         "},
{"| | | |       |                          |                                                                      | |                         "},
{"| | | |       |        LEVEL:            |                                                                      +-+                         "},
{"| | | |       |                          |                          VALVE                                       |-|                         "},
{"+-----+       |                          |                       +--------+                                     +-+                         "},
{"  | |         |                          |                       |        |                                     | |                         "},
{"  | +---------+                          +-----------------------+        +---------------------+               | |                         "},
{"  | +---------+                          +-----------------------+        +-------------------+ |               | |                         "},
{"  | |         +-----------+ +------------+                       +--------+                   | |               | |                         "},
{"  | |                     | |                                                                 | |     TANK1     | |                         "},
{"  | |                     | |                                                          +------+ +---------------+ +------+                  "},
{"  | |                     | |                                                          |                                 |                  "},
{"+-+ +-+CONTAMINATION      +-+                                                          |                                 |                  "},
{"|     |                   |-|                                                          |                                 |                  "},
{"+-+ +-+                   +-+                                                          |                                 |                  "},
{"  | |                     | +----------------------------------------------------------+             LEVEL:              |                  "},
{"  | |                     +------------------------------------------------------------+                                 |                  "},
{"+-+ +-+FLOW                                                                            |                                 |                  "},
{"|     | l/min                                                                          |                                 +--------------+   "},
{"+-+ +-+                                                                                |                                 |    HEATER    |   "},
{"  | |                                                                                  |                                 |              |   "},
{"  | |                                                                                  +-------------------+ +-----------+--------------+   "},
{"  | |          FILTER                                  PUMP                                                | |                              "},
{"  | |          +-----+                            +-----------------+                                      | |                              "},
{"  | +----------+     +----------------------------+                 +--------------------------------------+ +--------------------+         "},
{"  +------------+     +----------------------------+                 +-----------------------------------------------------------+ |         "},
{"               +-----+                            +-----------------+                                                           +-+         "},
{"                                                                                                                                |-|         "},
{"                                                                                                                                +-+         "},
{"                                                                                                                                | |         "},
{"                                                                                                                                + +         "}};

void printImage() {
#ifndef DEBUG
    system("clear");
#endif
    for (int rowIdx = 0; rowIdx < FESTO_IMG_HEIGHT; ++rowIdx) {
        std::cout << festoImg[rowIdx] << "\n";
    }
    std::cout << std::endl;
}

void writeState(char festoImg[FESTO_IMG_HEIGHT][FESTO_IMG_WIDTH], char * state, int stateLength, int row, int column) {
    for (int inIdx = 0; inIdx < stateLength; ++inIdx) {
        festoImg[row][column + inIdx] = state[inIdx];
    }
}

void writeUpperValveState(bool state) {
    char state_on[] = "OPENED";
    char state_off[] = "CLOSED";
    int stateLen = 6;
    char * currentState;

    if (state)
        currentState = state_on;
    else
        currentState = state_off;

    writeState(festoImg, currentState, stateLen, UPPER_VALVE_ROW, UPPER_VALVE_COL);
}

void writePumpState(bool state) {
    char state_on[] = "ON ";
    char state_off[] = "OFF";
    int stateLen = 3;
    char * currentState;

    if (state)
        currentState = state_on;
    else
        currentState = state_off;

    writeState(festoImg, currentState, stateLen, PUMP_ROW, PUMP_COL);
}

void writeHeaterState(bool state) {
    char state_on[] = "ON ";
    char state_off[] = "OFF";
    int stateLen = 3;
    char * currentState;

    if (state)
        currentState = state_on;
    else
        currentState = state_off;

    writeState(festoImg, currentState, stateLen, HEATER_ROW, HEATER_COL);
}


void writeFilterState(bool state) {
    char state_on[] = "ON ";
    char state_off[] = "OFF";
    int stateLen = 3;
    char * currentState;

    if (state)
        currentState = state_on;
    else
        currentState = state_off;

    writeState(festoImg, currentState, stateLen, FILTER_ROW, FILTER_COL);
}

void writeInflowState(bool state) {
    char state_on[] = "OPENED";
    char state_off[] = "CLOSED";
    int stateLen = 6;
    char * currentState;

    if (state)
        currentState = state_on;
    else
        currentState = state_off;

    writeState(festoImg, currentState, stateLen, INFLOW_ROW, INFLOW_COL);
}

void writeTank1Level(float level) {
    unsigned int percent = roundf(level * 100);
    percent = percent <= 100 ? percent : 100;
    char char_level[5] = {0};
    int stateLen = sprintf(char_level, "%3d%%", percent);

    writeState(festoImg, char_level, stateLen, TANK1_ROW, TANK1_COL);
}
void writeTank2Level(float level) {
    unsigned int percent = roundf(level * 100);
    percent = percent <= 100 ? percent : 100;
    char char_level[5] = {0};
    int stateLen = sprintf(char_level, "%3d%%", percent);

    writeState(festoImg, char_level, stateLen, TANK2_ROW, TANK2_COL);
}

void writeContaminationLevel(float level) {
    unsigned int percent = level * 100;
    percent = percent <= 100 ? percent : 100;
    char char_level[5] = {0};
    int stateLen = sprintf(char_level, "%3d%%", percent);

    writeState(festoImg, char_level, stateLen, CONTAMINATION_ROW, CONTAMINATION_COL);
}

void writeFlowValue(float value) {
    value = value >= 0 ? value : 0.0f;
    value = value <= 9.99 ? value : 9.99f;
    char char_level[5] = {0};
    int stateLen = sprintf(char_level, "%4.2f", value);

    writeState(festoImg, char_level, stateLen, FLOW_ROW, FLOW_COL);
}
