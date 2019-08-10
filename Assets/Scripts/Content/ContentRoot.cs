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
    private void OnEnable()
    {
        closeAllContent();
        resetresources();

    }
    public void resetresources()
    {
        FeedbackGenerator.clear_all_feedback();
        SceneObjectManager.mSOManager.Reset();
        Drawing2D.Reset();
        Dialogs.Reset();
        SystemContent.EnableLeftUserUI(false);
        CameraImage.resume_image();
    }
    public void updateContentUse(ProblemType p, bool onoff)
    {
        if (p == ProblemType.p1_counting) mContentCounting.SetActive(onoff);
        if (p == ProblemType.p2_addition) mContentAddition.SetActive(onoff);
        if (p == ProblemType.p3_multiplication) mContentMult.SetActive(onoff);
        if (p == ProblemType.p4_geometry) mGeometry.SetActive(onoff);
        if (p == ProblemType.p3_division) mContentDiv.SetActive(onoff);

    }
    public void closeAllContent()
    {
       
        if (mContentCounting) mContentCounting.SetActive(false);
        if (mContentAddition) mContentAddition.SetActive(false);
        if (mContentSubtraction) mContentSubtraction.SetActive(false);
        if (mContentMult) mContentMult.SetActive(false);
        if (mContentDiv) mContentDiv.SetActive(false);
        if (mGeometry) mGeometry.SetActive(false);
    }
    
    public void enableContentCounting(bool t)
    {
        //mContentCounting.SetActive(!mContentCounting.activeSelf);
        resetresources();
        mContentCounting.SetActive(t);
        SystemContent.EnableLeftUserUI(false);
        if (t)
        {
            mContentAddition.SetActive(false);
            mGeometry.SetActive(false);
            mContentMult.SetActive(false);
            mContentDiv.SetActive(false);
           
            //mContentCounting.GetComponent<ContentCounting>().Reset();
        }
    }
    public void enableContentAddition(bool t)
    {
        //mContentAddition.SetActive(!mContentAddition.activeSelf);
        resetresources();
        mContentAddition.SetActive(t);
       
        if (t)
        {
            mContentCounting.SetActive(false);
            mGeometry.SetActive(false);
            mContentMult.SetActive(false);
            mContentDiv.SetActive(false);
            
            mContentAddition.GetComponent<ContentAddition>().Reset();

        }
    }
    public void enableContentSubtraction(bool t)
    {
        mContentSubtraction.SetActive(!mContentSubtraction.activeSelf);
    }
    public void enableContentMulti(bool t)
    {
        resetresources();
        mContentMult.SetActive(t);       

        if (t)
        {
            mContentCounting.SetActive(false);
            mGeometry.SetActive(false);
            mContentAddition.SetActive(false);
            mContentDiv.SetActive(false);            
            mContentMult.GetComponent<ContentMulti>().Reset();
            
        }
        
    }
    public void enableContentDivision(bool t)
    {
        resetresources();
        mContentDiv.SetActive(t);
     

        if (t)
        {
            mContentCounting.SetActive(false);
            mGeometry.SetActive(false);
            mContentAddition.SetActive(false);
            mContentMult.SetActive(false);            
            mContentDiv.GetComponent<ContentDiv>().Reset();

        }

    }
    public void enableContentGeometry(bool t)
    {
        //mContentAddition.SetActive(!mContentAddition.activeSelf);
        resetresources();
        mGeometry.SetActive(t);
     
        if (t)
        {
            mContentMult.SetActive(false);
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
