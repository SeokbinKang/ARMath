using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GeometryOpener : MonoBehaviour {

    public GameObject dialogue_text_status;

    public GameObject dialogue_text_prompt;
    public GameObject ContentModuleRoot;
    // Use this for initialization
    void Start()
    {
        //load dialogue text
        Reset();
    }
    void OnEnable()
    {
        Reset();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Reset()
    {
        dialogue_text_status.SetActive(false);

        dialogue_text_prompt.SetActive(false);
        loadDialogue();
    }
    public void loadDialogue()
    {

        string target_object_name = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_name;



        //  Debug.Log("[ARMATH] : " + ContentModuleRoot.GetComponent<ContentCounting>().target_object_name + " \t " + ContentModuleRoot.GetComponent<ContentCounting>().found_object_count);
        if (target_object_name == "") {
        }
        else
        {
            dialogue_text_status.SetActive(true);
            dialogue_text_status.GetComponent<Text>().text = "Oh! I've found a " + target_object_name + " there!";
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_status.GetComponent<Text>().text);
        }



    }
    
    public void showprompt()
    {
        

        string target_object_name = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_name;
        dialogue_text_prompt.SetActive(true);
        dialogue_text_prompt.GetComponent<Text>().text = "Can you help me identify the "+ target_object_name+" shape?";
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_prompt.GetComponent<Text>().text);
    }
}
