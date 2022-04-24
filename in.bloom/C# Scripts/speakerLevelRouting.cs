using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class speakerLevelRouting : MonoBehaviour
{
    [SerializeField] OscIn _oscIn = null;

    const string addressSpeakerLevel = "/spkLev";

    void Start()
    {
    }

    private void OnEnable()
    {
        _oscIn.Map(addressSpeakerLevel, speakerLevel);
    }

    private void OnDisable()
    {
        _oscIn.Unmap(speakerLevel);
    }


    void speakerLevel( OscMessage speakerMessage)
    {
        int speakerId;
        float speakerValue;

        if(
            speakerMessage.TryGet( 0, out speakerId) &&
            speakerMessage.TryGet( 1, out speakerValue)
            )
        {
            Debug.Log("Recevied subTwo\n" + speakerId + " " + speakerValue + "\n");
        }
    }
}
