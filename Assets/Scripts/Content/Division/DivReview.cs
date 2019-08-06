using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivReview : MonoBehaviour {

    // Use this for initialization
    public GameObject ContentModuleRoot;
    public GameObject context;
    public GameObject agent;
    public GameObject msg1;
    public GameObject msg2_box;
    public GameObject msg3_box;
    public GameObject reward;
    public GameObject virtual_solver;
    


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {        
        init_review();
        agent.SetActive(true);
        reward.SetActive(false);
    }
    private void OnDisable()
    {
        agent.SetActive(false);
        reward.SetActive(false);
    }

    private void init_review()
    {

        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        

        msg1.GetComponent<Text>().text = "Thank you! We distributed " + dividend + " chocolates equally to " + divisor + " boxes.";
        msg2_box.GetComponentInChildren<Text>().text = "Let's count how many chocolates are there in a box!";
        msg3_box.GetComponentInChildren<Text>().text = "Alright! Can you select the answer at the top?";

        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();

        FeedbackGenerator.init_counter(new CallbackFunction(after_counting_all), quotient);
        if (interaction_touch_enalbed)
        { //turn on either of two solvers.
            highlight_objects_virtual(3, 12);
        }
        else
        {
            highlight_objects_tangible(3, 12);
        }

        Dialogs.set_topboard_highlight(true, 0, 4);
        Dialogs.set_topboard_animated(1, "÷ " +divisor,6);
        //Dialogs.set_topboard_highlight(true, 1, 6);
        msg2_box.GetComponent<DelayedImage>().setCallback(show_top_answer, "= ?");
        //msg3_box.GetComponent<DelayedImage>().setCallback(show_selection, "");
        msg2_box.SetActive(true);
        msg3_box.SetActive(false);
    }
    public void show_top_answer(string t)
    {
        Dialogs.set_topboard_animated(true, 2, t);


    }
    public void show_selection(string t)
    {
        
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        
        int[] ans = new int[4];
        ans[0] = quotient - 2;
        ans[1] = quotient;
        ans[2] = quotient + 2;
        ans[3] = quotient + 5;
        Dialogs.review(
            "",
            ans,
            1,
            new CallbackFunction(OnCompletion)
            );
    }
    public void OnCompletion(string t)
    {
        reward.SetActive(true);
        
    }
    private void highlight_objects_tangible(float delay1, float delay2)
    {
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        List<Vector2> tangibles = context.GetComponent<GroupGuide>().get_tangible_objects_in_cells();

        int k = 0;
        for (int c = 0; c < divisor; c++)
        {
            for (int i = 0; i < quotient && k < tangibles.Count; i++)
            {  //TODO: detach higher number label
                if (c == 0) FeedbackGenerator.create_target(tangibles[k], delay2, 120, 2, true);
                FeedbackGenerator.create_target(tangibles[k++], delay1 + ((float)c) * 1.5f, 2, 0,false);
                
            }
        }

   


    }
    private void highlight_objects_virtual(float delay1, float delay2)
    {

        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        List<GameObject> virtuals = context.GetComponent<GroupGuide>().get_virtual_objects_in_cells();
        int k = 0;

        k = 0;
        bool countable = true;

        for (int c = 0; c < divisor; c++)
        {
            
            for (int i = 0; i < quotient && k < virtuals.Count; i++)
            {  //TODO: detach higher number label     
                if (countable) FeedbackGenerator.create_target(virtuals[k], delay2, 120, 2, countable);
                FeedbackGenerator.create_target(virtuals[k++], delay1+((float)c)*1.5f, 2, 0, false);
                

                    //else FeedbackGenerator.create_target(virtuals[k++], delay2 + c * 2, 3, 2, countable);


            }
            countable = false;
        }
    }
    public void after_counting_all(string t)
    {
        msg2_box.SetActive(false);
        //msg3_box.GetComponentInChildren<Text>().text = "Alright! Can you select the answer at the top? ";
        msg3_box.SetActive(true);
        show_selection("");
    }

}

