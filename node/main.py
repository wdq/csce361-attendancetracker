import bluetooth, time
import network

search_time = 10

# You can hardcode the desired device ID here as a string to skip the discovery stage
addr = "24:da:9b:13:7e:2b"

print("Welcome to the Bluetooth Detection Demo! \nMake sure your desired Bluetooth-capable device is turned on and discoverable.")
print("The script will now scan for the device %s." % (addr))
print("Feel free to move near and far away from the BeagleBone to see the state change on the LED.\nUse Ctrl+c to exit...")


while True:
    # Try to gather information from the desired device.
    # We're using two different metrics (readable name and data services)
    # to reduce false negatives.
    state = bluetooth.lookup_name(addr, timeout=5)
    services = bluetooth.find_service(address=addr)
    # Flip the LED pin on or off depending on whether the device is nearby
    if state == None and services == []:
        print("No device detected in range...")
    else:
        print("Device detected!")
    # Arbitrary wait time
    time.sleep(0)
