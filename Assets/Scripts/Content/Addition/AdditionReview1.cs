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
        msg2.GetComponent<Text>().text = "Then, how many coins do we have in total?";

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
        int goal_n = content_root.GetComponent<ContentAddition>().goal_object_count;
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
    public void OnCompletion(string t)
    {
        msg2_box.SetActive(false);

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
