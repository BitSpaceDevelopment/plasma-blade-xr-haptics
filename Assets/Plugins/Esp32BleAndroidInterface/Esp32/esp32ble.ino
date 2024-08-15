#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

BLEServer* pServer = NULL;
BLECharacteristic* pCharacteristic = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
uint32_t value = 0;

#define SERVICE_UUID        "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"

char formatted_msg[128];
int degx[8 * 12];
int degy[8 * 12] ;

char param[128];
float x = 0.12;
float y = 0.13;
char z[] = "0.14";
//char Str4[ ] = "arduino";

class Esp32ServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      deviceConnected = true;
      value=0;
      BLEDevice::startAdvertising();
    };

    void onDisconnect(BLEServer* pServer) {
      deviceConnected = false;
    }
};

void generatedata() {
  int point, x, y;
  
  point = 0;
  for (x = -40; x < 40; x += 10) {
    for (y = -30; y < 30; y += 5) {
      degx[point] = x;
      degy[point] = y;
//      sprintf(param, "degx:%d degy:%d", degx[point], degy[point]);
//      Serial.println(param);
      point ++;
    }
  }
}


void setup() {
  Serial.begin(115200);

  generatedata();

  // Create the BLE Device
  BLEDevice::init("Esp32Ble");

  // Create the BLE Server
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new Esp32ServerCallbacks());

  // Create the BLE Service
  BLEService *pService = pServer->createService(SERVICE_UUID);

  // Create a BLE Characteristic
  pCharacteristic = pService->createCharacteristic(
                      CHARACTERISTIC_UUID,
                      BLECharacteristic::PROPERTY_READ   |
                      BLECharacteristic::PROPERTY_WRITE  |
                      BLECharacteristic::PROPERTY_NOTIFY |
                      BLECharacteristic::PROPERTY_INDICATE
                    );

  pCharacteristic->addDescriptor(new BLE2902());

  // Start the service
  pService->start();

  // Start advertising
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(false);
  pAdvertising->setMinPreferred(0x0);  // set value to 0x00 to not advertise this parameter
  BLEDevice::startAdvertising();
  Serial.println("Waiting a client connection to notify...");
}

void loop() {
  char sx[7];
  char sy[7];

  // notify changed value
  if (deviceConnected) {
    //    pCharacteristic->setValue((uint8_t*)&value, 4);
    sprintf(param, "degx:%d degy:%d", degx[value], degy[value]);
    Serial.println(param);

    x = sin(degx[value] / (180 / PI));
    y = sin(degy[value] / (180 / PI));
    dtostrf(x, 4, 2, sx);
    dtostrf(y, 4, 2, sy);

    sprintf(formatted_msg, "%s,%s,%s,", sx, sy, z);
    Serial.println(formatted_msg);
    int i = 0;
    for (i = 0; formatted_msg[i] != '\0'; ++i);
    formatted_msg[i] = 12;
    formatted_msg[i+1] = 34;
    i+= 2;
  
    for (int k = 0; k < i; k++) {
      Serial.print((uint8_t)formatted_msg[k]);
      Serial.print(" ");
    }
    Serial.println("");
    
    pCharacteristic->setValue((uint8_t*)formatted_msg, i);
    pCharacteristic->notify();
    value++;
    if(value >= (8*12)){
      value=0;
    }
    delay(1000); 
  }
  // disconnecting
  if (!deviceConnected && oldDeviceConnected) {
    delay(500); // give the bluetooth stack the chance to get things ready
    pServer->startAdvertising(); // restart advertising
    Serial.println("start advertising");
    oldDeviceConnected = deviceConnected;
  }
  // connecting
  if (deviceConnected && !oldDeviceConnected) {
    // do stuff here on connecting
    oldDeviceConnected = deviceConnected;
  }
}
