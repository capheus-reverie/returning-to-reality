using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Uduino;

public class keysTriggerRouting : MonoBehaviour
{

    [SerializeField] OscIn _oscIn = null;
    [SerializeField] public arduinoComm linkToFloor;

    const string addressKeys = "/keyTrig";

    public static int keyChordId;
    public static int keyChordVelocity;

 
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _oscIn.Map(addressKeys, keyTrig);
    }

    private void OnDisable()
    {
        _oscIn.Unmap(keyTrig);
    }

    void keyTrig( OscMessage keyTrigMessage)
    {
        if(
            keyTrigMessage.TryGet( 0, out keyChordId) &&
            keyTrigMessage.TryGet( 1, out keyChordVelocity)
            )
        {
            linkToFloor.keyTrig();
        }
    }
}
