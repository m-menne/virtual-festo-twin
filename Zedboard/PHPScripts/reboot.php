<?php
  // Creates a file named ".server-reboot" in the shutdown_file directory 
  $handle = fopen('/var/www/html/shutdown_file/.server-reboot', 'w') or die('Reboot command could not be executed!');
  echo 'Reboot will be executed!';
?>
