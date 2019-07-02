using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs : MonoBehaviour {

    public static Dialogs this_;
    public GameObject element_leftbottom;
    public GameObject prompt_rightbot;


    private List<DialogItem> mDialogueItems;
    private DialogItem cur_dialog;

    // Use this for initialization
    void Start () {
        mDialogueItems = new List<DialogItem>();
        element_leftbottom.SetActive(false);
        prompt_rightbot.SetActive(true);
        this_ = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnNext()
    {
        //remove the foremost dialog
        if (mDialogueItems.Count > 0)
        {
            mDialogueItems[0].invoke_callback();
            mDialogueItems.RemoveAt(0);
        }
        //load the new foremost dialog
        load_first_dialog();
    }
    public static void Prompt_RightBot(string str)
    {
        this_.prompt_rightbot.GetComponent<DialogPrompt>().setText(str,true);
        
    }
    
    private void load_first_dialog()
    {
        if (mDialogueItems == null || mDialogueItems.Count == 0)
        {
            element_leftbottom.SetActive(false);
            //set all elements inactive
            return;
        }
        DialogItem cur = mDialogueItems[0];

        if(cur.type== DialogueType.left_bottom_plain)
        {
            element_leftbottom.SetActive(true);
            //set all the other elements inactive
            element_leftbottom.GetComponent<DialogElement>().setText(cur.msg);
            if(cur.tts_enable)
            {
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(cur.msg);
            }
        } else
        {
            // NOT IMPLEMENTED
        }
        

    }
    public static void Reset()
    {
        this_.element_leftbottom.SetActive(false);
        this_.mDialogueItems.Clear();
    }
    public static void add_dialog(DialogItem t)
    {
        bool need_update = false;
        if (this_.mDialogueItems.Count == 0) need_update = true;
        this_.mDialogueItems.Add(t);
        if (need_update) this_.load_first_dialog();
    }


}




public class DialogItem
{
    public DialogueType type;
    public string msg;
    public string callback_msg;
    public bool tts_enable;
    CallbackFunction mCallback; //called when terminating the dialog.

    public DialogItem(DialogueType t, string m, bool tts_,CallbackFunction func, string cb_msg)
    {
        type = t;
        msg = m;
        mCallback = func;
        callback_msg = cb_msg;
        tts_enable = tts_;

    }

    public void invoke_callback()
    {
        if(mCallback!=null) mCallback(callback_msg);
    }

    
}
