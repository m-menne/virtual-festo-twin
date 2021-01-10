<?php
// This script sets the pump status, mode and power to the received values
// Parse the url string received from the HoloLens
parse_str($_SERVER['QUERY_STRING']);
// Call postData using the correct parameters
system('/home/ubuntu/software_zedboard/PostData/postData 1 '.$status.' '.$mode.' '.$power);
?>
