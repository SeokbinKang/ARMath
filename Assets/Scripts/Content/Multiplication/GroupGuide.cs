using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupGuide : MonoBehaviour {
    public GameObject pre_cell;
    public GameObject pre_bag;
    public GameObject pre_box;
    public Texture incomplete_box;
    public Texture complete_box;
    public float max_height;
    public float max_width;

    public bool progressive;
    private int progress_active_cell_idx;
    private List<GameObject> cells;

    public Color[] charColor;
    public List<GameObject> virtual_objs_in_cells;
    public List<SceneObject> tangible_objs_in_cells;
    private int[] cell_correction;
    private string obj_name;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        process_correction_touch();

    }
    void OnEnable()
    {
        //Reset();
    }
    public void Reset()
    {
        if(cells != null)
        {
            foreach(var e in cells)
            {
                Object.Destroy(e);
            }
            cells.Clear();
        }
        progress_active_cell_idx = -1;

        if (virtual_objs_in_cells == null) virtual_objs_in_cells = new List<GameObject>();
        else virtual_objs_in_cells.Clear();
        if (tangible_objs_in_cells == null) tangible_objs_in_cells = new List<SceneObject>();
        else tangible_objs_in_cells.Clear();

    }
    public List<GameObject> get_children()
    {
        return cells;
    }
    public void Setup_giftbox(int cell_count)
    {
        if (cell_count <= 0) return;
        Reset();
        if (cells == null) cells = new List<GameObject>();
        else cells.Clear();
        Debug.Log("[ARMath] Setting up cells " + cell_count);
        for (int i = 0; i < cell_count; i++)
        {
            GameObject cell = ARMathUtils.create_2DPrefab(pre_box, this.gameObject);
            cells.Add(cell);                        
            if (progressive)
            {
                cell.SetActive(false);
            }
        }
        cell_correction = new int[cell_count];
    }
    public int CheckBoxes(string obj_name_, int num_per_cell)
    {
        int res = 0;
        virtual_objs_in_cells.Clear();
        //tangible_objs_in_cells.Clear();
        Debug.Log("[ARMath] Check cells " + cells.Count);
        obj_name = obj_name_;
        for(int i=0;i<cells.Count;i++)
        {
            GameObject cell = cells[i];
            if (cell_correction[i]>0) {
                res++;
                continue;
            }

            List<SceneObject> objs_in_cell = cell.GetComponent<ObjectContainer>().get_objects_in_rect(obj_name_);
            if (objs_in_cell != null && (objs_in_cell.Count == num_per_cell))
            {
                res++;
                UpdateBoxAnim(cell, true, num_per_cell);
                //cell.GetComponent<Animator>().SetTrigger("close");
                //may want to put a message here.
                tangible_objs_in_cells.AddRange(objs_in_cell);
                cell_correction[i]++;
            } else  {
                int k;
                if (objs_in_cell == null) k = 0;
                else k = objs_in_cell.Count;
                //UpdateBoxAnim(cell, false, k);
            }
        }
        

        return res;
    }
    private void process_correction_touch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 user_pos = touch.position;
                
                for (int i = 0; i < cells.Count; i++)
                {
                    bool hit = ARMathUtils.check_in_recttransform(user_pos, cells[i]);
                    if (hit)
                    {
                        user_pos.y = Screen.height - user_pos.y;
                        CatalogItem ci = new CatalogItem();
                        ci.Box = new Rect(user_pos, new Vector2(80, 80));
                        ci.DisplayName = obj_name;
                        SceneObjectManager.add_new_object(ci, 180);

//                        cell_correction[i]++;
                    }
                }
                
              
             
            }

        }
    }
    public List<GameObject> get_virtual_objects_in_cells()
    {
        return this.virtual_objs_in_cells;
    }
    public List<SceneObject> get_tangible_objects_in_cells()
    {
        return this.tangible_objs_in_cells;
    }
    private void scaffold_box()
    {
        //check incomplete boxes and indicate an interaction.
    }
    public void Setup(int cell_count)
    {
        if (cell_count <= 0) return;
        Reset();
        if (cells == null) cells = new List<GameObject>();
       // Debug.Log("[ARMath] Setting up cells " + cell_count);
        for(int i = 0; i < cell_count; i++)
        {
            GameObject cell = ARMathUtils.create_2DPrefab(pre_cell, this.gameObject);
            cells.Add(cell);
            cell.GetComponent<RawImage>().texture = incomplete_box;
            cell.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            float aspect_ratio = ((float)incomplete_box.width) / ((float)incomplete_box.height);
            float w, h;
            if(aspect_ratio<=1)
            {
                h = this.max_height;
                w = h * aspect_ratio;
                
            } else
            {
                w = this.max_width;                
                h = w/aspect_ratio;
            }
            //this.GetComponent<GridLayoutGroup>().cellSize = new Vector2(w, h);

            if(progressive)
            {
                cell.SetActive(false);
            }

        }
        progress_active_cell_idx = 0;
        if (progressive) enableCell(progress_active_cell_idx);
        
    }
    public void Setup(int cell_count,float grid_line_alpha)
    {
        if (cell_count <= 0) return;
        Reset();
        if (cells == null) cells = new List<GameObject>();
        // Debug.Log("[ARMath] Setting up cells " + cell_count);
        for (int i = 0; i < cell_count; i++)
        {
            GameObject cell = ARMathUtils.create_2DPrefab(pre_cell, this.gameObject);
            cells.Add(cell);
            cell.GetComponent<RawImage>().texture = incomplete_box;
            cell.GetComponent<RawImage>().color = new Color(1, 1, 1, grid_line_alpha);
            float aspect_ratio = ((float)incomplete_box.width) / ((float)incomplete_box.height);
            float w, h;
            if (aspect_ratio <= 1)
            {
                h = this.max_height;
                w = h * aspect_ratio;

            }
            else
            {
                w = this.max_width;
                h = w / aspect_ratio;
            }
            //this.GetComponent<GridLayoutGroup>().cellSize = new Vector2(w, h);

            if (progressive)
            {
                cell.SetActive(false);
            }

        }
        progress_active_cell_idx = 0;
        if (progressive) enableCell(progress_active_cell_idx);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="num_per_cell"></param>
    /// <param name="obj_name"></param>
    /// <returns>num of complete cells</returns>
    public int CheckCells(int num_per_cell, string obj_name)
    {
        int res = 0;
        foreach(GameObject cell in cells)
        {
            List<SceneObject> objs_in_cell = cell.GetComponent<ObjectContainer>().get_objects_in_rect(obj_name);
            if (objs_in_cell != null && objs_in_cell.Count == num_per_cell)
            {
                res++;
                UpdateCell(cell, true, num_per_cell);
            } else
            {
                int k;
                if (objs_in_cell == null) k = 0;
                else k = objs_in_cell.Count;
                UpdateCell(cell, false, k);
            }

        }

        return res;
    }
    public int CheckCellsProgressive(int num_per_cell, string obj_name, ProblemType type)
    {
        int res = 0;
        if (progress_active_cell_idx >= cells.Count) return cells.Count;
        GameObject cell = cells[progress_active_cell_idx];
        {
            List<SceneObject> objs_in_cell = cell.GetComponent<ObjectContainer>().get_objects_in_rect(obj_name);
            if (objs_in_cell != null && objs_in_cell.Count >= num_per_cell)
            {
                string msg = "";
               
                UpdateCell(cell, true, msg);
                progress_active_cell_idx++;
                if (progress_active_cell_idx < cells.Count) enableCell(progress_active_cell_idx);
                
                // effect
            }
            else
            {
                string msg = "";
                int k;
                if (type == ProblemType.p3_multiplication)
                {
                    msg = "Give me \n" + num_per_cell + " " + obj_name + "s";
                }
                if (objs_in_cell == null) k = 0;
                else k = objs_in_cell.Count;
                UpdateCell(cell, false, msg);
            }

        }
        if (progress_active_cell_idx >= cells.Count) return cells.Count;
        return progress_active_cell_idx;
    }
    private void enableCell(int cell_index)
    {
        if (cell_index < cells.Count) {
            
            //may want to do some animations here.
            if (charColor != null)
            {
                int color_idx = Mathf.Min(cell_index, charColor.Length - 1);
                Animator animc = cells[cell_index].transform.GetComponentInChildren<Animator>(true);
                if(animc!=null)
                {
                    //character
                    if (animc.gameObject.GetComponent<Image>() != null)
                    {
                        Debug.Log("[ARMath] character color !@#@!#!@#!#!@");
                        animc.gameObject.GetComponent<Image>().color = charColor[color_idx];
                    }
                }
                
            }
            cells[cell_index].SetActive(true);
        }
    }
    private void UpdateBoxAnim(GameObject cell, bool complete, int num)
    {
        if (complete)
        {
            cell.GetComponent<Animator>().SetTrigger("close");
        }
        else
        {
            cell.GetComponent<Animator>().SetTrigger("open");
        }
        Text t_label = cell.GetComponentInChildren<Text>();
        if(t_label!=null) t_label.text = num.ToString();


    }
    private void UpdateCell(GameObject cell, bool complete, int num)
    {
        if (complete)
        {
            cell.GetComponent<RawImage>().texture = complete_box;
        } else
        {
            cell.GetComponent<RawImage>().texture = incomplete_box;
        }
        cell.GetComponentInChildren<Text>().text = num.ToString();


    }
    private void UpdateCell(GameObject cell, bool complete, string msg)
    {
        cell.GetComponentInChildren<Text>().text = msg;
        if (complete)
        {
            cell.GetComponent<RawImage>().texture = complete_box;
            //diable dialogue
            cell.GetComponent<cell_speech>().finish();            
            //show thumb up
        }
        else
        {
            cell.GetComponent<RawImage>().texture = incomplete_box;

        }    


    }


}
