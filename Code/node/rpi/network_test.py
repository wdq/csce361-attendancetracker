import socket
import sys

from time import sleep


def main():
	sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	server_address = ('attend.ddns.net', 8081)
	sock.connect(server_address)

	try:
	    
	    # Send data
	    message = 'h'
	    print >>sys.stderr, 'sending "%s"' % message
	    sock.send(message)
	    sock.send('\n')

	    # Look for the response
	    amount_received = 0
	    amount_expected = len(message)
	    
	    while amount_received < amount_expected:
	        data = sock.recv(16)
	        amount_received += len(data)
	        print >>sys.stderr, 'received "%s"' % data

	finally:
	    print >>sys.stderr, 'closing socket'
	    sock.close()


if __name__ == '__main__':
	while True:
		main()
		sleep(.5)
	