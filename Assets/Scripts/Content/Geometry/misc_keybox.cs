using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misc_keybox : MonoBehaviour {

    public GameObject keybox_dialog;
    public GameObject keybox_anim;
    public GameObject dialog;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(keybox_dialog.activeSelf)
        {
            if (keybox_anim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish")){
                keybox_dialog.SetActive(false);
                dialog.SetActive(true);
            }
        }
	}
}
