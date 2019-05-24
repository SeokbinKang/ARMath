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

    public List<SceneObject> get_objects_in_rect(string obj_name)
    {
        Vector3 center = region_rectangle.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), region_rectangle.GetComponent<RectTransform>().rect.size);
        
        List<SceneObject> ret = SceneObjectManager.mSOManager.get_objects_in_rect(rect, obj_name);
      //  Debug.Log("[ARMath] "+ret.Count+" objects are found in region rect:" + rect);
        return ret;
    }
    public bool in_container(GameObject o)
    {
       
        Vector3 center = region_rectangle.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), region_rectangle.GetComponent<RectTransform>().rect.size);
        RectTransform rectTrans2 = o.GetComponent<RectTransform>();
        Vector3 center2 = rectTrans2.position;
        Rect rect2 = new Rect(new Vector2(center2.x, center2.y), rectTrans2.rect.size);
        //bool ret = rect1.Overlaps(rect2);  //NOT WORKING
        bool ret = false;
        if (center2.x >= (rect.x - rect.width / 2) && center2.x <= (rect.x + rect.width / 2) &&
           center2.y >= (rect.y - rect.height / 2) && center2.y <= (rect.y + rect.height / 2))
        {

            ret = true;
        }
        Debug.Log("[ARMath] gameobject in container " + rect  + "    "+center2+"  =  "+ret);
        return ret;
    }

    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        /*  Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
          Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);*/
        Rect rect1 = new Rect(rectTrans1.position.x, rectTrans1.position.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.position.x, rectTrans2.position.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}
