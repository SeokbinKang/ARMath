using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        //how to disable?
    }
    public void addgem(string dummy)
    {
        SystemUser.AddGem(problem_type);
        SystemControl.Reset();
    }

}
