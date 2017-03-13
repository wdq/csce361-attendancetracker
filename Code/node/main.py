import bluetooth, time
import network
import json
import requests

from time import sleep
from uuid import getnode as get_mac

class serv():

    def __init__(self, **kwargs):
        self.kwargs = kwargs

    def setup(self):
        self.url = 'ws://echo.websocket.org'
        self.ip_addr = None
        self.conn_port  = 3128
        self.network = network.network(host = self.url, port = self.conn_port)

    def run_system(self):   
        while True:
            start_time = time.time()

            #connect to the scheduling server
            self.__connect_to_server__()

            # test connection
            self.network.test_connection()

            # send ip address to server 
            self.__send_ip_addr__()

            #get all students and place them into a dictionary of students
            self.student_dict = self.__get_student_dict__()

            #loop through the students and mark them present or absent
            for student in self.student_dict:

                # using state and service to remove false triggers
                # We might need to change this in the future
                try:
                    state = bluetooth.lookup_name(addr, timeout=15)
                    # services = bluetooth.find_service(address=addr)
                except Exception as e:
                    self.__send_error__(str(e))
                    print ("Sending error to server: " + str(e))
                    continue 
                
                # print state

                # Detect if the state is detected if it is see if the service is available
                if state != None:
                    self.student_dict[student] = True
                    print "Student present!"
                    # print state

                # No student detected. Mark as absent
                else:
                    self.student_dict[student] = False
                    print("Student absent! ")

                # self.network.send_key_value(key = student, value = self.student_dict[student])

            self.sleep_time = self.__get_sleep_time__()
                
            self.network.send_dictionary(self.student_dict)

            end_time = time.time()

            tmp = {
                "total_time_executed": (-start_time+end_time),
                "average_time_per_device":((-start_time+end_time)/len(self.student_dict))
                }

            self.network.send_dictionary(tmp)

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
       
    #Get the sleep duration 
    def __get_sleep_time__(self):
        # Send the request for a new sleep timer
        self.network.send_dictionary(self.__create_sleep_time_request__())

        # get return from the json request and convert it to a dictionary
        rtn = self.network.get_dictionary_return()

        # Use this for local testing
        rtn = self.network.test_sleep_time()

        return rtn['sleep_timer']

    #If an error occurs then post the error to the server to view its logs
    def __send_error__(self, error):
        tmp = dict()
        tmp['error'] = error
        self.network.send_dictionary(tmp)

    def __send_ip_addr__(self):
        public_ip = requests.get('http://ip.42.pl/raw').text
        print public_ip
        self.network.send_key_value("node_public_ip", public_ip)


if __name__ == "__main__":
    srv = serv(url = "none")

    srv.setup()
    srv.run_system()