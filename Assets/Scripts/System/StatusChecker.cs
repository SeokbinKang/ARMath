using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusChecker : MonoBehaviour {
    //check if user is unselected and prompt
    //check if question is unselected and prompt

    public GameObject promptUser;
    public GameObject promptQuestion;
    public GameObject[] ContentModules;
    private float nextActionTime = 0.0f;
    public float period = 3.0f;
    

    // Use this for initialization
    void Start () {
        //disable popups/
        Reset();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            // execute block of code here
            if(promptUser.activeSelf!=true && promptQuestion.activeSelf!=true) 
                CheckStatus();
        }
    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        promptUser.SetActive(false);
        promptQuestion.SetActive(false);
    }

    private void CheckStatus()
    {
        bool question_selected = false;
        if(SystemUser.current_user==null)
        {
            promptUser.SetActive(true);
            return;
        }

        foreach(GameObject t in ContentModules)
        {
            if(t!=null && t.activeSelf)
            {
                question_selected = true;
                break;
            }
        }
        if (!question_selected) promptQuestion.SetActive(true);
    }
    


}
