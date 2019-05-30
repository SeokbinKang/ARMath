using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogElement : MonoBehaviour {

    public GameObject DialogRoot;
    public GameObject mText;
    public GameObject mBG;
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
    }

    public void onClick()
    {
        DialogRoot.GetComponent<Dialogs>().OnNext();
    }
}
