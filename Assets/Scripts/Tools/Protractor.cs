using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Protractor : MonoBehaviour {

    public float time_to_wait = 3;
    public float angle;
    public float rot_factor;
    public bool xyflip;
    public GameObject needle;
    public GameObject call_out;
    public GameObject rect_wait;
    public angle_name target_angle;
    public Color text_incorrect;
    public Color text_correct;
    public bool read_name;
    private float margin = 10;
    private float demo_time = 0;
    private bool answer_found;
    private bool tested = false;


    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time - demo_time < 3f) demo();
        else
        {
            if (!answer_found)
            {
                handle_touch();
                angle_check();
            }
        }
    }
    private void OnEnable()
    {
        //randome move the needle...
        demo_time = Time.time;
        angle = UnityEngine.Random.Range(-179.5f, -0.5f);
        call_out.SetActive(false);
        rect_wait.SetActive(false);
        answer_found = false;
    }
    public bool IsFinished()
    {
        return answer_found;
    }
    private void demo()
    {
        angle = angle + ((Time.time - demo_time) - 1.5f) * UnityEngine.Random.Range(0.6f, 0.3f);
        if (angle > 0) angle -= 360;
        if (angle < -180) angle = -180;
        else if (angle > 0) angle = 0;
        Quaternion q = Quaternion.Euler(0, 0, angle);
        needle.GetComponent<RectTransform>().localRotation = q;
    }
    
    private float get_angle(Vector2 pos)
    {
        Vector2 basepoint = needle.GetComponent<RectTransform>().position;
        float angle_ = 0;
        
        {
            angle_ = Mathf.Atan2(pos.y - basepoint.y, pos.x - basepoint.x) * Mathf.Rad2Deg;
            angle = angle_ + rot_factor;
            if (angle > 0) angle -= 360;
            if (angle < -180) angle = -180;
            else if (angle > 0) angle = 0;
        }

        return angle_+rot_factor;
    }
    private void angle_check()
    {
        if (!rect_wait.activeSelf) return;
        bool rect_wait_finished = rect_wait.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish");
        if (rect_wait_finished && !call_out.activeSelf)
        {
            int angle_i = (int) Mathf.Abs(angle);
            if (angle_i < 85)
            {
                if (!read_name)
                {
                    call_out.GetComponent<Text>().text = angle_i + "°";
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("This is "+angle_i+" degree!");
                }
                else
                {
                    call_out.GetComponent<Text>().text = "\"acute\" angle";
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("This is an acute angle.");
                }
                call_out.GetComponent<Text>().color = text_incorrect;
                


            }
            else if (angle_i > 95)
            {
                if (!read_name)
                {
                    call_out.GetComponent<Text>().text = angle_i + "°";
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("This is " + angle_i + " degree!");
                }
                else
                {
                    call_out.GetComponent<Text>().text = "\"obtuse\" angle";
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("This is an obtuse angle!");
                }
                call_out.GetComponent<Text>().color = text_incorrect;
               

            }
            else
            {
                angle_i = 90;
                if (!read_name)
                {
                    call_out.GetComponent<Text>().text = angle_i + "°";
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("This is " + angle_i + " degree!");
                }
                else
                {
                    call_out.GetComponent<Text>().text = "\"right\" angle";
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("This is a right angle!");
                }
                call_out.GetComponent<Text>().color = text_correct;
                

            }

            call_out.SetActive(true);
            tested = false;
            Quaternion q = Quaternion.Euler(0, 0, 0);
            call_out.GetComponent<RectTransform>().rotation = q;

        } else if (rect_wait_finished && call_out.activeSelf && !tested)
        {
            if (call_out.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish"))
            {
                string correct_answer = Enum.GetName(typeof(angle_name),(int)target_angle);
                Debug.Log("[ARMath] correct_answer: " + correct_answer);    
                if(call_out.GetComponent<Text>().text.Contains(correct_answer) || call_out.GetComponent<Text>().text.Contains("90"))
                {
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Nice job");
                    answer_found = true;
                    //finished
                } else
                {
                    TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Let's try again");
                    //do nothing
                }
                tested = true;
            }
        }
    }
    private void handle_touch()
    {
        Vector2 pos2;
        
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 pos = Input.mousePosition;
            
            //calibration?
             get_angle(pos);
            Quaternion q = Quaternion.Euler(0, 0, angle);
            needle.GetComponent<RectTransform>().localRotation = q;
            rect_wait.SetActive(true);

        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
           
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                rect_wait.SetActive(false);
                call_out.SetActive(false);
                Vector2 pos = touch.position;

                 get_angle(pos);
                if (Mathf.Abs(angle - 90f) < 5) angle = 90 * Mathf.Sign(angle);
                Quaternion q = Quaternion.Euler(0, 0, angle);

                needle.GetComponent<RectTransform>().localRotation = q;
                


            }
            if (touch.phase == TouchPhase.Ended)
            {
                rect_wait.SetActive(true);
                
           
                
            }


        }
    }
}
