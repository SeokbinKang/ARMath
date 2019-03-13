using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class board : MonoBehaviour {
    public GameObject IconContainer;
    public GameObject math_text;

	// Use this for initialization
	void Start () {
        IconContainer.SetActive(true);
        math_text.SetActive(true);
        setMathText("");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMathText(string t)
    {
        math_text.GetComponent<Text>().text = t;
    }
}
