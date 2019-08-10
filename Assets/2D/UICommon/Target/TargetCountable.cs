using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCountable : MonoBehaviour {

    public bool sound_enabled;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void enable_sound(bool t)
    {
        sound_enabled = t;
    }
    public void onClick()
    {
        int num = FeedbackGenerator.target_counting(this.gameObject, 0, 120);
        if(sound_enabled)
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(num.ToString()+"!");
        }
        Destroy(this.gameObject);
    }
}
