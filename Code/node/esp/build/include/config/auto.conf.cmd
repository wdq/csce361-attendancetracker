deps_config := \
	/Users/michaelfay95/esp/esp-idf/components/aws_iot/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/bt/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/esp32/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/ethernet/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/fatfs/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/freertos/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/log/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/lwip/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/mbedtls/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/openssl/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/spi_flash/Kconfig \
	/Users/michaelfay95/esp/esp-idf/components/bootloader/Kconfig.projbuild \
	/Users/michaelfay95/esp/esp-idf/components/esptool_py/Kconfig.projbuild \
	/Users/michaelfay95/esp/esp-idf/components/partition_table/Kconfig.projbuild \
	/Users/michaelfay95/esp/esp-idf/Kconfig

include/config/auto.conf: \
	$(deps_config)


$(deps_config): ;
