using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultReview : MonoBehaviour {

    // Use this for initialization
    public GameObject content_root;
    public GameObject agent;
    public GameObject msg1;
    public GameObject msg2_box;
    public GameObject msg3_box;
    public GameObject reward;
    public GameObject virtual_solver;
    public GameObject trees;
    

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
        FeedbackGenerator.clear_all_feedback();
        
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
        string obj_name = content_root.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = content_root.GetComponent<ContentMulti>().target_base_num;
        int num_cells = content_root.GetComponent<ContentMulti>().target_mult_num;

        msg1.GetComponent<Text>().text = "Alright! We turned on the"+ num_cells + " trees, and used " + num_per_cell + " batteries for each.";
        msg2_box.GetComponentInChildren<Text>().text = "Let's count how many batteries we used in total.";
        msg3_box.GetComponentInChildren<Text>().text = "Can you select the answer at the top?";
        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();


        if (interaction_touch_enalbed)
        { //turn on either of two solvers.
            highlight_objects_virtual(3, 5);
        }
        else
        {
            highlight_objects_tangible(3, 5);
        }
        msg2_box.SetActive(true);
        msg3_box.SetActive(false);
        Dialogs.set_topboard_highlight(true, 0, 4);
        Dialogs.set_topboard_highlight(true, 1, 6);
        msg2_box.GetComponentInChildren<DelayedImage>().setCallback(show_top_answer, "= ?");
        //msg3_box.GetComponentInChildren<DelayedImage>().setCallback(show_selection, "");
    }
    public void show_top_answer(string t)
    {
        Dialogs.set_topboard_animated(true, 2, t);


    }
    public void show_selection(string t)
    {
        int num_per_cell = content_root.GetComponent<ContentMulti>().target_base_num;
        int num_cells = content_root.GetComponent<ContentMulti>().target_mult_num;
        int goal_n = num_per_cell * num_cells;
        int[] ans = new int[4];
        ans[0] = goal_n - 2;
        ans[1] = goal_n;
        ans[2] = goal_n + 2;
        ans[3] = goal_n + 5;
        Dialogs.review(
            "",
            ans,
            1,
            new CallbackFunction(OnCompletion)
            );
    }
    public void after_counting_all(string t)
    {
        msg2_box.SetActive(false);
        msg3_box.SetActive(true);
        show_selection("");
    }
    public void OnCompletion(string t)
    {
        FeedbackGenerator.clear_all_feedback();
        msg3_box.SetActive(false);
        msg2_box.SetActive(false);
        this.transform.parent.GetComponent<ContentSolver>().close_solvers();
        FeedbackGenerator.clear_all_feedback();
        reward.SetActive(true);
        //content_root.GetComponent<ContentAddition>().onSolved();
    }
    private void highlight_objects_tangible(float delay1, float delay2)
    {
        int num_per_cell = content_root.GetComponent<ContentMulti>().target_base_num;
        int num_cells = content_root.GetComponent<ContentMulti>().target_mult_num;
        int goal_n = num_per_cell * num_cells;

        List<GameObject> tree_icons = trees.GetComponent<GroupTree>().get_virtual_trees_in_cells();
        List<Vector2> tangibles = trees.GetComponent<GroupTree>().get_tangible_objects_in_cells();

        int k = 0;
        

        FeedbackGenerator.init_counter(new CallbackFunction(after_counting_all), tangibles.Count);
        k = 0;
        for (int c = 0; c < num_cells; c++)
        {
            for (int i = 0; i < num_per_cell && k < tangibles.Count; i++)
            {  //TODO: detach higher number label
                //Vector3 targetPos = virtuals[k++].GetComponent<RectTransform>().position;
                FeedbackGenerator.create_target(tangibles[k++], delay2 + c * 2, 180, 2, true);

            }
        }

    }
    private void highlight_objects_virtual(float delay1, float delay2)
    {

        int num_per_cell = content_root.GetComponent<ContentMulti>().target_base_num;
        int num_cells = content_root.GetComponent<ContentMulti>().target_mult_num;
        int goal_n = num_per_cell * num_cells;

        List<GameObject> tree_icons = trees.GetComponent<GroupTree>().get_virtual_trees_in_cells();
        List<GameObject> virtuals = trees.GetComponent<GroupTree>().get_virtual_objects_in_cells();

        int k = 0;
        /*foreach(GameObject t in tree_icons)
        {           
            Vector3 targetPos = t.GetComponent<RectTransform>().position;
            FeedbackGenerator.create_target(targetPos, delay1, 3, 0);
        }*/

        FeedbackGenerator.init_counter(new CallbackFunction(after_counting_all), virtuals.Count);
        k = 0;
        for (int c = 0; c < num_cells; c++)
        {
            for (int i = 0; i < num_per_cell && k<virtuals.Count; i++)
            {  //TODO: detach higher number label
                //Vector3 targetPos = virtuals[k++].GetComponent<RectTransform>().position;
                FeedbackGenerator.create_target(virtuals[k++], delay2 + c * 2, 180, 2,true);
                
            }
        }
    }
}
