using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class reward : MonoBehaviour {
    public ProblemType problem_type;
    private TimerCallback mCallback;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mCallback != null && mCallback.tick()) mCallback = null;
    }
    public void onClick()
    {
        mCallback = new TimerCallback(addgem, "", 2);

        if (this.GetComponent<Animator>()!=null) this.GetComponent<Animator>().SetTrigger("credit");
        //playmusic
        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
        string mode = "tangible";
        if (interaction_touch_enalbed) mode = "virtual";
        string content = Enum.GetName(typeof(ProblemType), problem_type);
        string tt = "[End Content] " +content+" "+mode;
        SystemLog.WriteLog(tt);

        //how to disable?
    }
    public void addgem(string dummy)
    {
        SystemUser.AddGem(problem_type);
        SystemControl.Reset();
    }

}
