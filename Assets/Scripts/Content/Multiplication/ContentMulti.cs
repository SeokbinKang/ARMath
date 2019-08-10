using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentMulti : MonoBehaviour {

    public GameObject sub_intro;
    public GameObject sub_intro2;
    public GameObject sub_trees;
    public GameObject sub_explorer;
    
    public GameObject sub_solver;    
    


    public string target_object_name;    
    public Rect target_object_cluster;
    public int target_base_num;
    public int target_mult_num;
    

    


    private bool is_idle = true;
    private bool is_solved = false;

    private float nextActionTime = 0.5f;
    public List<Vector2> obj_pos_list;
    public Rect obj_rect;
    void Start()
    {
        obj_pos_list = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
          //  s1_objectfound();
        }
    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        
        if(sub_intro) sub_intro.SetActive(true);
        if (sub_intro2) sub_intro2.SetActive(false);
        if (sub_trees) sub_trees.SetActive(false);
        if (sub_explorer) sub_explorer.SetActive(false);
        if(sub_solver) sub_solver.SetActive(false);
        
        is_idle = true;
        is_solved = false;
        target_base_num = 0;
        obj_pos_list.Clear();
        target_object_cluster = new Rect();
        Dialogs.set_topboard_color(0, 0);
        Dialogs.set_topboard_color(1, 5);
        
    }
    private void OnDisable()
    {
        Dialogs.set_topboard_color(0, 0);
        Dialogs.set_topboard_color(1, 1);

    }

    public void onSolved()
    {
        sub_solver.SetActive(false);
        
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p4_geometry);
        is_solved = true;

    }
    public void s0_onIntro()
    {
        target_mult_num = (int)UnityEngine.Random.Range(2, 5);
        sub_intro2.SetActive(true);
        sub_trees.GetComponent<GroupTree>().Setup(target_mult_num,0);
        sub_trees.SetActive(true);
        
    }
    public void s1_initFinder()
    {
        Tools.finder_init("battery", 1, new CallbackFunction2(s1_objectfound), "", "Let's find some batteries!", 0.8f);
     //   Tools.finder_init("battery", 3, new CallbackFunction(s1_objectfound), "");
    }

    //invoke when there are battery objects
    public void s1_objectfound(string p, List<SceneObject> obj_list, Rect rt)
    {
        System.Random random = new System.Random();
        int n_batteries_found = System.Convert.ToInt32(p);
      
        Vector2 center_of_objects = new Vector2(0, 0);
        obj_pos_list.Clear();
        obj_rect = rt;

        if (is_idle)
        {
            //target_object_name = objects_cluster[0].catalogInfo.DisplayName;
            target_base_num = Math.Min(n_batteries_found,4);
            Rect cluster_box= new Rect(new Vector2(500, 450), new Vector2(600, 600));
            center_of_objects = cluster_box.center;
            target_object_cluster = cluster_box;
            is_idle = false;
            //pops up explorer

            float target_delay = 4;
            for(int i = 0; i < sub_trees.transform.childCount; i++) {

                GameObject tree = sub_trees.transform.GetChild(i).gameObject;                
                Vector3 targetPos = tree.GetComponent<RectTransform>().position;
                targetPos.x += 150;
                targetPos.y += 150;
                FeedbackGenerator.create_target(targetPos, target_delay, 5, 0);                
                target_delay += 0.3f;
            }

            target_delay = 10;
            for(int i=0;i<target_base_num && i<obj_list.Count;i++)
            {
                FeedbackGenerator.create_target(obj_list[i].get_screen_pos(), target_delay, 5, 5);
                target_delay += 0.2f;
                obj_pos_list.Add(obj_list[i].get_screen_pos());
            }

            
            callback_shownumber1(target_mult_num.ToString());
            callback_shownumber2("x " + target_base_num.ToString());
              Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "Oh thank you. We need to light up "+ target_mult_num+" trees using the batteries. ",
              true,
              null,
              "",
              9), 0
              );
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "And each tree needs "+ target_base_num+" batteries to turn on. ",
              true,
              null,
              "",
              5f), 0
              );
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "Can you help turn on the trees?",
              true,
              new CallbackFunction(s4_startsolver),
              "",
              5f), 0
              );



        }
    }
   
    public void callback_shownumber1(string t)
    {
        Dialogs.set_topboard_animated( 0, t,2.5f);

    }
    public void callback_shownumber2(string t)
    {

        Dialogs.set_topboard_animated( 1, t,10);
        //s4_startsolver("");
    }
    public void s2_OnExplorer()
    {
        SetIdle(false);
       
     //   target_mult_num = (int)Random.Range(2, 4);
        /*Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Oh! My friend minions are coming over to collect "+ target_object_name +"s. Can you help us?",
                 true,
                new CallbackFunction(s4_startsolver),
                ""
                ));*/

        s4_startsolver("");




    }    
    
    public void s4_startsolver(string p)
    {        
        sub_trees.GetComponent<GroupTree>().start_operation();
        sub_solver.SetActive(true);
    }
    
    public void SetIdle(bool t)
    {
        is_idle = t;
    }
}
