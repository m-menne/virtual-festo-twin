-makelib ies/xil_defaultlib -sv \
  "/opt/Xilinx/Vivado/2017.1/data/ip/xpm/xpm_cdc/hdl/xpm_cdc.sv" \
  "/opt/Xilinx/Vivado/2017.1/data/ip/xpm/xpm_memory/hdl/xpm_memory.sv" \
-endlib
-makelib ies/xpm \
  "/opt/Xilinx/Vivado/2017.1/data/ip/xpm/xpm_VCOMP.vhd" \
-endlib
-makelib ies/axi_infrastructure_v1_1_0 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/7e3a/hdl/axi_infrastructure_v1_1_vl_rfs.v" \
-endlib
-makelib ies/xil_common_vip_v1_0_0 -sv \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/2ad9/hdl/xil_common_vip_v1_0_vl_rfs.sv" \
-endlib
-makelib ies/smartconnect_v1_0 -sv \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/16a2/hdl/sc_util_v1_0_vl_rfs.sv" \
-endlib
-makelib ies/axi_protocol_checker_v1_1_13 -sv \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/8017/hdl/axi_protocol_checker_v1_1_vl_rfs.sv" \
-endlib
-makelib ies/axi_vip_v1_0_1 -sv \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/856d/hdl/axi_vip_v1_0_vl_rfs.sv" \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/d5eb/hdl/processing_system7_vip_v1_0_vl_rfs.sv" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_processing_system7_0_0/sim/design_1_processing_system7_0_0.v" \
-endlib
-makelib ies/axi_lite_ipif_v3_0_4 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/0ba0/hdl/axi_lite_ipif_v3_0_vh_rfs.vhd" \
-endlib
-makelib ies/lib_cdc_v1_0_2 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/52cb/hdl/lib_cdc_v1_0_rfs.vhd" \
-endlib
-makelib ies/interrupt_control_v3_1_4 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/e956/hdl/interrupt_control_v3_1_vh_rfs.vhd" \
-endlib
-makelib ies/axi_gpio_v2_0_14 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/3b45/hdl/axi_gpio_v2_0_vh_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_axi_gpio_0_0/sim/design_1_axi_gpio_0_0.vhd" \
-endlib
-makelib ies/proc_sys_reset_v5_0_11 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/5db7/hdl/proc_sys_reset_v5_0_vh_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_rst_ps7_0_100M_0/sim/design_1_rst_ps7_0_100M_0.vhd" \
  "../../../bd/design_1/hdl/design_1.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_0/sim/design_1_axi_gpio_1_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_0/sim/design_1_axi_gpio_in1_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_1/sim/design_1_axi_gpio_in1_1.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_3/sim/design_1_axi_gpio_in1_3.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_2/sim/design_1_axi_gpio_in1_2.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_4/sim/design_1_axi_gpio_in1_4.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_5/sim/design_1_axi_gpio_in1_5.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_in1_6/sim/design_1_axi_gpio_in1_6.vhd" \
-endlib
-makelib ies/generic_baseblocks_v2_1_0 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/f9c1/hdl/generic_baseblocks_v2_1_vl_rfs.v" \
-endlib
-makelib ies/axi_register_slice_v2_1_12 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/0e33/hdl/axi_register_slice_v2_1_vl_rfs.v" \
-endlib
-makelib ies/fifo_generator_v13_1_4 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/ebc2/simulation/fifo_generator_vlog_beh.v" \
-endlib
-makelib ies/fifo_generator_v13_1_4 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/ebc2/hdl/fifo_generator_v13_1_rfs.vhd" \
-endlib
-makelib ies/fifo_generator_v13_1_4 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/ebc2/hdl/fifo_generator_v13_1_rfs.v" \
-endlib
-makelib ies/axi_data_fifo_v2_1_11 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/5235/hdl/axi_data_fifo_v2_1_vl_rfs.v" \
-endlib
-makelib ies/axi_crossbar_v2_1_13 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/78eb/hdl/axi_crossbar_v2_1_vl_rfs.v" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_xbar_0/sim/design_1_xbar_0.v" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_1/sim/design_1_axi_gpio_1_1.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_2/sim/design_1_axi_gpio_1_2.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_3/sim/design_1_axi_gpio_1_3.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_4/sim/design_1_axi_gpio_1_4.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_5/sim/design_1_axi_gpio_1_5.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_6/sim/design_1_axi_gpio_1_6.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_7/sim/design_1_axi_gpio_1_7.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_8/sim/design_1_axi_gpio_1_8.vhd" \
-endlib
-makelib ies/mdm_v3_2_9 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/359c/hdl/mdm_v3_2_vh_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_mdm_1_0/sim/design_1_mdm_1_0.vhd" \
-endlib
-makelib ies/microblaze_v10_0_2 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/796f/hdl/microblaze_v10_0_vh_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_microblaze_0_0/sim/design_1_microblaze_0_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_0_1/sim/design_1_axi_gpio_0_1.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_0_2/sim/design_1_axi_gpio_0_2.vhd" \
  "../../../bd/design_1/ip/design_1_axi_max11046_0_SHDN_CONVST_0/sim/design_1_axi_max11046_0_SHDN_CONVST_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_max11046_0_CS_RD_0/sim/design_1_axi_max11046_0_CS_RD_0.vhd" \
-endlib
-makelib ies/lmb_v10_v3_0_9 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/162e/hdl/lmb_v10_v3_0_vh_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_dlmb_v10_0/sim/design_1_dlmb_v10_0.vhd" \
  "../../../bd/design_1/ip/design_1_ilmb_v10_0/sim/design_1_ilmb_v10_0.vhd" \
-endlib
-makelib ies/lmb_bram_if_cntlr_v4_0_11 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/5376/hdl/lmb_bram_if_cntlr_v4_0_vh_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_dlmb_bram_if_cntlr_0/sim/design_1_dlmb_bram_if_cntlr_0.vhd" \
  "../../../bd/design_1/ip/design_1_ilmb_bram_if_cntlr_0/sim/design_1_ilmb_bram_if_cntlr_0.vhd" \
-endlib
-makelib ies/blk_mem_gen_v8_3_6 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/4158/simulation/blk_mem_gen_v8_3.v" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_lmb_bram_0/sim/design_1_lmb_bram_0.v" \
  "../../../bd/design_1/ip/design_1_xbar_1/sim/design_1_xbar_1.v" \
-endlib
-makelib ies/axi_bram_ctrl_v4_0_11 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/9183/hdl/axi_bram_ctrl_v4_0_rfs.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_axi_bram_ctrl_0_1/sim/design_1_axi_bram_ctrl_0_1.vhd" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_blk_mem_gen_0_0/sim/design_1_blk_mem_gen_0_0.v" \
  "../../../bd/design_1/ip/design_1_xbar_2/sim/design_1_xbar_2.v" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_axi_gpio_1_9/sim/design_1_axi_gpio_1_9.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_0_3/sim/design_1_axi_gpio_0_3.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_2_0/sim/design_1_axi_gpio_2_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_3_0/sim/design_1_axi_gpio_3_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_4_0/sim/design_1_axi_gpio_4_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_5_0/sim/design_1_axi_gpio_5_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_6_0/sim/design_1_axi_gpio_6_0.vhd" \
  "../../../bd/design_1/ip/design_1_axi_gpio_7_0/sim/design_1_axi_gpio_7_0.vhd" \
-endlib
-makelib ies/axi_protocol_converter_v2_1_12 \
  "../../../../LinuxSys.srcs/sources_1/bd/design_1/ipshared/138d/hdl/axi_protocol_converter_v2_1_vl_rfs.v" \
-endlib
-makelib ies/xil_defaultlib \
  "../../../bd/design_1/ip/design_1_auto_pc_2/sim/design_1_auto_pc_2.v" \
  "../../../bd/design_1/ip/design_1_tier2_xbar_0_0/sim/design_1_tier2_xbar_0_0.v" \
  "../../../bd/design_1/ip/design_1_tier2_xbar_1_0/sim/design_1_tier2_xbar_1_0.v" \
  "../../../bd/design_1/ip/design_1_tier2_xbar_2_0/sim/design_1_tier2_xbar_2_0.v" \
  "../../../bd/design_1/ip/design_1_tier2_xbar_3_0/sim/design_1_tier2_xbar_3_0.v" \
  "../../../bd/design_1/ip/design_1_auto_pc_0/sim/design_1_auto_pc_0.v" \
  "../../../bd/design_1/ip/design_1_auto_pc_1/sim/design_1_auto_pc_1.v" \
-endlib
-makelib ies/xil_defaultlib \
  glbl.v
-endlib

