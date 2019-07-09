using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogElement : MonoBehaviour {

    public GameObject DialogRoot;
    public GameObject mText;
    public GameObject mBG;
    public bool preserveFontsize;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    private void OnEnable()
    {
        mText.SetActive(true);
        mBG.SetActive(true);
    }

    public void setText(string msg)
    {
        
        mText.GetComponent<Text>().text = msg;
        if (!preserveFontsize)
        {
            if (msg.Length > 95) mText.GetComponent<Text>().fontSize = 90;
            else mText.GetComponent<Text>().fontSize = 110;
        }
    }
    public void setTTS(bool t)
    {
        mText.GetComponent<DelayedImage>().TTS_text = t;
    }
    public void onClick()
    {
        DialogRoot.GetComponent<Dialogs>().OnNext();
    }
}
