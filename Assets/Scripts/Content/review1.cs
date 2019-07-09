using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class review1 : MonoBehaviour {

    public GameObject problem_text;
    public GameObject[] selections;

    CallbackFunction mCallback;
    private int answer_index;
    float close_at;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        check_timer();
    }
    private void OnEnable()
    {
        close_at = float.MaxValue;
    }
    private void check_timer()
    {
        if (Time.time > close_at)
        {
            this.gameObject.SetActive(false);
            if(mCallback!=null) mCallback("good");

        }
    }
    public void check_answer(int index)
    {
        if(index==answer_index)
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Nice job!");
            close_at = Time.time + 1.5f;
            
        } else
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Try again!");
        }
    }
    public void generate_problem(string problem_statement, int[] answers, int answer_idx, CallbackFunction cb)
    {
        problem_text.GetComponent<Text>().text = problem_statement;
        for(int i = 0; i < answers.Length;i++)
        {
            if (i < selections.Length) selections[i].GetComponent<Text>().text =  answers[i].ToString();
        }
        answer_index = answer_idx;
        mCallback = cb;
        close_at = float.MaxValue;
    }
}
