using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Protractor : MonoBehaviour {

    public float angle;
    public float rot_factor;
    public bool xyflip;
    public GameObject needle;
    public GameObject call_out;
    public Color text_incorrect;
    public Color text_correct;
    private float margin = 10;
    private float demo_time = 0;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        handle_touch();
        if (Time.time - demo_time < 3f) demo();
    }
    private void OnEnable()
    {
        //randome move the needle...
        demo_time = Time.time;
        angle = Random.Range(-179.5f, -0.5f);
        call_out.SetActive(false);

    }
    private void demo()
    {
        angle = angle + ((Time.time - demo_time) - 1.5f) * Random.Range(0.6f, 0.3f);
        if (angle > 0) angle -= 360;
        if (angle < -180) angle = -180;
        else if (angle > 0) angle = 0;
        Quaternion q = Quaternion.Euler(0, 0, angle);
        needle.GetComponent<RectTransform>().localRotation = q;
    }
    private void call_angle()
    {
        //call the name of the angle
        if(Mathf.Abs(angle)<85)
        {
            call_out.GetComponent<Text>().text = "\"acute\" angle";
            call_out.GetComponent<Text>().color = text_incorrect;
            
            
        } else if (Mathf.Abs(angle) > 95)
        {
            call_out.GetComponent<Text>().text = "\"obtuse\" angle";
            call_out.GetComponent<Text>().color = text_incorrect;
            

        } else
        {
            call_out.GetComponent<Text>().text = "\"right\" angle";
            call_out.GetComponent<Text>().color = text_correct;
            
        }
        call_out.SetActive(true);
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
    private void handle_touch()
    {
        Vector2 pos2;
        
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 pos = Input.mousePosition;


            // Position the cube.
            //Debug.Log("[ARMath] drawing at " + pos);

            
            //calibration?
             get_angle(pos);
            Quaternion q = Quaternion.Euler(0, 0, angle);
            needle.GetComponent<RectTransform>().localRotation = q;
            call_angle();
           
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
           
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {            
            
                Vector2 pos = touch.position;


                // Position the cube.
                //Debug.Log("[ARMath] drawing at " + pos);

                 get_angle(pos);
                Quaternion q = Quaternion.Euler(0, 0, angle);
                needle.GetComponent<RectTransform>().localRotation = q;
                


            }
            if (touch.phase == TouchPhase.Ended)
            {
                call_angle();
           
                
            }


        }
    }
}
