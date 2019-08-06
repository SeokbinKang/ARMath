using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCountable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick()
    {
        FeedbackGenerator.target_counting(this.gameObject, 0, 120);
        Destroy(this.gameObject);
    }
}
