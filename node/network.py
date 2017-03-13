import websocket
import json
from uuid import getnode as get_mac

class network:

	def __init__( self, host, port ):
		self.host = host
		self.port = port
		self.sock = None

	def connect(self):
		self.sock = websocket.create_connection(self.host)
		pass

	def __send__(self, data):
		print ""
		print data
		print ""
		self.sock.send(data)
		pass			

	def send_json(self, json_message):
		self.__send__(json_message)
		pass

	def send_dictionary(self, input_dict):
		# print input_dict
		tmp = input_dict
		tmp['node_id'] = get_mac()
		# print tmp
		self.__send__(self.__build_json__(tmp))
		pass

	def test_connection(self):
		websocket.enableTrace(True)
		test_string = "My Temp String"
		print "Sending test sequence to server..."
		self.sock.send(test_string)
		recv = self.sock.recv()

		if recv == test_string:
			print "Passed."
			return "Passed"
		else: 
			print "Error"
			return "Error"


	def disconnect(self):
		self.sock.close()
		pass

	def __convert_json_to_dict(self, json_input):
		return json.loads(json_input)

	def __build_json__(self,in_dict):
		# print in_dict
		if isinstance(in_dict, dict):
			# print "Shit"
			return json.dumps(in_dict)
		else:
			return "{}"
		
	def get_dictionary_return(self):
		result = self.sock.recv()
		return self.__convert_json_to_dict(result)


	def test_student_bank(self):
		tmp = dict()
		tmp['14:8f:21:2a:abd24'] = False
		tmp['24:da:9b:13:7e:2b'] = False
		tmp['24:da:9b:13:7e:2c'] = False
		tmp['B8:C6:8E:1F:B9:3D'] = False
		return tmp

	def test_sleep_time(self):
		tmp = dict()
		tmp['sleep_timer'] = 0
		return tmp

	def send_string(self, string):
		tmp = dict()
		tmp['string'] = string
		tmp_json = __build_json__(tmp)
		send_json(tmp_json)


