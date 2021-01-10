# ----------------------------------------------------------------------------
# JA Pmod - Bank 13
# ---------------------------------------------------------------------------- 
set_property PACKAGE_PIN Y11  [get_ports {gpio_pmod_ja_tri_io[0]}];  # "JA1"
set_property PACKAGE_PIN AA11 [get_ports {gpio_pmod_ja_tri_io[1]}];  # "JA2"
set_property PACKAGE_PIN Y10  [get_ports {gpio_pmod_ja_tri_io[2]}];  # "JA3"
set_property PACKAGE_PIN AA9  [get_ports {gpio_pmod_ja_tri_io[3]}];  # "JA4"
set_property PACKAGE_PIN AB11 [get_ports {gpio_pmod_ja_tri_io[4]}];  # "JA7"
set_property PACKAGE_PIN AB10 [get_ports {gpio_pmod_ja_tri_io[5]}];  # "JA8"
set_property PACKAGE_PIN AB9  [get_ports {gpio_pmod_ja_tri_io[6]}];  # "JA9"
set_property PACKAGE_PIN AA8  [get_ports {gpio_pmod_ja_tri_io[7]}];  # "JA10"


# ----------------------------------------------------------------------------
# JB Pmod - Bank 13
# ---------------------------------------------------------------------------- 
set_property PACKAGE_PIN W12 [get_ports {gpio_pmod_jb_tri_io[0]}];  # "JB1"
set_property PACKAGE_PIN W11 [get_ports {gpio_pmod_jb_tri_io[1]}];  # "JB2"
set_property PACKAGE_PIN V10 [get_ports {gpio_pmod_jb_tri_io[2]}];  # "JB3"
set_property PACKAGE_PIN W8  [get_ports {gpio_pmod_jb_tri_io[3]}];  # "JB4"
set_property PACKAGE_PIN V12 [get_ports {gpio_pmod_jb_tri_io[4]}];  # "JB7"
set_property PACKAGE_PIN W10 [get_ports {gpio_pmod_jb_tri_io[5]}];  # "JB8"
set_property PACKAGE_PIN V9  [get_ports {gpio_pmod_jb_tri_io[6]}];  # "JB9"
set_property PACKAGE_PIN V8  [get_ports {gpio_pmod_jb_tri_io[7]}];  # "JB10"

# ----------------------------------------------------------------------------
# JC Pmod - Bank 13
# ---------------------------------------------------------------------------- 
set_property PACKAGE_PIN AB6 [get_ports {gpio_pmod_jc_tri_io[1]}];  # "JC1_N"
set_property PACKAGE_PIN AB7 [get_ports {gpio_pmod_jc_tri_io[0]}];  # "JC1_P"
set_property PACKAGE_PIN AA4 [get_ports {gpio_pmod_jc_tri_io[3]}];  # "JC2_N"
set_property PACKAGE_PIN Y4  [get_ports {gpio_pmod_jc_tri_io[2]}];  # "JC2_P"
set_property PACKAGE_PIN T6  [get_ports {gpio_pmod_jc_tri_io[5]}];  # "JC3_N"
set_property PACKAGE_PIN R6  [get_ports {gpio_pmod_jc_tri_io[4]}];  # "JC3_P"
set_property PACKAGE_PIN U4  [get_ports {gpio_pmod_jc_tri_io[7]}];  # "JC4_N"
set_property PACKAGE_PIN T4  [get_ports {gpio_pmod_jc_tri_io[6]}];  # "JC4_P"


# ----------------------------------------------------------------------------
# IOSTANDARD Constraints
#
# Note that these IOSTANDARD constraints are applied to all IOs currently
# assigned within an I/O bank.  If these IOSTANDARD constraints are 
# evaluated prior to other PACKAGE_PIN constraints being applied, then 
# the IOSTANDARD specified will likely not be applied properly to those 
# pins.  Therefore, bank wide IOSTANDARD constraints should be placed 
# within the XDC file in a location that is evaluated AFTER all 
# PACKAGE_PIN constraints within the target bank have been evaluated.
#
# Un-comment one or more of the following IOSTANDARD constraints according to
# the bank pin assignments that are required within a design.
# ---------------------------------------------------------------------------- 

# Note that the bank voltage for IO Bank 33 is fixed to 3.3V on ZedBoard. 
set_property IOSTANDARD LVCMOS33 [get_ports -of_objects [get_iobanks 33]];

# Set the bank voltage for IO Bank 34 to 1.8V by default.
set_property IOSTANDARD LVCMOS33 [get_ports -of_objects [get_iobanks 34]];
# set_property IOSTANDARD LVCMOS25 [get_ports -of_objects [get_iobanks 34]];
# set_property IOSTANDARD LVCMOS18 [get_ports -of_objects [get_iobanks 34]];

# Set the bank voltage for IO Bank 35 to 1.8V by default.
set_property IOSTANDARD LVCMOS33 [get_ports -of_objects [get_iobanks 35]];
# set_property IOSTANDARD LVCMOS25 [get_ports -of_objects [get_iobanks 35]];
# set_property IOSTANDARD LVCMOS18 [get_ports -of_objects [get_iobanks 35]];

# Note that the bank voltage for IO Bank 13 is fixed to 3.3V on ZedBoard. 
set_property IOSTANDARD LVCMOS33 [get_ports -of_objects [get_iobanks 13]];