################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
LD_SRCS += \
../src/lscript.ld 

CC_SRCS += \
../src/main.cc 

CPP_SRCS += \
../src/analog_in.cpp \
../src/analog_out.cpp \
../src/digital_in.cpp \
../src/digital_out.cpp 

CC_DEPS += \
./src/main.d 

OBJS += \
./src/analog_in.o \
./src/analog_out.o \
./src/digital_in.o \
./src/digital_out.o \
./src/main.o 

CPP_DEPS += \
./src/analog_in.d \
./src/analog_out.d \
./src/digital_in.d \
./src/digital_out.d 


# Each subdirectory must supply rules for building sources it contributes
src/%.o: ../src/%.cpp
	@echo 'Building file: $<'
	@echo 'Invoking: MicroBlaze g++ compiler'
	mb-g++ -Wall -pedantic -O0 -g3 -c -fmessage-length=0 -MT"$@" -I../../io_ctrl_bsp/mb0_microblaze_0/include -mlittle-endian -mcpu=v10.0 -mxl-soft-mul -Wl,--no-relax -ffunction-sections -fdata-sections -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -o "$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '

src/%.o: ../src/%.cc
	@echo 'Building file: $<'
	@echo 'Invoking: MicroBlaze g++ compiler'
	mb-g++ -Wall -pedantic -O0 -g3 -c -fmessage-length=0 -MT"$@" -I../../io_ctrl_bsp/mb0_microblaze_0/include -mlittle-endian -mcpu=v10.0 -mxl-soft-mul -Wl,--no-relax -ffunction-sections -fdata-sections -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -o "$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '


