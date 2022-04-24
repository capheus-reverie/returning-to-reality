using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationTest : MonoBehaviour
{
    #region Variables

    // Prefabs for instantiation
    [SerializeField] private GameObject interactiveObject;
    [SerializeField] private bool includeInteractiveObjects = false;
    [SerializeField] private GameObject coreObject;
    [SerializeField] private bool includeCoreObjects = false;
    // [SerializeField] private GameObject spokenObject;
    // [SerializeField] private bool includeSpokenObjects = false;
    private List<float> probs = new List<float>();

    private Dictionary<string, GameObject> objToInstantiate = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> coreObjToInstantiate = new Dictionary<string, GameObject>();
    private HashSet<CoreSong_ObjectController> coreObjects = new HashSet<CoreSong_ObjectController>();
    private Dictionary<string, GameObject> objToDestroy = new Dictionary<string, GameObject>();

    // Bounds for instantiation
    [SerializeField] private Collider boundary;
    [SerializeField] private Collider[] preassignedPositions;
    private Vector3 boundary_min, boundary_size;

    // Parameters
    private string parentName;
    private List<float> x_values = new List<float>();
    private List<float> z_values = new List<float>();
    private HashSet<Vector3> potentialPositions = new HashSet<Vector3>();
    [SerializeField] private float spacePerObject = 4;

    // Probabilities for each object type
    [SerializeField] private float coreProb = 40;
    [SerializeField] private float interactiveProb = 50;
    // [SerializeField] private float spokenProb = 10;
    private float maxProb = 0;


    private int maxObjNum = 0;
    [SerializeField] [Range(0, 0.5f)]
    private float fillPercentage = 0.2f;
    private bool isCoreObjectInit = false;
    [SerializeField] private float heightVariation = 0;


    #endregion

    #region Unity Methods

    void Start()
	{
        // Find the available space for instantiating objects
        parentName = transform.parent.name;
        boundary_size = boundary.bounds.size;
        boundary_min = boundary.bounds.min;

        // Find number of x axis columns
        int x_columns = (int)(boundary_size.x / spacePerObject);
        float x_width = boundary_size.x / x_columns;
        float x = 0.0f;
        for (int i = 0; i < x_columns; i++)
        {
            if (i == 0)
            {
                x = boundary_min.x + (x_width / 2);
            }
            else
            {
                x += x_width;
            }
            x_values.Add(x);
            // Debug.Log("Adding X Value: " + x);
        }

        // Find number z axis columns
        int z_columns = (int)(boundary_size.z / spacePerObject);
        float z_width = boundary_size.z / z_columns;
        float z = 0.0f;
        for (int k = 0; k < z_columns; k++)
        {
            if (k == 0)
            {
                z = boundary_min.z + (z_width / 2);
            }
            else
            {
                z += z_width;
            }
            z_values.Add(z);
            // Debug.Log("Adding Z Values: " + z); 
        }

        // Identify potential positions that don't lie within preassigned spaces (i.e. for doors)
        HashSet<Vector3> obstructions = new HashSet<Vector3>();

        for(int l = 0; l < x_values.Count; l++)
		{
            for(int i = 0; i < z_values.Count; i++)
			{
                float y = heightVariation + Random.Range(-0.45f, 0.45f);
                
                Vector3 newPos = new Vector3(x_values[l], 2.0f + y, z_values[i]);
                // Debug.Log("newPos = " + newPos);
                if(preassignedPositions.Length > 0)
				{
                    for (int d = 0; d < preassignedPositions.Length; d++)
                    {
                        if (!preassignedPositions[d].bounds.Contains(newPos))
                        {
                            potentialPositions.Add(newPos);
                            // Debug.Log("Added: " + newPos + " to potentialPositions");
                        }
                        else
                        {
                            obstructions.Add(newPos);
                            // Debug.Log("Doorway at: " + newPos);
                        }
                    }
                }
                else
				{
                    potentialPositions.Add(newPos);
                    // Debug.Log("No obstructions. Added: " + newPos + " to potentialPositions in Room: " + parentName);
				}
                
			}
		}
        potentialPositions.ExceptWith(obstructions);
        // Debug.Log(potentialPositions);

        // find the total number of objects able to be instantiated (percentage of potential positions)
        maxObjNum = Mathf.FloorToInt(potentialPositions.Count * fillPercentage);

        // Establish the probabilities based on assigned objects
        if (includeInteractiveObjects == true) probs.Add(interactiveProb);
        if (includeCoreObjects == true) probs.Add(coreProb);
        // if (includeSpokenObjects == true) probs.Add(spokenProb);
        for (int v = 0; v < probs.Count; v++) maxProb += probs[v];
        
        // Update room objects
        updateRoomObjects();

        if(includeCoreObjects == true)
		{
            List<Vector3> posAssign = new List<Vector3>();
            foreach (Vector3 pos in potentialPositions)
            {
                posAssign.Add(pos);
                // Debug.Log("Added " + pos + " to posAssign from object: " + parentName);
            }
            // Instantiate all core objects and remove the assigned positions from the potentialPositions List 
            foreach (KeyValuePair<string, GameObject> coreObj in coreObjToInstantiate)
            {
                // Debug.Log("Core objects to instantiation total: " + coreObjToInstantiate.Count);
                int pos = Random.Range(0, (posAssign.Count - 1));

                var position = posAssign[pos];

                GameObject instObj = Instantiate(coreObj.Value, position, Quaternion.identity);
                // Debug.Log("Instantiated Core object: " + coreObj.Value + " at " + position);
                instObj.name = coreObj.Key;

				// Add instantiated object's CoreSong_ObjectController to HashSet
				if (instObj.GetComponent<CoreSong_ObjectController>())
				{
                    coreObjects.Add(instObj.GetComponent<CoreSong_ObjectController>());
                }

                // Remove positions from available positions
                posAssign.RemoveAt(pos);
            }

            // Update values for other object instantiate moving forward
            potentialPositions = new HashSet<Vector3>(); // Reset
            foreach(Vector3 pos in posAssign)
			{
                potentialPositions.Add(pos);
			}
            maxProb -= coreProb;
            maxObjNum -= coreObjToInstantiate.Count;
            isCoreObjectInit = true;
            
        }

	}

    private void updateRoomObjects()
	{
        // If this is the first update (i.e. on start), initialise all objects.
		if (isCoreObjectInit == false && includeCoreObjects == true)
		{
            objToInstantiate = new Dictionary<string, GameObject>();
            coreObjToInstantiate = new Dictionary<string, GameObject>();

            int n = Random.Range(1, maxObjNum);   // Select maximum number of objects for space

            for (int l = 0; l < n; l++)
            {
                int r = (int)Random.Range(0, maxProb);                      // Randomly allocate an object type
                string name = parentName + '_' + (l + 1).ToString();        // Assign unique name

                if (includeInteractiveObjects == true)
                {
                    if (r <= interactiveProb) objToInstantiate.Add(name+"_Int", interactiveObject);
                    else
                    {
                        if (includeCoreObjects == true)
                        {
                            if (r <= (interactiveProb + coreProb)) coreObjToInstantiate.Add(name+"_Core", coreObject);
                            else
                            {
                                /*
                                if (includeSpokenObjects == true)
                                {
                                    if (r <= maxProb) objToInstantiate.Add(name+"_Spoken", spokenObject);
                                }
                                */
                            }
                        }
                        /*
                        else if (includeSpokenObjects == true)
                        {
                            if (r <= (interactiveProb + spokenProb)) objToInstantiate.Add(name+"_Spoken", spokenObject);
                        }
                        */
                    }
                }
                else if (includeCoreObjects == true)
                {
                    if (r <= coreProb) coreObjToInstantiate.Add(name+"_Core", coreObject);
                    /*
                    else
                    {
                        if (includeSpokenObjects == true) objToInstantiate.Add(name + "_Spoken", spokenObject);
                    }
                    */
                }
                /*
                else if (includeSpokenObjects == true)
                {
                    objToInstantiate.Add(name + "_Spoken", spokenObject);
                }
                */
            }
        } 
        else {

            objToInstantiate = new Dictionary<string, GameObject>();

            int n = Random.Range(1, maxObjNum);   // Select maximum number of objects for space
            // Debug.Log("Instantiating " + n + " objects on " + parentName);

            for (int l = 0; l < n; l++)
            {
                int r = (int)Random.Range(0, maxProb);                      // Randomly allocate an object type
                string name = parentName + '_' + (l + 1).ToString();        // Assign unique name

                if (includeInteractiveObjects == true)
                {
                    if (r <= interactiveProb) objToInstantiate.Add(name + "_Int", interactiveObject);
                    /*
                    else if (includeSpokenObjects == true)
                    {
                        objToInstantiate.Add(name + "_Spoken", spokenObject);
                    }
                    */
                }
                /*
                else if (includeSpokenObjects == true)
				{
                    objToInstantiate.Add(name + "_Spoken", spokenObject);
                }
                */            }
        }
        
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
            List<Vector3> posAssign = new List<Vector3>();
            foreach(Vector3 pos in potentialPositions)
			{
                posAssign.Add(pos);
                // Debug.Log("Added " + pos + " to posAssign from object: " + parentName);
			}

            objToDestroy = new Dictionary<string, GameObject>();

            foreach(KeyValuePair<string, GameObject> obj in objToInstantiate)
			{
                int pos = Random.Range(0, (posAssign.Count - 1));
                var position = posAssign[pos];
                // Debug.Log("Position: " + position);

                GameObject instObj = Instantiate(obj.Value, position, Quaternion.identity);
                // Debug.Log("Instantiated: " + obj.Value + " at " + position);
                instObj.name = obj.Key;

                objToDestroy.Add(instObj.name, instObj);
                posAssign.RemoveAt(pos);
			}
            objToInstantiate = null;
            posAssign = null;

            // Randomly allocate core Object instruments on triggerEnter
            foreach(CoreSong_ObjectController obj in coreObjects)
			{
                int i = Random.Range(0, obj.instruments.Length);
                obj.changeInstrument(i);
                // Debug.Log("Changed " + obj.name + " to instrument: " + i);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
           foreach(KeyValuePair<string, GameObject> obj in objToDestroy)
			{
                ObjectController objControl = obj.Value.GetComponent<ObjectController>();
                objControl.stopPlayback();
                // Debug.Log("Destroying: " + obj.Key + " " + obj.Value);
                Destroy(obj.Value, 1.0f);
			}
            objToDestroy = null;
            // Debug.Log("ObjToDestroy: " + objToDestroy);
            updateRoomObjects();
		}
	}

    public void objectLights (string state)
	{
		switch (state)
		{
            case "On":
                // Turn on all interactive object lights
                foreach(KeyValuePair<string, GameObject> obj in objToDestroy)
				{
                    ObjectController control = obj.Value.GetComponent<ObjectController>();
                    control.lightFade(state);
				}
                // Turn on all core object lights
                foreach(CoreSong_ObjectController coreObj in coreObjects)
				{
                    coreObj.lightFade(state);
				}
                break;

            case "Off":
                // Turn off all interactive object lights
                foreach(KeyValuePair<string, GameObject> obj in objToDestroy)
				{
                    ObjectController control = obj.Value.GetComponent<ObjectController>();
                    control.lightFade(state);
				}
                // Turn off all core object lights
                foreach(CoreSong_ObjectController coreObj in coreObjects)
				{
                    coreObj.lightFade(state);
				}
                break;

            default:
                Debug.LogError("No valid state found");
                break;
		}
	}

	#endregion

}
