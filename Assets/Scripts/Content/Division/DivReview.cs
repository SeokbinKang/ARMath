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
    public GameObject msg2;
    public GameObject msg3_dummy;
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
        msg2.GetComponent<Text>().text = "Then, how many chocolate are there in each box?";

        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();


        if (interaction_touch_enalbed)
        { //turn on either of two solvers.
            highlight_objects_virtual(3, 5);
        }
        else
        {
            highlight_objects_tangible(3, 5);
        }

        Dialogs.set_topboard_highlight(true, 0, 4);
        Dialogs.set_topboard_highlight(true, 1, 6);
        msg2.GetComponent<DelayedImage>().setCallback(show_top_answer, "= ?");
        msg3_dummy.GetComponent<DelayedImage>().setCallback(show_selection, "");
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

        List<SceneObject> tangibles = context.GetComponent<GroupGuide>().get_tangible_objects_in_cells();

        int k = 0;
        for (int c = 0; c < divisor; c++)
        {
            for (int i = 0; i < quotient && k < tangibles.Count; i++)
            {  //TODO: detach higher number label
                
                FeedbackGenerator.create_target(tangibles[k++], delay2 + c * 2, 3, 0);
            }
        }

        /*foreach (GameObject t in context.GetComponent<GroupGuide>().get_children())
        {
            Vector3 targetPos = t.transform.GetChild(0).gameObject.GetComponent<RectTransform>().position;
            FeedbackGenerator.create_target(targetPos, delay1, 3, 0);
        }*/


    }
    private void highlight_objects_virtual(float delay1, float delay2)
    {

        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        List<GameObject> virtuals = context.GetComponent<GroupTree>().get_virtual_objects_in_cells();

        int k = 0;
        

        k = 0;
        for (int c = 0; c < divisor; c++)
        {
            for (int i = 0; i < quotient && k < virtuals.Count; i++)
            {  //TODO: detach higher number label
                Vector3 targetPos = virtuals[k++].GetComponent<RectTransform>().position;
                FeedbackGenerator.create_target(targetPos, delay2 + c * 2, 3, 5);
            }
        }
    }
}
