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

network_t network; 

void ping_task(void * pvParameters)
{
  uint64_t ping_count = 0;
  while(1)
  {
    printf("Ping task is called. %llu\n", ping_count);
    vTaskDelay(500 / portTICK_PERIOD_MS);
    ping_count ++;
  }
}

void mark_attendance_task(void * pvParameters)
{
  while(1)
  {
    printf("mark_attendance_task task is called. \n");
    vTaskDelay(2000 / portTICK_PERIOD_MS);
  }
}

void app_main() {
   nvs_flash_init();


   network.Wifi_ssid = "PasswordisTaco";
   network.Wifi_password = "";
//   network.Host = "attend.ddns.net";
//   network.Port = 989;

      network.Host = "echo.websocket.org";
      network.Port = 80;


   setup_wifi(&network);


   printf("Waiting for wifi to connect .");
     while(!is_connected(&network))
     {

        printf(" . ");
        vTaskDelay(250 * portTICK_PERIOD_MS);
     }

     printf("\n");


   while(1)
        {
     	   if(verify_connection(&network))
     	   {
     		   printf("Connection is successful! \n");
     	   }
     	   else
     	   {
     		   printf("Shit is fucked real bad! \n");
     	   }
     	         vTaskDelay(1000 * portTICK_PERIOD_MS);
        }




   setup_socket(&network);



   xTaskCreate(&ping_task, "ping_task", 2048, NULL, 5, NULL);
   xTaskCreate(&mark_attendance_task, "mark_attendance_task", 2048, NULL, 5, NULL);

}



