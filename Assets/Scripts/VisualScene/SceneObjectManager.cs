using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SceneObjectManager : MonoBehaviour {
    public static SceneObjectManager mSOManager;
    // Use this for initialization
    public List<SceneObject> mObjectPool;

    public string[] interactable_objects;
    private int object_id_counter;
	void Start () {
        mSOManager = this;
        mObjectPool = new List<SceneObject>();
        object_id_counter++;
    }
	
	// Update is called once per frame
	void Update () {
		
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
    public void print_object_logs()
    {
        Debug.Log("[ARMath]" + mObjectPool);
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
    public List<SceneObject> get_objects_in_rect(Rect rect)
    {
        List<SceneObject> ret = new List<SceneObject>();
        foreach (SceneObject so in mObjectPool)
        {
            if (so.check_overlap(rect))
            {
                ret.Add(so);
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

    public void get_dominant_object(ref string name, ref Vector2 center_, ref int count)
    {
        //find the type of dominant objects
        Dictionary<string, Vector2> center = new Dictionary<string, Vector2>();
        Dictionary<string, int> counter = new Dictionary<string, int>();
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
            if (kvp.Value > max_freq)
            {
                max_freq = kvp.Value;
                dominant_object_name = kvp.Key;
            }
        }
        Vector2 center_of_objects = center[dominant_object_name] / ((float)counter[dominant_object_name]);
        count = counter[dominant_object_name];
        name = dominant_object_name;
        center_ = center_of_objects;

    }

}

public class SceneObject
{
    
    public CatalogItem catalogInfo;
    public float time_instantiated;
    public float time_expire;
    public int id;


    public SceneObject()
    {
        init();
        catalogInfo = null;
    }
    public SceneObject(CatalogItem ci)
    {
        catalogInfo = ci;
        init();        
    }
    public bool check_if_dead()
    {
        if (Time.time > time_expire) return true;
        return false;
    }
    
    public void extend_life()
    {
        time_expire = Time.time + SystemParam.param_object_lifetime;
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
        
        Debug.Log("[ARMath] box overlap test: object[" + box + "]  region[" + rect + "]  =  "+ret);
        return ret;
    }
    private void init()
    {
        time_instantiated = Time.time;
        time_expire = Time.time + SystemParam.param_object_lifetime;
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


}
