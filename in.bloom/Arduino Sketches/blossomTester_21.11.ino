#include <NewPing.h>
#include <Servo.h>
#include <Adafruit_NeoPixel.h>
#include <Uduino.h>

// HR-SC04 Definitions
#define TRIGGER_PIN_ONE 2
#define ECHO_PIN_ONE 3
#define TRIGGER_PIN_TWO 4
#define ECHO_PIN_TWO 5
#define SONAR_NUM 2
#define MAX_DISTANCE 400

// Servo Definitions
#define SERVO_PIN 11

// LED Definitions
#define NUM_LEDS 59
#define OBJ_LED_PIN 6
#define EXT_LEDS 90
#define EXT_LED_PIN 10

// Sensor Object
NewPing blomSense[SONAR_NUM] = {
  NewPing(TRIGGER_PIN_ONE, ECHO_PIN_ONE, MAX_DISTANCE),
  NewPing(TRIGGER_PIN_TWO, ECHO_PIN_TWO, MAX_DISTANCE)
};

// Servo Object
Servo blomMotor;

// LED Object
Adafruit_NeoPixel blomLeds(NUM_LEDS, OBJ_LED_PIN, NEO_GRBW);
Adafruit_NeoPixel extLeds(EXT_LEDS, EXT_LED_PIN, NEO_GRBW);

// Uduino object
Uduino uduino("blossomArduinoThree"); // Check this first!!

// Global variables
int blomProx;
int blomSenseOne;
int blomSenseTwo;
int oldSensorPos = 0;
int newSensorPos;
int ledSat;
int ledHue = 50;
int ledVal;
int ledDelay = 25;

unsigned long previousRefresh = 0;
const uint8_t uduinoRefresh = 15;
unsigned long previousSensorRefresh = 0;
const uint8_t sensorRefresh = 60;
unsigned long previousLedMotorRefresh = 0;
const uint8_t ledMotorRefresh = 60;

void setup() {

  // Initilize Serial connection
  Serial.begin(9600);

  // Initialize Servo
  blomMotor.attach(SERVO_PIN);
  blomMotor.write(0);

  // Initialize LEDs
  blomLeds.begin();
  blomLeds.show();
  blomLeds.setBrightness(50);
  extLeds.begin();
  extLeds.show();
  extLeds.setBrightness(50);

  // Add Unity commands
  uduino.addCommand("blomTrig", blomTrig);
  uduino.addCommand("readSensor", readSensor);
  
}

void loop() {

  unsigned long currentTime = millis();

  if(currentTime-previousRefresh >= uduinoRefresh){
    // Listen to Unity
    uduino.update();
  }
    
  // Read Sensor
    blomSenseOne = blomSense[0].convert_cm(blomSense[0].ping_median(10));
    delay(100);
    blomSenseTwo = blomSense[1].convert_cm(blomSense[1].ping_median(10));

    if(blomSenseOne == 0){
      blomSenseOne = MAX_DISTANCE;
    }
    if(blomSenseTwo == 0){
      blomSenseTwo = MAX_DISTANCE;
    }

    if(blomSenseOne > blomSenseTwo){
      blomProx = blomSenseTwo;
    }
    else if(blomSenseOne <= blomSenseTwo){
      blomProx = blomSenseOne;
    }
      
    newSensorPos = constrain(map(blomProx, 0, MAX_DISTANCE, 250, 0), 0, 180);
    ledSat = constrain(map(blomProx, 0, MAX_DISTANCE, 300, 50), 0, 255);

    if(oldSensorPos < newSensorPos){
      for(int p = oldSensorPos; p < newSensorPos; p++){
        blomMotor.write(p);
        delay(10);
      }
      oldSensorPos = newSensorPos;
    }
    if(oldSensorPos > newSensorPos){
      for(int p = oldSensorPos; p > newSensorPos; p--){
        blomMotor.write(p);
        delay(10);
      }
      oldSensorPos = newSensorPos;
    }

    ledControl(blomLeds.ColorHSV(ledHue, ledSat, ledVal), extLeds.ColorHSV(ledHue, ledSat, map(ledVal, 10, 255, 100, 255)), 10);
        
}
   



// Functions from Unity
void blomTrig(){

  int parameters = uduino.getNumberOfParameters();

  if(parameters > 0){
    
    //Parse the information accordingly
    ledVal = map(uduino.charToInt(uduino.getParameter(0)), -70, 0, 10, 255); 
  }
}

void ledControl(uint32_t colour, uint32_t extColour, uint8_t delayTime){
  for(int i = 0; i < NUM_LEDS; i++){
    blomLeds.setPixelColor(i, colour);
    blomLeds.show();
    delay(delayTime);
  }
  for(int j = 0; j < EXT_LEDS; j++){
    extLeds.setPixelColor(j, extColour);
    extLeds.show();
    delay(delayTime);
  }
}

void readSensor(){
  Serial.println(map(blomProx, 0, MAX_DISTANCE, 200, 0));
}
