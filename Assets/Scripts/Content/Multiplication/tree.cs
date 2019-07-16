using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree : MonoBehaviour {
    public GameObject mask_obj;

    public bool auto_turnoff;
    public float auto_turnoff_time;

    private float start_time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (auto_turnoff)
        {
            if (Time.time > start_time + auto_turnoff_time)
            {
                stop_randome_light(0.1f);
                auto_turnoff = false;
            }
        }
	}
    private void OnEnable()
    {
        start_time = Time.time;
    }
    public void stop_randome_light(float percent)
    {
        mask_obj.GetComponent<vertical_mask>().random_move = false;
        mask_obj.GetComponent<vertical_mask>().visible_percent = percent;
    }
}
