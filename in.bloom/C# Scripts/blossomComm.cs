using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using System;

public class blossomComm : MonoBehaviour
{
    // Initialize UduinoManager
    UduinoManager u;

    [SerializeField] public inBloomOSCRouting linkToBlomAudio;

    // Define variables for sensors and LED values
    public static int blomOneProx;
    public static int blomTwoProx;
    public static int blomThreeProx;
    public static int blomFourProx;
    public static int blomFiveProx;
    public static int blomSixProx;
    public static int blomSevenProx;
    public static int blomEightProx;
    public static int blomNineProx;
    public static int blomTenProx;

    // Initialize the multiple arduino connections
    UduinoDevice blomOne = null;
    UduinoDevice blomTwo = null;
    UduinoDevice blomThree = null;
    UduinoDevice blomFour = null;
    UduinoDevice blomFive = null;
    UduinoDevice blomSix = null;
    UduinoDevice blomSeven = null;
    UduinoDevice blomEight = null;
    UduinoDevice blomNine = null;
    UduinoDevice blomTen = null;

    // LED Values
    int newLedOneVal;
    int oldLedOneVal;
    int newLedTwoVal;
    int oldLedTwoVal;
    int newLedThreeVal;
    int oldLedThreeVal;
    int newLedFourVal;
    int oldLedFourVal;
    int newLedFiveVal;
    int oldLedFiveVal;
    int newLedSixVal;
    int oldLedSixVal;
    int newLedSevenVal;
    int oldLedSevenVal;
    int newLedEightVal;
    int oldLedEightVal;
    int newLedNineVal;
    int oldLedNineVal;
    int newLedTenVal;
    int oldLedTenVal;

    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.OnBoardConnected += OnBoardConnected;
        UduinoManager.Instance.OnDataReceived += OnDataReceived;
        
    }

    public void readSensors()
    {
        if(blomOne != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomOne, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmOneProx();

            // Update audio level
            newLedOneVal = blossomLevelRouting.blomOneLev;

            // Send new LED Value (old < new)
            if (oldLedOneVal < newLedOneVal)
            {
                for (int i = oldLedOneVal; i < newLedOneVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomOne, "blomTrig", i);
                }
                oldLedOneVal = newLedOneVal;
            }

            // Send new LED Value (old > new)
            if (oldLedOneVal > newLedOneVal)
            {
                for (int i = oldLedOneVal; i > newLedOneVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomOne, "blomTrig", i);
                }
                oldLedOneVal = newLedOneVal;
            }

        }
        if(blomTwo != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomTwo, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmTwoProx();

            // Update audio level
            newLedTwoVal = blossomLevelRouting.blomTwoLev;

            // Send new LED Value (old < new)
            if (oldLedTwoVal < newLedTwoVal)
            {
                for (int i = oldLedTwoVal; i < newLedTwoVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomTwo, "blomTrig", i);
                }
                oldLedTwoVal = newLedTwoVal;
            }

            // Send new LED Value (old > new)
            if (oldLedTwoVal > newLedTwoVal)
            {
                for (int i = oldLedTwoVal; i > newLedTwoVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomTwo, "blomTrig", i);
                }
                oldLedTwoVal = newLedTwoVal;
            }
        }
        if (blomThree != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomThree, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmThreeProx();

            // Update audio level
            newLedThreeVal = blossomLevelRouting.blomThreeLev;

            // Send new LED Value (old < new)
            if (oldLedThreeVal < newLedThreeVal)
            {
                for (int i = oldLedThreeVal; i < newLedThreeVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomThree, "blomTrig", i);
                }
                oldLedThreeVal = newLedThreeVal;
            }

            // Send new LED Value (old > new)
            if (oldLedThreeVal > newLedThreeVal)
            {
                for (int i = oldLedThreeVal; i > newLedThreeVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomThree, "blomTrig", i);
                }
                oldLedThreeVal = newLedThreeVal;
            }
        }
        if (blomFour != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomFour, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmFourProx();

            // Update audio level
            newLedFourVal = blossomLevelRouting.blomFourLev;

            // Send new LED Value (old < new)
            if (oldLedFourVal < newLedFourVal)
            {
                for (int i = oldLedFourVal; i < newLedFourVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomFour, "blomTrig", i);
                }
                oldLedFourVal = newLedFourVal;
            }

            // Send new LED Value (old > new)
            if (oldLedFourVal > newLedFourVal)
            {
                for (int i = oldLedFourVal; i > newLedFourVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomFour, "blomTrig", i);
                }
                oldLedFourVal = newLedFourVal;
            }
        }
        if (blomFive != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomFive, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmFiveProx();

            // Update audio level
            newLedFiveVal = blossomLevelRouting.blomFiveLev;

            // Send new LED Value (old < new)
            if (oldLedFiveVal < newLedFiveVal)
            {
                for (int i = oldLedFiveVal; i < newLedFiveVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomFive, "blomTrig", i);
                }
                oldLedFiveVal = newLedFiveVal;
            }

            // Send new LED Value (old > new)
            if (oldLedFiveVal > newLedFiveVal)
            {
                for (int i = oldLedFiveVal; i > newLedFiveVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomFive, "blomTrig", i);
                }
                oldLedFiveVal = newLedFiveVal;
            }
        }
        if (blomSix != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomSix, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmSixProx();

            // Update audio level
            newLedSixVal = blossomLevelRouting.blomSixLev;

            // Send new LED Value (old < new)
            if (oldLedSixVal < newLedSixVal)
            {
                for (int i = oldLedSixVal; i < newLedSixVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomSix, "blomTrig", i);
                }
                oldLedSixVal = newLedSixVal;
            }

            // Send new LED Value (old > new)
            if (oldLedSixVal > newLedSixVal)
            {
                for (int i = oldLedSixVal; i > newLedSixVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomSix, "blomTrig", i);
                }
                oldLedSixVal = newLedSixVal;
            }
        }
        if (blomSeven != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomSeven, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmSevenProx();

            // Update audio level
            newLedSevenVal = blossomLevelRouting.blomSevenLev;

            // Send new LED Value (old < new)
            if (oldLedSevenVal < newLedSevenVal)
            {
                for (int i = oldLedSevenVal; i < newLedSevenVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomSeven, "blomTrig", i);
                }
                oldLedSevenVal = newLedSevenVal;
            }

            // Send new LED Value (old > new)
            if (oldLedSevenVal > newLedSevenVal)
            {
                for (int i = oldLedSevenVal; i > newLedSevenVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomSeven, "blomTrig", i);
                }
                oldLedSevenVal = newLedSevenVal;
            }
        }
        if (blomEight != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomEight, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmEightProx();

            // Update audio level
            newLedEightVal = blossomLevelRouting.blomEightLev;

            // Send new LED Value (old < new)
            if (oldLedEightVal < newLedEightVal)
            {
                for (int i = oldLedEightVal; i < newLedEightVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomEight, "blomTrig", i);
                }
                oldLedEightVal = newLedEightVal;
            }

            // Send new LED Value (old > new)
            if (oldLedEightVal > newLedEightVal)
            {
                for (int i = oldLedEightVal; i > newLedEightVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomEight, "blomTrig", i);
                }
                oldLedEightVal = newLedEightVal;
            }
        }
        if (blomNine != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomNine, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmNineProx();

            // Update audio level
            newLedNineVal = blossomLevelRouting.blomNineLev;

            // Send new LED Value (old < new)
            if (oldLedNineVal < newLedNineVal)
            {
                for (int i = oldLedNineVal; i < newLedNineVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomNine, "blomTrig", i);
                }
                oldLedNineVal = newLedNineVal;
            }

            // Send new LED Value (old > new)
            if (oldLedNineVal > newLedNineVal)
            {
                for (int i = oldLedNineVal; i > newLedNineVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomNine, "blomTrig", i);
                }
                oldLedNineVal = newLedNineVal;
            }
        }
        if (blomTen != null)
        {
            // Read Sensor
            UduinoManager.Instance.sendCommand(blomTen, "readSensor");

            // Send to Audio
            linkToBlomAudio.sendBlmTenProx();

            // Update audio level
            newLedTenVal = blossomLevelRouting.blomTenLev;

            // Send new LED Value (old < new)
            if (oldLedTenVal < newLedTenVal)
            {
                for(int i = oldLedTenVal; i < newLedTenVal; i++)
                {
                    UduinoManager.Instance.sendCommand(blomTen, "blomTrig", i);
                }
                oldLedTenVal = newLedTenVal;
            }

            // Send new LED Value (old > new)
            if( oldLedTenVal > newLedTenVal)
            {
                for(int i = oldLedTenVal; i > newLedTenVal; i--)
                {
                    UduinoManager.Instance.sendCommand(blomTen, "blomTrig", i);
                }
                oldLedTenVal = newLedTenVal;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        readSensors();
    }

    void OnBoardConnected(UduinoDevice connectedDevice)
    {
        if (connectedDevice.name == "blossomArduinoOne")
        {
            blomOne = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoTwo")
        {
            blomTwo = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoThree")
        {
            blomThree = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoFour")
        {
            blomFour = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoFive")
        {
            blomFive = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoSix")
        {
            blomSix = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoSeven")
        {
            blomSeven = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoEight")
        {
            blomEight = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoNine")
        {
            blomNine = connectedDevice;
        }
        else if (connectedDevice.name == "blossomArduinoTen")
        {
            blomTen = connectedDevice;
        }
    }

    void OnDataReceived(string minDistance, UduinoDevice device)
    {
        if (device.name == "blossomArduinoOne")
        {
            blomOneProx = int.Parse(minDistance);
            Debug.Log(blomOneProx);
        }
        else if (device.name == "blossomArduinoTwo") blomTwoProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoThree") blomThreeProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoFour") blomFourProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoFive") blomFiveProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoSix") blomSixProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoSeven") blomSevenProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoEight") blomEightProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoNine") blomNineProx = int.Parse(minDistance);
        else if (device.name == "blossomArduinoTen") blomTenProx = int.Parse(minDistance);
    }
}
