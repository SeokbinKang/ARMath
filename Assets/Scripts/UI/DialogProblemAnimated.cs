﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogProblemAnimated : MonoBehaviour
{

    public GameObject term1_num;
    public GameObject term2_num;
    public GameObject term3_equal;
    List<TimerCallback> cbs;


    // Use this for initialization
    void Start()
    {
        cbs = new List<TimerCallback>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<cbs.Count;i++)        
        {
            if (cbs[i].tick()) cbs.RemoveAt(i);
        }
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        term1_num.SetActive(false);
        term2_num.SetActive(false);
        term3_equal.SetActive(false);

    }
    public void set_term(int idx, string txt)
    {
        if (idx == 0)
        {
            term1_num.GetComponentInChildren<Text>().text = txt;
            term1_num.SetActive(true);
            return;
        }
        if (idx == 1)
        {
            term2_num.GetComponentInChildren<Text>().text = txt;
            term2_num.SetActive(true);
            return;
        }
        if (idx == 2)
        {
            term3_equal.GetComponentInChildren<Text>().text = txt;
            term3_equal.SetActive(true);
            return;
        }

    }
    public void highlight_term(int idx)
    {
        if (idx == 0)
            term1_num.GetComponentInChildren<Animator>().SetTrigger("highlight");
        else if (idx == 1)
            term2_num.GetComponentInChildren<Animator>().SetTrigger("highlight");
        else if (idx == 2)
            term3_equal.GetComponentInChildren<Animator>().SetTrigger("highlight");
    }
    public void highlight_term(string idx)
    {
        Debug.Log("[ARMath] highlight top board term " + idx);
        if (idx == "0")
            term1_num.GetComponentInChildren<Animator>().SetTrigger("highlight");
        else if (idx == "1")
            term2_num.GetComponentInChildren<Animator>().SetTrigger("highlight");
        else if (idx == "2")
            term3_equal.GetComponentInChildren<Animator>().SetTrigger("highlight");
    }
    public void highlight_term(int idx, float delay)
    {
        cbs.Add(new TimerCallback(highlight_term, idx.ToString(), delay));
    }
}
