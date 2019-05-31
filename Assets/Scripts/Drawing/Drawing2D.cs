using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing2D : MonoBehaviour {
    public static Drawing2D mThis;
    public GameObject pre_quadliteral;
    public GameObject brush_circle;
    public Color brush_color;

    private Dictionary<string, List<GameObject>> mDrawingObjects;

    
	// Use this for initialization
	void Start () {
        mThis = this;
        mDrawingObjects = new Dictionary<string, List<GameObject>>();
        //test drawings
        /* Vector2[] v = new Vector2[4];
         for(int i=0;i<v.Length;i++)
         {
             v[i].x = Random.Range(-1000, 1000);
             v[i].y = Random.Range(-600, 600);
         }
         create_2DRect("testrect", v, Color.cyan);*/

        // test outer mask
        /*
        Color t = Color.black;
        t.a = 0.7f;
        draw_mask_outrect("sdfsdf", new Vector2(-400, 300), new Vector2(500, -200), t);
        */
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //
    public static void Reset()
    {
        if (mThis==null || mThis.mDrawingObjects == null) return;
        foreach(var t in mThis.mDrawingObjects.Keys)
        {
            destroy_polygons(t);
        }
    }
    public static void Correct_Rectangle(string name,List<Vector2> new_pos)
    {
        if (!mThis.mDrawingObjects.ContainsKey(name))
        {
            return;
        }
        for (int i = 0; i < mThis.mDrawingObjects[name].Count; i++)
        {
            if(i<new_pos.Count)
                ARMathUtils.move2D_ScreenCoordinate(mThis.mDrawingObjects[name][i], new_pos[i]);
        }
            


    }
    public static GameObject draw_pen(string name, Vector2 pos)
    {
        if (!mThis.mDrawingObjects.ContainsKey(name))
        {
            mThis.mDrawingObjects[name] = new List<GameObject>();
            

        }
        UnityEngine.GameObject label = ARMathUtils.create_2DPrefab(mThis.brush_circle, mThis.gameObject,pos);
        
        mThis.mDrawingObjects[name].Add(label);

        return label;
    }
    public static GameObject create_2DRect(string name, Vector2[] mCorners, Color c)
    {
        if(mCorners.Length<4 || mThis.pre_quadliteral==null)
        {
            Debug.Log("[ARMath] fail to create_2DRect: insufficient corner infor");
            return null;
        }
        Vector3 targetPos = new Vector3(0, 0, 0);
        UnityEngine.GameObject label;
        if (mThis.mDrawingObjects.ContainsKey(name)) label = mThis.mDrawingObjects[name][0];
        else label= ARMathUtils.create_2DPrefab(mThis.pre_quadliteral, mThis.gameObject);
        PrimQuadLiteral ql = label.GetComponent<PrimQuadLiteral>();
        if (ql == null) return null;
        ql.setVertices(mCorners);
        ql.setColor(c);
        if (!mThis.mDrawingObjects.ContainsKey(name))
        {
            List<GameObject> l = new List<GameObject>();
            l.Add(label);
            mThis.mDrawingObjects[name] = l;
        }

        return null;
    }
    public static void destroy_polygons(string name)
    {
        if (mThis.mDrawingObjects.ContainsKey(name) == false) return;
        foreach(GameObject go in mThis.mDrawingObjects[name])
        {
            Destroy(go);
        }
        mThis.mDrawingObjects[name].Clear();
        mThis.mDrawingObjects.Remove(name);
    }
    public static void draw_mask_outrect(string name,Vector2 lefttop,Vector2 rightbottom, Color c)
    {
        List<GameObject> l;
        PrimQuadLiteral ql;
        UnityEngine.GameObject label;
       // Debug.Log("[ARMath] draw_mask_outrect" + lefttop + "  " + rightbottom);
        if (mThis.mDrawingObjects.ContainsKey(name) && mThis.mDrawingObjects[name].Count==4)
        {
           
        } else
        {
            l= new List<GameObject>();
            for (int i = 0; i < 4; i++) {
                label = ARMathUtils.create_2DPrefab(mThis.pre_quadliteral, mThis.gameObject);
                ql = label.GetComponent<PrimQuadLiteral>();
                if (ql == null) return;
                l.Add(label);
            }
            mThis.mDrawingObjects[name] = l;
        }
         
        //top
        
        Vector2[] mCorners = new Vector2[4];
        mCorners[0].x = -1*Screen.width / 2;
        mCorners[0].y = Screen.height / 2;
        mCorners[1].x = Screen.width / 2;
        mCorners[1].y = Screen.height / 2;
        mCorners[2].x = Screen.width / 2;
        mCorners[2].y = lefttop.y;
        mCorners[3].x = -1 * Screen.width / 2;
        mCorners[3].y = lefttop.y;
        ql = mThis.mDrawingObjects[name][0].GetComponent<PrimQuadLiteral>();
        ql.setVertices(mCorners);
        ql.setColor(c);       
        

        //bottom
        mCorners[0].x = -1 * Screen.width / 2;
        mCorners[0].y = rightbottom.y;
        mCorners[1].x = Screen.width / 2;
        mCorners[1].y = rightbottom.y;
        mCorners[2].x = Screen.width / 2;
        mCorners[2].y = -1* Screen.height / 2;
        mCorners[3].x = -1 * Screen.width / 2;
        mCorners[3].y = -1 * Screen.height / 2;
        ql = mThis.mDrawingObjects[name][1].GetComponent<PrimQuadLiteral>();
        ql.setVertices(mCorners);
        ql.setColor(c);
        
        //left
        
        mCorners[0].x = -1 * Screen.width / 2;
        mCorners[0].y = lefttop.y;
        mCorners[1].x = lefttop.x;
        mCorners[1].y = lefttop.y;
        mCorners[2].x = lefttop.x;
        mCorners[2].y = rightbottom.y;
        mCorners[3].x = -1 * Screen.width / 2;
        mCorners[3].y = rightbottom.y;
        ql = mThis.mDrawingObjects[name][2].GetComponent<PrimQuadLiteral>();
        ql.setVertices(mCorners);
        ql.setColor(c);
        
        //right
        
        mCorners[0].x = rightbottom.x;
        mCorners[0].y = lefttop.y;
        mCorners[1].x = Screen.width / 2;
        mCorners[1].y = lefttop.y;
        mCorners[2].x = Screen.width / 2;
        mCorners[2].y = rightbottom.y;
        mCorners[3].x = rightbottom.x;
        mCorners[3].y = rightbottom.y;
        ql = mThis.mDrawingObjects[name][3].GetComponent<PrimQuadLiteral>();
        ql.setVertices(mCorners);
        ql.setColor(c);       

        
    }

}
