#!/usr/bin/env bash

# This script updates and redeploys the txts service.

# Runtime variables
WORKING_DIRECTORY=/srv/txts # change this to match your deployment
GIT_REMOTE=origin
GIT_BRANCH=master

# Update the service.
cd $WORKING_DIRECTORY || exit 1

# Pull the latest changes from the git repository.
sudo git pull $GIT_REMOTE $GIT_BRANCH

# Restart the service.
sudo systemctl restart txts.service

# Exit with a success code.
exit 0