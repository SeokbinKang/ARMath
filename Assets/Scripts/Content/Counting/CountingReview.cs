using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingReview : MonoBehaviour {
   // public GameObject[] quizzes;
    public GameObject ContentModuleRoot;
    public GameObject prompt;
    public GameObject icons;

    public GameObject answer;
    // Use this for initialization
    void Start () {
        //updateQuizzes();

    }
	void OnEnable()
    {
       // updateQuizzes();
    }
	// Update is called once per frame
	void Update () {
		
	}
    private void Reset()
    {
        
    }
    public void updateQuiz()
    {
        string target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
        int target_object_count = ContentModuleRoot.GetComponent<ContentCounting>().found_object_count;

        System.Random random = new System.Random();
        int randomNumber = random.Next(3, 15);
     
        prompt.GetComponent<Text>().text = "Here are some "+ target_object_name+"s I found. Can you count them and write the number in the white box?";

        
        icons.GetComponent<board>().enable_visual_only(target_object_name, randomNumber);
        answer.GetComponent<InputNumber>().setAnswer(randomNumber);
        answer.GetComponent<InputNumber>().setProblemType(ProblemType.p1_counting);

        //Debug.Log("[ARMath] quize[2]'s text:" + quizzes[2].GetComponent<board>().math_text.GetComponent<Text>().text);
        //Debug.Log(quizzes[2].GetComponent<board>().math_text);
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Let's solve quizzes for bonus. "+prompt.GetComponent<Text>().text);
    }
    /*
    public void updateQuizzes()
    {
        string target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
        int target_object_count = ContentModuleRoot.GetComponent<ContentCounting>().found_object_count;

        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 100);
        int number_range = 1;

        updateQuiz(quizzes[0], target_object_name,random.Next(1, 4));
        updateQuiz(quizzes[1], target_object_name,random.Next(2, 8));
        updateQuiz(quizzes[2], target_object_name,random.Next(5, 9));
        updateQuiz(quizzes[3], target_object_name,random.Next(12, 18));

        //Debug.Log("[ARMath] quize[2]'s text:" + quizzes[2].GetComponent<board>().math_text.GetComponent<Text>().text);
        //Debug.Log(quizzes[2].GetComponent<board>().math_text);
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Let's solve quizzes for bonus!");

    }
    public void updateQuiz(GameObject go, string obj_name, int value)
    {
        if (go == null) return;
        //Debug.Log("[ARMath] Updating quize:" + obj_name + "  " + value);
        go.GetComponent<board>().setIcon(obj_name, value);
        go.GetComponent<board>().setMathText("= "+value.ToString());
        go.GetComponent<board>().setAnswer(value);
        go.GetComponent<board>().setProblemType(ProblemType.p1_counting);

    }*/

}
