using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneObjectManager : MonoBehaviour {
    public static SceneObjectManager mSOManager;
    // Use this for initialization
    public List<SceneObject> mObjectPool;
	void Start () {
        mSOManager = this;       


    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Reset()
    {
        mObjectPool.Clear();
    }

    public void add_new_object()
    {
        if(mObjectPool==null) mObjectPool = new List<SceneObject>();

        mObjectPool.RemoveAll(s => s.check_if_dead()); //clear all decayed object
    }
    

}

public class SceneObject
{
    
    public CatalogItem catalogInfo;
    public float time_instantiated;
    public float time_expire;



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
        float area_2 = rect2.height * rect2.height;
        
        if(area_1==0 || area_2==0)
        {
            Debug.Log("[ARMath] bounding box of an object is empty");
            return 0;
        }

        float area_intersect = Math.Max(0, Math.Min(rect1.xMax, rect2.xMax) - Math.Max(rect1.xMin, rect2.xMin)) * Math.Max(0, Math.Min(rect1.yMax, rect2.yMax) - Math.Max(rect1.yMin, rect2.yMin));

        float area_union = area_1 + area_2 - area_intersect;

        float overlap_portion = area_intersect / area_union;
        Debug.Log("[ARMath] intersecting portion = "+overlap_portion);
        return overlap_portion;
    }
}
