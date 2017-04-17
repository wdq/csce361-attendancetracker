/*
 * bluetooth.h
 *
 *  Created on: Apr 16, 2017
 *      Author: michaelfay95
 */

#ifndef MAIN_BLUETOOTH_H_
#define MAIN_BLUETOOTH_H_

#include "bt.h"
#include "esp_bt_main.h"
#include "esp_bt_device.h"
#include "esp_gap_bt_api.h"
#include "esp_a2dp_api.h"
#include "esp_avrc_api.h"

typedef struct bt_t
{
	char addr[8];
} bt_t;

void init_bt(bt_t *);
void connect_to_device(char *);

#endif /* MAIN_BLUETOOTH_H_ */
