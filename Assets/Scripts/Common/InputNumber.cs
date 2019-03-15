using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour {

    public GameObject input_text;
    public GameObject button_obj;
    public Texture button_before;
    public Texture button_after;
    public GameObject feedback_yes;
    public GameObject feedback_no;

    public ProblemType problem_type;
    public int answer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        this.GetComponent<InputField>().text = "";
        button_obj.GetComponent<RawImage>().texture = this.button_before;
        input_text.GetComponent<Text>().text = "";
    }

    public void showresult()
    {
        bool result = checkAnswer();

        if (result)
        {
            //show 'O'

            feedback_yes.GetComponent<Animator>().SetTrigger("fadein");
            //enable gem button
            button_obj.GetComponent<RawImage>().texture = this.button_after;

            return;
        }
        else
        {
            //show X
            feedback_no.GetComponent<Animator>().SetTrigger("fadein");
            button_obj.GetComponent<RawImage>().texture = this.button_before;
            return;
        }
    }
    public void setAnswer(int a)
    {
        answer = a;        
        Reset();
    }
    public void setProblemType(ProblemType p)
    {
        this.problem_type = p;
    }
    public bool checkAnswer()
    {
        
        int user_answer = int.Parse(input_text.GetComponent<Text>().text);

        if (user_answer == answer)
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("good job!");
            SystemUser.AddGem(this.problem_type);
            return true;
        }
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("let's try it again");
        return false;
    }

   
}
