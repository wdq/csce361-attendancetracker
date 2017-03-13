import bluetooth, time
import network
import json

from time import sleep
from uuid import getnode as get_mac

# # Student class to hold a student BT mac address and weather or not they are preset.
# class student_t():
#     """docstring for student_t"""
#     def __init__(self, **kwargs):
#         self.kwargs = kwargs
#         self.bluetooth_addr = None
#         self.present        = False

class serv():

    def __init__(self, **kwargs):
        self.kwargs = kwargs

    def setup(self):
        self.url = "ws://echo.websocket.org/"
        self.ip_addr = None
        self.conn_port  = 80
        self.network = network.network(host = self.url, port = self.conn_port)

    def run_system(self):   
        while True:
            #connect to the scheduling server
            self.__connect_to_server__()

            # test connection
            self.network.test_connection()

            #get all students and place them into a dictionary of students
            self.student_dict = self.__get_student_dict__()

            #loop through the students and mark them present or absent
            for student in self.student_dict:

                # Get addr and present status
                addr    = student

                #print address
                print addr

                # using state and service to remove false triggers
                # We might need to change this in the future
                try:
                    state = bluetooth.lookup_name(addr, timeout = 5)
                    # services = bluetooth.find_service(address=addr)
                    services = "15"
                except Exception as e:
                    self.__send_error__(str(e))
                    print ("Sending error to server: " + str(e))
                    continue 
                

                # Detect if the state is detected if it is see if the service is available
                if state != None and services != []:
                    self.student_dict[student] = True
                    print "Student present!"

                # No student detected. Mark as absent
                else:
                    self.student_dict[student] = False
                    print("Student absent! ")

            self.sleep_time = self.__get_sleep_time__()

            # Close the connection because we are not using it
            self.__disconnect_from_server()

            sleep(self.sleep_time)

    # Function to connect to the server
    def __connect_to_server__(self):
        if self.conn_port is None:
            return "Error: No port specified."
        if self.ip_addr is None and self.url is None:
            return "Error: No server address specified."
        
        if self.ip_addr is None:
            self.network.connect() 

        if self.ip_addr is None:    
            self.network.connect() 
        
    def __disconnect_from_server(self):
        self.network.disconnect()
        pass

    # Function used to get the list of mac addresses needed to ping for
    def __get_student_dict__(self):
        self.network.send_dictionary(self.__create_student_list_request__())

        # convert json object to dictionary 
        rtn = self.network.get_dictionary_return()

        # for local testing
        rtn =  self.network.test_student_bank()

        return rtn

    # Function used to create the request to get the BT mac addresses
    def __create_student_list_request__(self):
        # create message to be sent to get the student list to processes
        tmp_dict = dict()
        tmp_dict['request'] = "bt_data_set"
        tmp_dict['node_id'] =  get_mac()

        return tmp_dict

    # Used to create the string needed to request the sleep duration
    def __create_sleep_time_request__(self):
        tmp_dict = dict()
        tmp_dict['request'] = "sleep_time"
        tmp_dict['node_id'] =  get_mac()

        return tmp_dict
        
    def __get_sleep_time__(self):
        # Send the request for a new sleep timer
        self.network.send_dictionary(self.__create_sleep_time_request__())

        # get return from the json request and convert it to a dictionary
        rtn = self.network.get_dictionary_return()

        # Use this for local testing
        rtn = self.network.test_sleep_time()

        return rtn['sleep_timer']

    def __send_error__(self, error):
        tmp = dict()
        tmp['error'] = error
        self.network.send_dictionary(tmp)


if __name__ == "__main__":
    srv = serv(url = "none")

    srv.setup()
    srv.run_system()