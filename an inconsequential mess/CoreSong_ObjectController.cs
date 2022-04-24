using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSong_ObjectController : MonoBehaviour
{
    #region Variables

    [Header("Left Hand Manipulation")]

    [Range(0f, 100f)]
    public float leftXAxis = 90f;
    [Range(0f, 100f)]
    public float leftYAxis = 40f;
    [Range(0f, 100f)]
    public float leftZAxis = 100f;
    [Range(0f, 100f)]
    public float leftYAxisRotation = 40f;

    [SerializeField] private AK.Wwise.RTPC leftXAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC leftYAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC leftZAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC leftYAxisRotationRTPC;

    [Header("Right Hand Manipulation")]

    [Range(0f, 100f)]
    public float rightXAxis = 50f;
    [Range(0f, 100f)]
    public float rightYAxis = 50f;
    [Range(0f, 100f)]
    public float rightZAxis = 50f;
    [Range(0f, 100f)]
    public float rightYAxisRotation = 50f;

    [SerializeField] private AK.Wwise.RTPC rightXAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC rightYAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC rightZAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC rightYAxisRotationRTPC;

    [Header("Instrument Setup")]

    public AK.Wwise.Switch[] instruments;
    private int instrument;
    [SerializeField] private AK.Wwise.Event mute;
    [SerializeField] private AK.Wwise.Event unmute;
    private bool isMuted = false;


    [Header("Lighting and Colour Setup")]

    // Initialize Lighting Specs
    private Light objectLight = null;
    [SerializeField] private float lightIntensity = 1;
    [SerializeField] [Range(0.01f, 5f)] private float lightFadeDuration = 1f;
    private Component halo;

    private Renderer objectRenderer;
    [SerializeField] private Color[] instrumentColours;
    [SerializeField] private Color muteColour;
    private Color preMuteColour;

    [SerializeField] private bool randomMuteOnStart = true;
    private float mutedProb = 50f;
    private float unmutedProb = 50f;

	#endregion

	#region Unity Methods

	private void Awake()
	{
        objectLight = this.gameObject.GetComponentInChildren<Light>();
        objectRenderer = this.gameObject.GetComponent<Renderer>();
        halo = objectLight.GetComponent("Halo");
        objectSelectHalo("Off"); // Turn halo off by default to start
    }

	void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject, gameObject.name);
        
        // Randomly assign first instrument
        instrument = Random.Range(0, instruments.Length);
        changeInstrument(instrument);

        if (randomMuteOnStart)
        {
            mutedProb = Random.Range(0.0f, 50.0f);
            unmutedProb = Random.Range(0.0f, 50.0f);

            float muteSelect = Random.Range(0.0f, mutedProb + unmutedProb);
            if (muteSelect <= mutedProb)
            {
                isMuted = true;
                
                Debug.Log(gameObject.name + " to be muted");
            }
            else isMuted = false; // Default and higher value.
        }
    }

	private void Update()
	{
        leftXAxisRTPC.SetValue(this.gameObject, leftXAxis);
        leftYAxisRTPC.SetValue(this.gameObject, leftYAxis);
        leftZAxisRTPC.SetValue(this.gameObject, leftZAxis);
        leftYAxisRotationRTPC.SetValue(this.gameObject, leftYAxisRotation);

        rightXAxisRTPC.SetValue(this.gameObject, rightXAxis);
        rightYAxisRTPC.SetValue(this.gameObject, rightYAxis);
        rightZAxisRTPC.SetValue(this.gameObject, rightZAxis);
        rightYAxisRotationRTPC.SetValue(this.gameObject, rightYAxisRotation);
    }

    public void objectAdmin(string state)
	{
        // Debug.Log("current instrument: " + instrument);
        switch (state)
        {
            case "addInst":
				if (!isMuted)
				{
                    if (instrument < instruments.Length - 1) instrument++;
                    else instrument = 0;
                    changeInstrument(instrument);
                }
                break;

            case "subInst":
				if (!isMuted)
				{
                    if (instrument != 0) instrument--;
                    else instrument = instruments.Length - 1;
                    changeInstrument(instrument);
                }
                break;

            case "muteUnmute":
				if (!isMuted)
				{
                    mute.Post(gameObject);
                    preMuteColour = objectRenderer.material.color;
                    objectRenderer.material.color = muteColour;
                    objectLight.color = muteColour;
                    objectLight.intensity = Mathf.Lerp(objectLight.intensity, objectLight.intensity / 2, 3.0f);
                    isMuted = true;
				}
                else
				{
                    unmute.Post(gameObject);
                    objectRenderer.material.color = preMuteColour;
                    objectLight.color = muteColour;
                    objectLight.intensity = Mathf.Lerp(objectLight.intensity, objectLight.intensity * 2, 3.0f);
                    isMuted = false;
				}
                break;

            default:
                Debug.LogError("No defined objectAdmin for string: " + state);
                break;
        }
	}

    public void coreSongTriggered()
	{
        if (isMuted && randomMuteOnStart)
        {
            StartCoroutine(initMute());
        }
	}

    private IEnumerator initMute()
	{
        yield return new WaitForSecondsRealtime(1.5f);

        mute.Post(gameObject);
        preMuteColour = objectRenderer.material.color;
        objectRenderer.material.color = muteColour;
        objectLight.color = muteColour;
        objectLight.intensity /= 2;
        Debug.Log("Muted " + gameObject.name);

    }

    public void changeInstrument(int inst)
	{
        // Debug.Log("Changing instrument to: " + inst);
        instruments[inst].SetValue(this.gameObject);
        if (!isMuted) updateColour(inst);
        else preMuteColour = instrumentColours[inst];
	}

	public void displayOnPlay(object in_cookie, AkCallbackType in_type, object in_info)
	{
        // Insert visual cue for activated Core Song objects
	}

    public void objectSelectHalo(string state)
    {
        switch (state)
        {
            case "On":
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                // Debug.Log("Halo turned on");
                break;
            case "Off":
                halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                // Debug.Log("Halo turned off");
                break;
            default:
                // Debug.LogError("No valid state for selection halo");
                break;
        }
    }

    public void lightFade(string state)
    {
        switch (state)
        {
            case "On":
                float c = objectLight.intensity;
                objectLight.intensity = Mathf.Lerp(c, lightIntensity, lightFadeDuration);
                Debug.Log("Object: " + gameObject.name + "'s light is now on at: " + lightIntensity);
                break;
            case "Off":
                float d = objectLight.intensity;
                objectLight.intensity = Mathf.Lerp(d, 0.0f, lightFadeDuration);
                Debug.Log("Object: " + gameObject.name + "'s light is now off.");
                break;
            default:
                Debug.LogError("No valid state found");
                break;
        }
    }

    public void updateColour(int inst)
	{        
		if (instrumentColours[inst] != null)
		{
            Color newColour = instrumentColours[inst];

            // Update Object Colour
            objectRenderer.material.color = newColour;

            // Update Light Colour
            objectLight.color = newColour;
        }       
	}

    #endregion

}
