using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemContent : MonoBehaviour {
    public GameObject contentRoot;
    // Use this for initialization
    public GameObject bg_music;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        bg_music.GetComponent<AudioSource>().volume = 0.2f;
    }
    void OnDisable()
    {
        contentRoot.GetComponent<ContentRoot>().closeAllContent();
        bg_music.GetComponent<AudioSource>().volume = 1f;

    }
}
