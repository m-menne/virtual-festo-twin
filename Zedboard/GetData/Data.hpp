// Class: Data
// This class provides the data structure for the complete measured data.

#ifndef Data_hpp
#define Data_hpp

#include <stdio.h>

class Data
{
    // ---------------- Member variables ------------------------------------------------
    
    private:
    
        // Sensor - Input

        // Binary
        bool rightTank_overflow;        // Variable for the overflow sensor of the right water tank S111.
        bool leftTank_floatswitch;      // Variable for the float switch of the left water tank S112.
        bool rightTank_minswitch;       // Variable for the minimal switch of the right water tank B113.
        bool rightTank_maxswitch;       // Variable for the maximal switch of the right water tank B114.
        bool upperValve_closed;         // Variable for the status of the upper valve S115/S116. (True, if the valve is closed)
        bool upperValve_open;           // Variable for the status of the upper valve S115/S116. (True, if the valve is open)
    
        // Analog
        float leftTank_waterlevel;      // Variable for the water level of the left water tank LIC B101.
        float pipeline_waterflow;       // Variable for the water flow in pipeline FIC B102.
        float pipeline_pressure;        // Variable for the pressure in pipeline PIC B103.
        float rightTank_temperature;    // Variable for the temperature in the right water tank TIC B104.
        float pipeline_saturation;      // Variable for the saturation of the water in pipeline.
    
        // Actuator - Output
    
        // Binary
        bool heater_status;             // Variable for the status of the heater E104.
        bool pump_mode;                 // Variable for the mode of the pump (0 - binary, 1 - analog) K1.
        bool pump_status;               // Variable for the status of the pump (0 - power off, 1 - power on) M1.
        bool lowerValve_status;         // Variable for the status of the lower valve.
        bool filter_status;             // Variable for the status of the filter.
        bool inflow_status;             // Variable for the status of the inflow.

        // Analog
        float pump_power;               // Variable for the power of the pump P101.
        float lowerValve_flowrate;      // Variable for the flow rate of the lower valve.
    
    
    // ---------------- Methods ---------------------------------------------------------
    
    public:
        // Constructor
        // Creates a empty data object.
        Data();
    
        // Destructor of the Data object.
        ~Data();
    
        // Getter
    
        // Returns true if the absolute maximal water level of the right water tank is reached.
        bool getRightTank_overflow();
        // Returns the status of the float switch of the left water tank.
        bool getLeftTank_floatswitch();
        // Returns true if the minimal water level of the right water tank is reached.
        bool getRightTank_minswitch();
        // Returns true if the maximal water level of the right water tank is reached.
        bool getRightTank_maxswitch();
        // Returns true if the upper valve is closed.
        bool getUpperValve_closed();
        // Returns true if the upper valve is open.
        bool getUpperValve_open();
    
        // Returns the water level of the left water tank.
        float getLeftTank_waterlevel();
        // Returns the water flow in pipeline.
        float getPipeline_waterflow();
        // Returns the pressure in pipeline.
        float getPipeline_pressure();
        // Returns the temperature of water in the right water tank.
        float getRightTank_temperature();
        // Returns the saturation of water in pipeline.
        float getPipeline_saturation();
    
        // Returns true if the heater is on.
        bool getHeater_status();
        // Returns true if the pump runs in the analog mode.
        bool getPump_mode();
        // Returns true if the pump is on.
        bool getPump_status();
        // Returns true if the lower valve is open.
        bool getLowerValve_status();
        // Returns true if the water is filtered.
        bool getFilter_status();
        // Returns true if the inflow is open.
        bool getInflow_status();
    
        // Returns the power of the pump.
        float getPump_power();
        // Returns the flow rate of the lower valve.
        float getLowerValve_flowrate();
    
        // Setter
    
        // Sets the absolute maximal water level of the right water tank.
        void setRightTank_overflow(bool x);
        // Sets the status of the float switch of the left water tank.
        void setLeftTank_floatswitch(bool x);
        // Sets the minimal water level of the right water tank.
        void setRightTank_minswitch(bool x);
        // Sets the maximal water level of the right water tank.
        void setRightTank_maxswitch(bool x);
        // Sets the upper valve.
        void setUpperValve_closed(bool x);
        void setUpperValve_open(bool x);
    
        // Sets the water level of the left watertank.
        void setLeftTank_waterlevel(float x);
        // Sets the water flow in pipeline.
        void setPipeline_waterflow(float x);
        // Sets the pressure in pipeline.
        void setPipeline_pressure(float x);
        // Sets the temperature of water in the right water tank.
        void setRightTank_temperature(float x);
        // Sets the saturation of water in pipeline.
        void setPipeline_saturation(float x);
    
        // Sets the heater status.
        void setHeater_status(bool x);
        // Sets the mode of the pump.
        void setPump_mode(bool x);
        // Sets the status of the pump.
        void setPump_status(bool x);
        // Sets the status of the lower valve.
        void setLowerValve_status(bool x);
        // Sets the status of the filter.
        void setFilter_status(bool x);
        // Sets the status of the inflow.
        void setInflow_status(bool x);
    
        // Sets the power of the pump.
        void setPump_power(float x);
        // Sets the flow rate of the lower valve.
        void setLowerValve_flowrate(float x);
    
};

#endif /* Data_hpp */
