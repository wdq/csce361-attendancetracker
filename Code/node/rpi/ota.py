# Ota update file.
import time



def code_update():
	time.sleep(1)
	target.write( "Updating code\n" )
	time.sleep(1)
	target.write( "Calling config update\n" )
	config_update()

def main():
	code_update()

def config_update():
	time.sleep(1)
	target.write( "Config updating\n" )

if __name__ == '__main__':
	target = open("ota.log", 'w+')
	main()
	target.close()
	exit()
