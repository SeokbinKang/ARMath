using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryVisRect : MonoBehaviour {


    public GameObject solver;
    public GameObject primitives;
    public GameObject[] vertices;
    public bool[] onVertex;

    public GameObject[] sides;
    public bool[] onSides;

    public GameObject[] angles;
    public bool[] onAngles;
    // Use this for initialization
    public GeometryPrimitives interactive_primitive;

    public List<int> selected_index;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(interactive_primitive == GeometryPrimitives.vertex)
            check_vertices2();
        if (interactive_primitive == GeometryPrimitives.side_horizontal || interactive_primitive == GeometryPrimitives.side_vertical) check_sides2();
        if (interactive_primitive == GeometryPrimitives.angle) check_angles();
        update_visual();
	}

    void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        for(int i = 0; i < onVertex.Length; i++)
        {
            onVertex[i] = false;
        }
        for (int i = 0; i < onSides.Length; i++)
        {
            onSides[i] = false;
        }
        for (int i = 0; i < onAngles.Length; i++)
        {
            onAngles[i] = false;
        }
        interactive_primitive = GeometryPrimitives.none;
        if (selected_index != null) selected_index.Clear();
        else selected_index = new List<int>();
    }
    private void update_visual()
    {
        for (int i = 0; i < onVertex.Length; i++)
        {
            vertices[i].SetActive(onVertex[i]);
        }
        for (int i = 0; i < onSides.Length; i++)
        {
            sides[i].SetActive(onSides[i]);
        }
        for (int i = 0; i < onAngles.Length; i++)
        {
            angles[i].SetActive(onAngles[i]);
        }
    }


    private void feedback_bad(Vector2 pos)
    {

    }
    private void feedback_good(Vector2 pos)
    {

    }


    private void check_vertices2()
    {
        //test touch point
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            bool all_found = true;
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 user_pos = touch.position;
                bool incorrect = true;
                for (int i = 0; i < onVertex.Length; i++)
                {
                    if (onVertex[i]) continue;
                    all_found = false;
                    Vector2 real_pos = vertices[i].GetComponent<RectTransform>().position;
                    Vector2 diff = user_pos - real_pos;
                    if (diff.magnitude <= SystemParam.vertext_proximity)
                    {
                        onVertex[i] = true;
                        FeedbackGenerator.create_sticker_ox_dispose(user_pos, true);
                        incorrect = false;
                        break;
                        //FEEDBACK
                    }
                }
                if(incorrect) FeedbackGenerator.create_sticker_ox_dispose(user_pos, false);


            }
        }
        bool iscomplete = true;
        foreach (bool b in onVertex)
        {
            if (!b) iscomplete = false;
        }
        if (iscomplete && solver.GetComponent<GeometryVirtual_Rect>().mStep < 4) solver.GetComponent<GeometryVirtual_Rect>().nextStep(4);
    }
    private void check_sides2()
    {
        //test touch point
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            bool all_found = true;
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 user_pos = touch.position;
                for (int i = 0; i < onSides.Length; i++)
                {
                    if (onSides[i]) continue;
                    RectTransform rt = sides[i].GetComponent<RectTransform>();
                    float width = rt.rect.width;
                    float height = rt.rect.height;
                    if (width < 50) width = 100;
                    if (height < 50) height = 100;

                    Debug.Log("[ARMath] "+i+" box : " + rt.position + " w: " + width + " h: " + height + "     touch:" + user_pos);
                    if (user_pos.x >= rt.position.x - width/2 && user_pos.x <=rt.position.x+width/2 && user_pos.y >= rt.position.y - height/2 && user_pos.y<=rt.position.y+height/2)
                    {
                        onSides[i] = true;
                        selected_index.Add(i);
                        break;
                        
                        //FEEDBACK
                    }
                }
                if(selected_index.Count==2)
                {
                    if(selected_index[0]%2 != selected_index[1] % 2)
                    {
                        feedback_bad(user_pos);
                        onSides[selected_index[1]] = false;
                        selected_index.RemoveAt(1);
                        FeedbackGenerator.create_sticker_ox_dispose(user_pos, false);

                    } else
                    {
                        FeedbackGenerator.create_sticker_ox_dispose(user_pos, true);
                        feedback_good(user_pos);
                        selected_index.Clear();
                    }
                }


            }
        }
        bool iscomplete = true;
        foreach (bool b in onSides)
        {
            if (!b) iscomplete = false;
        }
        if (iscomplete && solver.GetComponent<GeometryVirtual_Rect>().mStep < 8) solver.GetComponent<GeometryVirtual_Rect>().nextStep(8);
    }


    private void check_vertices()
    {
        GameObject[] interactive_objects = primitives.GetComponent<GridPrimitives>().GetAllPrimitives(GeometryPrimitives.vertex);
        bool iscomplete = true;
        foreach(GameObject go in interactive_objects)
        {
            if (go.activeSelf == false) continue;
            for (int i = 0; i < onVertex.Length; i++)
            {
                if (onVertex[i]) continue;
                Vector2 user_pos = go.GetComponent<RectTransform>().position;
                Vector2 real_pos = vertices[i].GetComponent<RectTransform>().position;
                Vector2 diff = user_pos - real_pos;
                if(diff.magnitude<=SystemParam.vertext_proximity)
                {
                    onVertex[i] = true;
                    go.SetActive(false);
                }
            }
            
        }
        foreach (bool b in onVertex)
        {
            if (!b) iscomplete = false;
        }
        if (iscomplete && solver.GetComponent<GeometryVirtual_Rect>().mStep<4) solver.GetComponent<GeometryVirtual_Rect>().nextStep(4);

    }

    private void check_sides()
    {
        GameObject[] interactive_objects = primitives.GetComponent<GridPrimitives>().GetAllPrimitives(GeometryPrimitives.side_horizontal);
        foreach (GameObject go in interactive_objects)
        {
            Debug.Log("[ARMath]name: " + go.name);
            if (go.activeSelf == false || go.name.IndexOf("correct")<0) continue;
            Vector2 user_pos = go.GetComponent<RectTransform>().position;
            Vector2 real_pos = this.GetComponent<RectTransform>().position;

            if(!onSides[0])
            {
                real_pos = sides[0].GetComponent<RectTransform>().position;
                Debug.Log("[ARMath] user: " + user_pos + " target: " + real_pos);
                Vector2 diff = user_pos - real_pos;
                if (diff.magnitude <= SystemParam.vertext_proximity)
                {
                    onSides[0] = true;
                    onSides[2] = true;
                    go.SetActive(false);
                }
            }

            if (!onSides[2])
            {
                real_pos = sides[2].GetComponent<RectTransform>().position;
                Vector2 diff = user_pos - real_pos;
                if (diff.magnitude <= SystemParam.vertext_proximity)
                {
                    onSides[0] = true;
                    onSides[2] = true;
                    go.SetActive(false);
                }
            }
        }
        if(onSides[0] && onSides[2] && solver.GetComponent<GeometryVirtual_Rect>().mStep<6 ) solver.GetComponent<GeometryVirtual_Rect>().nextStep(6);
        interactive_objects = primitives.GetComponent<GridPrimitives>().GetAllPrimitives(GeometryPrimitives.side_vertical);
        foreach (GameObject go in interactive_objects)
        {
            if (go.activeSelf == false || !go.name.Contains("correct")) continue;
            Vector2 user_pos = go.GetComponent<RectTransform>().position;
            Vector2 real_pos = this.GetComponent<RectTransform>().position;

            if (!onSides[1])
            {
                real_pos = sides[1].GetComponent<RectTransform>().position;
                Vector2 diff = user_pos - real_pos;
                if (diff.magnitude <= SystemParam.vertext_proximity)
                {
                    onSides[1] = true;
                    onSides[3] = true;
                    go.SetActive(false);
                }
            }

            if (!onSides[3])
            {
                real_pos = sides[3].GetComponent<RectTransform>().position;
                Vector2 diff = user_pos - real_pos;
                if (diff.magnitude <= SystemParam.vertext_proximity)
                {
                    onSides[1] = true;
                    onSides[3] = true;
                    go.SetActive(false);
                }
            }
        }

        if (onSides[1] && onSides[3] && solver.GetComponent<GeometryVirtual_Rect>().mStep<8) solver.GetComponent<GeometryVirtual_Rect>().nextStep(8);
    }

    private void check_angles()
    {
        GameObject[] interactive_objects = primitives.GetComponent<GridPrimitives>().GetAllPrimitives(GeometryPrimitives.angle);
        bool iscomplete = true;
        bool shuffle = false;
        foreach (GameObject go in interactive_objects)
        {
            if (go.activeSelf == false || go.name.IndexOf("correct")<0) continue;
            for (int i = 0; i < onAngles.Length; i++)
            {
                if (onAngles[i]) continue;
                Vector2 user_pos = go.GetComponent<RectTransform>().position;
                Vector2 real_pos = angles[i].GetComponent<RectTransform>().position;
                Vector2 diff = user_pos - real_pos;
                if (diff.magnitude <= SystemParam.vertext_proximity)
                {
                    onAngles[i] = true;
                    go.SetActive(false);
                    //shuffle = true;
                }
            }

        }
        foreach (bool b in onAngles)
        {
            if (!b) iscomplete = false;
        }
       
        if (iscomplete && solver.GetComponent<GeometryVirtual_Rect>().mStep < 10) solver.GetComponent<GeometryVirtual_Rect>().nextStep(10);

    }
}
