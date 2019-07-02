using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentDiv : MonoBehaviour {


    public GameObject sub_intro;
    public GameObject sub_explorer;
    public GameObject sub_solver;
    public GameObject sub_review;


    public string target_object_name = "";
    public Rect target_object_cluster;    
    public int object_num;
    public int dividend;
    public int divisor;
    public int quotient;
    public Vector2 center_of_objects;




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

        if (sub_intro) sub_intro.SetActive(true);
        if (sub_explorer) sub_explorer.SetActive(false);
        if (sub_solver) sub_solver.SetActive(false);
        if (sub_review) sub_review.SetActive(false);
        is_idle = true;
        is_solved = false;
        object_num = 0;
        dividend = 0;
        divisor = 0;
        quotient = 0;
        target_object_name = "";
        target_object_cluster = new Rect();

        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help the minion solve division problems and collect purple gems!");
    }
    public void onSolved()
    {
        sub_solver.SetActive(false);
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p3_division);
        is_solved = true;

    }
    public void s1_UpdateExplorer()
    {
        System.Random random = new System.Random();

        string dominant_object_name = "";
        Vector2 center = new Vector2(0, 0);
        int object_count = 0;

        if (is_solved || sub_intro.activeSelf)
        {
            sub_explorer.SetActive(false);
            return;
        }

        List<SceneObject> objects_cluster;
        
        SceneObjectManager.mSOManager.get_dominant_object(ref dominant_object_name, ref center, ref object_count);
        if (is_idle)
        {
            if (dominant_object_name == null || object_count <=5)
            {
                sub_explorer.SetActive(false);
                return;
            }
            target_object_name = dominant_object_name;
            object_num = object_count;
            
            center_of_objects = center;            


            bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            if (interaction_touch_enalbed)
            {
                //TBD
                dividend = object_num;
                List<int> divisor_list = new List<int>();
                for (int i = 2; i <= object_num / 2; i++)
                {
                    if (object_num % i == 0) divisor_list.Add(i);
                }
                if (divisor_list.Count == 0)
                {
                    sub_explorer.SetActive(false);
                    return;
                }
                divisor = divisor_list[(int)Random.Range(0, divisor_list.Count - 1)];
                quotient = dividend / divisor;

            }
            else
            {
                dividend = object_num;
                List<int> divisor_list = new List<int>();
                for(int i = 2; i <= object_num / 2; i++)
                {
                    if (object_num % i == 0) divisor_list.Add(i);
                }
                if (divisor_list.Count == 0)
                {
                    sub_explorer.SetActive(false);
                    return;
                }
                divisor = divisor_list[(int) Random.Range(0, divisor_list.Count)];
                quotient = dividend / divisor;
            }
            //pops up explorer
            sub_explorer.SetActive(true);
            RectTransform r = sub_explorer.GetComponent<RectTransform>();
            r.position = new Vector3(center_of_objects.x, center_of_objects.y, 0);

        }
        else
        {
            sub_explorer.SetActive(false);

        }
    }
    public void s2_OnExplorer()
    {
        SetIdle(false);
        
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Oh! There are " + dividend+" "+target_object_name + "s. Can you help me distribute them?",
                 true,
                new CallbackFunction(s4_startsolver),
                ""
                ));

        



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
