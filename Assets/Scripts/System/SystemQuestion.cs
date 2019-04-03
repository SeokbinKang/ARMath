using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemQuestion : MonoBehaviour {

    public GameObject content_root;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        content_root.GetComponent<ContentRoot>().closeAllContent();
    }
}
