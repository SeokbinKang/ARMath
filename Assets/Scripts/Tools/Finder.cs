using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finder : MonoBehaviour
{

    public GameObject view;
    public GameObject txt;
    public GameObject confirm_txt;
    private CallbackFunction call_back;
    private CallbackFunction2 call_back2;
    public string obj_name;
    public int min_number_of_objects;
    private string param;
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
            if (call_back != null || call_back2 != null)
            {
                if (txt.activeSelf) check_object();
            }

        }
        if(confirm_txt.activeSelf) handle_touch();

    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        txt.SetActive(true);
        confirm_txt.SetActive(false);
        
    }
    private void check_object()
    {
        
        List<SceneObject> objs = ARMathUtils.get_objects_in_rect(view, obj_name);
        //Debug.Log("[ARMath] checking objs in the finder "+objs.Count);


        if (objs.Count >= min_number_of_objects)
        {
            float target_delay = 0;
            foreach (SceneObject so in objs)
            {
                Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                FeedbackGenerator.create_target(targetPos, target_delay, 2, 4);
                target_delay += 0.2f;
            }

            confirm_objects(objs.Count);

        }

    }
    private void confirm_objects(int found_n)
    {
        confirm_txt.GetComponent<Text>().text = "I think there are " + AssetManager.Get_object_text(obj_name, found_n) + ". \n Please tap it if I missed any! ";
        txt.SetActive(false);
        confirm_txt.SetActive(true);
    }
    public void complete_finder()
    {
        List<SceneObject> objs = ARMathUtils.get_objects_in_rect(view, obj_name);
        //Debug.Log("[ARMath] checking objs in the finder "+objs.Count);


        if (objs.Count >= min_number_of_objects)
        {
            float target_delay = 0;

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
        }
    }
    public void set_finder(string obj, int min_count, CallbackFunction cb, string p)
    {
        call_back = cb;
        call_back2 = null;
        obj_name = obj;
        min_number_of_objects = min_count;
        param = p;
        txt.GetComponent<Text>().text = "Let's find some batteries!";
        nextActionTime = Time.time + 8; // animation delay
        this.GetComponent<RectTransform>().localScale = Vector3.one;
       
    }
    public void set_finder(string obj, int min_count, CallbackFunction2 cb, string p, string prompt, float scale)
    {
        call_back = null;
        call_back2 = cb;
        obj_name = obj;
        min_number_of_objects = min_count;
        param = p;
        txt.GetComponent<Text>().text = prompt;
        nextActionTime = Time.time + 8; // animation delay
        this.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
     
    }
    
    private void handle_touch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                bool hit = ARMathUtils.check_in_recttransform(pos, view);
                if (!hit)
                {
                    return;
                }

                    pos.y = Screen.height - pos.y;
                CatalogItem ci = new CatalogItem();
                ci.Box = new Rect(pos, new Vector2(80, 80));
                ci.DisplayName = obj_name;
                SceneObjectManager.add_new_object(ci, 180);


                List<SceneObject> objs = ARMathUtils.get_objects_in_rect(view, obj_name);
                //Debug.Log("[ARMath] checking objs in the finder "+objs.Count);


                if (objs.Count >= min_number_of_objects)
                {
                    float target_delay = 0;
                    foreach (SceneObject so in objs)
                    {
                        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                        FeedbackGenerator.create_target(targetPos, target_delay, 2, 4);
                        target_delay += 0.2f;
                    }
                    

                }
            }

        }
    }
}
