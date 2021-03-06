﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class SceneObjectManager : MonoBehaviour {
    public static SceneObjectManager mSOManager;
    // Use this for initialization
    public List<SceneObject> mObjectPool;
    public GameObject[] virtualSolvers;

    public string[] interactable_objects;
    private int object_id_counter;
	void Start () {
        mSOManager = this;
        mObjectPool = new List<SceneObject>();
        object_id_counter++;
    }
	
	// Update is called once per frame
	void Update () {
        handle_touch();

    }
    
    public void Reset()
    {
        //clear_attached_feedback();
        clear_scene_objects();
    }
    private void clear_scene_objects()
    {
        foreach (SceneObject so in mObjectPool)
        {
            so.clear_all_feedback();
        }
        mObjectPool.Clear();

    }
    private void clear_attached_feedback()
    {
        foreach (SceneObject so in mObjectPool)
        {
            so.clear_all_feedback();
        }
    }
    public void add_new_object(List<CatalogItem> catalog_items)
    {
        foreach(CatalogItem i in catalog_items)
        {
            if (interactable_objects.Contains(i.DisplayName)){
                SceneObject new_object = new SceneObject(i);
                new_object.id = object_id_counter++;
                add_new_object(new_object);
            }
        }
    }
    public static void add_new_object(CatalogItem i, float life_sec)
    {
        
            if (mSOManager.interactable_objects.Contains(i.DisplayName))
            {
                SceneObject new_object = new SceneObject(i);
                new_object.id = mSOManager.object_id_counter++;
            new_object.extend_life(life_sec);
                mSOManager.add_new_object(new_object);
                
            }
        
    }
    public SceneObject get_object(int id_)
    {
        foreach (SceneObject so in mObjectPool)
        {
            if (so.id == id_) return so;
        }
        return null;
    }
    public void print_object_logs()
    {
        
        Debug.Log("[ARMath]" + mObjectPool);
    }
    public static void retire_old_objects()
    {
        mSOManager.mObjectPool.RemoveAll(s => s.check_if_dead());
    }
    public void add_new_object(SceneObject o)
    {
        if(mObjectPool==null) mObjectPool = new List<SceneObject>();

        int n_before = mObjectPool.Count;

        mObjectPool.RemoveAll(s => s.check_if_dead()); //clear all decayed object
        int n_after = mObjectPool.Count;



        bool is_exist = false;

        foreach(SceneObject so in mObjectPool)
        {
            if (o.check_overlap(so))
            {
                is_exist = true;
                so.extend_life();                
                break;
            }
        }
        if(!is_exist) mObjectPool.Add(o);

        
      //  Debug.Log("[ARMath] # of objects " + mObjectPool.Count + " is new overlapped ? "+is_exist+"   deleted:"+(n_after-n_before));
    }
    public List<SceneObject> get_a_cluster_objects()
    {
        List<SceneObject> ret = SceneAnalysis.GetCluster(mObjectPool);
        
        return ret;
    }
    public List<SceneObject> get_a_cluster_objects(string object_name)
    {
        List<SceneObject> named_objs = get_objects_by_name(object_name);

        List<SceneObject> ret = SceneAnalysis.GetCluster(named_objs);

        return ret;
    }

    public List<SceneObject> get_objects_in_rect(Rect rect, string obj_name)
    {
        List<SceneObject> ret = new List<SceneObject>();
        //Debug.Log("[ARMath] get_objects_in_rect " + rect.position + "  " + obj_name + "  ");
        foreach (SceneObject so in mObjectPool)
        {
            if (so.catalogInfo.DisplayName==obj_name && so.check_in_box(rect))
            {
                ret.Add(so);
            }
        }
        return ret;
    }
    public List<SceneObject> get_objects_in_rect(Rect rect, string obj_name, ref List<SceneObject> out_of_rect)
    {
        List<SceneObject> ret = new List<SceneObject>();
        foreach (SceneObject so in mObjectPool)
        {
            if (so.catalogInfo.DisplayName == obj_name && so.check_in_box(rect))
            {
                ret.Add(so);
            } else if(so.catalogInfo.DisplayName == obj_name)
            {
                if (out_of_rect != null) out_of_rect.Add(so);
            }
        }
        return ret;
    }
    public List<SceneObject> get_objects_by_name(string name)
    {
        List<SceneObject> ret = new List<SceneObject>();
        foreach (SceneObject so in mObjectPool)
        {
            if (name == so.catalogInfo.DisplayName)
            {
                ret.Add(so);
            }
        }
        return ret;
    }
    public List<SceneObject> get_objects_by_name(List<string> name_list)
    {
        List<SceneObject> ret = new List<SceneObject>();
        foreach (SceneObject so in mObjectPool)
        {
            //Debug.Log("[ARMath] found objects = " + so.catalogInfo.DisplayName);
            if (name_list.FindIndex(so.catalogInfo.DisplayName.Equals)>=0)
            {
                ret.Add(so);
            }
        }
        return ret;
    }
    public List<SceneObject> get_objects_on_the_left(string name)
    {
        List<SceneObject> ret = new List<SceneObject>();
        foreach (SceneObject so in mObjectPool)
        {
            if (so.check_in_leftside() && name == so.catalogInfo.DisplayName)
            {
                ret.Add(so);
            }
        }
        return ret;
    }
    public void get_dominant_object(ref string name, ref Vector2 center_, ref int count)
    {
        //find the type of dominant objects
        Dictionary<string, Vector2> center = new Dictionary<string, Vector2>();
        Dictionary<string, int> counter = new Dictionary<string, int>();
        if (mObjectPool == null) return;
        foreach (SceneObject so in mObjectPool)
        {
            if (counter.ContainsKey(so.catalogInfo.DisplayName))
            {
                counter[so.catalogInfo.DisplayName]++;
            }
            else
            {
                counter[so.catalogInfo.DisplayName] = 1;
                center[so.catalogInfo.DisplayName] = new Vector2(0, 0);
            }
            center[so.catalogInfo.DisplayName] += so.catalogInfo.Box.center;
        }
        int max_freq = 0;
        string dominant_object_name = "";
        foreach (KeyValuePair<string, int> kvp in counter)
        {
           // Debug.Log("[ARMath] object pool: " + kvp.Key + "  ->  " + kvp.Value);
            if (kvp.Value > max_freq)
            {
                max_freq = kvp.Value;
                dominant_object_name = kvp.Key;
            }
        }
        //debug objects
        
        if (dominant_object_name == "") return;
        Vector2 center_of_objects = center[dominant_object_name] / ((float)counter[dominant_object_name]);
        count = counter[dominant_object_name];
        name = dominant_object_name;
        center_ = center_of_objects;

    }

    public static bool kill_sceneObject_close_to(Vector2 screen_pos, float dist_threshold)
    {
        float min_dist = float.MaxValue;
        SceneObject closest_obj = null;
        foreach (SceneObject so in mSOManager.mObjectPool)
        {
            float dist = so.distance_to_screen_pos(screen_pos);
            if (dist<dist_threshold && dist < min_dist)
            {
                min_dist = dist;
                closest_obj = so;
            }
            
        }
        if (closest_obj != null)
        {
            closest_obj.retire_yield();
            return true;
        }
        return false;

    }
    private void handle_touch()
    {
        bool require_correction = false;
        foreach(GameObject v_solvers in virtualSolvers)
        {
            if (v_solvers.activeSelf) require_correction = true;
        }
        if (!require_correction) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 pos = touch.position;
                Vector2 screenpos = touch.position;
                /*bool hit = ARMathUtils.check_in_recttransform(pos, view);
                if (!hit)
                {
                    return;
                }*/
                string obj_name = "";
                Vector2 c=new Vector2(0,0);
                int dum=4;
                get_dominant_object(ref obj_name, ref c, ref dum);
                if (!SceneObjectManager.kill_sceneObject_close_to(screenpos, 80f))
                {
                    pos.y = Screen.height - pos.y;
                    CatalogItem ci = new CatalogItem();
                    ci.Box = new Rect(pos, new Vector2(80, 80));
                    ci.DisplayName = obj_name;
                    SceneObjectManager.add_new_object(ci, 180);                    
                }
                else
                {
                    Debug.Log("[ARMath] object killed");
                    SceneObjectManager.retire_old_objects();
                }
                
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

public class SceneObject { 
    public static int global_id_counter=0;
    public CatalogItem catalogInfo;
    public float time_instantiated;
    public float time_expire;
    public int id;

    private bool been_interacted;
    private List<GameObject> attached_feedback_gameobject;
    
    public SceneObject()
    {
        init();
        
    }
    ~SceneObject()
    {
        clear_all_feedback();

    }
    public SceneObject(CatalogItem ci)
    {
        
        init();
        catalogInfo = ci;
        
    }
    public bool check_if_dead()
    {
        if (Time.time > time_expire)
        {
            clear_all_feedback();
            return true;
        }
        return false;
    }
    public void retire_yield()
    {
        this.time_expire = 0;
    }
    public bool interact()
    {
        if (been_interacted) return false;
        been_interacted = true;
        return true;
    }
    public bool is_interactive()
    {
        return !been_interacted;
    }
    
    

  
    
    public void extend_life()
    {
        time_expire = Time.time + SystemParam.param_object_extendlife;
    }
    public void extend_life(float t)
    {
        time_expire = Time.time + t;
    }

    public bool check_overlap(SceneObject o)
    {
        if (calculate_overlap(o) >= SystemParam.param_object_rect_overlap_portion) return true;
        return false;
    }
    public bool check_overlap(Rect rect)
    {
        bool ret = false;
        Rect box = this.catalogInfo.Box;
        box.center = new Vector2(box.center.x, Screen.height - box.center.y);
        ret = box.Overlaps(rect, true);
        
       // Debug.Log("[ARMath] box overlap test: object[" + box + "]  region[" + rect + "]  =  "+ret);
        return ret;
    }
    public bool check_in_leftside()
    {
        bool ret = false;
        Rect box = this.catalogInfo.Box;
        box.center = new Vector2(box.center.x, Screen.height - box.center.y);
        if (box.center.x < Screen.width / 2) ret = true;
        
        //if (box.center.x < 750 + 419 && box.center.x > 750 - 419 && box.center.y < 830 + 407 && box.center.y > 830 - 407)
        //{
        //    Debug.Log("[ARMath] pre-addtion in box: " + box.center);
        //    ret = true;
        //}
        return ret;
    }
    public Vector2 get_screen_pos()
    {
        Rect box = this.catalogInfo.Box;
        return new Vector2(box.center.x, Screen.height - box.center.y);

    }
    public float distance_to_screen_pos(Vector2 screen_pos)
    {
        Rect box = this.catalogInfo.Box;
        Vector2 dist = new Vector2(box.center.x, Screen.height - box.center.y);
        dist = dist - screen_pos;
        return dist.magnitude;
    }
    public bool check_in_box(Rect rect)
    {
        
        bool my_ret = false;
        
        Rect box = this.catalogInfo.Box;
        box.center = new Vector2(box.center.x, Screen.height - box.center.y);
        
        //Debug.Log("[ARMath] box container test: " + box.center.x + "  " + (rect.x - rect.width / 2) + "  " + (rect.x + rect.width / 2) + "  " + box.center.y + "  " + (rect.y - rect.height / 2) + "  " + (rect.y + rect.height / 2));
        if (box.center.x >= (rect.x-rect.width/2) && box.center.x <= (rect.x + rect.width / 2) &&
            box.center.y >= (rect.y - rect.height / 2) && box.center.y <= (rect.y + rect.height / 2))
        {
            
            my_ret = true;
        }
    //    Debug.Log("[ARMath] box container test: object[" + box.center + "]  region[" + rect + "]  =  " + ret +" or "+my_ret);
        return my_ret;
    }
    private void init()
    {
        time_instantiated = Time.time;
        time_expire = Time.time + SystemParam.param_object_initialLife;
        catalogInfo = null;
        been_interacted = false;
        attached_feedback_gameobject = new List<GameObject>();
      
    }
    private float calculate_overlap(SceneObject o2)
    {
        Rect rect1 = catalogInfo.Box;
        Rect rect2 = o2.catalogInfo.Box;

        float area_1 = rect1.height * rect1.width;
        float area_2 = rect2.height * rect2.width;
        
        if(area_1==0 || area_2==0)
        {
            Debug.Log("[ARMath] bounding box of an object is empty");
            return 0;
        }

        float area_intersect = Math.Max(0, Math.Min(rect1.xMax, rect2.xMax) - Math.Max(rect1.xMin, rect2.xMin)) * Math.Max(0, Math.Min(rect1.yMax, rect2.yMax) - Math.Max(rect1.yMin, rect2.yMin));

        float area_union = area_1 + area_2 - area_intersect;

        float overlap_portion = area_intersect / area_union;
        //Debug.Log("[ARMath] intersecting portion = "+overlap_portion);
        return overlap_portion;
    }
  
    public void clear_all_feedback()
    {
        if (attached_feedback_gameobject != null)
        {
            foreach (var i in attached_feedback_gameobject)
            {

                GameObject.Destroy(i);
            }
            attached_feedback_gameobject.Clear();
        }

    }
    public void clear_number_feedback()
    {
        if (attached_feedback_gameobject != null)
        {
            foreach (var i in attached_feedback_gameobject)
            {
                number_cartoon n_c = i.GetComponent<number_cartoon>();
                if (n_c != null)
                {
                    GameObject.Destroy(i);
                }
            }

        }
    }
    public bool is_feedback_attached()
    {
        attached_feedback_gameobject.RemoveAll(s => s==null);
        if (attached_feedback_gameobject.Count > 0) return true;
        return false;
    }

    public int get_number_feedback()
    {
        int ret = -1;
        attached_feedback_gameobject.RemoveAll(s => s == null);
        if (attached_feedback_gameobject != null)
        {
            foreach (GameObject o in attached_feedback_gameobject)
            {
                if (o == null) continue;
                number_cartoon n_c = o.GetComponent<number_cartoon>();
                if (n_c != null) return n_c.num;
            }
        }
        return ret;
    }
    public int get_all_feedback_count()
    {
        int ret = -1;
        attached_feedback_gameobject.RemoveAll(s => s == null);
        if (attached_feedback_gameobject != null)
        {
            ret = 0;
            foreach (var o in attached_feedback_gameobject)
            {
                if (o != null) ret++;
            }
            return ret;
        }
        return 0;
    }
    public bool attach_object(GameObject feedback_go)
    {
        if (attached_feedback_gameobject == null) attached_feedback_gameobject = new List<GameObject>();
        if (feedback_go != null) this.attached_feedback_gameobject.Add(feedback_go);
        return true;
    }
    public GameObject attached_button_visibility(float alpha)
    {
        foreach (GameObject o in attached_feedback_gameobject)
        {
            if (o.GetComponent<Button>() == null || o.GetComponent<Image>() == null) continue;
            Color c = o.GetComponent<Image>().color;
            c.a = alpha;
            o.GetComponent<Image>().color = c;
            return o;
        }
        return null;
    }

}
