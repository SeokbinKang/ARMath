using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPrompt : MonoBehaviour {

    public GameObject DialogRoot;
    public GameObject mText;
    public GameObject mBG;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnEnable()
    {
        mText.SetActive(true);
        mBG.SetActive(true);
    }

    public void setText(string msg,bool play)
    {
        mText.GetComponent<Text>().text = msg;
        if (play) this.animate("");
    }
    public void animate(string anim_name)
    {
        //animate the prompt dialogue
        this.GetComponent<Animator>().SetTrigger("slideleft");
        //sound effect
        this.GetComponent<AudioSource>().Play();
    }

    public void onClick()
    {
        //hide
    }
}
