using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hardclick : MonoBehaviour {
    public int click_to_activate;

    private int click_n;
	// Use this for initialization
	void Start () {
        click_n = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onClickUser()
    {
        click_n++;
        if(click_n % click_to_activate ==0)
        {
            SystemControl.mSystemControl.onUser();
        }
    }
    public void onQuestion()
    {
        
        click_n++;
        if (click_n % click_to_activate == 0)
        {
            SystemControl.mSystemControl.onSelectionQuestion();
        }
    }
}
