using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppGem : MonoBehaviour {

    public GameObject AppCountingUI;
    public string objectName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GemOpened()
    {
        AppCountingUI.GetComponent<AppCounting>().SetUIQuestion(objectName);
    }
}
