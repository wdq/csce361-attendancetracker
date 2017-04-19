import os
import sys
import subprocess

DETACHED_PROCESS = 0x00000008

def main():
    # os.execl(sys.executable, "python", 'ota.py')
    os.system('python ota.py &')

if __name__ == '__main__':
    # print("Starting Application")
    main()
    sys.exit()