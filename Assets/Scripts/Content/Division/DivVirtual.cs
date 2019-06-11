using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivVirtual : MonoBehaviour {


    public GameObject board;
    
    public GameObject problemboard;
    public GameObject problemboard_text;
    public GameObject ContentModuleRoot;
//    public GameObject bag_base;
    public GameObject pre_bag_static;
    public GameObject grouping;
    public bool UserInteracting;
    // Use this for initialization
    private float nextActionTime = 0.0f;
    private List<GameObject> bag_list;


    void Start()
    {

    }
    void OnEnable()
    {
        Reset();
        loadPrompt();
    }
    private void Reset()
    {
        board.SetActive(false);
        grouping.SetActive(false);
        problemboard.SetActive(false);
        UserInteracting = false;
        if (bag_list == null) bag_list = new List<GameObject>();
        else
        {
            foreach(GameObject o in bag_list)
            {
                GameObject.Destroy(o);
            }
            bag_list.Clear();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (UserInteracting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;

            count_object();
        }
    }

    private void count_object()
    {
        if (bag_list==null || !UserInteracting) return;

        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        int full_cells = bag_list.Count;

        if (full_cells == quotient) OnCompletion();

    }

    private void OnCompletion()
    {
        UserInteracting = false;
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Good Job! How many bags do we need? [TODO:input UI]",
              true,
              new CallbackFunction(OnCompletion2),
              "none"
              ));

        //Answer UI needs to be added

    }
    private void OnCompletion2(string p)
    {

        ContentModuleRoot.GetComponent<ContentDiv>().onSolved();
    }
    public void loadPrompt()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;

        
        Debug.Log("[ARMath] -----------------------");
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "We want to place "+ obj_name + "s in bags. Every bag can contain " + divisor+" "+ obj_name + "s. How many bags do we need?",
              true,
              null,
              ""
              ));
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Let's select a group of "+ divisor+" "+obj_name+"s by drawing boundaries on the touchscreen. [TODO: demo interaction]",
               true,
               new CallbackFunction(StartOperation),
               "none"
               ));

    }
    public void onNewGroup(Vector2 group_center)
    {
        //create a new virtual bag
        UnityEngine.GameObject label = ARMathUtils.create_2DPrefab(this.pre_bag_static, this.gameObject, group_center);

        this.bag_list.Add(label);

    }
    public void StartOperation(string p)
    {
        UserInteracting = true;

        UpdateBoard();

        problemboard.SetActive(true);
        board.SetActive(false);
        grouping.SetActive(true);

    }

    private void UpdateBoard()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;

        //if (board.activeSelf != false) board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
        problemboard_text.GetComponent<Text>().text = dividend + "(" + obj_name + "s) ÷ " + divisor + " ("+obj_name+"s) = ? (bags)";

    }
}
