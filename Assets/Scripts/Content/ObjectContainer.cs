using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour {


    public GameObject region_rectangle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<SceneObject> get_objects_in_rect()
    {
        Vector3 center = region_rectangle.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), region_rectangle.GetComponent<RectTransform>().rect.size);
        
        List<SceneObject> ret = SceneObjectManager.mSOManager.get_objects_in_rect(rect);
        Debug.Log("[ARMath] "+ret.Count+" objects are found in region rect:" + rect);
        return ret;
    }
}
