using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentAddition : MonoBehaviour, IContentModule
{

    public GameObject sub_intro;
    public GameObject sub_explorer;
    public GameObject sub_opener;
    public GameObject sub_helper;
    public GameObject sub_solver;

    public GameObject sub_ceremony;
    public GameObject sub_review;

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
            UpdateExplorer();
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
        sub_opener.SetActive(false);
        sub_solver.SetActive(false);
        sub_review.SetActive(false);
        sub_ceremony.SetActive(false);
        is_idle = true;
        is_solved = false;
        init_object_count = 0;
        goal_object_count = 0;
        current_object_count = 0;
        SceneObjectManager.mSOManager.Reset();
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
            if (dominant_object_name == "")
            {
                sub_explorer.SetActive(false);
                return;
            }
            target_object_name = dominant_object_name;
            found_object_count = object_count;
           
            bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            if(interaction_touch_enalbed)
            {
               // List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_on_the_left(target_object_name);
                List<SceneObject> objs = virtual_container.GetComponent<ObjectContainer>().get_objects_in_rect(target_object_name);
                init_object_count = objs.Count;
                goal_object_count = objs.Count + random.Next(2, 6);
                add_object_count = goal_object_count - init_object_count;
                
            } else
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

        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "I've found "+ init_object_count + " " + target_object_name + "s. But, I need " + add_object_count + " more  " + target_object_name + "s to buy an icecream!",
            true,
            null,
            "",
            7), 0
            );
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "Then, how may coins do we need in total to buy an icecream?",
            true,
            new CallbackFunction(initSolver),
            "",
            4
            ), 0
            );
        


    }
    public void initSolver(string t)
    {
        sub_solver.SetActive(true);
    }
    public void UpdateCVResult(CVResult cv)
    {




    }
    public void SetIdle(bool t)
    {
        is_idle = t;
    }
}
