using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioEventTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField] private AK.Wwise.Event[] enterEvents = null;
    [SerializeField] private AK.Wwise.Event[] exitEvents = null;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            int n = 0;

            do
            {
                enterEvents[n].Post(gameObject);
                n++;
            } while (n < enterEvents.Length);

            
        }

    }

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
            int n = 0;

            do
            {
                exitEvents[n].Post(gameObject);
                n++;
            } while (n < exitEvents.Length);
		}
	}


	#endregion

}
