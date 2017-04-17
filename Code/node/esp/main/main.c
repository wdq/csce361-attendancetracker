/* Hello World Example

 This example code is in the Public Domain (or CC0 licensed, at your option.)

 Unless required by applicable law or agreed to in writing, this
 software is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 CONDITIONS OF ANY KIND, either express or implied.
 */
#include <stdio.h>
#include <string.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "freertos/event_groups.h"
#include "esp_system.h"
#include "esp_wifi.h"
#include "esp_event_loop.h"
#include "esp_log.h"
#include "nvs_flash.h"
#include "driver/ledc.h"
#include "lwip/err.h"
#include "lwip/arch.h"
#include "lwip/api.h"

#include "network.h"
#include "bluetooth.h"

network_t network; 

SemaphoreHandle_t net_sem;

void ping_task(void * pvParameters)
{
  uint64_t ping_count = 0;
  char message[256];
  while(1)
  {
	snprintf(message, sizeof(message), "Ping:%llu\n",ping_count);
    printf(message);
    vTaskDelay(10 * portTICK_PERIOD_MS);
    while(xSemaphoreTake(net_sem,0) != pdTRUE)
    	  {
    		  vTaskDelay(5 * portTICK_PERIOD_MS);
    	  }
    if(connect_to_server(&network) != -1)
	{
		send_string(&network, message);
		ping_count ++;
	}
    disconnect_from_server(&network);
    xSemaphoreGive(net_sem);

  }
}

void mark_attendance_task(void * pvParameters)
{
	  char message[256];
	  while(1)
	  {
		  snprintf(message, sizeof(message), "mark_attendance_task task is called. \n");
		printf(message);
		vTaskDelay(100 * portTICK_PERIOD_MS);
		while(xSemaphoreTake(net_sem,0) != pdTRUE)
			  {
				  vTaskDelay(5 * portTICK_PERIOD_MS);
			  }
		if(connect_to_server(&network) != -1)
		{
			send_string(&network, message);
		}
		disconnect_from_server(&network);
		xSemaphoreGive(net_sem);

	  }
}

void app_main() {
   nvs_flash_init();

   net_sem = xSemaphoreCreateMutex();

   network.Wifi_ssid = "PasswordisTaco";
   network.Wifi_password = "";

   network.Host = "cse-iis.quade.co";
   network.Port = 989;



   setup_wifi(&network);


   printf("Waiting for wifi to connect .");
     while(!is_connected(&network))
     {

        printf(" . ");
        vTaskDelay(250 * portTICK_PERIOD_MS);
     }

     printf("\n");


     setup_socket(&network);

     //wait until the connection is verified. Once the connection is verified then continue
     while(!verify_connection(&network));


   xTaskCreate(&ping_task, "ping_task", 2048, NULL, 6, NULL);
   xTaskCreate(&mark_attendance_task, "mark_attendance_task", 2048, NULL, 5, NULL);

}



