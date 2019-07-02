using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingOpener : MonoBehaviour {

    
    public GameObject ContentModuleRoot;
	// Use this for initialization
	void Start () {
        //load dialogue text
       // Reset();
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
       
        loadDialogue();
    }
    public void loadDialogue()
    {
        
        string target_object_name = "";
        target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
        int target_n_of_objects = ContentModuleRoot.GetComponent<ContentCounting>().found_object_count;

        if (target_object_name != "") 
        {


            /*dialogue_text_status.GetComponent<Text>().text = "Oh! I have been looking for " + target_object_name + "s! " + "There is a SET of them";
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(dialogue_text_status.GetComponent<Text>().text);*/
            Debug.Log("[ARMath]Load Dialogggg");
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "Oh! I have been looking for " + target_object_name + "s! " + "There is a SET of them",
              true,
              null,
              ""
              ));
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                   "Can you get me " + target_n_of_objects.ToString() + " " + target_object_name + "s?",
                   true,
                   new CallbackFunction(FinishOpener),
                   "none"
                   ));
        }




    }

    public void FinishOpener(string p)
    {
        ContentModuleRoot.GetComponent<ContentCounting>().StartSolver();
    }

}

