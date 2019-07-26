using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour {
    public static Tools mThis;
    public GameObject finder_;
    
	// Use this for initialization
	void Start () {
        finder_.SetActive(false);
        mThis = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void finder_init(string obj, int min_count, CallbackFunction cb, string p)
    {
        //call the callback when there is a set of objects inthe view. 
        mThis.finder_.GetComponent<Finder>().set_finder(obj, min_count, cb, p);
        mThis.finder_.SetActive(true);

    }
}
