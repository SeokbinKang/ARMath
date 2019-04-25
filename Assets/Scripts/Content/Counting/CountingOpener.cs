using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingOpener : MonoBehaviour {

    public GameObject dialogue_text_status;
    public GameObject dialogue_text_none;
    public GameObject dialogue_text_prompt;
    public GameObject ContentModuleRoot;
	// Use this for initialization
	void Start () {
        //load dialogue text
        Reset();
    }
    void OnEnable()
    {
        Reset();
    }
    // Update is called once per frame
    void Update () {
		
	}
    private void Reset()
    {
        dialogue_text_status.SetActive(false);
        dialogue_text_none.SetActive(false);
        dialogue_text_prompt.SetActive(false);
        loadDialogue();
    }
    public void loadDialogue()
    {
        string dialogue = "Oh!There is a set of OOOs.";
        string target_object_name = "";
        target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;

      //  Debug.Log("[ARMATH] : " + ContentModuleRoot.GetComponent<ContentCounting>().target_object_name + " \t " + ContentModuleRoot.GetComponent<ContentCounting>().found_object_count);
        if (target_object_name == "") dialogue_text_none.SetActive(true);
        else
        {
            dialogue_text_status.SetActive(true);
            dialogue_text_status.GetComponent<Text>().text = "Oh!I have been looking for " + target_object_name + "s! " + "There is a SET of them";
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_status.GetComponent<Text>().text);
        }



    }
    public void showprompt()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 100);
        string target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
        int target_n_of_objects = ContentModuleRoot.GetComponent<ContentCounting>().found_object_count;
        //        if (target_n_of_objects > 1) target_n_of_objects = random.Next(2, target_n_of_objects);
        dialogue_text_prompt.SetActive(true);
        dialogue_text_prompt.GetComponent<Text>().text = "Can you get me " + target_n_of_objects.ToString() + " " + target_object_name + "s?";
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_prompt.GetComponent<Text>().text);
    }
}

