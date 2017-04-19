# Ota update file.
import time
import os
import sys

def code_update():
	time.sleep(1)
	target.write( "Updating code\n" )
	os.system("git checkout master-node")
	os.system("git pull")
	target.write( "Code updated\n" )
	time.sleep(1)
	target.write( "Calling config update\n" )
	config_update()

def main():
	code_update()

def config_update():
	time.sleep(1)
	target.write( "Config updating\n" )

if __name__ == '__main__':
	target = open("ota.log", 'a+')
	main()
	target.close()

	os.system('python tmp.py &')
	sys.exit()
