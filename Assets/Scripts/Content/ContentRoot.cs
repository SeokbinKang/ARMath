﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentRoot : MonoBehaviour {
    public GameObject mContentCounting;
    public GameObject mContentAddition;
    public GameObject mContentSubtraction;
    public GameObject mContentMult;
    public GameObject mContentDiv;


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

        mContentCounting.SetActive(!mContentCounting.activeSelf);
    }
    public void enableContentAddition(bool t)
    {
        mContentAddition.SetActive(!mContentAddition.activeSelf);
    }
    public void enableContentSubtraction(bool t)
    {
        mContentSubtraction.SetActive(!mContentSubtraction.activeSelf);
    }
    public void enableContentMulti(bool t)
    {
        mContentMult.SetActive(!mContentMult.activeSelf);
    }
    public void enableContentDivision(bool t)
    {
        mContentDiv.SetActive(!mContentDiv.activeSelf);
    }
}
