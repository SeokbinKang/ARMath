using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour {
    public static Tools mThis;
    public GameObject finder_;
    public GameObject finder_geometry;
    
	// Use this for initialization
	void Start () {
        finder_.SetActive(false);
        finder_geometry.SetActive(false);
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
    public static void finder_init(string obj, int min_count, CallbackFunction2 cb, string cb_param, string prompt, float scale)
    {
        //call the callback when there is a set of objects inthe view. 
        mThis.finder_.GetComponent<Finder>().set_finder(obj, min_count, cb, cb_param, prompt, scale);
        mThis.finder_.SetActive(true);

    }

    public static void finder_geometry_init(List<string> obj_names, CallbackFunction cb, string p)
    {
        //call the callback when there is a set of objects inthe view.
        mThis.finder_geometry.GetComponent<Finder_geometry>().set_finder(obj_names, 1, cb, p);        
        mThis.finder_geometry.SetActive(true);

    }
}
