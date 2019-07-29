using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentDiv : MonoBehaviour {


    public GameObject sub_intro;
    public GameObject sub_intro2;
    public GameObject sub_explorer;
    public GameObject sub_solver;
    public GameObject sub_review;
    public GameObject sub_context;

    public string target_object_name ;
    public Rect target_object_cluster;    
    
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
            //s1_search_objects();
        }
    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {

        if (sub_intro) sub_intro.SetActive(true);
        if (sub_intro2) sub_intro2.SetActive(false);
        if (sub_intro2) sub_context.SetActive(false);
        if (sub_explorer) sub_explorer.SetActive(false);
        if (sub_solver) sub_solver.SetActive(false);
        if (sub_review) sub_review.SetActive(false);

        is_idle = true;
        is_solved = false;
        
        dividend = 0;
        divisor = 0;
        quotient = 0;
        
        target_object_cluster = new Rect();

      
    }
    public void onSolved()
    {
        sub_solver.SetActive(false);
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p3_division);
        is_solved = true;

    }

    //start a conversation whene there are some objects to divide
    public void s1_search_objects(string t)
    {
        if (is_solved)
        {           
            return;
        }
        Tools.finder_init(target_object_name, 4, new CallbackFunction2(s2_objectfound), "","Let's find some chocolates!",1f);

    }
    public void s2_objectfound(string p,  List<SceneObject> obj_list, Rect rt)
    {
        System.Random random = new System.Random();
        this.dividend = System.Convert.ToInt32(p);
        
       

        if (is_idle)
        {            
            List<int> divisor_list = new List<int>();
            for (int i = 3; i <= dividend / 2; i++)
            {
                if (dividend % i == 0) divisor_list.Add(i);
            }
            if (divisor_list.Count == 0)
            {
                Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                  "Hmm... I think we need more chocolates. Can you find more?",
                true,
                 new CallbackFunction(s1_search_objects),
                "",
                  5), 0
                 );
                return;
            }
            divisor = divisor_list[(int)Random.Range(0, divisor_list.Count)];
            quotient = dividend / divisor;
            is_idle = false;
           

            //indicate objects


            float target_delay = 2;
            foreach (SceneObject so_ in obj_list) {    
                FeedbackGenerator.create_target(so_.get_screen_pos(), target_delay, 5, 0);
                target_delay += 0.2f;
            }
        
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "Oh thanks. We found " + dividend + " chocolates!.  ",
            true,
            new CallbackFunction(callback_shownumber1),
           dividend.ToString(),
            6), 0
            );
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
             "Can you distribute the chocolates to "+divisor+" gift boxes?",
             //"Can you help distribute the chocolates to gift boxes?",
              true,
              new CallbackFunction(callback_shownumber2),
              "÷ " + divisor.ToString(),
              7f), 0
            );
        }
    }
    public void callback_shownumber1(string t)
    {
        Dialogs.set_topboard_animated(true, 0, t);

        sub_context.SetActive(true);
        sub_context.GetComponent<GroupGuide>().Setup_giftbox(divisor);
        //indicate gif boxes
        float target_delay = 2;
      /*  foreach (GameObject go_ in sub_context.GetComponent<GroupGuide>().get_children())
        {
            Debug.Log("[ARMath] box " + target_delay + "  " + go_.GetComponent<RectTransform>().position);
            FeedbackGenerator.create_target(go_, target_delay, 5, 1);
            target_delay += 0.2f;
        }*/

    }
    public void callback_shownumber2(string t)
    {

        Dialogs.set_topboard_animated(true, 1, t);
        s4_startsolver("");
    }    

    public void s4_startsolver(string p)
    {
        //sub_trees.GetComponent<GroupTree>().start_operation();
        sub_solver.SetActive(true);
    }
    


 
    public void SetIdle(bool t)
    {
        is_idle = t;
    }
}
