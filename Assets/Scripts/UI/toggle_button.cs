using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggle_button : MonoBehaviour {


    public bool value_;

    public GameObject toggle_off;
    public GameObject toggle_on;

	// Use this for initialization
	void Start () {
    

    }
	
	// Update is called once per frame
	void Update () {
        if (value_)
        {
            toggle_on.SetActive(true);
            toggle_off.SetActive(false);
        } else
        {
            toggle_on.SetActive(false);
            toggle_off.SetActive(true);
        }
		
	}
    
    public void set_value(bool t)
    {
        value_ = t;
    }
    public void toggle()
    {
        value_ = !value_;
    }
}
