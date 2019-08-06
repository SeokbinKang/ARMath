using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

    public GameObject text;
    public GameObject feedback_o;
    public GameObject feedback_x;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        feedback_o.SetActive(false);
        feedback_x.SetActive(false);
        
    }
    public void set_answer(string txt)
    {
        if (text == null) return;
        text.GetComponent<Text>().text = txt;
    }
    public void active_feedback(bool t)
    {
        if (feedback_o == null || feedback_x == null) return;
        if (t) feedback_o.SetActive(true);
            else feedback_x.SetActive(true);
    }
}

