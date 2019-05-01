using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionReview : MonoBehaviour {

    
    public GameObject ContentModuleRoot;

    public GameObject prompt;
    public GameObject num1;
    public GameObject num2;
    public GameObject answer;
    // Use this for initialization
    void Start()
    {
        //updateQuizzes();

    }
    void OnEnable()
    {
       // updateQuiz();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Reset()
    {

    }
    public void updateQuiz()
    {
        string target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        int target_object_count = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;

        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 100);
        int number_range = 1;
        int val1 = random.Next(1, 10);
        int val2 = random.Next(1, 10);
        int val3 = val1 + val2;

        prompt.GetComponent<Text>().text = "I collected "+val1+" "+ target_object_name+"s yesterday and "+val2+" "+ target_object_name+"s this morning. How many "+ target_object_name+"s did I collected?";

        //num1.GetComponent<board>().setIcon(target_object_name, val1);
        num1.GetComponent<board>().enable_number_only(target_object_name, val1);
        num2.GetComponent<board>().enable_number_only(target_object_name, val2);
        answer.GetComponent<InputNumber>().setAnswer(val3);
        answer.GetComponent<InputNumber>().setProblemType(ProblemType.p2_addition);

        //Debug.Log("[ARMath] quize[2]'s text:" + quizzes[2].GetComponent<board>().math_text.GetComponent<Text>().text);
        //Debug.Log(quizzes[2].GetComponent<board>().math_text);
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(prompt.GetComponent<Text>().text);

    }


}
