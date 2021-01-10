<?php 
  // Creates a file named ".server-shutdown" in the shutdown_file directory
  $handle = fopen('/var/www/html/shutdown_file/.server-shutdown', 'w') or die('Shutdown command could not be executed!');
  echo 'Shutdown will be executed!';
?>
