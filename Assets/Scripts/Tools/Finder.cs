using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finder : MonoBehaviour {

    public GameObject view;
    public GameObject txt;
    private CallbackFunction call_back;
    public string obj_name;
    public int min_number_of_objects;
    private string param;
    private float nextActionTime = 0.0f;
    // Use this for initialization
    void Start () {
        //call_back = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
         //   Debug.Log("[ARMath] update finder ");
            if (call_back != null) check_object();
            
        }
      
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
                target_delay += 0.4f;
            }
            call_back(objs.Count.ToString());
            call_back = null;
            this.gameObject.SetActive(false);
        }
        
    }
    public void set_finder(string obj, int min_count,CallbackFunction cb,string p)
    {
        call_back = cb;
        obj_name = obj;
        min_number_of_objects = min_count;
        param = p;
        txt.GetComponent<Text>().text = "Let's find some batteries!";
        nextActionTime = Time.time + 8; // animation delay
    }
}
