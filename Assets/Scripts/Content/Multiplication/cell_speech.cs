using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell_speech : MonoBehaviour {
    public GameObject dialog_box;
    public GameObject textlabel;
    public GameObject character;
    public GameObject feedback_smile;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        dialog_box.SetActive(true);
        textlabel.SetActive(true);
        character.SetActive(true);
        feedback_smile.SetActive(false);
    }

    public void smile()
    {
        feedback_smile.SetActive(true);
    }
    public void finish()
    {
        textlabel.SetActive(false);
        dialog_box.SetActive(false);
        feedback_smile.SetActive(true);
    }
}
