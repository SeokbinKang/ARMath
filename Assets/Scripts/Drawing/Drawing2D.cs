using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing2D : MonoBehaviour {
    public static Drawing2D mThis;
    public GameObject pre_quadliteral;

    private Dictionary<string, GameObject> mDrawingObjects;
	// Use this for initialization
	void Start () {
        mThis = this;

        //test drawings
        Vector2[] v = new Vector2[4];
        for(int i=0;i<v.Length;i++)
        {
            v[i].x = Random.Range(-1000, 1000);
            v[i].y = Random.Range(-600, 600);
        }
        create_2DRect("testrect", v, Color.cyan);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static GameObject create_2DRect(string name, Vector2[] mCorners, Color c)
    {
        if(mCorners.Length<4 || mThis.pre_quadliteral==null)
        {
            Debug.Log("[ARMath] fail to create_2DRect: insufficient corner infor");
            return null;
        }
        Vector3 targetPos = new Vector3(0, 0, 0);
        UnityEngine.GameObject label = ARMathUtils.create_2DPrefab(mThis.pre_quadliteral, mThis.gameObject);
        PrimQuadLiteral ql = label.GetComponent<PrimQuadLiteral>();
        if (ql == null) return null;
        ql.setVertices(mCorners);
        ql.setColor(c);

        mThis.mDrawingObjects[name] = label;

        return null;
    }


}
