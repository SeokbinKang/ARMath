using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {

    public static AssetManager mThis;

    public string[] object_name;
    public GameObject[] object_icon;
    public string[] object_singular_name;
    public string[] object_plural_name;
    public Color[] colors;
    public GameObject[] reward_icons;
    private Dictionary<string,GameObject> object_icon_map;
    // Use this for initialization
    void Start () {
        mThis = this;
        init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void init()
    {
        object_icon_map = new Dictionary<string, GameObject>();
        for(int i = 0; i < object_name.Length; i++)
        {
            if(i<object_icon.Length)
            {
                object_icon_map[object_name[i]] = object_icon[i];
            }
        }
    }
    public static Color[] getColors()
    {
        return mThis.colors;
    }
    public static GameObject get_icon(string name)
    {
        GameObject ret = null;
        
        if (mThis.object_icon_map.ContainsKey(name))
        {
            return mThis.object_icon_map[name];
        }
        Debug.Log("[ARMath] fail to return AssetManager.get_icon()");
        return ret;
    }
    public static string Get_object_text(string obj_name, int count)
    {
        string ret = "";
        int idx = 0;
        for(idx=0;idx<mThis.object_name.Length;idx++)
        {
            if (mThis.object_name[idx] == obj_name) break;
        }
        if (idx >= mThis.object_name.Length || count==0) return obj_name;
        if (count == 1) return mThis.object_singular_name[idx];
            else
        {
            return count + " " + mThis.object_plural_name[idx];
        }
        return ret;
    }
    public static GameObject reward_icon(ProblemType t)
    {
        return mThis.reward_icons[(int)t];
    }
}
