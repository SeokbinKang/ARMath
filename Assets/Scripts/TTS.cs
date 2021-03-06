﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using FantomLib;

public class TTS : MonoBehaviour {

    public static TTS mTTS;
    public GameObject receiveObject;
    public Text displayText;
    public Text statusText;
//    public Animator statusAnimator;

    public Text speedText;
    public Text pitchText;
    public float speakPicthStep = 0.25f;    //Text reading pitch step
    public float speakSpeedStep = 0.25f;    //Text reading speed step
    public float defaultpitch=1f;
    public float defaultspeed=0.8f;


    // Use this for initialization
    private void Start()
    {
        mTTS = this;
        if (receiveObject == null)
            receiveObject = this.gameObject;
        defaultpitch = 1f;
        defaultspeed = 0.8f;
#if UNITY_EDITOR
    Debug.Log("InitSpeechRecognizer");

#elif UNITY_ANDROID
        AndroidPlugin.InitTextToSpeech(receiveObject.name, "OnStatus"); //Check the initialize status
        //defaultpitch = AndroidPlugin.GetTextToSpeechPitch();
        //defaultspeed = AndroidPlugin.GetTextToSpeechSpeed();
        AndroidPlugin.SetTextToSpeechSpeed(defaultspeed);
        Debug.Log("[ARMath] TTS pitch="+defaultpitch+"   speed="+defaultspeed);

#endif
    }

    // Update is called once per frame
    //private void Update () {

    //}



    //Reading text currently displayed (for button)
    public void PlayTextToSpeech()
    {
        if (displayText != null && !string.IsNullOrEmpty(displayText.text))
            StartTextToSpeech(displayText.text);
    }


    //Start Text To Speech
    public void StartTextToSpeech(string message)
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        Debug.Log("StartTextToSpeech : message = " + message);
        AndroidPlugin.StartTextToSpeech(message, receiveObject.name, "OnStatus", "OnStart", "OnDone", "OnStop");
#endif
    }


    //Text To Speech status callback handler
    private void OnStatus(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnStatus");
#endif
        if (statusText != null)
            statusText.text = message;

        if (displayText != null)
        {
            if (message.StartsWith("SUCCESS_INIT"))
                displayText.text += "\nText To Speech is available.";
            else if (message.StartsWith("ERROR_LOCALE_NOT_AVAILABLE"))
                displayText.text += "\nFailed to initialize Text To Speech. It is a language that can not be used.";
            else if (message.StartsWith("ERROR_INIT"))
                displayText.text += "\nFailed to initialize Text To Speech.";
        }
    }

    //Callback handler when start reading text
    private void OnStart(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnStart");
#endif
        

        if (statusText != null)
        {
            //statusText.text = message;
            statusText.text = "Speaking";
        }
    }

    //Callback handler when finish reading text
    private void OnDone(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnDone");
#endif
        
        if (statusText != null)
        {
            //statusText.text = message;
            statusText.text = "Finished";
        }
    }

    //Callback handler when interrupted reading text
    private void OnStop(string message)
    {
#if UNITY_EDITOR
        Debug.Log("OnStop");
#endif
        
        if (statusText != null)
        {
            //statusText.text = message;
            statusText.text = "Stopped" + (message.StartsWith("INTERRUPTED") ? "(interrupted)" : "");
        }
    }


    //Interrupted reading text
    public void StopTextToSpeech()
    {
#if UNITY_EDITOR
        Debug.Log("StopTextToSpeech called");
#elif UNITY_ANDROID
        AndroidPlugin.StopTextToSpeech();
#endif
    }


#if UNITY_EDITOR
    //For debug (Editor only)
    private IEnumerator DebugSimulate()
    {
        OnStart("onStart");
        yield return new WaitForSeconds(3f);

        OnDone("onDone");
    }
#endif


    //Increase utterance speed of Text To Speech
    public void SpeakSpeedUp()
    {
#if UNITY_EDITOR
        Debug.Log("SpeakSpeedUp called");
#elif UNITY_ANDROID
        SetSpeedText(AndroidPlugin.AddTextToSpeechSpeed(speakSpeedStep));
#endif
    }

    
    //Decrease utterance speed of Text To Speech
    public void SpeakSpeedDown()
    {
#if UNITY_EDITOR
        Debug.Log("SpeakSpeedDown called");
#elif UNITY_ANDROID
        SetSpeedText(AndroidPlugin.AddTextToSpeechSpeed(-speakSpeedStep));
#endif
    }


    //Reset utterance speed of Text To Speech (1.0f)
    public void SpeakSpeedReset()
    {
#if UNITY_EDITOR
        Debug.Log("SpeakSpeedReset called");

#elif UNITY_ANDROID
        //SetSpeedText(AndroidPlugin.ResetTextToSpeechSpeed());
        AndroidPlugin.SetTextToSpeechSpeed(defaultspeed);
#endif
    }


    //Increase utterance pitch of Text To Speech
    public void SpeakPitchUp()
    {
        SetPitchText(AndroidPlugin.AddTextToSpeechPitch(speakPicthStep));
#if UNITY_EDITOR
        Debug.Log("SpeakPitchUp called");
#elif UNITY_ANDROID
        SetPitchText(AndroidPlugin.AddTextToSpeechPitch(speakPicthStep));
#endif
    }


    //Decrease utterance pitch of Text To Speech
    public void SpeakPitchDown()
    {
#if UNITY_EDITOR
        Debug.Log("SpeakPitchDown called");
#elif UNITY_ANDROID
        SetPitchText(AndroidPlugin.AddTextToSpeechPitch(-speakPicthStep));
#endif
    }


    //Reset utterance pitch of Text To Speech (1.0f)
    public void SpeakPitchReset()
    {
#if UNITY_EDITOR
        Debug.Log("SpeakPitchReset called");

#elif UNITY_ANDROID
        //SetPitchText(AndroidPlugin.ResetTextToSpeechPitch());
        AndroidPlugin.SetTextToSpeechPitch(defaultpitch);

#endif
    }



    //Display utterance speed
    private void SetSpeedText(float speed)
    {
        if (speedText != null)
            speedText.text = string.Format("Speed : {0:F2}", speed);
    }

    //Display utterance pitch
    private void SetPitchText(float pitch)
    {
        if (pitchText != null)
            pitchText.text = string.Format("Pitch : {0:F2}", pitch);
    }



    //Call the text edit Dialog
    public void EditText()
    {
        if (displayText != null)
        {
#if UNITY_EDITOR
            Debug.Log("EditText called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowMultiLineTextDialog("Edit text", displayText.text, 0, 9, receiveObject.name, "OnEditText");
#endif
        }
    }

    //Callback handler in text edit Dialog
    private void OnEditText(string message)
    {
        if (displayText != null)
            displayText.text = message.Trim();
    }
}
