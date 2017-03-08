using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;
using Vuforia;

public class PPTManager : MonoBehaviour {
    //private GameObject[] ppt;
    static int index = 0;
    public GameObject[] arr;
    public static PPTManager Instance { get; private set; }
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    GestureRecognizer recognizer;

    // Use this for initialization
    void Start () {
        //ppt = GetComponents<GameObject>();

        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            this.gameObject.SendMessageUpwards("OnSelect");
 
        };
        recognizer.StartCapturingGestures();

        foreach (var im in arr)
        {
            im.gameObject.SetActive(false);
        }

        arr[0].gameObject.SetActive(true);

        keywords.Add("Next", () =>
        {
            this.gameObject.SendMessageUpwards("OnSelect");
        });

        keywords.Add("Quit", () =>
        {
            this.gameObject.SendMessageUpwards("OnQuit");
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void OnSelect()
    {
        arr[index].gameObject.SetActive(false);
        if (index < (arr.Length - 1))
        {
            index++;
        }else
        {
            index = 0;
        }
        arr[index].gameObject.SetActive(true);
    }

    void OnQuit()
    {
        this.gameObject.SetActive(false);
    }
}
