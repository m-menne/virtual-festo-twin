<?php
// This script sets the status of the inflow to the received status
// Parse the url string received from the HoloLens
parse_str($_SERVER['QUERY_STRING']);
// Call postData using the correct parameters
system('/home/ubuntu/software_zedboard/PostData/postData 6 '.$status);
?>
