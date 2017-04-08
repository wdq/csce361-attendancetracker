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

  /* create the socket */
  net->socket = lwip_socket(AF_INET, SOCK_STREAM, 0);
  LWIP_ASSERT("s >= 0", net->socket >= 0);

  return true;
}

bool reset_socket_info(network_t * net)
{
  //todo have logic to see if the socket is closed, if so continue else exit

  //Destroy sender address
  memset(&(net->sendAddress), 0, sizeof(net->sendAddress));

  return true; 
}

int connect_to_server(network_t * net)
{

	if(net->socket == -1)
	{
		printf("re-setting up socket! \n");
		setup_socket(net);
	}

	int ret;

	/* connect */
  ret = lwip_connect(net->socket, (struct sockaddr*)&net->sendAddress, sizeof(net->sendAddress));
  /* should succeed */
  printf("lwip_connect return: %d\n", ret);
  LWIP_ASSERT("ret == 0", ret == 0);

  return 0; 
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


//bool verify_connection(network_t * net)
//{
//	string tmp = "\"verify_connection\":\"AABBCCDD112233\"";
//
//	int tmp_recv;
//
//	connect_to_server(net);
//
//	if((tmp_recv = write(net->socket, tmp, sizeof(*tmp))) < 0)
//	  {
//		printf("WTF %d\n", tmp_recv);
//		return false;
//	  }
//
//	int recv_bytes = recv(net->socket, (void *)net->rx_buffer, sizeof(net->rx_buffer), 0);
//
//	net->rx_buffer[recv_bytes] = '\0';
//
//	printf("Returned data: %s", net->rx_buffer);
//
//	if(recv_bytes < sizeof(tmp) )
//		  {
//			return false;
//		  }
//
//
//	return true;
//
//}



bool verify_connection(network_t * net)
{

	int lSocket;
	struct sockaddr_in sLocalAddr;

	lSocket = lwip_socket(AF_INET, SOCK_STREAM, 0);
	if (lSocket < 0) return false;

	memset((char *)&sLocalAddr, 0, sizeof(sLocalAddr));
	sLocalAddr.sin_family = AF_INET;
	sLocalAddr.sin_len = sizeof(sLocalAddr);
	sLocalAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	sLocalAddr.sin_port = htons(989;

	if (lwip_bind(lSocket, (struct sockaddr *)&sLocalAddr, sizeof(sLocalAddr)) < 0) {
	        lwip_close(lSocket);
	        printf("WTF\n");
	        return false;
	}

	if ( lwip_listen(lSocket, 20) != 0 ){
	        lwip_close(lSocket);
	        printf("SHIT\n");
	        return false;
	}

	while (1) {
	        int clientfd;
	        struct sockaddr_in client_addr;
	        u32_t addrlen=sizeof(client_addr);
	        char buffer[1024];
	        int nbytes;


	        printf("TITS3\n");
	        vTaskDelay(500*portTICK_PERIOD_MS);

	        printf("TITS\n");
	        clientfd = lwip_accept(lSocket, (struct sockaddr*)&client_addr, (socklen_t *)&addrlen);

	        printf("TITS1\n");
	        if (clientfd>0){
	        	printf("HELL\n");
	            do{
	                nbytes=lwip_recv(clientfd, buffer, sizeof(buffer),0);
	                if (nbytes>0) lwip_send(clientfd, buffer, nbytes, 0);
	                printf("Bitch\n");
	            }  while (nbytes>0);

	             lwip_close(clientfd);
	          }
	        else
	        {
	        	printf("BALLZ\n");
	        }
//	        vTaskDelay(500*portTICK_PERIOD_MS);
	    }
	    lwip_close(lSocket);

	    return true;


}











