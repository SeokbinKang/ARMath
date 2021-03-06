﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupTree : MonoBehaviour {
    public GameObject pre_tree;
    public GameObject pre_bag;
    public Texture incomplete_box;
    public Texture complete_box;    

    public bool progressive;
    public List<GameObject> virtual_objs_in_cells;
    public List<Vector2> tangible_objs_in_cells;
    private int progress_active_cell_idx;
    private List<GameObject> cells;
    private TimerCallback mCallback;
    public Color[] charColor;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mCallback != null && mCallback.tick()) mCallback = null;
    }
    void OnEnable()
    {
        //Reset();
    }
    public void Reset()
    {
        if (cells != null)
        {
            foreach (var e in cells)
            {
                Object.Destroy(e);
            }
            cells.Clear();
        }
        progress_active_cell_idx = -1;
        if (virtual_objs_in_cells == null) virtual_objs_in_cells = new List<GameObject>();
        else virtual_objs_in_cells.Clear();
        if (tangible_objs_in_cells == null) tangible_objs_in_cells = new List<Vector2>();
        else tangible_objs_in_cells.Clear();
    }

    public void Setup(int cell_count, float grid_line_alpha)
    {
        if (cell_count <= 0) return;
        Reset();
        if (cells == null) cells = new List<GameObject>();
        // Debug.Log("[ARMath] Setting up cells " + cell_count);
        for (int i = 0; i < cell_count; i++)
        {
            GameObject cell = ARMathUtils.create_2DPrefab(pre_tree, this.gameObject);
            cells.Add(cell);
            cell.GetComponent<RawImage>().texture = incomplete_box;
            cell.GetComponent<RawImage>().color = new Color(1, 1, 1, grid_line_alpha);
            float aspect_ratio = ((float)incomplete_box.width) / ((float)incomplete_box.height);

            //this.GetComponent<GridLayoutGroup>().cellSize = new Vector2(w, h);
            //bool interaction_virtual = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            
            if (progressive)
            {
                cell.SetActive(true);
            }

        }
        progress_active_cell_idx = -1;
        //if (progressive) enableCell(progress_active_cell_idx);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="num_per_cell"></param>
    /// <param name="obj_name"></param>
    /// <returns>num of complete cells</returns>
   
    public int CheckCellsProgressive(int num_per_cell, string obj_name, ProblemType type)
    {
        int res = 0;
        if (progress_active_cell_idx >= cells.Count) return cells.Count;
        GameObject cell = cells[progress_active_cell_idx];
        {
            ObjectContainer cell_container = cell.GetComponent<ObjectContainer>();
            List<SceneObject> objs_in_cell = cell_container.get_objects_in_rect(obj_name);
            int item_in_cell = objs_in_cell.Count;
            foreach (SceneObject o in objs_in_cell)
            {
                if (o.is_feedback_attached()) continue;
                GameObject f = FeedbackGenerator.create_target(o, 0, 90, 5, false);
                o.attach_object(f);
            }
            if (objs_in_cell != null && item_in_cell == num_per_cell)
            {
                string msg = "";

                UpdateCell(cell, true, msg);
                progress_active_cell_idx++;
                if (progress_active_cell_idx < cells.Count) enableCell(progress_active_cell_idx,6);
                foreach (SceneObject so in objs_in_cell)
                {
                    tangible_objs_in_cells.Add(so.get_screen_pos());                    
                }
                // effect
            }
            else
            {

                if (item_in_cell != num_per_cell)
                {
                    if (cell_container.last_count != item_in_cell)
                    {
                        FeedbackGenerator.create_dialog(cell, 0, 4, "Hmm... we need more here", true,false);
                        //TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Hmm.. that's not enough. Can you get " + (num_per_cell - item_in_cell) + " more batteries?");
                        cell_container.last_count = item_in_cell;
                    }

                }
                else
                {
                    if (cell_container.last_count != item_in_cell)
                    {
                        FeedbackGenerator.create_dialog(cell, 0,4, "Hmm... this is too many", true,false);
                        //TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Hmm... that's too many. Can you give me exactly " + num_per_cell + " batteries?");
                        cell_container.last_count = item_in_cell;
                    }
                    
                }
                string msg = "";
                UpdateCell(cell, false, msg);
            }
           

        }
        if (progress_active_cell_idx >= cells.Count) return cells.Count;
        return progress_active_cell_idx;
    }
    public void start_operation()
    {
        progress_active_cell_idx = 0;
        for(int i=0;i<cells.Count;i++)
        {
            if (i == progress_active_cell_idx) cells[i].SetActive(true);
                else cells[i].SetActive(false);
        }

    }
    public List<GameObject> get_virtual_objects_in_cells()
    {
        return this.virtual_objs_in_cells;
    }
    public List<Vector2> get_tangible_objects_in_cells()
    {
        return this.tangible_objs_in_cells;
    }
    public List<GameObject> get_virtual_trees_in_cells()
    {
        List<GameObject> tmp_objs = new List<GameObject>();
        foreach(GameObject c in cells)
        {
            tmp_objs.Add(c.GetComponent<tree>().mask_obj);
        }
        return tmp_objs;
    }
    public int CheckCellProgressive(List<GameObject> batteries, int num_in_cell)
    {
        int res = 0;
        if (progress_active_cell_idx >= cells.Count) return cells.Count;
        GameObject cell = cells[progress_active_cell_idx];
        int item_in_cell = 0;
        List<GameObject> tmp_objs = new List<GameObject>();
        cell.GetComponent<ObjectContainer>().enable_hourGlass(true);
        if (cell.GetComponent<ObjectContainer>().hourglass_wait())
        {
            UpdateCell(cell, false, "");
            return progress_active_cell_idx;
        }
        foreach (GameObject battery in batteries)
        {
            if (battery.GetComponent<DragObject>().onDragging) continue;
            bool inContainer = cell.GetComponent<ObjectContainer>().in_container(battery);
            if (inContainer)
            {
                item_in_cell++;
                tmp_objs.Add(battery);
                if (!battery.GetComponent<DragObject>().is_feedback_attached())
                {
                    //attach feedback                                      
                    GameObject label = FeedbackGenerator.create_target(battery, 0, 600, 1, false);
                    battery.GetComponent<DragObject>().attach_object(label);

                }
            }

        }
        if (item_in_cell == cell.GetComponent<ObjectContainer>().last_count) return progress_active_cell_idx;
        cell.GetComponent<ObjectContainer>().last_count = item_in_cell;
        if (item_in_cell==num_in_cell)
        {
            string msg = "";
            UpdateCell(cell, true, msg);
            cell.GetComponent<ObjectContainer>().enable_hourGlass(false);
            progress_active_cell_idx++;
            if (progress_active_cell_idx < cells.Count) enableCell(progress_active_cell_idx,6);
            // effect
            while (tmp_objs.Count > num_in_cell)
                tmp_objs.RemoveAt(0);
            virtual_objs_in_cells.AddRange(tmp_objs);
        }
        else
        {
            if (item_in_cell < num_in_cell)
            {
                FeedbackGenerator.create_dialog(cell, 0, 4, "Hmm... we need more here", true,false);
                //TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Hmm.. that's not enough. Can you get " + (num_in_cell - item_in_cell) + " more batteries?");


            }
            else
            {
                FeedbackGenerator.create_dialog(cell, 0, 4, "Hmm... this is too many", true,false);
                //TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Hmm... that's too many. Can you give me exactly " + num_in_cell + " batteries?");
            }
            string msg = "";
            UpdateCell(cell, false, msg);
        }


        if (progress_active_cell_idx >= cells.Count) return cells.Count;
        return progress_active_cell_idx;
    }

    
    private void enableCell(int cell_index,float delay)
    {
        if (cell_index < cells.Count)
        {

            //may want to do some animations here.
            mCallback = new TimerCallback(enableCell, cell_index.ToString(), delay);
            
            //cells[cell_index].GetComponent<ObjectContainer>().enable_hourGlass(true);


        }
    }
    public void enableCell(string param)
    {
        int idx = System.Convert.ToInt32(param);
        cells[idx].SetActive(true);
    }
    private void UpdateCell(GameObject cell, bool complete, int num)
    {
        if (complete)
        {
            cell.GetComponent<RawImage>().texture = complete_box;
        }
        else
        {
            cell.GetComponent<RawImage>().texture = incomplete_box;
        }
        cell.GetComponentInChildren<Text>().text = num.ToString();


    }
    private void UpdateCell(GameObject cell, bool complete, string msg)
    {
        if(cell.GetComponentInChildren<Text>()) cell.GetComponentInChildren<Text>().text = msg;
        if (complete)
        {
            cell.GetComponent<RawImage>().texture = complete_box;
            cell.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            //diable dialogue
            cell.GetComponent<tree>().stop_randome_light(1);
            cell.GetComponent<tree>().character_nicejob();
            cell.GetComponent<ObjectContainer>().enable_hourGlass(false);
            //if(cell.GetComponent<cell_speech>()) cell.GetComponent<cell_speech>().finish();
            //show thumb up
        }
        else
        {
            cell.GetComponent<RawImage>().texture = incomplete_box;
            cell.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);

        }


    }
}
