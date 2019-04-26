using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionOpener : MonoBehaviour
{

    public GameObject dialogue_text_status;
    public GameObject dialogue_text_none;
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
        dialogue_text_none.SetActive(false);
        dialogue_text_prompt.SetActive(false);
        loadDialogue();
    }
    public void loadDialogue()
    {

        string target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;

        



        //  Debug.Log("[ARMATH] : " + ContentModuleRoot.GetComponent<ContentCounting>().target_object_name + " \t " + ContentModuleRoot.GetComponent<ContentCounting>().found_object_count);
        if (target_object_name == "") dialogue_text_none.SetActive(true);
        else
        {
            dialogue_text_status.SetActive(true);
            dialogue_text_status.GetComponent<Text>().text = "Oh! I need to collect " + goal_n + " " + target_object_name + "s!";
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_status.GetComponent<Text>().text);
        }



    }
    private void set_init_and_goal_count()
    {

    }
    public void showprompt()
    {
        System.Random random = new System.Random();

        string target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        //        if (target_n_of_objects > 1) target_n_of_objects = random.Next(2, target_n_of_objects);
        dialogue_text_prompt.SetActive(true);
        dialogue_text_prompt.GetComponent<Text>().text = "I've got only " + init_n + " " + target_object_name + "s. Can you bring me more?";
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_prompt.GetComponent<Text>().text);
    }
}

