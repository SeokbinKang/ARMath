using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPrimitives : MonoBehaviour {

    public GameObject content_root;
    public GeometryPrimitives shown_prim_type;
    public GameObject[] prim_vertex;
    public GameObject[] prim_side_horizontal;
    public GameObject[] prim_side_vertical;
    public GameObject[] prim_angle;
    // Use this for initialization
    private float nextActionTime = 0.0f;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period+1f;
            check_status();
        }
        

    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset(GeometryPrimitives t)
    {
        
        shown_prim_type = t;
        Reset();
    }
    private void Reset()
    {
        float init_y = 484;
        float offset_y = -300;        
        if (prim_vertex!=null)
        {
            if (shown_prim_type == GeometryPrimitives.vertex)
            {

                foreach (var v in prim_vertex)
                {
                    v.SetActive(true);
                    RectTransform rt = v.GetComponent<RectTransform>();
                    Vector3 v3 = rt.localPosition;
                    v3.x = 0;
                    v3.z = 0;
                    v3.y = init_y;
                    init_y += offset_y;
                    rt.localPosition = v3;
                }
            }
            else
            {
                foreach (var v in prim_vertex)
                    v.SetActive(false);
            }
        }
        if (prim_side_horizontal != null)
        {
            if (shown_prim_type == GeometryPrimitives.side_short)
            {
                foreach (var v in prim_side_horizontal)
                {
                    v.SetActive(true);
                    RectTransform rt = v.GetComponent<RectTransform>();
                    Vector3 v3 = rt.localPosition;
                    v3.x = 0;
                    v3.z = 0;
                    v3.y = init_y;
                    init_y += offset_y;
                    rt.localPosition = v3;
                }
            }
            else
            {
                foreach (var v in prim_side_horizontal)
                    v.SetActive(false);
            }
        }
        if (prim_side_vertical != null)
        {
            if (shown_prim_type == GeometryPrimitives.side_long)
            {
                foreach (var v in prim_side_vertical)
                {
                    v.SetActive(true);
                    RectTransform rt = v.GetComponent<RectTransform>();
                    Vector3 v3 = rt.localPosition;
                    v3.x = 0;
                    v3.z = 0;
                    v3.y = init_y;
                    init_y += offset_y;
                    rt.localPosition = v3;
                }
            }
            else
            {
                foreach (var v in prim_side_vertical)
                    v.SetActive(false);
            }
        }
        if (prim_angle != null)
        {

            if (shown_prim_type == GeometryPrimitives.angle)
            {
                //swap
                for(int i = 0; i < 5; i++)
                {
                    int obj_i = Random.Range(0, prim_angle.Length);
                    int obj_j = Random.Range(0, prim_angle.Length);
                    if (obj_i != obj_j)
                    {
                        GameObject temp = prim_angle[obj_i];
                        prim_angle[obj_i] = prim_angle[obj_j];
                        prim_angle[obj_j] = temp;
                    }
                }
                foreach (var v in prim_angle)
                {
                    v.SetActive(true);
                    RectTransform rt = v.GetComponent<RectTransform>();
                    Vector3 v3 = rt.localPosition;
                    v3.x = 0;
                    v3.z = 0;
                    v3.y = init_y;
                    init_y += offset_y;
                    rt.localPosition = v3;
                    v.GetComponent<DragObject>().AlwaysShowChildren(); //set the visibility of children true.
                }
            }
            else
            {
                foreach (var v in prim_angle)
                    v.SetActive(false);
            }
        }
    }
    public GameObject[] GetAllPrimitives(GeometryPrimitives type)
    {
        if (type == GeometryPrimitives.vertex) return prim_vertex;
        if (type == GeometryPrimitives.side_short) return prim_side_horizontal;
        if (type == GeometryPrimitives.side_long) return prim_side_vertical;
        if (type == GeometryPrimitives.angle)
        {
            
                return prim_angle;
        }
        return null;
    }
    private void check_status()
    {
        if (shown_prim_type == GeometryPrimitives.angle)
        {
            foreach (var v in prim_angle)
            {
                if (v.name.IndexOf("correct") >= 0 && !v.activeSelf)
                {
                    Reset(GeometryPrimitives.angle);
                }
            }
        }
    }
}
