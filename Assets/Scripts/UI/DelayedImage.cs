using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedImage : MonoBehaviour {

    public float delay;
    private float time_after_enable;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time - time_after_enable >=delay)
        {
            RawImage ri = this.GetComponent<RawImage>();
            if (ri == null) return;
            Color c = Color.white;            
            ri.color = c;
            time_after_enable = float.MaxValue;
        }
		
	}

    private void OnEnable()
    {
        RawImage ri = this.GetComponent<RawImage>();
        if (ri == null) return;
        Color c = Color.white;
        c.a = 0;
        ri.color = c;
        time_after_enable = Time.time;
    }
    private void OnDisable()
    {
        RawImage ri = this.GetComponent<RawImage>();
        if (ri == null) return;
        Color c = Color.white;
        c.a = 0;
        ri.color = c;
    }

}
