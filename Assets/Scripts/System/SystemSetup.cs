using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSetup : MonoBehaviour {


    public GameObject button_interaction_touch;
    public GameObject button_interaction_object;


    // Use this for initialization
    void Start () {
	    // retrieve initial setting	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool setting_interaction_touch()
    {
        return button_interaction_touch.GetComponent<toggle_button>().value_;

    }
    public bool setting_interaction_object()
    {
        return button_interaction_object.GetComponent<toggle_button>().value_;

    }
}
