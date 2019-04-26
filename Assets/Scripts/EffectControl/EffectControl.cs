using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour {

    public static EffectControl mThis;

    public GameObject balloon_ceremony;    

	// Use this for initialization
	void Start () {
        mThis = this;
       

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Reset()
    {
        balloon_ceremony.SetActive(false);
    }
    public static void ballon_ceremony()
    {
        mThis.Reset();
        mThis.balloon_ceremony.SetActive(true);
        
    }
}
