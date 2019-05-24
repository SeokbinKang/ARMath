using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryVisContainer : MonoBehaviour {

    public GameObject contentroot;
    public GameObject VisRectangle;
    public GameObject VisTriangle;
    public GameObject VisCircle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        if(contentroot.GetComponent<ContentGeometry>().target_object_shape==GeometryShapes.Rectangle)
        {
            VisRectangle.SetActive(true);
            if(VisTriangle) VisTriangle.SetActive(false);
            if(VisCircle) VisCircle.SetActive(false);

            Rect target_box = contentroot.GetComponent<ContentGeometry>().final_object_shape_rect;
            Debug.Log("[ARMath] target box: " + target_box+"  center: "+target_box.center);
            
            VisRectangle.GetComponent<RectTransform>().position = new Vector3(target_box.center.x,target_box.center.y,0);
            VisRectangle.GetComponent<RectTransform>().sizeDelta = target_box.size;

            Debug.Log("[ARMath] target box real: " + VisRectangle.GetComponent<RectTransform>().position + "  center: " + VisRectangle.GetComponent<RectTransform>().sizeDelta);

        } else if (contentroot.GetComponent<ContentGeometry>().target_object_shape == GeometryShapes.Triangle)
        {
            VisRectangle.SetActive(false);
            if (VisTriangle) VisTriangle.SetActive(true);
            if (VisCircle) VisCircle.SetActive(false);

        } else if(contentroot.GetComponent<ContentGeometry>().target_object_shape == GeometryShapes.Circle){
            VisRectangle.SetActive(false);
            if(VisTriangle) VisTriangle.SetActive(false);
            if (VisCircle) VisCircle.SetActive(true);

        }

    }
}
