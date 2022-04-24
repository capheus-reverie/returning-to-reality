using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class loadingScreenPoems : MonoBehaviour
{
    #region Variables

    [Tooltip("Each line of a poem should be an array element within the poem's array")]
    [SerializeField]
    private string[] poemOne;
    [SerializeField]
    private string[] poemTwo;

    [SerializeField]
    private Color defaultColour;
    [SerializeField]
    private Color climaxColour;

    private float startTime = 5.0f;
    private float climaxTime = 45.0f;
    private float transitionTime = 1.0f;
    private float textDur = 5.0f;
    
    private TextMeshProUGUI text;
    private Image background;

	#endregion

	#region Unity Methods

	private void Awake()
	{
        text = this.gameObject.GetComponent<TextMeshProUGUI>() ?? this.gameObject.AddComponent<TextMeshProUGUI>();
        background = this.gameObject.GetComponentInParent<Image>();
	}

	public void StartLoadingScreen()
    {
        int r = Random.Range(1,3);
        // Debug.Log("selected: " + r);

        switch (r)
		{
            case 1:
                StartCoroutine(poemTextUpdate(poemOne));
                break;
            case 2:
                StartCoroutine(poemTextUpdate(poemTwo));
                break;
		}
    }

    private IEnumerator poemTextUpdate(string[] poem)
	{
        /*
        Debug.Log("Poem update coroutine started fine, with the following poem: ");
        foreach(string l in poem)
		{
            Debug.Log(l);
		}
        */


		float timePerLine = (climaxTime - startTime) / (poem.Length - 1); // since the last array is always "an inconsequential mess"
        if (0.2f * timePerLine < 1.0f) transitionTime = 0.2f * timePerLine;
        textDur = timePerLine - (2.0f * transitionTime);
        int line = 0;
        bool isFinal = false;

        yield return new WaitForSecondsRealtime(startTime);

        while (line < poem.Length)
		{
            // Debug.Log("Updated Line " + (line + 1));

            Color colour = defaultColour;
            if (line == poem.Length - 1)
            {
                // Debug.Log("Setting up isFinal and climaxcolour");
                colour = climaxColour;
                isFinal = true;
            }

            colour.a = 0.0f;
            text.color = colour;
            text.text = poem[line];

            line++;
            yield return StartCoroutine(updateLineText(isFinal));
        }
        yield return null;

        Manager.instance.loadingScreenDone();
    }

    private IEnumerator updateLineText(bool isFinal)
	{
        float timeElapsed = 0.0f;
        Color colour = text.color;
        Color backgroundColour = background.color;

        // Debug.Log("Fading in text");

        while (timeElapsed < transitionTime)
		{
            float t = timeElapsed / transitionTime;
            t = t * t * (3f - 2f * t);

            colour.a = Mathf.Lerp(0.0f, 1.0f, t);
            text.color = colour;
            timeElapsed += Time.deltaTime;
                
            yield return null;
		}

        yield return new WaitForSecondsRealtime(textDur);

        // Debug.Log("Fading out text");

        timeElapsed = 0.0f;

        while (timeElapsed < transitionTime)
		{
            float t = timeElapsed / transitionTime;
            t = t * t * (3f - 2f * t);

            colour.a = Mathf.Lerp(1.0f, 0.0f, t);
            text.color = colour;

            if (isFinal == true)
            {
                float b = timeElapsed / (transitionTime * 3);
                backgroundColour.a = Mathf.Lerp(1.0f, 0.0f, b);
                background.color = backgroundColour;

                // Debug.Log("Updating background colour to: " + backgroundColour);
                //CanvasRenderer background =  GetComponentInParent<CanvasRenderer>();
                //background.SetAlpha(colour.a);
            }

            timeElapsed += Time.deltaTime;

            yield return null;

		}
	}

	



            // int totalVisibleCharacters = text.textInfo.characterCount;
            // int counter = 0;

            //while (counter < totalVisibleCharacters)
			//{
                //int visibleCount = counter % (totalVisibleCharacters + 1);
                //text.maxVisibleCharacters = visibleCount;
                
                //counter += 1;

                //Debug.Log("Character Update");

                //yield return new WaitForSecondsRealtime(timePerChar);
			//}

    #endregion

}
