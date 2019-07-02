using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textspeech : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        if (this.GetComponent<Text>() == null) return;
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(this.GetComponent<Text>().text);
    }
}
