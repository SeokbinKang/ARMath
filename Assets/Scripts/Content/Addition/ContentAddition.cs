﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentAddition : MonoBehaviour, IContentModule
{

    public GameObject sub_intro;
    public GameObject sub_explorer;
    public GameObject sub_truck;
    
    public GameObject sub_solver;

    public GameObject sub_region;
    

    public GameObject virtual_container;
    public string target_object_name = "";
    public int found_object_count = 0;

    public int init_object_count = 0;
    public int add_object_count = 0;
    public int goal_object_count = 0;
    public int current_object_count = 0;
    private bool is_idle = true;
    private bool is_solved = false;

    private float nextActionTime = 0.0f;

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
            //UpdateExplorer();
            s1_truck_and_opener();
        }
    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        sub_intro.SetActive(true);
        sub_explorer.SetActive(false);
        sub_truck.SetActive(false);
        sub_solver.SetActive(false);

        sub_region.GetComponent<RegionControl>().Reset();
        is_idle = true;
        is_solved = false;
        init_object_count = 0;
        goal_object_count = 0;
        current_object_count = 0;
        SceneObjectManager.mSOManager.Reset();
        Dialogs.set_topboard_color(0, 0);
        Dialogs.set_topboard_color(1, 1);
        //        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help the minion solve addition problems and collect green gems!");
    }
    public void onSolved()
    {
        sub_solver.SetActive(false);
        //sub_ceremony.SetActive(true);
        
        /*Dialogs.add_dialog(new DialogItem(DialogueType.right_pop,
                        "Nice job!",
                         true,
                        new CallbackFunction(ceremony),
                        "",
                        3f
                        ),3);*/
        //TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Nice Job! The answer is " + ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count + "!");
        ceremony("");
         is_solved = true;
      
    }
    public void ceremony(string o)
    {
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("I've got a gift for you!");
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p2_addition);
    }
    public void s1_search_objects(string t)
    {
        if (is_solved)
        {
            return;
        }
        sub_explorer.SetActive(false);
        target_object_name = "coin";
        Tools.finder_init(target_object_name, 3, new CallbackFunction2(s2_objectfound), "", "Let's find some coins!", 0.8f);

    }
    public void s2_objectfound(string p, List<SceneObject> obj_list, Rect rt)
    {
        System.Random random = new System.Random();
        if (is_solved )
        {            
            return;
        }
        SetIdle(false);
        
        init_object_count = obj_list.Count;
        goal_object_count = init_object_count + random.Next(2, 6);
        add_object_count = goal_object_count - init_object_count;
        float target_delay = 4f;
        int i = 1;
        foreach (SceneObject so in obj_list)
        {
            Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
            FeedbackGenerator.create_target(targetPos, target_delay, 6, 0);
            GameObject num_label = FeedbackGenerator.create_number_feedback(targetPos, i++,target_delay,6);
            //so.attach_object(num_label);
            target_delay += 0.4f;
        }
        
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "I've found a few coins on the table. Let's count them first",
            true,
            new CallbackFunction(callback_shownumber1),
            init_object_count.ToString(),
            7), 0
            );

        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
           "There are " + init_object_count + " coins, but it's not enough to buy an ice cream.",
           true,
           null,
           "+ " + add_object_count,
           4), 0
           );
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
           "Can you get me " + add_object_count + " more " + target_object_name + "s? You can place them in the green box",
           true,
           new CallbackFunction(callback_shownumber2),
           "+ " + add_object_count,
           4), 0
           );

        //set region indicator
        rt.size = rt.size * 0.8f;
        Vector2 r2_pos = rt.position;
        
        Vector2 r2_size = rt.size;
        sub_region.GetComponent<RegionControl>().setRegion(0, rt,true);
        
        r2_pos.x += rt.size.x + 50;
        r2_size.x *= 0.6f;
        r2_size.y *= 1f;
        rt.position = r2_pos;
        rt.size = r2_size;
        sub_region.GetComponent<RegionControl>().setRegion(1, rt,true);  // need some delay
    }
    
    public void callback_shownumber1(string t)
    {
        Dialogs.set_topboard_animated(true, 0, t);

    }
    public void callback_shownumber2(string t)
    {
        
        Dialogs.set_topboard_animated(true, 1, t);
        sub_solver.SetActive(true);
    }
   
   
    
    public void SetIdle(bool t)
    {
        is_idle = t;
    }
    public void s1_truck_and_opener()
    {
        if (is_solved || sub_intro.activeSelf || !is_idle || sub_explorer.activeSelf)
        {
            return;
          
           
        }
        if (!sub_truck.activeSelf)
        {
            sub_truck.SetActive(true);
            return;
        }
        if (!sub_truck.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish"))
        {
            return;
        }
        sub_explorer.SetActive(true);
        SetIdle(false);
    }
    public void UpdateExplorer()
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
        SceneObjectManager.mSOManager.get_dominant_object(ref dominant_object_name, ref center_of_objects, ref object_count);
        if (is_idle)
        {
            if (dominant_object_name == "" || !dominant_object_name.Contains("coin"))
            {
                sub_explorer.SetActive(false);
                return;
            }
            if (!sub_truck.activeSelf)
            {
                sub_truck.SetActive(true);
                return;
            }
            if (!sub_truck.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish"))
            {
                return;
            }

            target_object_name = dominant_object_name;
            found_object_count = object_count;

            bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            if (interaction_touch_enalbed)
            {
                // List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_on_the_left(target_object_name);
                List<SceneObject> objs = virtual_container.GetComponent<ObjectContainer>().get_objects_in_rect(target_object_name);
                init_object_count = objs.Count;
                goal_object_count = objs.Count + random.Next(2, 6);
                add_object_count = goal_object_count - init_object_count;

            }
            else
            {
                init_object_count = object_count;
                goal_object_count = init_object_count + random.Next(2, 6);
                add_object_count = goal_object_count - init_object_count;
            }
            //pops up explorer
            sub_explorer.SetActive(true);
            RectTransform r = sub_explorer.GetComponent<RectTransform>();
            r.position = new Vector3(center_of_objects.x, Screen.height - center_of_objects.y, 0);


        }
        else
        {
            sub_explorer.SetActive(false);

        }
    }
    public void OnOpener()
    {
        Debug.Log("clieck");
        SetIdle(false);
        List<SceneObject> init_objs = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        float target_delay = 4f;
        int i = 1;
        foreach (SceneObject so in init_objs)
        {
            Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
            FeedbackGenerator.create_target(targetPos, target_delay, 6, 0);
            GameObject num_label = FeedbackGenerator.create_number_feedback(targetPos, i++, target_delay, 6);
            so.attach_object(num_label);
            target_delay += 0.4f;
        }

        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "I've found a few coins on the table. Let's count them first",
            true,
            new CallbackFunction(callback_shownumber1),
            init_object_count.ToString(),
            7), 0
            );

        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
           "There are " + init_object_count + " coins, but it's not enough to buy an ice cream.",
           true,
           null,
           "+ " + add_object_count,
           4), 0
           );
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
           "Can you get me " + add_object_count + " more  " + target_object_name + "s?",
           true,
           new CallbackFunction(callback_shownumber2),
           "+ " + add_object_count,
           4), 0
           );
        /*
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "Then, how may coins do we need in total to buy an icecream?",
            true,
            new CallbackFunction(initSolver),
            "= ?",
            4
            ), 0
            );*/
    }
}
