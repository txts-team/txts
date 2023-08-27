#!/bin/bash

# This script updates and redeploys the txts service.

# Runtime variables
WORKING_DIRECTORY=/srv/txts # change this to match your deployment
GIT_REMOTE=origin
GIT_BRANCH=master

# Check for root privileges.
if [ "$EUID" -ne 0 ]
  then echo "This script needs to be run as root!"
  exit 1
fi

# Update the service.
cd $WORKING_DIRECTORY || exit 1

# Pull the latest changes from the git repository.
sudo git pull $GIT_REMOTE $GIT_BRANCH

# Restart the service.
sudo systemctl restart txts.service

# Exit with a success code.
exit 0