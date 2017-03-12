import bluetooth, time
import network
import json

# Student class to hold a student BT mac address and weather or not they are preset.
class student_t():
    """docstring for student_t"""
    def __init__(self, arg):
        super(student, self).__init__()
        self.arg = arg
        self.bluetooth_addr = ""
        self.present        = False

class serv():

    def __init__(self, arg):
        super(serv, self).__init__()
        self.arg = arg

    def setup(self):
        self.url = None
        self.ip_addr = None
        self.conn_port  = None
        self.network = None

    def run_system(self):   
        while True:
            #conncet to the scheduling server
            self.__connect_to_server__()

            #get all students and place them into a dictionary of students
            self.student_dict = self.__get_student_list__()

            #loop through the students and mark them present or absent
            for student in student_dict:

                # Get addr and present status
                addr    = student.bluetooth_addr

                #print address
                print addr

                # using state and service to remove false triggers
                # We might need to change this in the future
                state = bluetooth.lookup_name(addr)
                services = bluetooth.find_service(address=addr)

                # Detect if the state is detected if it is see if the service is available
                if state != None and services != []:
                    student.present = True
                    print "Student present!"

                # No student detected. Mark as absent
                else:
                    student.present = False
                    print("Student absent! ")


            # Needs to be implemented
            self.__disconnect_from_server__()

            sleep(self.sleep_time)

    # Function to connect to the server
    def __connect_to_server__(self):
        if self.conn_port is None:
            return "Error: No port specified."
        if self.ip_addr is None and self.url is None:
            return "Error: No server address specified."
        
        if self.ip_addr is None:
            net.connect(host = self.url, port = self.conn_port) 

        if self.ip_addr is None:    
            net.connect(host = self.url, port = self.conn_port) 
        

    # Function used to get the list of mac addresses needed to ping for
    def __get_student_list__(self):
        rtn_value = net.send(self.__create_student_list_request__)

        # convert json object to dictionary 

        return json_return

    # Function used to create the request to get the BT mac addresses
    def __create_student_list_request__(self):
        pass

        # create message to be sent to get the student list to processes


    # Used to create the string needed to request the sleep duration
    def __create_sleep_time_request__(self):
        pass

    # Used to 
    def __create_json_send__(self, student_dict):

        output_dict = dict()
        
        for student in student_dict:
            if student is student_t:
                output_string  = "{"
                output_string += ("\"bt_address:\"" + student.bluetooth_addr)
                output_string += ("\"present:\"" + student.present)
                output_string += "}"

                output_dict.append(output_string)
        


if __name__ == "__main__":
    srv = serv()

    srv.setup()
    srv.run_system()