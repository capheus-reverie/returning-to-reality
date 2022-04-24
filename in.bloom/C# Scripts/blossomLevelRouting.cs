using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class blossomLevelRouting : MonoBehaviour
{
    [SerializeField] OscIn _oscIn = null;
    [SerializeField] public blossomComm linkToBlom;

    const string addressBlossomLevel = "/blomLev";

    public static int blossomId;
    public static int blossomValue;

    public static int blomOneLev;
    public static int blomTwoLev;
    public static int blomThreeLev;
    public static int blomFourLev;
    public static int blomFiveLev;
    public static int blomSixLev;
    public static int blomSevenLev;
    public static int blomEightLev;
    public static int blomNineLev;
    public static int blomTenLev;

    void Start()
    {
    }

    private void OnEnable()
    {
        _oscIn.Map(addressBlossomLevel, blossomLevel);
    }

    private void OnDisable()
    {
        _oscIn.Unmap(blossomLevel);
    }


    void blossomLevel( OscMessage blossomMessage)
    { 
        if(
            blossomMessage.TryGet(0, out blossomId) &&
            blossomMessage.TryGet(1, out blossomValue)
            )
        {
            Debug.Log("Received blossomLevel\n" + blossomId + " " + blossomValue + "\n");

            if(blossomId == 1)
            {
                blomOneLev = blossomValue;
            }

            else if(blossomId == 2)
            {
                blomTwoLev = blossomValue;
            }

            else if(blossomId == 3)
            {
                blomThreeLev = blossomValue;
            }

            else if(blossomId == 4)
            {
                blomFourLev = blossomValue;
            }

            else if(blossomId == 5)
            {
                blomFiveLev = blossomValue;
            }

            else if(blossomId == 6)
            {
                blomSixLev = blossomValue;
            }

            else if(blossomId == 7)
            {
                blomSevenLev = blossomValue;
            }

            else if(blossomId == 8)
            {
                blomEightLev = blossomValue;
            }

            else if(blossomId == 9)
            {
                blomNineLev = blossomValue;
            }

            else if(blossomId == 10)
            {
                blomTenLev = blossomValue;
            }
        }
    }
}
