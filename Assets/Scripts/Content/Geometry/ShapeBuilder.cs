using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBuilder : MonoBehaviour {


    public GameObject contentRoot;
    public GeometryShapes shape;
    public string cur_drawing_name;

    public GameObject virtual_solver;

    private int drawing_id = 0;
    private List<Vector2> drawing_points;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        handle_touch();

    }
    
    private void handle_touch()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {

            if (cur_drawing_name == "")
            {
                cur_drawing_name = "custom_draw" + drawing_id++;
            }
            Vector2 pos = Input.mousePosition;
            Debug.Log("[ARMath] drawing at " + pos);
            Drawing2D.draw_pen(cur_drawing_name, pos);
        } else
        {
            cur_drawing_name = "";
        }
        return;*/
        if (Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if( touch.phase == TouchPhase.Began)
            {                
                cur_drawing_name = "custom_draw" + drawing_id++;
                drawing_points = new List<Vector2>();
            }
            if (touch.phase == TouchPhase.Moved )
            {
                Vector2 pos = touch.position;
                /*pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(-pos.x, pos.y, 0.0f);*/

                // Position the cube.
                //Debug.Log("[ARMath] drawing at " + pos);
                Drawing2D.draw_pen(cur_drawing_name, pos);
                drawing_points.Add(pos);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                //finalize the shape
                if (this.shape == GeometryShapes.Rectangle)
                {
                    bool res = false;
                    res = EvaluteShape(cur_drawing_name);
                    cur_drawing_name = "";
                    if (res)
                    {
                        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                          "Nice job! Can you tell me more about the rectangle?",
                           true,
                          new CallbackFunction(start_geometry_solver),
                          ""
                          ));

                    }
                }
                if(this.shape == GeometryShapes.CustomGroup)
                {
                    Vector2 c=Vector2.zero;
                    if (drawing_points.Count < 4)
                    {
                        Drawing2D.destroy_polygons(cur_drawing_name);

                    }
                    else
                    {
                        foreach (Vector2 p in drawing_points)
                        {
                            c += p;
                        }
                        c /= (float)drawing_points.Count;
                        virtual_solver.GetComponent<DivVirtual>().onNewGroup(c);
                    }
                }
            }

           
        }
    }
    public void start_geometry_solver(string t)
    {
        contentRoot.GetComponent<ContentGeometry>().s4_startsolver();
    }
    private void feedback_badShape()
    {
        //feedback "draw again"

        //delete the drawing
        Drawing2D.destroy_polygons(cur_drawing_name);
    }
    private void good_shape()
    {

    }
    private bool EvaluteShape(string drawing_name)
    {
        if(drawing_points==null || drawing_points.Count<8)
        {
            feedback_badShape();
            return false;
        }
        //evaluate if the start point is close to the end point
        Vector2 dist = drawing_points[0] - drawing_points[drawing_points.Count - 1];
        if(dist.magnitude>100)
        {
            //feedback "draw again"
            feedback_badShape();
            return false;
        }
        //TODO: shape evaluation (vertices and sides evaluation). maybe animated.
        if(this.shape==GeometryShapes.Rectangle)
        {
            //SKIP evaluation
            Vector2 LT, RB;
            LT.x = 99999;
            LT.y = -99999;
            RB.x = -99999;
            RB.y = 99999;
            for(int i = 0; i < drawing_points.Count; i++)
            {
                if (drawing_points[i].x < LT.x) LT.x = drawing_points[i].x;
                if (drawing_points[i].y > LT.y) LT.y = drawing_points[i].y;
                if (drawing_points[i].x > RB.x) RB.x = drawing_points[i].x;
                if (drawing_points[i].y < RB.y) RB.y = drawing_points[i].y;
            }

            //rectify the drawn shape
            int[] four_corners_indices = new int[4];
            float[] four_corners_distnace = new float[4];
            //find vertices those are the closest to the 4 corners
            Vector2[] four_corners_pos = new Vector2[4];
            four_corners_pos[0] = LT;
            four_corners_pos[1] = LT;
            four_corners_pos[1].x = RB.x;
            four_corners_pos[2] = RB;
            four_corners_pos[3] = RB;
            four_corners_pos[3].x = LT.x;
            for (int i = 0; i < 4; i++)
                four_corners_distnace[i] = float.MaxValue;
            for (int i = 0; i < drawing_points.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector2 dist2 = four_corners_pos[j] - drawing_points[i];
                    if(dist2.magnitude< four_corners_distnace[j])
                    {
                        four_corners_indices[j] = i;
                        four_corners_distnace[j] = dist2.magnitude;
                    }

                }
            }
            //
            
            int side_length = drawing_points.Count / 4;
            float step = 1.0f / (float)side_length;
            for (int i = 0; i < drawing_points.Count; i++)
            {
                int side_no = (i / side_length) % 4;
                int index_within_side = i % side_length;
                Debug.Log("[ARMath] D1.5 " + i + "  " + side_length + "   " + side_no);
                Vector2 adjust_pos = four_corners_pos[side_no];
                Vector2 diff_vector = four_corners_pos[(side_no + 1) % 4] - adjust_pos;
                adjust_pos = adjust_pos + diff_vector * step * ((float)index_within_side);
                drawing_points[i] = adjust_pos;
            }
            
            Drawing2D.Correct_Rectangle(drawing_name, drawing_points);

            contentRoot.GetComponent<ContentGeometry>().final_object_shape_rect = new Rect(LT.x,  RB.y , RB.x - LT.x, LT.y - RB.y);
            return true;
        }
        return false;
        
    }
}
