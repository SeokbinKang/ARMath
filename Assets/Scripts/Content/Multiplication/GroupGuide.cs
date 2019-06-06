using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupGuide : MonoBehaviour {
    public GameObject pre_cell;
    public GameObject pre_bag;
    public Texture incomplete_box;
    public Texture complete_box;
    public float max_height;
    public float max_width;

    private List<GameObject> cells;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    }
    public void Setup()
    {
        
    }
    public void Setup(int cell_count)
    {
        if (cell_count <= 0) return;
        Reset();
        if (cells == null) cells = new List<GameObject>();
        Debug.Log("[ARMath] Setting up cells " + cell_count);
        for(int i = 0; i < cell_count; i++)
        {
            GameObject cell = ARMathUtils.create_2DPrefab(pre_cell, this.gameObject);
            cells.Add(cell);
            cell.GetComponent<RawImage>().texture = incomplete_box;
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
            this.GetComponent<GridLayoutGroup>().cellSize = new Vector2(w, h);
        }
        
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


}
