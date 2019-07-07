using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedImage : MonoBehaviour {

    public float delay;
    public bool TTS_text;
    private float time_after_enable;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if(Time.time - time_after_enable >=delay)
        {
            RawImage ri = this.GetComponent<RawImage>();            
            if (ri != null) ri.enabled = true;

            Image ii = this.GetComponent<Image>();
            if (ii != null) ii.enabled = true;

            Text label = this.GetComponent<Text>();
            if (label != null)
            {
                label.enabled = true;

                if (TTS_text) TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(label.text.Replace("\n", ""));
            }
            time_after_enable = float.MaxValue;

        }
		
	}

    private void OnEnable()
    {
        RawImage ri = this.GetComponent<RawImage>();
        
        if (ri != null)
        {
            ri.enabled = false;            
        }

        Image ii = this.GetComponent<Image>();        
        if (ii != null)
        {
            ii.enabled = false;            
        }

        Text label = this.GetComponent<Text>();
        if (label != null)
        {
            label.enabled = false;
        }

        time_after_enable = Time.time;
    }
    private void OnDisable()
    {

        RawImage ri = this.GetComponent<RawImage>();      
        if (ri != null) ri.enabled = false;
        Image ii = this.GetComponent<Image>();
        if (ii != null) ii.enabled = true;
        Text label = this.GetComponent<Text>();
        if (label != null) label.enabled = false;
    }

}
