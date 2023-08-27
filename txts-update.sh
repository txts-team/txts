#!/bin/bash

# This script updates and redeploys the txts service.

# Runtime variables
WORKING_DIRECTORY=/srv/txts # change this to match your deployment
GIT_REMOTE=origin
GIT_BRANCH=master

# Helper functions
function log() {
	echo "[$(date)] $*"
}

# Check for root privileges.
if [ "$EUID" -ne 0 ]
  then echo "This script needs to be run as root!"
  exit 1
fi

# Update the service.
log "Working directory for updater script is $WORKING_DIRECTORY"
cd $WORKING_DIRECTORY || exit 1

# Pull the latest changes from the git repository.
log "Git remote for updater script is $GIT_REMOTE"
log "Git branch for updater script is $GIT_BRANCH"
sudo git pull $GIT_REMOTE $GIT_BRANCH

# Restart the service.
log "Restarting the txts system service"
sudo systemctl restart txts.service

# Exit with a success code.
log "Update completed"
exit 0