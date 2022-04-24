using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioStateTrigger : MonoBehaviour
{
    #region Variables
    [HideInInspector] public int arrayIdx = 0;
	// [HideInInspector] public string[] sectionArray = new string[] { "Chorus", "Verse", "Instrumental"}; // No state for "Training" because this is a triggered event instead

	[SerializeField] private float sequenceOneProb = 20;
	[SerializeField] private float sequenceTwoProb = 40;
	[SerializeField] private float sequenceThreeProb = 25;
	[SerializeField] private float startMenuProb = 0;

	#endregion

	#region Unity Methods

	void Start()
	{
		int r = (int)Random.Range(0, sequenceOneProb + sequenceTwoProb + sequenceThreeProb + startMenuProb);

		if (r <= sequenceOneProb) arrayIdx = 0;
		else if (r <= (sequenceOneProb + sequenceTwoProb)) arrayIdx = 1;
		else if (r <= (sequenceOneProb + sequenceTwoProb + sequenceThreeProb)) arrayIdx = 2;
		else if (r <= (sequenceOneProb + sequenceTwoProb + sequenceThreeProb + startMenuProb)) arrayIdx = 3;
		else arrayIdx = Random.Range(0, 3); // 3 as the maximum means this will only choose between 0 and 2 since integers are exclusive on the maximum range

	}

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Manager.instance.changeState(arrayIdx);
			InstantiationTest interactiveObject = transform.parent.GetComponent<InstantiationTest>();
			if(interactiveObject) interactiveObject.objectLights("On");
        }
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			transform.parent.GetComponent<InstantiationTest>().objectLights("Off");
		}
	}

	#endregion

}