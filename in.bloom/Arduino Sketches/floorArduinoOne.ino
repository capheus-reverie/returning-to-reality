#include <FastLED.h>
#include <Uduino.h>

// Define connections

#define NUM_LEDS 300
#define ExtraDATA_PIN 5
#define CloseDATA_PIN 6
#define FarDATA_PIN 7

CRGB closeLeds[NUM_LEDS];
CRGB farLeds[NUM_LEDS];

// red = 240 - 15
// orange = 16 - 47
// yellow = 48 - 79
// green = 80 - 111
// aqua = 112 - 143
// blue = 144 - 175
// purple = 176 - 207
// pink = 208 - 239ß

Uduino uduino("floorArduinoOne"); // Change this to the appropriate number

void setup() {
  
  Serial.begin(9600);

  // Define Interaction Functions
  uduino.addCommand("indIntClose", indIntClose);
  uduino.addCommand("indIntFar", indIntFar);
  uduino.addCommand("grpIntClose", grpIntClose);
  uduino.addCommand("grpIntFar", grpIntFar);
  uduino.addCommand("grpIntBoth", grpIntBoth);
  uduino.addCommand("supGrpInt", supGrpInt);

  FastLED.addLeds<WS2812B, CloseDATA_PIN>(closeLeds, NUM_LEDS);
  FastLED.addLeds<WS2812B, FarDATA_PIN>(farLeds, NUM_LEDS);
  
  // Set initial parameters
  byte h = 0;
  byte s = 150;
  byte v = 255;
  
}

void loop() {
  flashOne();
  
}

void flashOne() {
  FastLED.clear();
  fill_solid( closeLeds, NUM_LEDS, CHSV(150, 255, 255));
  FastLED.show();
  delay(1000);
  FastLED.clear();
  fill_solid( closeLeds, NUM_LEDS, CHSV(70, 255, 255));
  FastLED.show();
  delay(800);

  FastLED.clear();
  fill_solid( farLeds, NUM_LEDS, CHSV(120, 255, 255));
  FastLED.show();
  delay(500);
  FastLED.clear();
  fill_solid( farLeds, NUM_LEDS, CHSV(15, 255, 255));
  FastLED.show();
  delay(1000);
}




// Define Called Interactions
void indIntClose() {
  // Insert individual close animation
  
}

void indIntFar() {
  // Insert individual far animation
  
}

void grpIntClose() {
  // Insert group close animation
}

void grpIntFar() {
  // Insert group far animation
}

void grpIntBoth() {
  // Insert group both animation
}

void supGrpInt() {
  // Insert super group animation
}

// Define effects utilised

void flashingRainbow(){
 
}

void runningRainbow(){
  byte h = 0;
  byte s = 150;
  byte v = 75;
  
  // Close LED Out
  for (int dot = 0; dot < 75; dot++) {
    closeLeds[dot] = CHSV(h++, s--, v++);
    FastLED.show();
    delay(300);
  }
  for (int dot = 75; dot < 150; dot++) {
    closeLeds[dot] = CHSV(h++, s++, v++);
    FastLED.show();
    delay(300);
  }
  for (int dot = 150; dot < 225; dot++) {
    closeLeds[dot] = CHSV(h++, s++, v--);
    FastLED.show();
    delay(300);
  }
  for (int dot = 225; dot < 300; dot++) {
    closeLeds[dot] = CHSV(h++, s--, v--);
    FastLED.show();
    delay(300);
  }

  // Far LED In
  for (int dot = 300; dot > 225; dot--) {
    farLeds[dot] = CHSV(h--, s--, v++);
    FastLED.show();
    delay(300);
  }
  for (int dot = 225; dot > 150; dot--) {
    farLeds[dot] = CHSV(h--, s++, v++);
    FastLED.show();
    delay(300);
  }
  for (int dot = 150; dot > 75; dot--) {
    farLeds[dot] = CHSV(h--, s++, v--);
    FastLED.show();
    delay(300);
  }
  for (int dot = 75; dot > 0; dot--) {
    farLeds[dot] = CHSV(h--, s--, v--);
    FastLED.show();
    delay(300);
  }
}
