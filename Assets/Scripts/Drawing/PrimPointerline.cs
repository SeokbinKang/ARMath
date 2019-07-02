using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class PrimPointerline : Graphic {

    public Vector2 src; //local position.
    public Vector2 dest;
    public float line_length;
    public float dash_length;
    public float line_width;


    public GameObject target_circle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Setup(Vector2 global_src, Vector2 global_dst)
    {
        Vector2 center = this.GetComponent<RectTransform>().position;
        src = global_src - center;
        dest = global_dst - center;
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        draw_line(vh);
        if (target_circle)
        {
            target_circle.GetComponent<RectTransform>().localPosition = new Vector3(src.x,src.y);
        }
        return;

    }
    
    private void draw_line(VertexHelper vh)
    {
        
        if (line_width <= 0 || line_length <= 0) return ;
        vh.Clear();
        Vector2[] mVect = new Vector2[4];
        Vector2 direction = dest -src;
        Vector2 direction_unit = direction.normalized;
        Vector3 direction_90rot = direction_unit;
        Vector2 direction_90rot2;
        Vector2 cur=src;
        bool draw_line = true;
        int k = 0;

        direction_90rot = Quaternion.Euler(0, 90, 0) * direction_90rot;
        direction_90rot = direction_90rot * line_width;
        direction_90rot2 = direction_90rot;
        UIVertex vert = UIVertex.simpleVert;
        int num_vertices = 0;
        Vector2 last_diff = dest - cur;
        while (k++<100)
        {
            Vector2 diff = dest - cur;
            Debug.Log("[ARMath] cur :" + cur +"diff:"+diff);
            if (diff.magnitude < SystemParam.vertex_overlap_distance || diff.x*last_diff.x<0 || diff.y * last_diff.y < 0) break;
            //draw either line or dash segment
            if(draw_line)
            {
                //draw line segment
                
                mVect[0] = cur;
                mVect[1] = cur + direction_unit * line_length;
                mVect[2] = mVect[1] + direction_90rot2 * line_width;
                mVect[3] = cur + direction_90rot2 * line_width;                

                vert.position = new Vector2(mVect[0].x, mVect[0].y);
                vert.color = color;
                vh.AddVert(vert);

                vert.position = new Vector2(mVect[1].x, mVect[1].y);
                vert.color = color;
                vh.AddVert(vert);

                vert.position = new Vector2(mVect[2].x, mVect[2].y);
                vert.color = color;
                vh.AddVert(vert);

                vert.position = new Vector2(mVect[3].x, mVect[3].y);
                vert.color = color;
                vh.AddVert(vert);
                num_vertices += 4;

                cur = mVect[1];
            } else
            {
                //skip dash
                cur += diff.normalized * dash_length;
            }
            
            draw_line = !draw_line;
            last_diff = diff;
        }
        for(int i = 0; i < num_vertices; i+=4)
        {
            vh.AddTriangle(i, i+1, i+2);
            vh.AddTriangle(i+2, i+3, i);
        }
    }

   
}
