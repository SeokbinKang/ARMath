using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour {

    public GameObject hourglass;
    public GameObject region_rectangle;
    private bool hourglass_enalbed;
    private float last_interact;
    public int last_count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hourglass_enalbed) handle_touch();
	}
    private void OnEnable()
    {

        if(hourglass!=null) hourglass.SetActive(false);
        last_count = 0;
    }
    public void enable_hourGlass(bool t)
    {
        hourglass_enalbed = t;
        if(hourglass.activeSelf!=t) hourglass.SetActive(t);
    }
    public bool hourglass_wait()
    {
        if (!hourglass_enalbed ) return false;
        if (!hourglass.activeSelf) return true;
        return !hourglass.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("end");
    }
    public void hourglass_compelte()
    {
        hourglass.GetComponent<Animator>().SetFloat("speed",10);
    }
    private void handle_touch()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Move the cube if the screen has the finger moving.
            Vector2 pos = touch.position;
            Vector2 screenpos = touch.position;
            bool hit = ARMathUtils.check_in_recttransform(pos, region_rectangle);
            if (hit)
            {
                return;
            }
            hourglass.SetActive(false);
        } else {
            if (!hourglass.activeSelf)
            {
                hourglass.GetComponent<Animator>().SetFloat("speed", 1);
                hourglass.SetActive(true);
            }

        }
    }
    public List<SceneObject> get_objects_in_rect(string obj_name)
    {
        Vector3 center = region_rectangle.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), region_rectangle.GetComponent<RectTransform>().rect.size);
        
        List<SceneObject> ret = SceneObjectManager.mSOManager.get_objects_in_rect(rect, obj_name);
        //Debug.Log("[ARMath] "+ret.Count+" "+obj_name+" objects are found in region rect:" + rect);
        return ret;
    }
    public List<SceneObject> get_objects_in_rect(string obj_name, ref List<SceneObject> out_of_rect)
    {
        Vector3 center = region_rectangle.GetComponent<RectTransform>().position;
        Rect rect = new Rect(new Vector2(center.x, center.y), region_rectangle.GetComponent<RectTransform>().rect.size);

        List<SceneObject> ret = SceneObjectManager.mSOManager.get_objects_in_rect(rect, obj_name, ref out_of_rect);
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
