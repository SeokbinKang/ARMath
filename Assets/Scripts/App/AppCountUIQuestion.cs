using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppCountUIQuestion : MonoBehaviour {

    public GameObject promptString;
    public GameObject AppCountingUI;
    public GameObject TTS;
    private string targetobjectname;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updatePromptText(string t)
    {
        promptString.GetComponent<Text>().text = "Let's count the number of "+t+"s\n\n"+"Touch them on the screen";

        targetobjectname = t;

        TTS.GetComponent<TTS>().StartTextToSpeech("Let's count the number of " + t + "s");
    }

    public void StartCounting()
    {
        AppCountingUI.GetComponent<AppCounting>().SetUICount(targetobjectname);
    }
}
