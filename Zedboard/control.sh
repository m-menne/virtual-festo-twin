#!/bin/bash
#Check if a ".server-shutdown" file in the shutdown_file directory exists.
#If there is a ".server-shutdown" file, remove this file and start a shutdown command.
if [ -f /var/www/html/shutdown_file/.server-shutdown ]; then
  rm -f /var/www/html/shutdown_file/.server-shutdown
  /sbin/shutdown -h now
fi
#Check if a ".server-reboot" file in the shutdown_file directory exists.
#If there is a ".server-reboot" file, remove this file and start a reboot command.
if [ -f /var/www/html/shutdown_file/.server-reboot ]; then
  rm -f /var/www/html/shutdown_file/.server-reboot
  /sbin/shutdown -r now
fi
