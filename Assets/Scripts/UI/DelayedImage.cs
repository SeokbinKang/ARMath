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
            /*  if (ri == null) return;
              Color c = Color.white;            
              ri.color = c;
              */
            if (ri != null) ri.enabled = true;
            Text label = this.GetComponent<Text>();
            if (label != null) label.enabled = true;
            time_after_enable = float.MaxValue;

        }
		
	}

    private void OnEnable()
    {
        RawImage ri = this.GetComponent<RawImage>();
        time_after_enable = Time.time;
        if (ri != null)
        {
            
           /* Color c = Color.white;
            c.a = 0;
            ri.color = c;
            time_after_enable = Time.time;*/
            ri.enabled = false;
            
        }
        Text label = this.GetComponent<Text>();
        if (label != null)
        {

            label.enabled = false;

        }
    }
    private void OnDisable()
    {

        RawImage ri = this.GetComponent<RawImage>();
      
        if (ri != null) ri.enabled = false;
        Text label = this.GetComponent<Text>();
        if (label != null) label.enabled = false;
    }

}
