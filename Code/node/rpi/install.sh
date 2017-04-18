#!/bin/bash
echo "Installing python componentents"
sudo apt install python-setuptools python-dev build-essential 
sudo easy_install pip 
sudo pip install --upgrade virtualenv 
pip install pybluez
pip install websocket-client
echo "Done installing components"