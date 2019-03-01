using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCounting : MonoBehaviour {

    enum AppCountingStatus { init, explore, question, count, summary };
    private AppCountingStatus userStatus;

    public GameObject UIInit;
    public GameObject UIExplore;
    public GameObject UIQuestion;
    public GameObject UICount;
    public GameObject UISummary;

    // Use this for initialization
    void Start () {
        userStatus = AppCountingStatus.init;

    }
	
	// Update is called once per frame
	void Update () {
        ControlUI();

    }
    public void SetUIExplore()
    {
        userStatus = AppCountingStatus.explore;
        Debug.Log("CLICKED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
    public void SetUIQuestion(string objectname)
    {
        userStatus = AppCountingStatus.question;
        UIQuestion.SetActive(true);
        UIQuestion.GetComponent<AppCountUIQuestion>().updatePromptText(objectname);
    }
    public void SetUICount(string objectname)
    {
        userStatus = AppCountingStatus.count;
        UICount.SetActive(true);
        UICount.GetComponent<AppCountingUICounting>().TargetObjectName = objectname;
    }
    public void objectFound(Rect box, string objname)
    {
        
        if (userStatus == AppCountingStatus.explore)
        {
            UIExplore.GetComponent<AppCountingUIExplore>().LocateGem(box, objname);
        }

        if (userStatus == AppCountingStatus.count)
        {
            UICount.GetComponent<AppCountingUICounting>().AddTargetObject(box, objname);
        }



    }
    void OnGUI()
    {
       
    }
    private void ControlUI()
    {

        if(userStatus== AppCountingStatus.init)
        {
            UIInit.SetActive(true);
            UIExplore.SetActive(false);
            UIQuestion.SetActive(false);
            UICount.SetActive(false);
            UISummary.SetActive(false);
        }
        if (userStatus == AppCountingStatus.explore)
        {
            UIInit.SetActive(false);
            UIExplore.SetActive(true);
            UIQuestion.SetActive(false);
            UICount.SetActive(false);
            UISummary.SetActive(false);
        }
        if (userStatus == AppCountingStatus.question)
        {
            UIInit.SetActive(false);
            UIExplore.SetActive(false);
            UIQuestion.SetActive(true);
            UICount.SetActive(false);
            UISummary.SetActive(false);
        }
        if (userStatus == AppCountingStatus.count)
        {
            UIInit.SetActive(false);
            UIExplore.SetActive(false);
            UIQuestion.SetActive(false);
            UICount.SetActive(true);
            UISummary.SetActive(false);
        }
        if (userStatus == AppCountingStatus.summary)
        {
            UIInit.SetActive(false);
            UIExplore.SetActive(false);
            UIQuestion.SetActive(false);
            UICount.SetActive(false);
            UISummary.SetActive(true);
        }
    }


    
}
