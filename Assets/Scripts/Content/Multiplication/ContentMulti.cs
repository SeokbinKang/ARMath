using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentMulti : MonoBehaviour {

    public GameObject sub_intro;
    public GameObject sub_explorer;        
    public GameObject sub_solver;    
    public GameObject sub_review;


    public string target_object_name = "";    
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
            s1_UpdateExplorer();
        }
    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        
        if(sub_intro) sub_intro.SetActive(true);
        if(sub_explorer) sub_explorer.SetActive(false);
        if(sub_solver) sub_solver.SetActive(false);
        if(sub_review) sub_review.SetActive(false);        
        is_idle = true;
        is_solved = false;
        target_base_num = 0;
        target_object_name = "";
        target_object_cluster = new Rect();
        
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help the minion solve multiplication problems and collect purple gems!");
    }
    public void onSolved()
    {
        sub_solver.SetActive(false);
        
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p4_geometry);
        is_solved = true;

    }
    public void s1_UpdateExplorer()
    {
        System.Random random = new System.Random();

        string dominant_object_name = "";
        Vector2 center_of_objects = new Vector2(0, 0);
        int object_count = 0;

        if (is_solved || sub_intro.activeSelf)
        {
            sub_explorer.SetActive(false);
            return;
        }
        
        List<SceneObject> objects_cluster;
        objects_cluster = SceneObjectManager.mSOManager.get_a_cluster_objects();
        if (is_idle)
        {
            if (objects_cluster == null || objects_cluster.Count == 0)
            {
                sub_explorer.SetActive(false);
                return;
            }
            target_object_name = objects_cluster[0].catalogInfo.DisplayName;
            target_base_num = objects_cluster.Count;
            Rect cluster_box = SceneAnalysis.get_bounding_box(objects_cluster);
            center_of_objects = cluster_box.center;
            target_object_cluster = cluster_box;
            
           
            bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            if (interaction_touch_enalbed)
            {




            }
            else
            {

            }
            //pops up explorer
            sub_explorer.SetActive(true);
            RectTransform r = sub_explorer.GetComponent<RectTransform>();
            r.position = new Vector3(center_of_objects.x, center_of_objects.y, 0);

            //pops up debugging bounding box.
            /*
            Vector2[] corners = new Vector2[4];
            corners[0].x = target_object_cluster.xMin - Screen.width/2;
            corners[0].y = target_object_cluster.yMax - Screen.height/2;
            corners[1].x = target_object_cluster.xMax - Screen.width / 2;
            corners[1].y = target_object_cluster.yMax - Screen.height / 2;
            corners[2].x = target_object_cluster.xMax - Screen.width / 2;
            corners[2].y = target_object_cluster.yMin - Screen.height / 2;
            corners[3].x = target_object_cluster.xMin - Screen.width / 2;
            corners[3].y = target_object_cluster.yMin - Screen.height / 2;
            Color c = Color.cyan;
            c.a = 0.4f;
            Debug.Log("[ARMath] cluster : " + target_object_cluster);
            Drawing2D.create_2DRect("debuggingbox", corners, c);*/

        }
        else
        {
            sub_explorer.SetActive(false);

        }
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
        sub_solver.SetActive(true);
    }
    
    public void SetIdle(bool t)
    {
        is_idle = t;
    }
}
