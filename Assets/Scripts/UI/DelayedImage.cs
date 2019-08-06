using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedImage : MonoBehaviour {

    public float delay;
    public bool TTS_text;
    public bool TTS_pitch_down;
    
    private float time_after_enable;

    private CallbackFunction callback;
    private string callback_param;
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

                if (TTS_text)
                {
                    
                    if (TTS_pitch_down)
                    {
                        
                        TTS.mTTS.SpeakPitchDown();
                        TTS.mTTS.SpeakSpeedDown();

                    }
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(label.text.Replace("\n", ""));
                    if (TTS_pitch_down)
                    {
                        TTS.mTTS.SpeakPitchReset();
                        TTS.mTTS.SpeakSpeedReset();
                        /*TTS.mTTS.SpeakPitchUp();
                        TTS.mTTS.SpeakSpeedUp();*/

                    }
                }
            }
            Animator animc = this.GetComponent<Animator>();
            if (animc != null)
            {
                animc.enabled = true;
            }
            AudioSource asrc = this.GetComponent<AudioSource>();
            if (asrc != null)
            {
                asrc.enabled = true;
            }

            time_after_enable = float.MaxValue;
            if (callback != null)
            {
                callback(callback_param);
                callback = null;
            }


        }
		
	}
    public void setDelay(float t)
    {
        this.delay = t;
    }
    public void setCallback(CallbackFunction c,string p)
    {
        callback = c;
        callback_param = p;
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
        Animator animc = this.GetComponent<Animator>();
        if (animc != null)
        {
            animc.enabled = false;
        }
        AudioSource asrc = this.GetComponent<AudioSource>();
        if (asrc != null)
        {
            asrc.enabled = false;
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
        Animator animc = this.GetComponent<Animator>();
        if (animc != null)
        {
            animc.enabled = false;
        }

        AudioSource asrc = this.GetComponent<AudioSource>();
        if (asrc != null)
        {
            asrc.enabled = false;
        }
    }

}
