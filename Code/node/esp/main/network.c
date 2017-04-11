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

           break;

        default:
            break;
   }
   return ESP_OK;
}

bool is_connected(network_t * net)
{

    return net->isConnected;
}

void setup_wifi(network_t * network)
{

    _initialise_wifi(network);
}

void _initialise_wifi(network_t * net) {
    tcpip_adapter_init();

    #ifdef WIFI_DEBUG
      esp_log_level_set("wifi", ESP_LOG_INFO);
    #endif
    #ifndef WIFI_DEBUG
      esp_log_level_set("wifi", ESP_LOG_ERROR);
    #endif

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
}

bool disconnect(network_t * net)
{
    if(net->isConnected == true)
    {
        esp_wifi_disconnect();
        vTaskDelay(1000* portTICK_PERIOD_MS);
    }

    return net->isConnected;
}

void _found_dns_cb(const char *name, const ip_addr_t *ipaddr, void *callback_arg)
{
	network_t * net = (network_t *)callback_arg;
    net->serverIP = *ipaddr;
    net->dnsLock = true;
}

bool _setup_ip_by_dns(network_t * net)
{

	printf("Node IP Address %s\n",inet_ntoa(net->localIP) );

   IP_ADDR4( &(net->serverIP), 0,0,0,0 );
    printf("Get IP for URL: %s\n", net->Host );
    dns_gethostbyname(net->Host, &net->serverIP, _found_dns_cb, net );

    while(!net->dnsLock)
    {
    	printf("Waiting for DNS lock! \n");
    	vTaskDelay(500*portTICK_PERIOD_MS);
    }
        
    printf( "DNS found: %i.%i.%i.%i\n",
        ip4_addr1(&(net->serverIP).u_addr.ip4),
        ip4_addr2(&net->serverIP.u_addr.ip4),
        ip4_addr3(&net->serverIP.u_addr.ip4),
        ip4_addr4(&net->serverIP.u_addr.ip4) );

   return true;
}

bool setup_socket(network_t * net)
{
  if(net->isConnected == false)
  {
    return false;
  }

  _setup_ip_by_dns(net);

  //Create socket
	net->socket = socket(AF_INET , SOCK_STREAM , 0);
	if (net->socket == -1)
	{
		printf("Could not create socket");
	}

	net->server.sin_addr.s_addr = inet_addr("13.65.210.250");
	net->server.sin_family = AF_INET;
	net->server.sin_port = htons( 989 );


  return true;
}

bool reset_socket_info(network_t * net)
{
  //todo have logic to see if the socket is closed, if so continue else exit

  //Destroy sender address
	close(net->socket);
  memset(&(net->server), 0, sizeof(net->server));

  return true; 
}

int connect_to_server(network_t * net)
{

	if(net->socket == -1)
	{
		printf("re-setting up socket! \n");
		setup_socket(net);
	}

	if (connect(net->socket , (struct sockaddr *)&net->server , sizeof(net->server)) < 0)
		{
			perror("connect failed. Error");
			return 1;
		}

  return 0; 
}

bool disconnect_socket(network_t * net)
{
	if(net->isConnected == true)
		close(net->socket);
	return true;
}

bool _send_pkt(network_t * net, pkt_t * pkt)
{
  if(write(net->socket, &(pkt->node_id), sizeof(pkt->node_id)) <= 0)
  {
    return false; 
  }

  if(write(net->socket, &(pkt->configurations), sizeof(pkt->configurations)) <= 0)
  {
    return false; 
  }

  if(write(net->socket, &(pkt->payload_size), sizeof(pkt->payload_size)) <= 0)
  {
    return false; 
  }

  if(write(net->socket, pkt->payload, pkt->payload_size) <= 0)
  {
    return false; 
  }

  if(write(net->socket, &(pkt->crc), sizeof(pkt->crc)) <= 0)
  {
    return false; 
  }  
  return true;
}
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

pkt_t _recv_pkt(network_t * net)
{
    pkt_t pkt;

    int recv_bytes = recv(net->socket, (void *)net->rx_buffer, sizeof(net->rx_buffer), 0);

    net->rx_buffer[recv_bytes] = '\0';

    printf("Data recieved: %s \n", net->rx_buffer);

    return pkt;
}

string _recv(network_t * net)
{
  pkt_t pkt = _recv_pkt(net);

  printf("Packet recieved. Data contents: %s\n", (string) pkt.payload);

  return (string)pkt.payload;
}


bool verify_connection(network_t * net)
{

	 int sock;
	    struct sockaddr_in server;
	    char message[] = "{\"verify_connection\":\"AABBCCDD112233\"}" , server_reply[2000];



	    char *server_ip = inet_ntoa(*(struct in_addr *) &net->serverIP);

	    printf("Test message: %s\n", message);

	    int rcv = 0;

	    //Create socket
	    sock = socket(AF_INET , SOCK_STREAM , 0);
	    if (sock == -1)
	    {
	        printf("Could not create socket");
	    }
	    server.sin_addr.s_addr = inet_addr(server_ip);
	    server.sin_family = AF_INET;
	    server.sin_port = htons( net->Port );

	    //Connect to remote server
	    if (connect(sock , (struct sockaddr *)&server , sizeof(server)) < 0)
	    {
	        perror("connect failed. Error");
	        close(sock);
	        return 0;
	    }

	    printf("Connected\n");


	        //Send some data
	        if( send(sock , message , strlen(message) , 0) < 0)
	        {
	        	printf("fuck this shit");
	            return 0;
	        }

//	        vTaskDelay();

	        //Receive a reply from the server
	        if( (rcv = recv(sock , server_reply , 2000 , 0) )< 0)
	        {
	        	printf("recv failed");
	        	return 0;
	        }

	        server_reply[rcv] = '\0';

	        printf("Server reply: %s \n", server_reply);

	    close(sock);
	    return 1;
}






