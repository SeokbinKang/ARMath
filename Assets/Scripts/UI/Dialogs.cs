using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs : MonoBehaviour {

    public static Dialogs this_;
    public GameObject element_leftbottom;
    
    public GameObject element_popright;
    
    public GameObject element_topboard_animated;
    public GameObject element_review;


    private List<DialogItem> mDialogueItems;
    private DialogItem cur_dialog;
    private float nextActionTime = 1.0f;
    private float nextUpdateTime = 0;

    
    // Use this for initialization
    void Start () {
        mDialogueItems = new List<DialogItem>();
        element_leftbottom.SetActive(false);
        element_topboard_animated.SetActive(false);
        element_popright.SetActive(false);
        element_review.SetActive(true);
        
        this_ = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            checkTimeoutDialog();

        }
    }
    private void checkTimeoutDialog()
    {
        if (mDialogueItems.Count > 0)
        {
            if (mDialogueItems[0].isTimeout())
            {
                OnNext();
            }
        }
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
   
    
    private void load_first_dialog()
    {
        if (mDialogueItems == null || mDialogueItems.Count == 0)
        {
            element_leftbottom.SetActive(false);
            element_popright.SetActive(false);

            //set all elements inactive
            return;
        }
        DialogItem cur = mDialogueItems[0];

        if (cur.type == DialogueType.left_bottom_plain)
        {
            element_leftbottom.GetComponent<DialogElement>().setText(cur.msg);
            element_leftbottom.GetComponent<DialogElement>().setTTS(cur.tts_enable);
            cur.StartTimeout();
            if(element_leftbottom.activeSelf) TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(cur.msg);
            element_leftbottom.SetActive(true);
            
            //set all the other elements inactive            
            if (cur.tts_enable)
            {
                //TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(cur.msg);
                //TTS is done in the text label automatically. ALWAYS
            } else
            {
                
            }
        }
        else if (cur.type == DialogueType.right_pop)
        {
            element_popright.SetActive(true);
            cur.StartTimeout();
            //set all the other elements inactive
            element_popright.GetComponent<DialogElement>().setText(cur.msg);
            if (cur.tts_enable)
            {
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(cur.msg);
            }
        }
        else if (cur.type == DialogueType.Dummy)
        {            
            cur.StartTimeout();
            //set all the other elements inactive           
         
        }



    }
    public static void Reset()
    {
        this_.element_leftbottom.SetActive(false);
        this_.element_popright.SetActive(false);
       
        this_.element_topboard_animated.SetActive(false);
        this_.mDialogueItems.Clear();
        
    }
    public static void review(string problem, int[] answers, int answer_index, CallbackFunction cb)
    {
        Debug.Log("[ARMath] start review...");
        this_.element_review.GetComponent<review1>().generate_problem(problem, answers, answer_index,cb);
      //  this_.element_review.SetActive(true);
       // this_.element_topboard.SetActive(false);
    }
    public static void review_shape(string problem,CallbackFunction cb)
    {
        Debug.Log("[ARMath] start review...");
        this_.element_review.GetComponent<review1>().generate_shape_problem(problem, cb);
        //this_.element_review.SetActive(true);
        // this_.element_topboard.SetActive(false);
    }
    public static void add_dialog(DialogItem t)
    {
        bool need_update = false;
        if (this_.mDialogueItems.Count == 0) need_update = true;
        this_.mDialogueItems.Add(t);
        if (need_update) this_.load_first_dialog();
    }
    public static void add_dialog(DialogItem t, float delay)
    {
        Dialogs.add_dialog(new DialogItem(DialogueType.Dummy,
               "",
               true,
               null,
               "",
               delay
               ));

        add_dialog(t);
    }
    public static void set_topboard(bool enabled, string txt)
    {
        /*this_.element_topboard.GetComponent<DialogElement>().setText(txt);
        this_.element_topboard.SetActive(enabled);*/
        
    }
    public static void set_topboard_color(int term_idx, int color_index)
    {
        this_.element_topboard_animated.GetComponent<DialogProblemAnimated>().set_term_color(term_idx, color_index);
        


    }
    
    public static void set_topboard_animated(bool enabled, int idx,string txt)
    {
        this_.element_topboard_animated.GetComponent<DialogProblemAnimated>().set_term(idx, txt);
        this_.element_topboard_animated.SetActive(enabled);

        
    }
    public static void set_topboard_update_highlight(int idx, string txt )
    {
        this_.element_topboard_animated.GetComponent<DialogProblemAnimated>().update_and_highlight(idx, txt);
        this_.element_topboard_animated.SetActive(true);
    }
    public static void set_topboard_highlight(bool enabled, int idx, float delay)
    {
        this_.element_topboard_animated.GetComponent<DialogProblemAnimated>().highlight_term(idx, delay);
        this_.element_topboard_animated.SetActive(enabled);

        
    }

}




public class DialogItem
{
    public DialogueType type;
    public string msg;
    public string callback_msg;
    public bool tts_enable;
    CallbackFunction mCallback; //called when terminating the dialog.
    private float start_time;
    private float time_out;
    public DialogItem(DialogueType t, string m, bool tts_,CallbackFunction func, string cb_msg)
    {
        type = t;
        msg = m;
        mCallback = func;
        callback_msg = cb_msg;
        tts_enable = tts_;
        time_out = SystemParam.timeout_for_prompt_disappear;
        this.start_time = Time.time;
    }
    public DialogItem(DialogueType t, string m, bool tts_, CallbackFunction func, string cb_msg, float timeout)
    {
        type = t;
        msg = m;
        mCallback = func;
        callback_msg = cb_msg;
        tts_enable = tts_;
        this.start_time = Time.time;
        this.time_out = timeout;
    }
    public void StartTimeout()
    {
        this.start_time = Time.time;
    }
    public bool isTimeout()
    {
        if (Time.time > start_time + time_out) return true;
        else return false;
    }
    public void invoke_callback()
    {
        if(mCallback!=null) mCallback(callback_msg);
    }

    
}
