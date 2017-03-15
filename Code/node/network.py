import websocket
import json
import time
import threading
from uuid import getnode as get_mac

class network:

    def __init__( self, host, port ):
        self.host = host
        self.port = port
        self.sock = None
        self.network_in_use  = threading.BoundedSemaphore(value =1)

    def connect(self):
        if self.sock != None:
            print "Opening socket"
            self.disconnect()
            self.connect()
        else:
            try:
                self.sock = websocket.create_connection(self.host, timeout = 2)
            except:
                print "Couldn't open socket. Trying to re-open socket"
                time.sleep(.1)
                self.connect()
        pass

    def __send__(self, data):
        print ""
        print data
        print ""

        self.network_in_use.acquire()

        try:
            self.sock.send(data)
        except:
            self.disconnect()
            self.connect()

            print "Error sending. Resetting socket connection. "

            try:
                self.sock.send(data)
            except:
                print "Error. Socket is broken."

        self.network_in_use.release()


    def send_json(self, json_message):
        self.__send__(json_message)
        pass

    def send_dictionary(self, input_dict):
        # print input_dict
        tmp = input_dict
        tmp['node_id'] = get_mac()
        self.__send__(self.__build_json__(tmp))

    def test_connection(self):
        # Disable verbose websockets
        websocket.enableTrace(False)

        # Create a test connection string to verify connectivity to the sytem
        test_string = {'verify_connection':'AABBCCDD112233'}

        # Send the json request after building it
        self.send_json(self.__build_json__(test_string))
        print "Sending test sequence to server..."

        # recieve the sent json request
        recv = self.__recv__()

        # verify what we sent is what we got if not send an error
        if  json.loads(recv) == test_string:
                print "Passed."
                return "Passed"
        else:
                print "Error"
                return "Error"


    def disconnect(self):
        if self.sock != None:
            self.sock.close()
            self.sock = None
        pass

    def __convert_json_to_dict(self, json_input):
        return json.loads(json_input)

    def __build_json__(self,in_dict):
        # print in_dict
        if isinstance(in_dict, dict):
            return json.dumps(in_dict)
        else:
            return "{}"

    def __recv__(self):
        try:
            return self.sock.recv()
        except:
            print "Error: Timeout of socket."
            return "{}"
            
        
    def get_dictionary_return(self):
        result = self.__recv__()
        return self.__convert_json_to_dict(result)

    def send_string(self, string):
        tmp = dict()
        tmp['string'] = string
        tmp_json = __build_json__(tmp)
        send_json(tmp_json)

    def send_key_value(self, key, value):
        tmp = dict()
        tmp[key] = value
        self.send_dictionary(tmp)

    def test_student_bank(self):
        tmp = dict()

        tmp['38:CA:DA:BF:84:02'] = False
        tmp['24:da:9b:13:7e:2b'] = False
        tmp['24:da:9b:13:7e:2c'] = False
        tmp['B8:C6:8E:1F:B9:3D'] = False

        return tmp

    def get_socket(self):
        return self.sock

    def test_sleep_time(self):
        tmp = dict()
        tmp['sleep_timer'] = 10
        return tmp