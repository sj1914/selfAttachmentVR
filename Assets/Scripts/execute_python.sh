#!/bin/bash
echo 'Starting script...' 
source activate menpo >> logfile.log
echo 'Environment Activated...' >> logfile.log
python Child\ Customisation/Assets/Scripts/gui.py >> logfile.log
echo 'Python Script Run...' >> logfile.log
source deactivate >> logfile.log
echo 'Environment Deactivated.' >> logfile.log
echo 'Preparing for application launch...' >> logfile.log
# open Child\ Customisation/Customiser.app