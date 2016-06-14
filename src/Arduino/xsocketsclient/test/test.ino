#include <XSocketClient.h>

#include <Ethernet.h>
#include <SPI.h>

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
XSocketClient client;

void setup() {
  Serial.begin(9600);
  Ethernet.begin(mac);
  Serial.println("Start to connect");
  if(client.connect("10.0.0.8",4502)){
      Serial.println("Connected!");
  }
  else{
      Serial.println("Error connecting");
  }
  
  client.setOnMessageDelegate(onMessage);
  delay(2000);
}

void loop() {
  client.receiveData();
  // call the data method on the sensor controller and pass the value 12
  client.send("sensor","data","12");
  delay(500);
}

void onMessage(XSocketClient client, String data) {
  Serial.println(data);
}
