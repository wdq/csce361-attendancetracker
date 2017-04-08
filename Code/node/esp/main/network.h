//Header for the network interface
#ifndef __NETWORK_H__
#define __NETWORK_H__

#include <string.h>

//Includes FreeRTOS
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "freertos/event_groups.h"

//Includes ESP
#include "esp_system.h"
#include "esp_wifi.h"
#include "esp_event_loop.h"
#include "esp_event.h"
#include "esp_log.h"

//TCP/IP stack implementation
#include "lwip/err.h"
#include "lwip/arch.h"
#include "lwip/api.h"
#include "lwip/sockets.h"
#include "lwip/inet.h"
#include "lwip/ip4_addr.h"
#include "lwip/dns.h"
#include "lwip/sys.h"


//typedef
#define string char *
#define json_t char *
#define json_dict_t char *

#define bool uint8_t
#define true 1
#define false 0

#define RX_BUFFER_SIZE 1024
#define TX_BUFFER_SIZE 1024

#define WIFI_DISCONNECT_TIMEOUT 10

#define MAX_PAYLOAD_SIZE 1024

static const int CONNECTED_BIT = BIT0;

typedef struct network_t
{
	bool isConnected;

	//Host to connect to
	string Host;

	char rx_buffer[RX_BUFFER_SIZE];
	char tx_buffer[TX_BUFFER_SIZE]; 

	//Port to connect to
	int Port;

	//WIFI_SSID
	string Wifi_ssid;

	//WIFI_PASSWORD
	string Wifi_password;

	//network socket
	int socket; 

	// socket address structs
	struct sockaddr_in sendAddress;

	//localIP
	ip4_addr_t localIP;

	//serverIP
	ip_addr_t serverIP;


	//node_id
	int node_id;

	bool dnsLock; 


}network_t;


typedef struct pkt_t
{
	int node_id;
	int configurations;
	int payload_size;
	void * payload;
	int crc; 
}pkt_t;



//network ftn's
 bool disconnect(network_t *); //
 bool is_connected(network_t *); //
bool send_key_value(network_t *,string key, string value);
bool verify_connection(network_t *);
int send_json(network_t * , json_t json_message);
int send_map(network_t *, json_dict_t data);
int send_string(network_t *,string data);
json_dict_t get_map_return(network_t *);
int connect_to_server(network_t *);
bool setup_socket(network_t *);
 bool reset_socket_into(network_t *); //
 void setup_wifi(network_t * network); //


// private functions
bool _convert_json_to_map(network_t *,json_t json); 
bool _get_socket(network_t *);
bool _send(network_t *,string data);
bool _test_student_bank(network_t *);
 esp_err_t _wifi_event_handler(void *ctx, system_event_t *event); //
int _recieve_pkts(network_t *);
 void _initialise_wifi(network_t *); //
 pkt_t _recv_pkt(network_t * net);
 bool _setup_ip_by_dns(network_t * ); //

#endif
