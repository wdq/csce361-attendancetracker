#!/bin/bash
echo "Installing python componentents"
# Getting dependencies
sudo apt install python-setuptools python-dev build-essential 
sudo get install python-pip python-dev ipython
sudo get install bluetooth libbluetooth-dev

# Installing PIP
sudo easy_install pip 

# Installing packages needed through pip
sudo pip install pybluez
pip install pybluez
pip install websocket-client

# Setting up virtual envierment
sudo pip install --upgrade virtualenv 
echo "Done installing components"