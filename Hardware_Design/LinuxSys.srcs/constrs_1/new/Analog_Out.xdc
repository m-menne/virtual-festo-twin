# Analog Output Board (Alameda (MAXREFDES24#))
# Pin Documentation:
# https://pdfserv.maximintegrated.com/en/an/UG5847.pdf

# Signal/Pin Assignment
set_property PACKAGE_PIN W7 [get_ports {gpio_pmod_jd_tri_io[1]}];  # "JD1_N"
set_property PACKAGE_PIN V7 [get_ports {gpio_pmod_jd_tri_io[0]}];  # "JD1_P"
set_property PACKAGE_PIN V4 [get_ports {gpio_pmod_jd_tri_io[3]}];  # "JD2_N"
set_property PACKAGE_PIN V5  [get_ports {gpio_pmod_jd_tri_io[2]}];  # "JD2_P"
set_property PACKAGE_PIN W5  [get_ports {gpio_pmod_jd_tri_io[5]}];  # "JD3_N"
set_property PACKAGE_PIN W6  [get_ports {gpio_pmod_jd_tri_io[4]}];  # "JD3_P"
set_property PACKAGE_PIN U5  [get_ports {gpio_pmod_jd_tri_io[7]}];  # "JC4_N"
set_property PACKAGE_PIN U6  [get_ports {gpio_pmod_jd_tri_io[6]}];  # "JC4_P"

set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[0]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[1]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[2]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[3]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[4]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[5]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[6]}];
set_property IOSTANDARD LVCMOS33 [get_ports {gpio_pmod_jd_tri_io[7]}];