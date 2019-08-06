using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finder : MonoBehaviour
{

    public GameObject finder_obj;
    public GameObject view;
    public GameObject text1_instruction;
    public GameObject text2_confirmation;
    public GameObject text3_correction;
    private CallbackFunction call_back;
    private CallbackFunction2 call_back2;
    public string obj_name;
    public string obj_name_plural;
    public int min_number_of_objects;
    private string param;
    private float nextActionTime = 0.0f;

    List<SceneObject> objs;
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
            nextActionTime = Time.time + SystemParam.system_object_checking_period;
            // execute block of code here
            //   Debug.Log("[ARMath] update finder ");
            if (call_back != null || call_back2 != null)
            {
               check_object(false);
            }

        }
        if(text3_correction.activeSelf) handle_touch();

    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        text1_instruction.SetActive(true);
        text2_confirmation.SetActive(false);
        text3_correction.SetActive(false);
        objs = null;
    }
    public void check_object(bool confirm)
    {
        
         objs= ARMathUtils.get_objects_in_rect(view, obj_name);
        //Debug.Log("[ARMath] checking objs in the finder "+objs.Count);


        if (objs.Count >= min_number_of_objects)
        {
            float target_delay = 0;
            foreach (SceneObject so in objs)
            {
                
                if (so.is_feedback_attached()) continue;
                
                Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                GameObject f = FeedbackGenerator.create_target(targetPos, target_delay, 200, 4);
                so.attach_object(f);
                target_delay += 0.1f;
            }

            if (text1_instruction.activeSelf || confirm)
            {
               
                confirm_objects(objs.Count);
            }

        }

    }
    private void confirm_objects(int found_n)
    {
        foreach (SceneObject so in objs)
        {
            so.extend_life(30f);
        }
        text2_confirmation.GetComponentInChildren<Text>().text = "I see " + AssetManager.Get_object_text(obj_name, found_n) + ", am I right?";
        text1_instruction.SetActive(false);
        text2_confirmation.SetActive(true);
        text3_correction.SetActive(false);
    }
    public void confirm_failed()
    {
        text1_instruction.SetActive(false);
        text2_confirmation.SetActive(false);
        text3_correction.GetComponentInChildren<Text>().text = "Can you tap uncircled "+obj_name_plural+" on the screen?";
        text3_correction.SetActive(true);
    }
    public void complete_finder()
    {
        //List<SceneObject> objs = ARMathUtils.get_objects_in_rect(view, obj_name);
        //Debug.Log("[ARMath] checking objs in the finder "+objs.Count);

        foreach (SceneObject so in objs)
        {
            so.extend_life(30f);
        }
        if (objs.Count >= min_number_of_objects)
        {
            float target_delay = 0;
            FeedbackGenerator.clear_all_feedback();
            if (call_back != null)
            {
                call_back(objs.Count.ToString());
                call_back = null;
            }
            if (call_back2 != null)
            {
                
                Rect view_rect = new Rect(view.GetComponent<RectTransform>().position, view.GetComponent<RectTransform>().sizeDelta);

                call_back2(objs.Count.ToString(), objs, view_rect);
                call_back2 = null;
            }
            this.gameObject.SetActive(false);
        }  else
        {
            //need more objects
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Oops, I actually need more "+ obj_name_plural);

        }

    }
    public void set_finder(string obj, int min_count, CallbackFunction cb, string p)
    {
        call_back = cb;
        call_back2 = null;
        obj_name = obj;
        min_number_of_objects = min_count;
        param = p;
        obj_name_plural = AssetManager.Get_object_text(obj_name, 2);
        obj_name_plural = obj_name_plural.Substring(obj_name_plural.IndexOf(' ')+1);
        text1_instruction.GetComponent<Text>().text = "Let's find some "+ obj_name_plural;
        nextActionTime = Time.time + 8; // animation delay
        finder_obj.GetComponent<RectTransform>().localScale = Vector3.one;
       
    }
    public void set_finder(string obj, int min_count, CallbackFunction2 cb, string p, string prompt, float scale)
    {
        call_back = null;
        call_back2 = cb;
        obj_name = obj;
        min_number_of_objects = min_count;
        param = p;
        obj_name_plural = AssetManager.Get_object_text(obj_name, 2);
        obj_name_plural = obj_name_plural.Substring(obj_name_plural.IndexOf(' ') + 1);        
        text1_instruction.GetComponent<Text>().text = prompt;
        nextActionTime = Time.time + 8; // animation delay
        finder_obj.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
     
    }
    
    private void handle_touch()
    {

        if (Input.touchCount > 0 && text3_correction.activeSelf)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                Vector2 screenpos = touch.position;
                bool hit = ARMathUtils.check_in_recttransform(pos, view);
                if (!hit)
                {
                    return;
                }
                if (!SceneObjectManager.kill_sceneObject_close_to(screenpos, 100f)) {
                    pos.y = Screen.height - pos.y;
                    CatalogItem ci = new CatalogItem();
                    ci.Box = new Rect(pos, new Vector2(80, 80));
                    ci.DisplayName = obj_name;
                    SceneObjectManager.add_new_object(ci, 180);
                    nextActionTime = Time.time + SystemParam.system_object_checking_period;
                } else
                {
                    SceneObjectManager.retire_old_objects();
                }
                
                check_object(false);
                //List<SceneObject> objs = ARMathUtils.get_objects_in_rect(view, obj_name);
                ////Debug.Log("[ARMath] checking objs in the finder "+objs.Count);


                //if (objs.Count >= min_number_of_objects)
                //{
                //    float target_delay = 0;
                //    foreach (SceneObject so in objs)
                //    {
                //        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                //        FeedbackGenerator.create_target(targetPos, target_delay, 2, 4);
                //        target_delay += 0.2f;
                //    }
                    

                //}
            }

        }
    }
}
