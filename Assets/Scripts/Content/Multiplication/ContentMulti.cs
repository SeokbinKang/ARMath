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
    
    void Start()
    {
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
        
        target_object_cluster = new Rect();
        Dialogs.set_topboard_color(0, 0);
        Dialogs.set_topboard_color(1, 5);
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help the minion solve multiplication problems and collect purple gems!");
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
        target_mult_num = (int)Random.Range(2, 5);
        sub_intro2.SetActive(true);
        sub_trees.GetComponent<GroupTree>().Setup(target_mult_num,0);
        sub_trees.SetActive(true);
        
    }
    public void s1_initFinder()
    {
        Tools.finder_init("battery", 3, new CallbackFunction(s1_objectfound), "");
    }

    //invoke when there are battery objects
    public void s1_objectfound(string p)
    {
        System.Random random = new System.Random();
        int n_batteries_found = System.Convert.ToInt32(p);
      
        Vector2 center_of_objects = new Vector2(0, 0);
      
        List<SceneObject> objects_cluster;
      
        if (is_idle)
        {
            //target_object_name = objects_cluster[0].catalogInfo.DisplayName;
            target_base_num = n_batteries_found;
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
                target_delay += 0.4f;
            }

            bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            
              Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "Oh thank you. We need to light up "+ target_mult_num+" trees using the batteries. ",
              true,
              new CallbackFunction(callback_shownumber1),
             target_mult_num.ToString(),
              7), 0
              );
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "And each tree needs "+ target_base_num+" batteries to turn on. ",
              true,
              new CallbackFunction(callback_shownumber2),
              "x " + target_base_num.ToString(),
              4.5f), 0
              );
          


        }
    }
    //public void s1_objectfound_DEPRECATED(string p)
    //{
    //    System.Random random = new System.Random();
    //    int n_batteries_found = System.Convert.ToInt32(p);
    //    string dominant_object_name = "";
    //    Vector2 center_of_objects = new Vector2(0, 0);
    //    int object_count = 0;

    //    if (is_solved || sub_intro.activeSelf || sub_intro2.activeSelf)
    //    {
    //        sub_explorer.SetActive(false);
    //        return;
    //    }

    //    List<SceneObject> objects_cluster;
    //    objects_cluster = SceneObjectManager.mSOManager.get_a_cluster_objects(target_object_name);
    //    if (is_idle)
    //    {
    //        if (objects_cluster == null || objects_cluster.Count == 0)
    //        {
    //            sub_explorer.SetActive(false);
    //            return;
    //        }
    //        //target_object_name = objects_cluster[0].catalogInfo.DisplayName;
    //        target_base_num = Mathf.Max(objects_cluster.Count, 2);
    //        Rect cluster_box = SceneAnalysis.get_bounding_box(objects_cluster);
    //        center_of_objects = cluster_box.center;
    //        target_object_cluster = cluster_box;

    //        //pops up explorer
    //        sub_explorer.SetActive(true);
    //        RectTransform r = sub_explorer.GetComponent<RectTransform>();
    //        r.position = new Vector3(center_of_objects.x, center_of_objects.y, 0);
    //        is_idle = false;


    //        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();

    //        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
    //        "Oh there are some batteries. We need to light up " + target_mult_num + " trees using the batteries. ",
    //        true,
    //        new CallbackFunction(callback_shownumber1),
    //       target_mult_num.ToString(),
    //        7), 0
    //        );
    //        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
    //          "And each tree needs " + target_base_num + " batteries to turn on. ",
    //          true,
    //          new CallbackFunction(callback_shownumber2),
    //          "x " + target_base_num.ToString(),
    //          4.5f), 0
    //          );



    //    }
    //    else
    //    {
    //        sub_explorer.SetActive(false);

    //    }
    
    public void callback_shownumber1(string t)
    {
        Dialogs.set_topboard_animated(true, 0, t);

    }
    public void callback_shownumber2(string t)
    {

        Dialogs.set_topboard_animated(true, 1, t);
        s4_startsolver("");
    }
    public void s2_OnExplorer()
    {
        SetIdle(false);
       
        target_mult_num = (int)Random.Range(2, 4);
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
