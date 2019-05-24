using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum SystemStatus
{

    Opening,
    // The flag for Spoiler is 0010.
    MainMenu,
    // The flag for FogLights is 0100.
    UserAccount,
    // The flag for TintedWindows is 1000.
    SelectionQuestion,
    Content
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
    Trapezoid
}
public enum GeometryPrimitives
{
    vertex,
    side_horizontal,
    side_vertical,
    angle
}
public class SystemParam
{
    public static float param_object_lifetime = 3.2f;  //second
    public static float param_object_rect_overlap_portion = 0.1f;
    public static float system_update_period = 0.5f;  //second

    public static int image_size = 300;

    public static float vertext_proximity = 80;

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

    public static void move2dobject(GameObject go, Vector2 global_position)
    {
        Vector3 targetPos = new Vector3(global_position.x, Screen.height - global_position.y, 0);                
        RectTransform r = go.GetComponent<RectTransform>();
        r.position = targetPos;        
        //label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);        
    }
}