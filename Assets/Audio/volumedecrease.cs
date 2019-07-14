using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumedecrease : MonoBehaviour {

    public float init_volume;
    public float delay;
    public float final_volume;
    // Use this for initialization
    private float start_time;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > start_time + delay)
        {

            float cur = this.GetComponent<AudioSource>().volume;
            if (Mathf.Abs(cur - final_volume) < 0.05) return;
            this.GetComponent<AudioSource>().volume = cur * 0.8f + final_volume * 0.2f;

        }

	}
    private void OnEnable()
    {
        this.GetComponent<AudioSource>().volume = init_volume;
        start_time = Time.time;
    }
}
