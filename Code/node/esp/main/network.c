#include "network.h"

esp_err_t _wifi_event_handler(void *ctx, system_event_t *event) {
  //cast ctx to net type
  network_t * net = (network_t *) ctx;


   switch (event->event_id) {
       case SYSTEM_EVENT_STA_START:
          esp_wifi_connect();
          break;

       case SYSTEM_EVENT_STA_GOT_IP:
          net->localIP = event->event_info.got_ip.ip_info.ip;
           net->isConnected = true;
           break;

       case SYSTEM_EVENT_STA_DISCONNECTED:
           net->isConnected = false;
           close(net->socket);

           break;

        default:
            break;
   }
   return ESP_OK;
}

/********WIFI NETWORK CONFIGS AND FUNCTIONS**********/

//public function to setup wifi
void setup_wifi(network_t * network)
{
	wifi_sem = xSemaphoreCreateMutex();
    _initialise_wifi(network);
}

//private function that inits wifi
void _initialise_wifi(network_t * net) {
    tcpip_adapter_init();

    #ifdef WIFI_DEBUG
      esp_log_level_set("wifi", ESP_LOG_INFO);
    #endif
    #ifndef WIFI_DEBUG
      esp_log_level_set("wifi", ESP_LOG_ERROR);
    #endif

    while(xSemaphoreTake(wifi_sem,0) != pdTRUE);

    ESP_ERROR_CHECK(esp_event_loop_init(_wifi_event_handler, (void *) net));

    wifi_init_config_t cfg = WIFI_INIT_CONFIG_DEFAULT();
    ESP_ERROR_CHECK(esp_wifi_init(&cfg));
    ESP_ERROR_CHECK(esp_wifi_set_storage(WIFI_STORAGE_RAM));

    printf("Wifi SSID: %s \n Wifi Password: %s \n", net->Wifi_ssid, net->Wifi_password);

    wifi_config_t wifi_config = {
            .sta = {
                .password = "Lemur3spelledout",
				.ssid = "PasswordisTaco"
            },
        };

    ESP_ERROR_CHECK(esp_wifi_set_mode(WIFI_MODE_STA));
    ESP_ERROR_CHECK(esp_wifi_set_config(WIFI_IF_STA, &wifi_config));
    ESP_ERROR_CHECK(esp_wifi_start());

    xSemaphoreGive(wifi_sem);
}

//public function that returns wifi connectivity status
bool is_connected(network_t * net)
{
    return net->isConnected;
}

//public function that disconnects from wifi
bool disconnect(network_t * net)
{
	while(xSemaphoreTake(wifi_sem,0) != pdTRUE);
    if(net->isConnected == true)
    {
        esp_wifi_disconnect();
        vTaskDelay(1000* portTICK_PERIOD_MS);
    }

    xSemaphoreGive(wifi_sem);

    return net->isConnected;
}

//private function that finds the dns of the host and sets net's serverIPString
bool _setup_ip_by_dns(network_t * net)
{

	printf("Node IP Address %s\n",inet_ntoa(net->localIP) );

	while(xSemaphoreTake(wifi_sem,0) != pdTRUE);
	IP_ADDR4( &(net->serverIP), 0,0,0,0 );
    printf("Get IP for URL: %s\n", net->Host );
    dns_gethostbyname(net->Host, &net->serverIP, _found_dns_cb, net );

    while(!net->dnsLock)
    {
//    	printf("Waiting for DNS lock! \n");
    	vTaskDelay(500*portTICK_PERIOD_MS);
    }
        
    printf( "DNS found: %i.%i.%i.%i\n",
        ip4_addr1(&(net->serverIP).u_addr.ip4),
        ip4_addr2(&net->serverIP.u_addr.ip4),
        ip4_addr3(&net->serverIP.u_addr.ip4),
        ip4_addr4(&net->serverIP.u_addr.ip4) );

   net->serverIPString = inet_ntoa(*(struct in_addr *) &net->serverIP);

   xSemaphoreGive(wifi_sem);

   return true;
}

//private function used for the dns callback
void _found_dns_cb(const char *name, const ip_addr_t *ipaddr, void *callback_arg)
{
	network_t * net = (network_t *)callback_arg;
    net->serverIP = *ipaddr;
    net->dnsLock = true;
}


/********SOCKET CONFIGS AND FUNCTIONS**********/

//public function that configures the socket parameters
bool setup_socket(network_t * net)
{
  if(net->isConnected == false)
  {
    return false;
  }

  _setup_ip_by_dns(net);

	net->server.sin_addr.s_addr = inet_addr(net->serverIPString);
	net->server.sin_family = AF_INET;
	net->server.sin_port = htons( net->Port );

	net->socket_sem = xSemaphoreCreateMutex();


  return true;
}

//public function that resets the socket parameters
bool reset_socket_info(network_t * net)
{

	xSemaphoreTakeRecursive(net->socket_sem, portTICK_PERIOD_MS * 1000);
	perror("Deleting socket");
	//Destroy sender address
	close(net->socket);
	memset(&(net->server), 0, sizeof(net->server));

	//reset socket params
	setup_socket(net);

	xSemaphoreGiveRecursive(net->socket_sem);

  return true; 
}

//public function to disconnect from the current socket
bool disconnect_socket(network_t * net)
{
	xSemaphoreTakeRecursive(net->socket_sem, portTICK_PERIOD_MS * 1000);
	while(xSemaphoreTake(wifi_sem,0) != pdTRUE);
	if(net->isConnected == true)
		close(net->socket);
	xSemaphoreGive(wifi_sem);
	xSemaphoreGiveRecursive(net->socket_sem);

	return true;
}


/********DATA TRANSPORT FUNCTIONS**********/

//public function that connects to the scheduler service server
int connect_to_server(network_t * net)
{
	if(net->socket != -1)
		close(net->socket);

	if(net->_socketConnected == true)
	{
		net->_socketConnected = false;
		connect_to_server(net);
	}

	while(xSemaphoreTake(net->socket_sem,0) != pdTRUE);
	//Create socket
	net->socket = socket(AF_INET , SOCK_STREAM , 0);
	if (net->socket == -1)
	{
		printf("Could not create socket");
	}

	//Connect to remote server
	if (connect(net->socket , (struct sockaddr *)&net->server , sizeof(net->server)) < 0)
	{
		perror("connect failed. Error");
		printf("trying to reconnect to the server!");
		close(net->socket);
		xSemaphoreGive(net->socket_sem);
		return -1;

	}

	net->_socketConnected = true;

	xSemaphoreGive(net->socket_sem);

	return 1;
}

//disconnects from the server
int disconnect_from_server(network_t * net)
{
	while(xSemaphoreTake(net->socket_sem,0) != pdTRUE);
	close(net->socket);
	net->_socketConnected = false;
	xSemaphoreGive(net->socket_sem);
	return 1;
}

//public function that sends a given string. This does not use any packet structure yet.
int send_string(network_t * net, string data)
{
	if(net->_socketConnected == false)
	{
		return -1;
	}

	_lock_network_conn(net);
	//Send some data
	if( send(net->socket , data , strlen(data) , 0) < 0)
	{
		_release_network_conn(net);
		return 0;
	}
	_release_network_conn(net);

	return 1;
}

//public function that recieves the raw data back. Returns the number of bytes recieved
int recieve_string(network_t * net)
{
	int rcv;
	//Receive a reply from the server
	_lock_network_conn(net);
	if( (rcv = recv(net->socket , net->rx_buffer , sizeof(net->rx_buffer) , 0) )< 0)
	{
		printf("recv failed");
		return 0;
	}
	net->rx_buffer[rcv] = '\0';
	_release_network_conn(net);

	return rcv;
}

//private function that sends a packet
//TODO refactor this to not be shitty
bool _send_pkt(network_t * net, pkt_t * pkt)
{

	_lock_network_conn(net);
  if(write(net->socket, &(pkt->node_id), sizeof(pkt->node_id)) <= 0)
  {
	  _release_network_conn(net);
    return false; 
  }

  if(write(net->socket, &(pkt->configurations), sizeof(pkt->configurations)) <= 0)
  {
	  _release_network_conn(net);
    return false; 
  }

  if(write(net->socket, &(pkt->payload_size), sizeof(pkt->payload_size)) <= 0)
  {
	  _release_network_conn(net);
    return false; 
  }

  if(write(net->socket, pkt->payload, pkt->payload_size) <= 0)
  {
	  _release_network_conn(net);
    return false; 
  }

  if(write(net->socket, &(pkt->crc), sizeof(pkt->crc)) <= 0)
  {
	  _release_network_conn(net);
    return false; 
  }  
  _release_network_conn(net);
  return true;
}

//private function that constructs the packet from a string of data.
bool _send(network_t * net, string data)
{
  pkt_t pkt;

  memset(&pkt, 0, sizeof(pkt));

  //generate CRC
  pkt.crc = 0xAABBCCDD;

  //calculate payload size
  pkt.payload_size = sizeof(data);

  //create packet
  pkt.node_id = net->node_id;
  pkt.payload = (void *) data;
  
  //send data
  return _send_pkt(net, &pkt);
}

//private function that constructs the recieved packet
pkt_t _recv_pkt(network_t * net)
{
    pkt_t pkt;
    _lock_network_conn(net);
    int recv_bytes = recv(net->socket, (void *)net->rx_buffer, sizeof(net->rx_buffer), 0);

    net->rx_buffer[recv_bytes] = '\0';
    _release_network_conn(net);

    printf("Data recieved: %s \n", net->rx_buffer);

    return pkt;
}

//private function that returns the data portion of the packet.
string _recv(network_t * net)
{
  pkt_t pkt = _recv_pkt(net);

  printf("Packet recieved. Data contents: %s\n", (string) pkt.payload);

  return (string)pkt.payload;
}


//function to validate configurations to the scheduling server
bool verify_connection(network_t * net)
{
	char message[] = "{\"verify_connection\":\"AABBCCDD112233\"}";
	int rcv_bytes = 0;

	bool return_val = false;;

	printf("Testing connection to scheduling server!\n");

	if(connect_to_server(net) == -1) return false;
	send_string(net, message);
	if((rcv_bytes = recieve_string(net)) > 0)
	{
		if(strcmp(message, net->rx_buffer) == 0)
		{
			return_val = true;
		}
	}

	disconnect_from_server(net);


	return return_val;
}

void _lock_network_conn(network_t * net)
{
	while(xSemaphoreTake(wifi_sem,0) != pdTRUE);
	while(xSemaphoreTake(net->socket_sem,0) != pdTRUE);

}

void _release_network_conn(network_t * net)
{
	xSemaphoreGive(wifi_sem);
	xSemaphoreGive(net->socket_sem);
}


