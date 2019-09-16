using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionReview1 : MonoBehaviour {

    public GameObject content_root;
    public GameObject agent;
    public GameObject msg1;
    public GameObject msg2_box;
    public GameObject msg2;
    public GameObject msg3_dummy;
    public GameObject reward;
    public GameObject virtual_solver;
    public GameObject region;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        Debug.Log("[ARMath] Addition review starts");
        init_review();
        agent.SetActive(true);
        reward.SetActive(false);
        msg2_box.SetActive(true);
        msg3_dummy.SetActive(false);
    }
    private void OnDisable()
    {
        agent.SetActive(false);
        reward.SetActive(false);
        msg2_box.SetActive(false);

    }

    private void init_review()
    {
        int init_n = content_root.GetComponent<ContentAddition>().init_object_count;
        int goal_n = content_root.GetComponent<ContentAddition>().goal_object_count;
        int added = goal_n - init_n;

        msg1.GetComponent<Text>().text = "Alright! We had "+init_n +" coins at the beginning, and added " + added + " coins. ";
        msg2.GetComponent<Text>().text = "Let's count how many coins we have in total.";
        /*
        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();       

        
        if (interaction_touch_enalbed)
        { //turn on either of two solvers.
            highlight_objects_virtual(3, 5);
        }
        else 
        {
            highlight_objects_tangible(3, 5);
        }*/

        Dialogs.set_topboard_highlight(true, 0, 3);
        Dialogs.set_topboard_animated(1,"+ "+added.ToString(),6);
        //Dialogs.set_topboard_highlight(true, 1, 6);
        msg2.GetComponent<DelayedImage>().setCallback(show_top_answer, "= ?");
       
    }
    public void show_top_answer(string t)
    {
        Dialogs.set_topboard_animated(true, 2, t);
        FeedbackGenerator.clear_all_feedback();
        region.GetComponent<RegionControl>().enaleRegion(0, false);
        region.GetComponent<RegionControl>().enaleRegion(1, false);
        List<Vector2> all_targets = content_root.GetComponent<ContentAddition>().obj_pos_list;
        int i = 0;
        float delay = 4;
        FeedbackGenerator.init_counter(new CallbackFunction(after_counting_all),all_targets.Count);
        for (i = 0; i < all_targets.Count; i++)
        {  //TODO: detach higher number label            
            FeedbackGenerator.create_target_countable(all_targets[i], delay, 600, 2,false);            
            delay += 0.5f;
        }
        //global counter callback
    }
    public void after_counting_all(string t)
    {
        msg2_box.SetActive(false);
        msg3_dummy.GetComponentInChildren<Text>().text = "Alright! Can you select the answer at the top? ";
        msg3_dummy.SetActive(true);
        show_selection("");
    }
    public void show_selection(string t)
    {
        ;
        int goal_n = content_root.GetComponent<ContentAddition>().goal_object_count;
        List<int> ans_val = new List<int>();
        ans_val.Add(goal_n);
        ans_val.Add(goal_n - Random.Range(1, 3));
        ans_val.Add(goal_n + Random.Range(1, 3));
        ans_val.Add(goal_n + Random.Range(3, 6));
        for(int i = 0; i < Random.Range(1, 12);i++)
        {
            ans_val.Add(ans_val[0]);
            ans_val.RemoveAt(0);
        }

        int[] ans = new int[4];
        int ans_idx = 0;
        for (int i = 0; i < ans.Length; i++) {
            ans[i] = ans_val[i];
            if (ans[i] == goal_n) ans_idx = i;
        }
        
        Dialogs.review(
            "",
            ans,
            ans_idx,
            new CallbackFunction(OnCompletion)
            );
    }
    public void OnCompletion(string t)
    {
        msg2_box.SetActive(false);
        msg3_dummy.SetActive(false);
        this.transform.parent.GetComponent<ContentSolver>().close_solvers();
        FeedbackGenerator.clear_all_feedback();
        reward.SetActive(true);
        //content_root.GetComponent<ContentAddition>().onSolved();
    }
    private void highlight_objects_tangible(float delay1,float delay2)
    {
        string target_object_name = content_root.GetComponent<ContentAddition>().target_object_name;
        List<SceneObject> objs = ARMathUtils.get_objects_in_rect(region.GetComponent<RegionControl>().getRegion(0), target_object_name);
        List<SceneObject> objs_added = ARMathUtils.get_objects_in_rect(region.GetComponent<RegionControl>().getRegion(1), target_object_name);
        int goal_n = content_root.GetComponent<ContentAddition>().goal_object_count;
        int init_n = content_root.GetComponent<ContentAddition>().init_object_count;
        int added_n = goal_n - init_n;

        //init_n = objs.Count;
        int i = 0;
        for (i = 0; i < init_n && i < objs.Count; i++)
        {  //TODO: detach higher number label
            Vector3 targetPos = new Vector3(objs[i].catalogInfo.Box.center.x, Screen.height - objs[i].catalogInfo.Box.center.y, 0);
            FeedbackGenerator.create_target(targetPos, delay1, 10-delay1, 0);
        }

        for (i=0; i < added_n && i < objs_added.Count; i++)
        {  //TODO: detach higher number label
            Vector3 targetPos = new Vector3(objs_added[i].catalogInfo.Box.center.x, Screen.height - objs_added[i].catalogInfo.Box.center.y, 0);
            FeedbackGenerator.create_target(targetPos, delay2, 10-delay2, 1);
        }


    }
    private void highlight_objects_virtual(float delay1, float delay2)
    {
        string target_object_name = content_root.GetComponent<ContentAddition>().target_object_name;
        List<SceneObject> objs = ARMathUtils.get_objects_in_rect(region.GetComponent<RegionControl>().getRegion(0), target_object_name);
        //List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        int init_n = content_root.GetComponent<ContentAddition>().init_object_count;
        int added = content_root.GetComponent<ContentAddition>().goal_object_count - init_n;

        //init_n = objs.Count;
        for (int i = 0; i < init_n && i < objs.Count; i++)
        {  //TODO: detach higher number label
            Vector3 targetPos = new Vector3(objs[i].catalogInfo.Box.center.x, Screen.height - objs[i].catalogInfo.Box.center.y, 0);
            FeedbackGenerator.create_target(targetPos, delay1, 10-delay1, 0);
        }

        List<GameObject> virtual_coins = virtual_solver.GetComponent<AdditionVirtual>().get_movables_in_container();

        for (int i=0; i < added && i < virtual_coins.Count; i++)
        {  //TODO: detach higher number label
            Vector3 targetPos = virtual_coins[i].GetComponent<RectTransform>().position;
            FeedbackGenerator.create_target(targetPos, delay2, 10 - delay2, 1);
        }


    }
}
