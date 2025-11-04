#include <WiFiS3.h>
#include "DHT.h"
#include <Arduino_JSON.h> 

#define DHTPIN 4
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);

// WiFi credentials
char ssid[] = "***";
char pass[] = "***"; 


char api_server[] = "192.168.1.141";
int api_port = 5091;


WiFiClient client;

void setup() {
  Serial.begin(115200);
  delay(1000); 

  Serial.println("Connecting to WiFi...");
  WiFi.begin(ssid, pass);

  int attempts = 0;
  const int maxAttempts = 20; 
  while (WiFi.status() != WL_CONNECTED && attempts < maxAttempts) {
    delay(2000);
    Serial.print(".");
    attempts++;
  }

  if (WiFi.status() == WL_CONNECTED) {
    delay(2000);
    IPAddress ip = WiFi.localIP();
    Serial.println("\nConnected!");
    Serial.print("Arduino IP Address: ");
    Serial.println(ip);
    Serial.print("API Address: http://");
    Serial.print(api_server);
    Serial.print(":");
    Serial.println(api_port);
  } else {
    Serial.println("\nFailed to connect to WiFi!");
    while (true) delay(1000);
  }

  dht.begin(); 
}

void loop() {
  
  float humidity = dht.readHumidity();
  float temperature = dht.readTemperature();

  if (isnan(humidity) || isnan(temperature)) {
    Serial.println("Failed to read from DHT sensor!");
    delay(5000); 
    return;
  }


  JSONVar jsonData;
  jsonData["humidity"] = (double)humidity;
  jsonData["temperature"] = (double)temperature;
  jsonData["deviceId"] = "uno-r4-main";

  String jsonString = JSON.stringify(jsonData);

  Serial.println("--------------------");
  Serial.print("Connecting to API at ");
  Serial.print(api_server);
  Serial.print(":");
  Serial.println(api_port);

 
  if (client.connect(api_server, api_port)) {
    Serial.println("Connected to API!");
    client.println("POST /api/sensor/reading HTTP/1.1");
    client.print("Host: ");
    client.println(api_server);
    client.println("Content-Type: application/json");
    client.print("Content-Length: ");
    client.println(jsonString.length());
    client.println("Connection: close");
    client.println(); 
    
    client.println(jsonString);

    Serial.println("Data sent:");
    Serial.println(jsonString);

    Serial.println("API Response:");
    while (client.connected()) {
      if (client.available()) {
        String line = client.readStringUntil('\n');
        Serial.println(line);
      }
    }
    client.stop();

  } else {
    Serial.println("API connection failed!");
  }

  Serial.println("Waiting 30 seconds...");
  delay(30000); 
}
