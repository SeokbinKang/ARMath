using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemContent : MonoBehaviour {
    public GameObject contentRoot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnDisable()
    {
        contentRoot.GetComponent<ContentRoot>().closeAllContent();

    }
}
