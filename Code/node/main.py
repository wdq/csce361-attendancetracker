import bluetooth, time
import network
import json
import requests
from threading import Thread
from time import sleep
from uuid import getnode as get_mac

# # Student class to hold a student BT mac address and weather or not they are preset.

class serv():

    def __init__(self, **kwargs):
        self.kwargs = kwargs

    def setup(self):
        self.url = 'ws://attend.ddns.net:989/node'
        # self.url = 'ws://192.168.0.50:989/node'
        self.ip_addr = None
        self.conn_port  = 989
        self.network = network.network(host = self.url, port = self.conn_port)

        self.stay_alive_count = 0

    def run_system(self, **kwargs):   

        

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

            # Close the connection because we are not using it
            self.__disconnect_from_server()

            #loop through the students and mark them present or absent
            for student in self.student_dict:

                # Get addr and present status
                addr    = student

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


            

            self.student_dict["bt_scan_results"] = True

            # connect to the scheduling server
            self.__connect_to_server__()
                
            self.network.send_dictionary(self.student_dict)

            # Close the connection because we are not using it
            self.__disconnect_from_server()

            end_time = time.time()

            

            tmp = {
                "node_stats"        : str(True),
                "total_time_executed": str((-start_time+end_time)),
                "average_time_per_device":str(((-start_time+end_time)/len(self.student_dict)))
                }

            self.__connect_to_server__()

            self.network.send_dictionary(tmp)

            self.__disconnect_from_server()


            self.sleep_time = int(self.__get_sleep_time__())

            # print self.sleep_time
            sleep(self.sleep_time)

    # Function to connect to the server
    def __connect_to_server__(self):
        if self.conn_port is None:
            return "Error: No port specified."
        if self.ip_addr is None and self.url is None:
            return "Error: No server address specified."
        
        self.network.connect()

        
    def __disconnect_from_server(self):
        
        self.network.disconnect()
        pass

    # Function used to get the list of mac addresses needed to ping for
    def __get_student_dict__(self):
        self.network.send_dictionary(self.__create_student_list_request__())

        # convert json object to dictionary 
        rtn = self.network.get_dictionary_return()

        # # for local testing
        # rtn =  self.network.test_student_bank()

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
        # rtn = self.network.test_sleep_time()

        try:
            return rtn['sleep_timer']
        except:
            print "Error get sleep time. Using default."
            return 5

    #If an error occurs then post the error to the server to view its logs
    def __send_error__(self, error):
        tmp = dict()
        tmp['error'] = error
        self.network.send_dictionary(tmp)

    def __send_ip_addr__(self):
        public_ip = requests.get('http://ip.42.pl/raw').text
        print public_ip
        self.network.send_key_value("node_public_ip", public_ip)

    def ping_home(self):
        while True:
            self.__connect_to_server__()
            self.network.send_key_value("ping", str(self.stay_alive_count))
            self.__disconnect_from_server()
            self.stay_alive_count += 1
            sleep(1)


if __name__ == "__main__":
    srv = serv(url = "none")
    srv.setup()
    srv.run_system()

    attend  = Thread(target = srv.run_system)
    ping    = Thread(target = srv.ping_home)

    attend.start()
    ping.start()
    Thread.join()

    while(raw_input("") != "q"):
        pass

    attend.kill()
    ping.kill()