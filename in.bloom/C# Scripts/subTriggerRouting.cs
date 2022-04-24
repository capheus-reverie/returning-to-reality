using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Uduino;

public class subTriggerRouting : MonoBehaviour
{
    [SerializeField] OscIn _oscIn = null;
    [SerializeField] public arduinoComm linkToFloor;

    const string addressSubOne = "/subOne";
    const string addressSubTwo = "/subTwo";

    public static int subOneId;
    public static int subOneVelocity;
    public static int subOnedB;
    
    public static int subTwoId;
    public static int subTwoVelocity;
    public static int subTwodB;

    private static int mindB = -40;

    void Start()
    {

    }

    private void OnEnable()
    {
        _oscIn.Map(addressSubOne, subOne);
        _oscIn.Map(addressSubTwo, subTwo);
    }

    private void OnDisable()
    {
        _oscIn.Unmap(subOne);
        _oscIn.Unmap(subTwo);
    }



    void subOne( OscMessage subOneMessage )
    {
        if(
            subOneMessage.TryGet( 0, out subOneId) &&
            subOneMessage.TryGet( 1, out subOneVelocity) &&
            subOneMessage.TryGet( 2, out subOnedB)
            )
        {
            if(subOnedB > mindB)
            {
                Invoke("subOneTrig", 0.5f);
            }
        }
    }

    void subOneTrig()
    {
        linkToFloor.subOneTrig();
    }

    void subTwo( OscMessage subTwoMessage)
    {
        if(
            subTwoMessage.TryGet( 0, out subTwoId) &&
            subTwoMessage.TryGet( 1, out subTwoVelocity) &&
            subTwoMessage.TryGet( 2, out subTwodB)
            )
        {
            if(subTwodB > mindB)
            {
                Invoke("subTwoTrig", 0.5f);
            }       
        }
        
    }

    void subTwoTrig()
    {
        linkToFloor.subTwoTrig();
    }
}
