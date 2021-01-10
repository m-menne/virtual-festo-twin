// Implementation of the Data class
#include "Data.hpp"

// ---------------- Constructor of the data object ----------------------------------
// Creates a empty data object.
Data
::
Data()
{
}

// ---------------- Destructor ------------------------------------------------------
Data
::
~Data()
{
}

// ---------------- Getter ----------------------------------------------------------
// Returns true if the absolute maximal water level of the right water tank is reached.
bool
Data
::
getRightTank_overflow(){
    return rightTank_overflow;
}
// Returns the status of the float switch of the left water tank.
bool
Data
::
getLeftTank_floatswitch(){
    return leftTank_floatswitch;
}
// Returns true if the minimal water level of the right water tank is reached.
bool
Data
::
getRightTank_minswitch(){
    return rightTank_minswitch;
}
// Returns true if the maximal water level of the right water tank is reached.
bool
Data
::
getRightTank_maxswitch(){
    return rightTank_maxswitch;
}
// Returns true if the upper valve is closed.
bool
Data
::
getUpperValve_closed(){
    return upperValve_closed;
}
// Returns true if the upper valve is open.
bool
Data
::
getUpperValve_open(){
    return upperValve_open;
}

// Returns the water level of the left watertank.
float
Data
::
getLeftTank_waterlevel(){
    return leftTank_waterlevel;
}
// Returns the water flow in pipeline.
float
Data
::
getPipeline_waterflow(){
    return pipeline_waterflow;
}
// Returns the pressure in pipeline.
float
Data
::
getPipeline_pressure(){
    return pipeline_pressure;
}
// Returns the temperature of water in the right water tank.
float
Data
::
getRightTank_temperature(){
    return rightTank_temperature;
}
// Returns the saturation of water in the pipeline.
float
Data
::
getPipeline_saturation(){
    return pipeline_saturation;
}

// Returns true if the heater is on.
bool
Data
::
getHeater_status(){
    return heater_status;
}
// Returns true if the pump runs in the analog mode.
bool
Data
::getPump_mode(){
    return pump_mode;
}
// Returns true if the pump is on.
bool
Data
::
getPump_status(){
    return pump_status;
}
// Returns true if the lower valve is open.
bool
Data
::
getLowerValve_status(){
    return lowerValve_status;
}
// Returns true if the water is filtered.
bool
Data
::
getFilter_status(){
    return filter_status;
}
// Returns true if the inflow is open.
bool
Data
::
getInflow_status(){
    return inflow_status;
}

// Returns the power of the pump.
float
Data
::
getPump_power(){
    return pump_power;
}
// Returns the flow rate of the lower valve.
float
Data
::getLowerValve_flowrate(){
    return lowerValve_flowrate;
}

// ---------------- Setter ----------------------------------------------------------
// Sets the absolute maximal water level of the right water tank.
void
Data
::
setRightTank_overflow(bool x){
    rightTank_overflow = x;
}
// Sets the status of the float switch of the left water tank.
void
Data
::
setLeftTank_floatswitch(bool x){
    leftTank_floatswitch = x;
}
// Sets the minimal water level of the right water tank.
void
Data
::
setRightTank_minswitch(bool x){
    rightTank_minswitch = x;
}
// Sets the maximal water level of the right water tank.
void
Data
::
setRightTank_maxswitch(bool x){
    rightTank_maxswitch = x;
}
// Sets the upper valve.
void
Data
::
setUpperValve_closed(bool x){
    upperValve_closed = x;
}
// Sets the upper valve.
void
Data
::
setUpperValve_open(bool x){
    upperValve_open = x;
}

// Sets the water level of the left water tank.
void
Data
::
setLeftTank_waterlevel(float x){
    leftTank_waterlevel = x;
}
// Sets the water flow in pipeline.
void
Data
::
setPipeline_waterflow(float x){
    pipeline_waterflow = x;
}
// Sets the pressure in pipeline.
void
Data
::
setPipeline_pressure(float x){
    pipeline_pressure = x;
}
// Sets the temperature of water in the right water tank.
void
Data
::
setRightTank_temperature(float x){
    rightTank_temperature = x;
}
// Sets the saturation of water in pipeline.
void
Data
::
setPipeline_saturation(float x){
    pipeline_saturation = x;
}

// Sets the heater status.
void
Data
::
setHeater_status(bool x){
    heater_status = x;
}
// Sets the mode of the pump.
void
Data
::
setPump_mode(bool x){
    pump_mode = x;
}
// Sets the status of the pump.
void
Data
::
setPump_status(bool x){
    pump_status = x;
}
// Sets the status of the lower valve.
void
Data
::
setLowerValve_status(bool x){
    lowerValve_status = x;
}
// Sets the status of the filter.
void
Data
::
setFilter_status(bool x){
    filter_status = x;
}
// Sets the status of the inflow.
void
Data
::
setInflow_status(bool x){
    inflow_status = x;
}

// Sets the power of the pump.
void
Data
::
setPump_power(float x){
    pump_power = x;
}

// Sets the flow rate of the lower valve.
void
Data
::
setLowerValve_flowrate(float x){
    lowerValve_flowrate = x;
}
