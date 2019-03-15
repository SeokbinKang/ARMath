using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentRoot : MonoBehaviour {
    public GameObject mContentCounting;


	// Use this for initialization
	void Start () {
        mContentCounting.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void updateContentUse(ProblemType p, bool onoff)
    {
        if (p == ProblemType.p1_counting) mContentCounting.SetActive(onoff);

    }
    public void closeAllContent()
    {
        mContentCounting.SetActive(false);
    }
    public void updateScenedata(CVResult cv)
    {
        //receive the recognition result

        //pass the recognition result to individual content unit. if it is turned on.
        if (mContentCounting.activeSelf) mContentCounting.GetComponent<ContentCounting>().UpdateCVResult(cv);
    }

    public void enableContentCounting(bool t)
    {
        mContentCounting.SetActive(t);
    }
    public void enableContentAddition(bool t)
    {
        mContentCounting.SetActive(t);
    }
    public void enableContentSubtraction(bool t)
    {
        mContentCounting.SetActive(t);
    }
    public void enableContentMulti(bool t)
    {
        mContentCounting.SetActive(t);
    }
    public void enableContentDivision(bool t)
    {
        mContentCounting.SetActive(t);
    }
}
