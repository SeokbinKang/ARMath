using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum SystemStatus
{

    Opening,
    // The flag for Spoiler is 0010.
    Setup,
    // The flag for FogLights is 0100.
    UserAccount,
    // The flag for TintedWindows is 1000.
    SelectionQuestion,
    Content,
    Correction
}
public enum ProblemType
{
    p1_counting,
    p2_addition,
    p4_geometry,
    p3_multiplication,
    p3_division,
    
}
public enum GeometryShapes
{
    Rectangle,
    Circle,
    Triangle,
    CustomGroup,
    Trapezoid
}
public enum GeometryPrimitives
{
    vertex,
    side_short,
    side_long,
    angle,
    none
}

public enum DialogueType
{
    left_bottom_plain,
    right_pop,
    Center,
    Dummy
}
public enum angle_name
{
    acute,
    right,
    obtuse
}
public class SystemParam
{
    public static float param_object_lifetime = 3.2f;  //second
    public static float param_object_rect_overlap_portion = 0.1f;
    public static float system_update_period = 0.5f;  //second

    public static int image_size = 300;

    public static float vertext_proximity = 80;
    public static float vertex_overlap_distance = 10;

    public static float cluster_neighboring_distance = 400;
    public static float cluster_min_count = 2;

    public static float timeout_for_interaction_prompt = 5;
    public static float timeout_for_prompt_disappear = 8;



}


public class ARMathDef : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public class CVResult
{
    public List<CatalogItem> mObjects;
}
public class Gem
{
    public ProblemType problem_type;
    public List<string> snapshots;
}
public class UserInfo
{
    public string user_name;
    public float user_id;
    public Dictionary<ProblemType,List<Gem>> gem_collection;
        
    public UserInfo()
    {
        gem_collection = new Dictionary<ProblemType, List<Gem>>();
        foreach (ProblemType t in Enum.GetValues(typeof(ProblemType)))
        {
            gem_collection[t] = new List<Gem>();
        }
    }

    public void AddGem(Gem g)
    {
        gem_collection[g.problem_type].Add(g);


    }
}

public class ARMathUtils
{
    
    public static GameObject create_2DPrefab(GameObject prefab, GameObject parent, Vector2 screen_pos)
    {
        Vector3 targetPos = screen_pos;
        UnityEngine.GameObject label = GameObject.Instantiate(prefab, targetPos, Quaternion.identity) as GameObject;
        label.transform.SetParent(parent.gameObject.transform);
        RectTransform r = label.GetComponent<RectTransform>();
        r.localScale = new Vector3(1f, 1f, 1f);
        r.position = targetPos;
        label.GetComponent<RectTransform>().position = r.position;
        label.GetComponent<RectTransform>().localScale = r.localScale;
        return label;
    }

    public static GameObject create_2DPrefab(GameObject prefab, GameObject parent)
    {
        Vector3 targetPos = new Vector3(0, 0, 0);
        UnityEngine.GameObject label = GameObject.Instantiate(prefab, targetPos, Quaternion.identity) as GameObject;
        label.transform.SetParent(parent.gameObject.transform);
        RectTransform r = label.GetComponent<RectTransform>();
        r.localScale = new Vector3(1f, 1f, 1f);
        r.position = targetPos;
        
        label.GetComponent<RectTransform>().localPosition = r.position;
        label.GetComponent<RectTransform>().localScale = r.localScale;

        return label;

    }
    public static void SetRecttrasnform(GameObject o, Rect r)
    {
        if (o == null) return;
        RectTransform rt = o.GetComponent<RectTransform>();
        if (rt == null) return;
        //assuming r is global position
        rt.position = r.center;
        rt.sizeDelta = r.size;

        Debug.Log("[ARMath] rect " + r + " -> object.recttransform.localpos " + rt.localPosition);

    }
    public static void SetRecttrasnform(GameObject o, Rect r, float scale)
    {
        if (o == null) return;
        RectTransform rt = o.GetComponent<RectTransform>();
        if (rt == null) return;
        //assuming r is global position
        rt.position = r.center;
        rt.sizeDelta = r.size*scale;

        Debug.Log("[ARMath] rect " + r + " -> object.recttransform.localpos " + rt.localPosition);

    }
    public static void move2D_ScreenCoordinate(GameObject go, Vector2 screen_position)
    {
        
        RectTransform r = go.GetComponent<RectTransform>();
        r.position = screen_position;
        go.GetComponent<RectTransform>().position = r.position;


    }
    public static void move2D_imageCoordinate(GameObject go, Vector2 global_position)
    {
        Vector3 targetPos = new Vector3(global_position.x, Screen.height - global_position.y, 0);                
        RectTransform r = go.GetComponent<RectTransform>();
        r.position = targetPos;        
        //label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);        
    }

    public static string shape_name(GeometryShapes shape)
    {
        if (shape == GeometryShapes.Circle) return "circle";
        if (shape == GeometryShapes.Rectangle) return "rectangle";
        if (shape == GeometryShapes.Triangle) return "triangle";
        return "unknown shape";

    }
    public static bool check_in_recttransform(Vector2 point, GameObject go)
    {
        if (go == null) return false;
        Vector3 center = go.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), go.GetComponent<RectTransform>().rect.size);

        if (point.x >= (rect.x - rect.width / 2) && point.x <= (rect.x + rect.width / 2) &&
            point.y >= (rect.y - rect.height / 2) && point.y <= (rect.y + rect.height / 2))
        {
            return true;
        }
        return false;
    }
    public static List<SceneObject> get_objects_in_rect(GameObject rect_obj, string obj_name)
    {
        Vector3 center = rect_obj.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), rect_obj.GetComponent<RectTransform>().rect.size);

        List<SceneObject> ret = SceneObjectManager.mSOManager.get_objects_in_rect(rect, obj_name);
        Debug.Log("[ARMath] "+ret.Count+" "+obj_name+" objects are found in region rect:" + rect);
        return ret;
    }


}

public delegate void CallbackFunction(string param);
public delegate void CallbackFunction2(string param,List<SceneObject> obj_list, Rect rt);