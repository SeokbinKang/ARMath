using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCountingTargetObject : MonoBehaviour {

    public GameObject UICount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onCounting()
    {
        UICount.GetComponent<AppCountingUICounting>().AddCount(this.GetComponent<RectTransform>().position);
    }
}
