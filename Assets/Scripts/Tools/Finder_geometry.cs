using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finder_geometry : MonoBehaviour {

    
    public GameObject view;
    public GameObject prompt_txt;
    public GameObject confirm_txt;
    public GameObject prompt2_txt;
    public GameObject confirm2_txt;
    private CallbackFunction call_back;    
    private List<string> obj_names;
    private string found_obj_name;
    
    private float nextActionTime = 0.0f;
    
    // Use this for initialization
    void Start()
    {
        //call_back = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            //   Debug.Log("[ARMath] update finder ");
            if (call_back != null)
            {
                if (prompt_txt.activeSelf) check_object();
            }

        }
        if (prompt2_txt.activeSelf) check_picture();
        //if (confirm_txt.activeSelf) handle_touch();

    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        prompt_txt.SetActive(true);
        prompt2_txt.SetActive(false);
        confirm_txt.SetActive(false);
        confirm2_txt.SetActive(false);
        //obj_names = null;

    }
    private void check_object()
    {
        if (obj_names == null) Debug.Log("[ARMath] obj_names is null?");
        foreach (string obj_name in obj_names)
        {
            List<SceneObject> objs = ARMathUtils.get_objects_in_rect(view, obj_name);
            if (objs.Count == 0) continue;
            //Debug.Log("[ARMath] checking objs in the finder "+objs.Count);

            float target_delay = 0;
            foreach (SceneObject so in objs)
            {
                Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                FeedbackGenerator.create_target(targetPos, target_delay, 2, 4);
                target_delay += 0.2f;
            }

            confirm_objects(obj_name);
            return;
        }
    }


    
    public void confirm_objects(string obj_name)
    {
        found_obj_name = obj_name;
        confirm_txt.GetComponent<Text>().text = "Does the "+obj_name+" look like a rectangle?";
        prompt_txt.SetActive(false);
        confirm_txt.SetActive(true);        
        prompt2_txt.SetActive(false);
        confirm2_txt.SetActive(false);

    }
    public void confirm_no()
    {
        prompt_txt.SetActive(true);
        prompt2_txt.SetActive(false);
        confirm_txt.SetActive(false);
        confirm2_txt.SetActive(false);
    }
    public void confirm_yes()
    {
        prompt_txt.SetActive(false);        
        confirm_txt.SetActive(false);
        //        prompt2_txt.GetComponent<Text>().text = "Alright, I am going ";
        prompt2_txt.SetActive(true);
        confirm2_txt.SetActive(false);

    }
    public void check_picture()
    {
        if (!prompt2_txt.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish")) return;
        CameraImage.pause_image();
        prompt_txt.SetActive(false);
        prompt2_txt.SetActive(false);
        confirm_txt.SetActive(false);
        confirm2_txt.SetActive(true);
    }

    public void confirm_no2()
    {
        call_back(found_obj_name);
        call_back = null;
        this.gameObject.SetActive(false);
    }
    public void confirm_yes2()
    {
        confirm_yes();
        CameraImage.resume_image();
        

    }
    
    public void set_finder(List<string> obj, int min_count, CallbackFunction cb, string p)
    {
        call_back = cb;
        
        obj_names = obj;
       
        nextActionTime = Time.time + 8; // animation delay
        this.GetComponent<RectTransform>().localScale = Vector3.one;
       
    }
 
}
