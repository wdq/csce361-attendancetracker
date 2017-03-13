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
        # Disable verbose websockets
        websocket.enableTrace(False)

        # Create a test connection string to verify connectivity to the sytem
        test_string = {'verify_connection':'AABBCCDD112233'}

        # Send the json request after building it
        self.send_json(self.__build_json__(test_string))
        print "Sending test sequence to server..."

        # recieve the sent json request
        recv = self.sock.recv()

        # verify what we sent is what we got if not send an error
        if  json.loads(recv) == test_string:
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

    def send_string(self, string):
        tmp = dict()
        tmp['string'] = string
        tmp_json = __build_json__(tmp)
        send_json(tmp_json)

    def send_key_value(self, key, value):
        tmp = dict()
        tmp[key] = value
        self.send_dictionary(tmp)

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

        tmp['14:AA:21:2a:abdd4'] = False
        tmp['24:AA:9b:13:7e:db'] = False
        tmp['24:AA:9b:13:7e:dc'] = False
        tmp['B8:AA:8E:1F:B9:dD'] = False

        tmp['38:BB:DA:BF:84:02'] = False
        tmp['24:BB:9b:13:7e:2b'] = False
        tmp['24:BB:9b:13:7e:2c'] = False
        tmp['B8:BB:8E:1F:B9:3D'] = False

        tmp['14:CC:21:2a:abd14'] = False
        tmp['24:CC:9b:13:7e:1b'] = False
        tmp['24:CC:9b:13:7e:1c'] = False
        tmp['B8:CC:8E:1F:B9:1D'] = False

        tmp['38:AB:DA:BF:84:02'] = False
        tmp['24:AB:9b:13:7e:2b'] = False
        tmp['24:AB:9b:13:7e:2c'] = False
        tmp['B8:AB:8E:1F:B9:3D'] = False

        tmp['14:AC:21:2a:abd64'] = False
        tmp['24:AC:9b:13:7e:6b'] = False
        tmp['24:AC:9b:13:7e:6c'] = False
        tmp['B8:AC:8E:1F:B9:6D'] = False

        tmp['38:AD:DA:BF:84:02'] = False
        tmp['24:AD:9b:13:7e:2b'] = False
        tmp['24:AD:9b:13:7e:2c'] = False
        tmp['B8:AD:8E:1F:B9:3D'] = False

        tmp['14:AE:21:2a:abd84'] = False
        tmp['24:AE:9b:13:7e:8b'] = False
        tmp['24:AE:9b:13:7e:8c'] = False
        tmp['B8:AE:8E:1F:B9:8D'] = False

        tmp['14:AF:21:2a:abd94'] = False
        tmp['24:AF:9b:13:7e:9b'] = False
        tmp['24:AF:9b:13:7e:9c'] = False
        tmp['B8:AF:8E:1F:B9:9D'] = False

        tmp['38:BA:DA:BF:84:02'] = False
        tmp['24:BA:9b:13:7e:2b'] = False
        tmp['24:BA:9b:13:7e:2c'] = False
        tmp['B8:BA:8E:1F:B9:3D'] = False

        tmp['38:BC:DA:BF:84:02'] = False
        tmp['24:BC:9b:13:7e:2b'] = False
        tmp['24:BC:9b:13:7e:2c'] = False
        tmp['B8:BC:8E:1F:B9:3D'] = False

        tmp['14:BD:21:2a:abdd4'] = False
        tmp['24:BD:9b:13:7e:db'] = False
        tmp['24:BD:9b:13:7e:dc'] = False
        tmp['B8:BD:8E:1F:B9:dD'] = False

        tmp['38:BE:DA:BF:84:02'] = False
        tmp['24:BE:9b:13:7e:2b'] = False
        tmp['24:BE:9b:13:7e:2c'] = False
        tmp['B8:BE:8E:1F:B9:3D'] = False

        tmp['14:BF:21:2a:abd14'] = False
        tmp['24:BF:9b:13:7e:1b'] = False
        tmp['24:BF:9b:13:7e:1c'] = False
        tmp['B8:BF:8E:1F:B9:1D'] = False

        tmp['38:CA:DA:BF:84:02'] = False
        tmp['24:CA:9b:13:7e:2b'] = False
        tmp['24:CA:9b:13:7e:2c'] = False
        tmp['B8:CA:8E:1F:B9:3D'] = False

        tmp['14:CD:21:2a:abd64'] = False
        tmp['24:CD:9b:13:7e:6b'] = False
        tmp['24:CD:9b:13:7e:6c'] = False
        tmp['B8:CD:8E:1F:B9:6D'] = False

        tmp['38:CE:DA:BF:84:02'] = False
        tmp['24:CE:9b:13:7e:2b'] = False
        tmp['24:CE:9b:13:7e:2c'] = False
        tmp['B8:CE:8E:1F:B9:3D'] = False

        tmp['14:AA:21:2a:abd84'] = False
        tmp['24:AA:9b:13:7e:8b'] = False
        tmp['24:AA:9b:13:7e:8c'] = False
        tmp['B8:AA:8E:1F:B9:8D'] = False

        return tmp

    def test_sleep_time(self):
        tmp = dict()
        tmp['sleep_timer'] = 10
        return tmp