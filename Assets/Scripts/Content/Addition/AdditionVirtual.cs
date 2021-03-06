﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionVirtual : MonoBehaviour
{

   

    


    public GameObject container;
    public GameObject root_movables;
    public GameObject region;
    public GameObject ContentModuleRoot;

    public GameObject[] movable_objects;
    public GameObject item_mask;
    public GameObject wallet;
    public GameObject[] piggybank;
    private List<GameObject> tap_list;
    

    public bool UserInteracting;
    
    private int prev_n;
    private string target_object_name;
    // Use this for initialization
    
    private float nextActionTime = 0.0f;


    void Start()
    {
        
    }
    void OnEnable()
    {
        Debug.Log("[ARMath] this should not start");
        Reset();
        loadPrompt();
    }
    private void Reset()
    {
        region.GetComponent<RegionControl>().getRegion(1).GetComponent<ObjectContainer>().enable_hourGlass(true);
        container.SetActive(false);
        root_movables.SetActive(false);        
        UserInteracting = false;
        prev_n = 0;
        tap_list = new List<GameObject>();        
       // arrange_movable_objects();

    }
    private void setup_coins()
    {
        if (root_movables.transform.childCount < 1) return;
        movable_objects = new GameObject[root_movables.transform.childCount];
        System.Random random = new System.Random();
        for (int i = 0; i < root_movables.transform.childCount; i++)
        {
            movable_objects[i] = root_movables.transform.GetChild(i).gameObject;
            movable_objects[i].SetActive(true);
            movable_objects[i].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);            
            RectTransform r = movable_objects[i].GetComponent<RectTransform>();            
            r.Rotate(0, 0, random.Next(0, 360));
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (UserInteracting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            //  UpdateInteractiveObjects();
            count_object_fix_realones();
        }
    }
    //private void arrange_movable_objects()
    //{
    //    target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
     
    //    GameObject icon_obj = AssetManager.get_icon(target_object_name);
    //    System.Random random = new System.Random();
    //    float w = icon_obj.GetComponent<RawImage>().texture.width;
    //    float h = icon_obj.GetComponent<RawImage>().texture.height;
    //    h = h * 150 / w;
    //    w = 150;
    //    foreach (GameObject o in movable_objects)
    //    {
    //        o.GetComponent<RawImage>().texture = icon_obj.GetComponent<RawImage>().texture;
    //        RectTransform rt = wallet.GetComponent<RectTransform>();            
    //        Vector3 targetPos = new Vector3(rt.position.x + random.Next(-80, 100), rt.position.y +random.Next(-50, 50), 0);
    //        RectTransform r = o.GetComponent<RectTransform>();
    //        r.position = targetPos;
    //        r.sizeDelta = new Vector2(w, h);
    //        r.Rotate(0, 0, random.Next(0, 360));
    //        o.GetComponent<DragObject>().SetAlphaAdjustment(true, 0.6f, 0.85f, 1);
    //    }
    //}

    private void count_object_fix_realones()
    {
        if (!UserInteracting) return;

        GameObject region_box = region.GetComponent<RegionControl>().getRegion(1);
        if (region_box.GetComponent<ObjectContainer>().hourglass_wait()) return;


        int virtual_n = 0;
        int cur_n = 0;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        //List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        


        bool[] number_label = new bool[init_n];
        
        virtual_n = 0;
        //check virtual objects in the bag
        GameObject container = region.GetComponent<RegionControl>().getRegion(1);
        List<GameObject> objs_in_container = new List<GameObject>();
        foreach (GameObject o in movable_objects)
        {
            bool contained = container.GetComponent<ObjectContainer>().in_container(o);
            if (contained)
            {
                virtual_n++;
                objs_in_container.Add(o);
                if (!o.GetComponent<DragObject>().is_feedback_attached())
                {
                    //attach feedback                                      
                      GameObject label = FeedbackGenerator.create_target(o, 0, 600, 1,false);
                      o.GetComponent<DragObject>().attach_object(label);
                    
                }
            }
        }

        cur_n = virtual_n;
        ContentModuleRoot.GetComponent<ContentAddition>().current_object_count = init_n + cur_n;
        if (prev_n != cur_n)            
        {
            float percent = ((float)(cur_n + init_n)) / (float)goal_n;
           // Debug.Log("[ARMath] Icecream percent = " + percent + "  " + cur_n + "  " + init_n + "  /  " + goal_n);
            item_mask.GetComponent<vertical_mask>().set_visible_percent(percent);
            prev_n = cur_n;
            OnCount(objs_in_container);
        }
    }
    
    public List<GameObject> get_movables_in_container()
    {
        List<GameObject> ret = new List<GameObject>();
        foreach (GameObject o in movable_objects)
        {
            bool contained = container.GetComponent<ObjectContainer>().in_container(o);
            if (contained) ret.Add(o);
        }
        return ret;
    }
    public void OnCount(List<GameObject> l_so)
    {
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int add_n = goal_n - init_n;
        int gap = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count - goal_n;


        if (gap == 0)
        {
            UserInteracting = false;
            List<Vector2> obj_pos_list = ContentModuleRoot.GetComponent<ContentAddition>().obj_pos_list;
            foreach(var o in movable_objects)
            {
                o.SetActive(false);
            }
            foreach (GameObject so in l_so)
            {
                obj_pos_list.Add(so.GetComponent<RectTransform>().position);
                so.SetActive(true);
                //so.extend_life(10);
            }
            GameObject region_box = region.GetComponent<RegionControl>().getRegion(1);
            region_box.GetComponent<ObjectContainer>().enable_hourGlass(false);
            this.transform.parent.GetComponent<ContentSolver>().start_review();

            return;
        }
        if (gap > 0) TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Hmm... that's too many. Can you give me exactly " + add_n + " coins?");
        else TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Hmm.. I need more. Can you get " + Mathf.Abs(gap) + " more coins?");
        /*
        foreach (GameObject so in l_so)
        {
            so.GetComponent<DragObject>().clear_all_feedback();
        }*/

            //UpdateBoard();
            //sound effect


        }
    
    public void OnCount()
    {

        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        
        
        //sound effect
        //Debug.Log("[ARMath] result " + ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count + "  =? " + ContentModuleRoot.GetComponent<ContentAddition>().current_object_count);
        if (prev_n == ContentModuleRoot.GetComponent<ContentAddition>().add_object_count)
        {
            UserInteracting = false;

            //diable interactive obejcts
            foreach(var o in piggybank)
            {
                o.SetActive(false);
            }
            foreach (GameObject o in movable_objects)
            {
                bool contained = container.GetComponent<ObjectContainer>().in_container(o);
                if (!contained) o.SetActive(false);
            }
            this.transform.parent.GetComponent<ContentSolver>().start_review();
        }
    }
   
    public void loadPrompt()
    {

        //prompt_text.SetActive(true);
        target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int cur_n = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count;
        int add_n = goal_n - init_n;
       
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "You can get coins out of the piggy bank by moving them on the screen. ",
            true,
            new CallbackFunction(StartOperation),
            "",
            5), 0
            );
       
        UserInteracting = false;
    }
    public void StartOperation(string o)
    {
        prev_n = 0;
        UserInteracting = true;
        
        UpdateBoard();        
        container.SetActive(true);        
        root_movables.SetActive(true);
        setup_coins();
        //arrange_movable_objects();
    }

    private void UpdateBoard()
    {
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int cur_n = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count;
        int add_n = ContentModuleRoot.GetComponent<ContentAddition>().add_object_count;
        string sign = " + ";
        if (cur_n - init_n < 0) sign = " - ";

       // item_mask.GetComponent<vertical_mask>().set_visible_percent(((float)cur_n / (float)goal_n));
        // board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
        //Dialogs.set_topboard(true, init_n + " (" + target_object_name + "s) + " + add_n + " (" + target_object_name + "s) = ?");

    }
    private void clearinteractiveobjects()
    {
        for (int k = 0; k < tap_list.Count; k++)
        {
            GameObject.Destroy(tap_list[k]);
        }
        tap_list.RemoveAll(s => s == null);
    }





}
