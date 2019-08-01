using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class review1 : MonoBehaviour {

    public GameObject dialog_bg;
    public GameObject problem_text;
    public GameObject[] selections;
    public GameObject[] selections_shapes;
    public GameObject answers_number;
    public GameObject answers_shape;
    public GameObject[] active_selections;
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
        answers_number.SetActive(false);
        answers_shape.SetActive(false);
        dialog_bg.SetActive(false);
    }
    private void check_timer()
    {
        if (Time.time > close_at)
        {
            answers_number.SetActive(false);
            answers_shape.SetActive(false);
            dialog_bg.SetActive(false);
            
            if (mCallback!=null) mCallback("good");

        }
    }
    public void check_answer(int index)
    {
        if(index==answer_index)
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Nice job!");
            close_at = Time.time + 1.5f;
            active_selections[index].GetComponent<Animator>().SetTrigger("o");
            if(selections==active_selections)
            {
                Dialogs.set_topboard_update_highlight(2, "= "+selections[index].GetComponentInChildren<Text>().text);
            }
            
        } else
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Try again!");
            active_selections[index].GetComponent<Animator>().SetTrigger("x");

        }
    }
    public void generate_problem(string problem_statement, int[] answers, int answer_idx, CallbackFunction cb)
    {
        dialog_bg.SetActive(true);
        answers_number.SetActive(true);
        
        problem_text.GetComponent<Text>().text = problem_statement;
        for(int i = 0; i < answers.Length;i++)
        {
            if (i < selections.Length) selections[i].GetComponentInChildren<Text>().text =  answers[i].ToString();
        }
        answer_index = answer_idx;
        mCallback = cb;
        close_at = float.MaxValue;
        active_selections = selections;
    }

    public void generate_shape_problem(string problem_statement, CallbackFunction cb)
    {
        dialog_bg.SetActive(true);
        answers_shape.SetActive(true);
        problem_text.GetComponent<Text>().text = problem_statement;
        active_selections = selections_shapes;
        answer_index = 1;
        mCallback = cb;
        close_at = float.MaxValue;
    }
}
