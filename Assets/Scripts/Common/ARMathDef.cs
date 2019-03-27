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
    p2_subtraction,
    p2_multiplication,
    p2_division
}
public class SystemParam
{
    public static float param_object_lifetime = 3;  //second
    public static float param_object_rect_overlap_portion = 0.6f;

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