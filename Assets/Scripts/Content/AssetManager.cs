using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {

    public static AssetManager mThis;

    public string[] object_name;
    public GameObject[] object_icon;
    public string[] object_singular_name;
    public string[] object_plural_name;
    
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

    public static GameObject get_icon(string name)
    {
        GameObject ret = null;

        if (mThis.object_icon_map.ContainsKey(name))
        {
            return mThis.object_icon_map[name];
        }
        return ret;
    }
}
