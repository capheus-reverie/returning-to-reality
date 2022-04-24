using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum SceneIndexes
{
    _MANAGER = 0,
    _TRAINING = 1,
    _STARTMENU = 2,
    _BLOCK1 = 3,
    _INSTANTIATETEST = 4
}

public class Manager : MonoBehaviour
{
    #region Variables

    // Initialise Game Manager instance
    public static Manager instance = null;

    [SerializeField]
    private loadingScreenPoems loadingScreenPoem;
    [SerializeField]
    private GameObject loadingScreenCanvas;

    // Add state changes for song sections (used to change the global form of the piece)
    [Header("Song Sections")]
    public AK.Wwise.State StartMenu;
    public AK.Wwise.State[] Sections;
    // public AK.Wwise.State[] Verses;
    // public AK.Wwise.State[] Choruses;
    // public AK.Wwise.State[] Instrumentals;

    /*
    [Header("Variation Randomisation Setup")]
    public AK.Wwise.State[] Variations;
    [SerializeField] private int chorusMaxVariations = 4;
    [SerializeField] private int verseMaxVariations = 2;
    [SerializeField] private int instMaxVariations = 2;
    */


    [Header("Events")]
    [SerializeField] private AK.Wwise.Event Intro;
    [SerializeField] private AkTriggerCoreSong droneDefault;
    [SerializeField] private GameObject localAvatar;

    // Initialize soundbanks for Instruments
    [Header("Interactive Object Soundbank Setup")]
    private string[] instrumentSoundbankNames = {"Instrument1", "Instrument2", "Instrument3", "Instrument4", "Instrument5", "Instrument6"};
    
    private static Dictionary<string, bool> soundbanks = null;

    [SerializeField] private AK.Wwise.Bank[] InstrumentSoundbanks;
    // [SerializeField] private ObjectController defaultInteractiveObject;
    private HashSet<string> loadedSoundbanks = null;
    public HashSet<string> _globalRequiredSoundbanks = null;
    private HashSet<string> banksToLoad;

    /*
    private bool isChorus = false;
    private bool isVerse = false;
    private bool isInstrumental = false;
    */

    // Initialise variables for time-based interactions

    [HideInInspector] public int beat = 0;
    [HideInInspector] public int bar = 0;
    [Tooltip("Make this value the top value of the initial time signature as set in Wwise")]
    [SerializeField] private int beatsPerBar = 9;
    [Tooltip("Make this value the tempo originally used in Wwise")]
    [SerializeField] private float tempo = 60;
    private float segment = 0;

    /*
    public int interactionEvents = 0;
    private float interactionDuration = 0;
    public float interactionRatio = 0;
    private float interactionLeftStart = 0;
    private float interactionRightStart = 0;
    private float interactionLeftStop = 0;
    private float interactionRightStop = 0;
    */

    /*
    public int currentVerse = 1;
    public int currentChorus = 1;
    public int currentInstrumental = 1;
    */

    //private bool banksUpdating = false;
    private Coroutine banksUpdating;

    [SerializeField] private AK.Wwise.Event Clicktrack;

    private Dictionary<string, int> _requiredSoundbanks = null;

    /*
    public PostTrainingDoorFade startMenuDoors;
    public bool hasTrained = false;
    */

    AkTriggerCoreSong[] triggers = null;
    CoreSong_ObjectController[] coreObjects = null;

    private int variation;

    #endregion

    #region Unity Methods

    private void Awake()
	{
		if (instance == null)
		{
            instance = this;
            // StartCoroutine(updateTime(1.0f));
        }
        else if (instance != null)
		{
            Destroy(gameObject);
		}

    }

	void Start()
    {
        // StartCoroutine(RandomiseVariations());

        StartCoroutine(startLoadingScreenMusic(1.0f));

        soundbanks = new Dictionary<string, bool>();
        foreach (string name in instrumentSoundbankNames)
		{
            soundbanks.Add(name, false);
		}
        //loadBanks = commenceSoundbankLoad();
        _requiredSoundbanks = new Dictionary<string, int>();
        _globalRequiredSoundbanks = new HashSet<string>();
        banksToLoad = new HashSet<string>();
        loadedSoundbanks = new HashSet<string>();
        
    }

    /*
    private IEnumerator testTraining()
	{
        Debug.Log("Coroutine started");
        // Wait 10 seconds
        yield return new WaitForSecondsRealtime(5);

        Debug.Log("Training test finished");
        // Pretend that training has been done
        hasTrained = true;
        yield break;
	}
    */

    // Do I still need to randomise variations???
    /*
    private IEnumerator RandomiseVariations()
	{
        
        while (true)
		{
            yield return new WaitForSecondsRealtime(5);
            updateVariations();
        }
	}
    */

    /*
    public void interactionAdd(string type)
	{
        switch (type)
		{
            case "leftStart":
                interactionLeftStart = Time.fixedUnscaledTime;
                interactionEvents++;
                break;
            case "rightStart":
                interactionRightStart = Time.fixedUnscaledTime;
                interactionEvents++;
                break;
            case "leftStop":
                interactionLeftStop = Time.fixedUnscaledTime;
                interactionDuration += interactionLeftStop - interactionLeftStart;
                break;
            case "rightStop":
                interactionRightStop = Time.fixedUnscaledTime;
                interactionDuration += interactionRightStop - interactionRightStart;
                break;
		}
	}
    */

    public void globalSyncCallback(object in_cookie, AkCallbackType in_type, object in_info)
	{
        // Send beat and bar messages to Manager script
        for (int i = 0; i < 1; i++)
        {
            if (in_type == AkCallbackType.AK_MusicSyncBeat)
            {
                beatAdd();
            }
            if (in_type == AkCallbackType.AK_MusicSyncBar)
            {
                barAdd();
            }
            if (in_type == AkCallbackType.AK_MusicSyncExit)
			{
                exitCueAdd();
			}

        }

        // Update bar and beat duration information and send to Manager script
        AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)in_info;
        float beatDuration = info.segmentInfo_fBeatDuration;
        float barDuration = info.segmentInfo_fBarDuration;
        updateTempoAndMetre(beatDuration, barDuration);

    }

    private void beatAdd()
    {

        if (beat < beatsPerBar) beat++;
        else beat = 1;

        // Debug.Log("Beat " + beat);

    }

    private void barAdd()
	{
        bar++;
	}

    private void exitCueAdd()
	{
        segment++;
	}

    private void updateTempoAndMetre(float beatDuration, float barDuration)
	{
        tempo = 60 / beatDuration;
        beatsPerBar = (int)(barDuration / beatDuration);
    }

    /*
    IEnumerator updateTime(float waitTime)
	{
		while (true)
		{
            // Only update once per second
            yield return new WaitForSecondsRealtime(waitTime);

            // Find ratio of interacting time in % (hence the *100)
            interactionRatio = (interactionDuration / Time.fixedUnscaledTime)*100;
            // Debug.Log(string.Format("Total duration is {0: #.00}s with {1: #.00}s of interactions making a total interaction ratio of {2: #.}%", Time.fixedUnscaledTime, interactionDuration, interactionRatio));
        }
        
    }
    */

    /*
    private void updateVariations()
	{
        if (isChorus)
        {
            variation = Random.Range(0, chorusMaxVariations - 1);
            Variations[variation].SetValue();
            Debug.Log("Variation " + variation);
        }
        else if (isVerse)
        {
            variation = Random.Range(0, verseMaxVariations - 1);
            Variations[variation].SetValue();
            Debug.Log("Variation " + variation);
        }
        else if (isInstrumental)
        {
            variation = Random.Range(0, instMaxVariations - 1);
            Variations[variation].SetValue();
            Debug.Log("Variation " + variation);
        }
    }
    */

    public void changeState(int section)
	{
        if(section < 3)
		{
            Sections[section].SetValue();
        }
        else if (section == 3)
		{
            StartMenu.SetValue();
		}
	}

    public void updateInstrumentSoundbanks(string objectID, int currentSoundbank)
    {
        banksToLoad.Clear();
        loadedSoundbanks.Clear();
        _globalRequiredSoundbanks.Clear();

        foreach (KeyValuePair<string, bool> soundbank in soundbanks)
		{
            if(soundbank.Value == true)
			{
                loadedSoundbanks.Add(soundbank.Key);
			}
		}

		// If _requiredSoundbanks is not null, start code. Otherwise, populate list.
		if (!_requiredSoundbanks.ContainsKey(objectID))
		{
			_requiredSoundbanks.Add(objectID, currentSoundbank);
		}
		else
		{
			_requiredSoundbanks[objectID] = currentSoundbank;
		}

        int j;
        int k;

        // Add required soundbanks to _global hashset
        foreach (KeyValuePair<string, int> soundbank in _requiredSoundbanks)
        {
            // Debug.Log("Object " + soundbank.Key + " is " + soundbank.Value);
            _globalRequiredSoundbanks.Add(instrumentSoundbankNames[soundbank.Value]);

            // Find the buffer zone value on either side
            if (soundbank.Value == 0)
            {
                j = InstrumentSoundbanks.Length - 1;
                k = soundbank.Value + 1;

                _globalRequiredSoundbanks.Add(instrumentSoundbankNames[j]);
                _globalRequiredSoundbanks.Add(instrumentSoundbankNames[k]);
            }
            else if (soundbank.Value == InstrumentSoundbanks.Length - 1)
            {
                j = soundbank.Value - 1;
                k = 0;

                _globalRequiredSoundbanks.Add(instrumentSoundbankNames[j]);
                _globalRequiredSoundbanks.Add(instrumentSoundbankNames[k]);
            }
            else
            {
                j = soundbank.Value - 1;
                k = soundbank.Value + 1;

                _globalRequiredSoundbanks.Add(instrumentSoundbankNames[j]);
                _globalRequiredSoundbanks.Add(instrumentSoundbankNames[k]);
            }

        }

        // For debugging purposes only
        foreach (string i in _globalRequiredSoundbanks)
        {
            if (soundbanks[i] == false)
            {
                banksToLoad.Add(i);
                //Debug.Log("toLoad " + i);
            }
        }

        loadedSoundbanks.ExceptWith(_globalRequiredSoundbanks);

        updateBanks();

        // DEBUGGING
        /*
        foreach (string i in loadedBanks)
		{
            Debug.Log("Banks to unload: " + i);
		}
        */
        /*
        foreach (KeyValuePair<string, bool> soundbank in soundbanks)
        {
            Debug.Log("Soundbank " + soundbank.Key + " is loaded: " + soundbank.Value);
        }
        */


    }

    private void updateBanks()
	{
        if (banksUpdating != null)
		{
            StopCoroutine(banksUpdating);
		}

        banksUpdating = StartCoroutine(commenceBankUpdate());
	}

    private IEnumerator commenceBankUpdate()
	{
        // Debug.Log("Coroutine Started");

        foreach (string i in banksToLoad)
		{
            bankLoad(i);
            yield return null;
		}

        yield return null;

        foreach(string i in loadedSoundbanks)
		{
            bankUnload(i);
            yield return null;
		}

        banksUpdating = null;
        yield break;
	}

    private void bankLoad(string bank)
	{
        switch (bank)
		{
            case "Instrument1":
                InstrumentSoundbanks[0].LoadAsync();
                break;
            case "Instrument2":
                InstrumentSoundbanks[1].LoadAsync();
                break;
            case "Instrument3":
                InstrumentSoundbanks[2].LoadAsync();
                break;
            case "Instrument4":
                InstrumentSoundbanks[3].LoadAsync();
                break;
            case "Instrument5":
                InstrumentSoundbanks[4].LoadAsync();
                break;
            case "Instrument6":
                InstrumentSoundbanks[5].LoadAsync();
                break;
        }

        soundbanks[bank] = true;
        // Debug.Log(bank + " loading");

    }

    private void bankUnload(string bank)
	{
        switch (bank)
        {
            case "Instrument1":
                InstrumentSoundbanks[0].Unload();
                return;
            case "Instrument2":
                InstrumentSoundbanks[1].Unload();
                break;
            case "Instrument3":
                InstrumentSoundbanks[2].Unload();
                break;
            case "Instrument4":
                InstrumentSoundbanks[3].Unload();
                break;
            case "Instrument5":
                InstrumentSoundbanks[4].Unload(); 
                break;
            case "Instrument6":
                InstrumentSoundbanks[5].Unload();
                break;
        }

        soundbanks[bank] = false;
        Debug.Log(bank + " unloading");
    }
        
    /*
    private IEnumerator commenceSoundbankUpdate()
	{
        yield return new WaitForSecondsRealtime(0.1f);



        foreach(int m in banksToLoad)
		{
            loadedSoundbanks.Add(m);
            Debug.Log("banksToLoad = " + m);
            yield return null;
		}

        foreach(int i in loadedSoundbanks)
		{
            Debug.Log("loaded soundbank " + i);
		}

        //banksToUnload = loadedSoundbanks;
        //banksToUnload.ExceptWith(_globalRequiredSoundbanks);

        foreach (int n in banksToUnload)
		{
            loadedSoundbanks.Remove(n);
            Debug.Log("banksToUnload = " + n);
            yield return null;
		}
        // While testing, do not continue code.
        yield break;

        foreach (int k in banksToUnload)
        {
            InstrumentSoundbanks[k].Unload();
            loadedSoundbanks.Remove(k);
            Debug.Log("Unloaded " + k);
        }
        yield return new WaitForFixedUpdate();

        // Load unloaded soundbanks
        foreach (int j in banksToLoad)
        {
            InstrumentSoundbanks[j].LoadAsync();
            loadedSoundbanks.Add(j);
            Debug.Log("Loaded " + j);
        }

        foreach(int i in loadedSoundbanks)
		{
            Debug.Log("Loaded TEST " + i);
		}

        


        yield break;
	}
     */  

    private IEnumerator startLoadingScreenMusic(float wait)
	{
        yield return new WaitForSecondsRealtime(wait);
        Intro.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncExit, startMenuStart);
        loadingScreenPoem.StartLoadingScreen();
    }

    public void loadingScreenDone()
	{
        localAvatar.SetActive(true);
        loadingScreenCanvas.SetActive(false);
	}

	void startMenuStart(object in_cookie, AkCallbackType in_type, object in_info)
	{
        // Debug.Log("Start Menu has commenced");
        changeState(3);

        // Find all core objects
        coreObjects = (CoreSong_ObjectController[]) GameObject.FindObjectsOfType(typeof(CoreSong_ObjectController));

        // Find all instantiated object's triggers for playback
        triggers = (AkTriggerCoreSong[]) GameObject.FindObjectsOfType(typeof(AkTriggerCoreSong));
        
        // Post the clicktrack and the core song object triggers
        Clicktrack.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncAll, globalSyncCallback);
        foreach (AkTriggerCoreSong trigger in triggers)
        {
            trigger.CoreSongStart();
        }
        foreach (CoreSong_ObjectController coreObject in coreObjects)
		{
            coreObject.coreSongTriggered();
		}

    
        

        // droneDefault.CoreSongStart();
        // GetComponent<AkTriggerCoreSong>().CoreSongStart();

    }


    private void OnDestroy()
    {
        // AkSoundEngine.UnregisterAllGameObj();
    }

    #endregion

}

