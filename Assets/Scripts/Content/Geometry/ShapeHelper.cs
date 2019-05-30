using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeHelper : MonoBehaviour {

    public GameObject ContentModuleRoot;
    public GeometryShapes mShapeType;
    [Header("Common")]
    public GameObject prompt;
    public GameObject OK_proxy;
    [Header("Rectangle")]
    public GameObject LT_proxy;
    public GameObject RB_proxy;
    

    [Header("Triangle")]
    public GameObject temp;
    [Header("Circle")]
    public GameObject temp2;
    [Header("Hexagon")]
    public GameObject temp3;
    // Use this for initialization
    

    void Start () {
		
	}
     void OnEnable()
    {
        mShapeType = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_shape;
        if (mShapeType == GeometryShapes.Rectangle) rect_helper_init();

    }
    // Update is called once per frame
    void Update () {
        mShapeType = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_shape;
        if (mShapeType == GeometryShapes.Rectangle) rect_helper();
	}
    private void rect_helper_init()
    {
        string obj = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_name;
        Rect bbox = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_rect;
        prompt.GetComponent<Text>().text = "Before get started, please move the two red circles that you can only see the " + obj;
                
        if (bbox != null)
        { //global position
            Debug.Log("[ARMath] Rectangle object box: " + bbox);
            ARMathUtils.move2D_imageCoordinate(LT_proxy, new Vector2(bbox.xMin, bbox.yMin));
            ARMathUtils.move2D_imageCoordinate(RB_proxy, new Vector2(bbox.xMax, bbox.yMax));
            
        }



    }
    public void distroygraphics()
    {
        Drawing2D.destroy_polygons("rect_helper");
    }
    private void rect_helper()
    {
        if (LT_proxy == null || RB_proxy == null) return;
        //retrive the coordinates of the rectangle
        RectTransform RT1 = LT_proxy.GetComponent<RectTransform>();
        Vector3 LT_center = RT1.localPosition;
        RectTransform RT2 = RB_proxy.GetComponent<RectTransform>();
        Vector3 RB_center = RT2.localPosition;
        //draw sides of the rectangle
        RectTransform RT3 = OK_proxy.GetComponent<RectTransform>();
        Vector3 OK_center = RT2.localPosition;

        //mask the outside of the rectangle
        Color t = Color.black;
        t.a = 0.7f;
        Drawing2D.draw_mask_outrect("rect_helper", LT_center, RB_center, t);
        OK_center = (LT_center + RB_center) / 2;
        RT3.localPosition = OK_center;
        
        ContentModuleRoot.GetComponent<ContentGeometry>().final_object_shape_rect = new Rect(RT1.position.x,RT2.position.y, RT2.position.x - RT1.position.x, RT1.position.y - RT2.position.y);
       // Debug.Log("[ARMath] LT: " + RT1.position + ", " + RT1.localPosition + "   RB: " + RT2.position + ", " + RT2.localPosition);
       // Debug.Log("[ARMath] shape rect: " + ContentModuleRoot.GetComponent<ContentGeometry>().final_object_shape_rect);

    }
}
