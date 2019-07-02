﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class number_cartoon : MonoBehaviour {


    public GameObject[] number_objects; //index -> number
    public GameObject[] number_objects2;

    public int num;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        
    }

    private void Reset()
    {
       
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }
    public void set_number(int i)
    {
        int k = i;
        num = i;

        
        if (k < 0) k = 0;
        k = k % 10;
        Reset();
        Vector3 targetPos = new Vector3(0, 0, 0);
        UnityEngine.GameObject label = Instantiate(number_objects2[k], targetPos, Quaternion.identity) as GameObject;
        label.transform.SetParent(this.gameObject.transform);
        RectTransform r = label.GetComponent<RectTransform>();
        r.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        r.position = targetPos;
        label.GetComponent<RectTransform>().localPosition = r.position;
        label.GetComponent<RectTransform>().localScale = r.localScale;

        int second_digit = i / 10;
        if (second_digit > 0 && second_digit<=9)
        {
            targetPos.x -= 70;
            label = Instantiate(number_objects2[second_digit], targetPos, Quaternion.identity) as GameObject;
            label.transform.SetParent(this.gameObject.transform);
            r = label.GetComponent<RectTransform>();
            r.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            r.position = targetPos;
            label.GetComponent<RectTransform>().localPosition = r.position;
            label.GetComponent<RectTransform>().localScale = r.localScale;
        }
        

    }
}
