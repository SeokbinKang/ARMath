using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemContent : MonoBehaviour {
    public GameObject contentRoot;
    // Use this for initialization
    public GameObject bg_music;
    public static SystemContent mInstance;
    public GameObject mUser;
	void Start () {
        mInstance = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        bg_music.GetComponent<AudioSource>().volume = 0f;
    }
    void OnDisable()
    {
        contentRoot.GetComponent<ContentRoot>().closeAllContent();
        bg_music.GetComponent<AudioSource>().volume = 1f;

    }
    public static void EnableLeftUserUI(bool t)
    {
        mInstance.mUser.SetActive(t);
    }
}
