using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class inBloomOSCRouting : MonoBehaviour
{
    [SerializeField] OscOut _oscOut = null;


    //float strings
    const string blossomProximity = "/blomProx";

    // OSCMessage for blomProxOut
    OscMessage _blomProxOne;
    OscMessage _blomProxTwo;
    OscMessage _blomProxThree;
    OscMessage _blomProxFour;
    OscMessage _blomProxFive;
    OscMessage _blomProxSix;
    OscMessage _blomProxSeven;
    OscMessage _blomProxEight;
    OscMessage _blomProxNine;
    OscMessage _blomProxTen;

    // trigger strings

    const string ambientKit = "/ambKit";
    const string ambientKey = "/ambKey";
    const string ambientDrone = "/ambDro";

    const string indInt = "/indInt";

    const string grpInt = "/grpInt";

    const string supGrpInt = "/supGrpInt";

    // Memory variables for less computer load
    int newBlomOneProx;
    int oldBlomOneProx;
    int newBlomTwoProx;
    int oldBlomTwoProx;
    int newBlomThreeProx;
    int oldBlomThreeProx;
    int newBlomFourProx;
    int oldBlomFourProx;
    int newBlomFiveProx;
    int oldBlomFiveProx;
    int newBlomSixProx;
    int oldBlomSixProx;
    int newBlomSevenProx;
    int oldBlomSevenProx;
    int newBlomEightProx;
    int oldBlomEightProx;
    int newBlomNineProx;
    int oldBlomNineProx;
    int newBlomTenProx;
    int oldBlomTenProx;


    void Start()
    {
        _blomProxOne = new OscMessage(blossomProximity);
        _blomProxOne.Add(1).Add(blossomComm.blomOneProx);

        _blomProxTwo = new OscMessage(blossomProximity);
        _blomProxTwo.Add(2).Add(blossomComm.blomTwoProx);

        _blomProxThree = new OscMessage(blossomProximity);
        _blomProxThree.Add(3).Add(blossomComm.blomThreeProx);

        _blomProxFour = new OscMessage(blossomProximity);
        _blomProxFour.Add(4).Add(blossomComm.blomFourProx);

        _blomProxFive = new OscMessage(blossomProximity);
        _blomProxFive.Add(5).Add(blossomComm.blomFiveProx);

        _blomProxSix = new OscMessage(blossomProximity);
        _blomProxSix.Add(6).Add(blossomComm.blomSixProx);

        _blomProxSeven = new OscMessage(blossomProximity);
        _blomProxSeven.Add(7).Add(blossomComm.blomSevenProx);

        _blomProxEight = new OscMessage(blossomProximity);
        _blomProxEight.Add(8).Add(blossomComm.blomEightProx);

        _blomProxNine = new OscMessage(blossomProximity);
        _blomProxNine.Add(9).Add(blossomComm.blomNineProx);

        _blomProxTen = new OscMessage(blossomProximity);
        _blomProxTen.Add(10).Add(blossomComm.blomTenProx);
    }

    private void Update()
    {
        
    }
         
    // ___________________________________________________________
    // Blossom Proximity Functions

    public void sendBlmOneProx()
    {

        newBlomOneProx = blossomComm.blomOneProx;

        if(oldBlomOneProx != newBlomOneProx)
        {
            _blomProxOne.Set(0, 1);
            _blomProxOne.Set(1, blossomComm.blomOneProx);
            _oscOut.Send(_blomProxOne);

            oldBlomOneProx = newBlomOneProx;
        }
        
    }

    public void sendBlmTwoProx()
    {

        newBlomTwoProx = blossomComm.blomTwoProx;

        if(oldBlomTwoProx != newBlomTwoProx)
        {
            _blomProxTwo.Set(0, 2);
            _blomProxTwo.Set(1, blossomComm.blomTwoProx);
            _oscOut.Send(_blomProxTwo);

            oldBlomTwoProx = newBlomTwoProx;
        }
        
    }

    public void sendBlmThreeProx()
    {

        newBlomThreeProx = blossomComm.blomThreeProx;

        if(oldBlomThreeProx != newBlomThreeProx)
        {
            _blomProxThree.Set(0, 3);
            _blomProxThree.Set(1, blossomComm.blomThreeProx);
            _oscOut.Send(_blomProxThree);

            oldBlomThreeProx = newBlomThreeProx;
        }
        
    }

    public void sendBlmFourProx()
    {

        newBlomFourProx = blossomComm.blomFourProx;

        if(oldBlomFourProx != newBlomFourProx)
        {
            _blomProxFour.Set(0, 4);
            _blomProxFour.Set(1, blossomComm.blomFourProx);
            _oscOut.Send(_blomProxFour);

            oldBlomFourProx = newBlomFourProx;
        }
        
    }

    public void sendBlmFiveProx()
    {

        newBlomFiveProx = blossomComm.blomFiveProx;

        if(oldBlomFiveProx != newBlomFiveProx)
        {
            _blomProxFive.Set(0, 5);
            _blomProxFive.Set(1, newBlomFiveProx);
            _oscOut.Send(_blomProxFive);

            oldBlomFiveProx = newBlomFiveProx;
        }
    }

    public void sendBlmSixProx()
    {

        newBlomSixProx = blossomComm.blomSixProx;

        if(oldBlomSixProx != newBlomSixProx)
        {
            _blomProxSix.Set(0, 6);
            _blomProxSix.Set(1, blossomComm.blomSixProx);
            _oscOut.Send(_blomProxSix);

            oldBlomSixProx = newBlomSixProx;
        }
        
    }

    public void sendBlmSevenProx()
    {

        newBlomSevenProx = blossomComm.blomSevenProx;

        if(oldBlomSevenProx != newBlomSevenProx)
        {
            _blomProxSeven.Set(0, 7);
            _blomProxSeven.Set(1, blossomComm.blomSevenProx);
            _oscOut.Send(_blomProxSeven);

            oldBlomSevenProx = newBlomSevenProx;
        }
        
    }

    public void sendBlmEightProx()
    {

        newBlomEightProx = blossomComm.blomEightProx;

        if(newBlomEightProx != oldBlomEightProx)
        {
            _blomProxEight.Set(0, 8);
            _blomProxEight.Set(1, blossomComm.blomEightProx);
            _oscOut.Send(_blomProxEight);

            oldBlomEightProx = newBlomEightProx;
        }
        
    }

    public void sendBlmNineProx()
    {

        newBlomNineProx = blossomComm.blomNineProx;

        if(newBlomNineProx != oldBlomNineProx)
        {
            _blomProxNine.Set(0, 9);
            _blomProxNine.Set(1, blossomComm.blomNineProx);
            _oscOut.Send(_blomProxNine);

            oldBlomNineProx = newBlomNineProx;
        }
        
    }

    public void sendBlmTenProx()
    {

        newBlomTenProx = blossomComm.blomTenProx;

        if(oldBlomTenProx != newBlomTenProx)
        {
            _blomProxTen.Set(0, 10);
            _blomProxTen.Set(1, newBlomTenProx);
            _oscOut.Send(_blomProxTen);

            oldBlomTenProx = newBlomTenProx;
        }
        
    }

}
