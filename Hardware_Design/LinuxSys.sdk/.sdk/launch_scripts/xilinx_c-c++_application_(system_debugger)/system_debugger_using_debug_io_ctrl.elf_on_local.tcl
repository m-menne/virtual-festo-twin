connect -url tcp:127.0.0.1:3121
targets -set -filter {jtag_cable_name =~ "Digilent Zed 210248493211" && level==0} -index 1
fpga -file /home/tux/projects/HoloLensRUB/HW_FestoHoloLens/LinuxSys.sdk/design_1_wrapper_hw_platform_0/design_1_wrapper_clock.bit
targets -set -nocase -filter {name =~ "microblaze*#0" && bscan=="USER2"  && jtag_cable_name =~ "Digilent Zed 210248493211"} -index 1
rst -processor
targets -set -nocase -filter {name =~ "microblaze*#0" && bscan=="USER2"  && jtag_cable_name =~ "Digilent Zed 210248493211"} -index 1
dow /home/tux/projects/HoloLensRUB/HW_FestoHoloLens/LinuxSys.sdk/io_ctrl/Debug/io_ctrl.elf
bpadd -addr &main
