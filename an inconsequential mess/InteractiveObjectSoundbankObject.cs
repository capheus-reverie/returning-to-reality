using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractiveObjectSoundbankObject : IComparable<InteractiveObjectSoundbankObject>
{
    #region Variables

    public string ObjectID { get; set; }
    public int CurrentSoundbank { get; set; }

    #endregion

    #region Unity Methods

    public InteractiveObjectSoundbankObject(string newID, int newSoundbank)
	{
        ObjectID = newID;
        CurrentSoundbank = newSoundbank;
	}

    public int CompareTo(InteractiveObjectSoundbankObject other)
	{
        if(other == null) { return 1; }
        return CurrentSoundbank = other.CurrentSoundbank;
	}

    #endregion

}
