# Pin Documentation:
# https://datasheets.maximintegrated.com/en/ds/MAX11044-MAX11056.pdf
# Page 8+9

# XDC Syntax Examples
# set_property IOSTANDARD LVCMOS33 [get_ports -of_objects [get_iobanks 33]];
# set_property PACKAGE_PIN B21 [get_ports {FMC_LA33_P}];  # "FMC-LA33_P"

# Signal/Pin Assignment
# 16-Bit Parallel Data Bus Digital Output Bits 
set_property PACKAGE_PIN J18 [get_ports {axi_max11046_0_DB16_CR4[0]}];
set_property PACKAGE_PIN L22 [get_ports {axi_max11046_0_DB16_CR4[1]}];
set_property PACKAGE_PIN K18 [get_ports {axi_max11046_0_DB16_CR4[2]}];
set_property PACKAGE_PIN R20 [get_ports {axi_max11046_0_DB16_CR4[3]}];
set_property PACKAGE_PIN R19 [get_ports {axi_max11046_0_DB16_CR4[4]}];
set_property PACKAGE_PIN R21 [get_ports {axi_max11046_0_DB16_CR4[5]}];
set_property PACKAGE_PIN T19 [get_ports {axi_max11046_0_DB16_CR4[6]}];
set_property PACKAGE_PIN L17 [get_ports {axi_max11046_0_DB16_CR4[7]}];
set_property PACKAGE_PIN M17 [get_ports {axi_max11046_0_DB16_CR4[8]}];
set_property PACKAGE_PIN K19 [get_ports {axi_max11046_0_DB16_CR4[9]}] ;
set_property PACKAGE_PIN K20 [get_ports {axi_max11046_0_DB16_CR4[10]}];
set_property PACKAGE_PIN B19 [get_ports {axi_max11046_0_DB16_CR4[11]}];
set_property PACKAGE_PIN B20 [get_ports {axi_max11046_0_DB16_CR4[12]}];
set_property PACKAGE_PIN D20 [get_ports {axi_max11046_0_DB16_CR4[13]}];
set_property PACKAGE_PIN E15 [get_ports {axi_max11046_0_DB16_CR4[14]}];
set_property PACKAGE_PIN D15 [get_ports {axi_max11046_0_DB16_CR4[15]}]; 
# Shutdown Input
set_property PACKAGE_PIN N19 [get_ports {axi_max11046_0_SHDN_pin[0]}];
# Convert Start Input
set_property PACKAGE_PIN N20 [get_ports {axi_max11046_0_CONVST_pin[0]}];
# Active-Low Chip Select
set_property PACKAGE_PIN E21 [get_ports {axi_max11046_0_CS_B_pin[0]}];
# Active-Low Read Input
set_property PACKAGE_PIN C20 [get_ports {axi_max11046_0_RD_B_pin[0]}];
# Active-Low Write Input
set_property PACKAGE_PIN D21 [get_ports {axi_max11046_0_WR_B_pin[0]}];
# Active-Low End of Conversion
set_property PACKAGE_PIN L21 [get_ports {axi_max11046_0_EOC_B_pin[0]}];


# I/O-Standard Assignment
# Data Signals
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[1]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[2]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[3]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[4]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[5]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[6]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[7]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[8]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[9]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[10]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[11]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[12]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[13]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[14]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_DB16_CR4[15]}];
# Control Signals
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_SHDN_pin[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_CONVST_pin[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_CS_B_pin[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_RD_B_pin[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_WR_B_pin[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {axi_max11046_0_EOC_B_pin[0]}];
