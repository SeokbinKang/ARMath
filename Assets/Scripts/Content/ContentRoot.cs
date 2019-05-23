using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentRoot : MonoBehaviour {
    public GameObject mContentCounting;
    public GameObject mContentAddition;
    public GameObject mContentSubtraction;
    public GameObject mContentMult;
    public GameObject mContentDiv;
    public GameObject mGeometry;


    // Use this for initialization
    void Start () {
        closeAllContent();


    }

    // Update is called once per frame
    void Update () {
		
	}
    
    public void updateContentUse(ProblemType p, bool onoff)
    {
        if (p == ProblemType.p1_counting) mContentCounting.SetActive(onoff);
        if (p == ProblemType.p2_addition) mContentAddition.SetActive(onoff);
        if (p == ProblemType.p4_geometry) mGeometry.SetActive(onoff);

    }
    public void closeAllContent()
    {
        
        mContentCounting.SetActive(false);
        mContentAddition.SetActive(false);
        mContentSubtraction.SetActive(false);
        mContentMult.SetActive(false);
        mContentDiv.SetActive(false);
        mGeometry.SetActive(false);
    }
    
    public void enableContentCounting(bool t)
    {
        //mContentCounting.SetActive(!mContentCounting.activeSelf);
        mContentCounting.SetActive(t);
        if (t)
        {
            mContentAddition.SetActive(false);
            mGeometry.SetActive(false);
            mContentCounting.GetComponent<ContentCounting>().Reset();
        }
    }
    public void enableContentAddition(bool t)
    {
        //mContentAddition.SetActive(!mContentAddition.activeSelf);
        mContentAddition.SetActive(t);
        if (t)
        {
            mContentCounting.SetActive(false);
            mGeometry.SetActive(false);
            mContentAddition.GetComponent<ContentAddition>().Reset();
        }
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
    public void enableContentGeometry(bool t)
    {
        //mContentAddition.SetActive(!mContentAddition.activeSelf);
        mGeometry.SetActive(t);
        if (t)
        {
            mContentAddition.SetActive(false);
            mContentCounting.SetActive(false);
            mGeometry.GetComponent<ContentGeometry>().Reset();
        }
    }

    public int number_active_content()
    {
        int ret = 0;
        if(mContentCounting!=null)
        {
            if (mContentCounting.activeSelf) ret++;
        }
        if (mContentAddition != null)
        {
            if (mContentAddition.activeSelf) ret++;
        }
        if (mContentSubtraction != null)
        {
            if (mContentSubtraction.activeSelf) ret++;
        }
        if (mContentMult != null)
        {
            if (mContentMult.activeSelf) ret++;
        }
        if (mContentDiv != null)
        {
            if (mContentDiv.activeSelf) ret++;
        }
        if (mGeometry != null)
        {
            if (mGeometry.activeSelf) ret++;
        }
        return ret;
    }
}
