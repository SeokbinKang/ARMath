using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivVirtual : MonoBehaviour {


    
    public GameObject ContentModuleRoot;
//    public GameObject bag_base;    
    public bool UserInteracting;
    // Use this for initialization
    private float nextActionTime = 0.0f;
    public GameObject groups;
    public GameObject tray;
    private List<GameObject> obj_movable;

    public GameObject pre_bag_static; //DEPRECATED. used in hand grouping interface
    public GameObject grouping;  //DEPRECATED. used in hand grouping interface
    void Start()
    {
      //  obj_movable = new List<GameObject>();
    }
    void OnEnable()
    {
        Reset();
        loadPrompt();
    }
    private void Reset()
    {       
        grouping.SetActive(false);
       
        UserInteracting = false;
       
        if (obj_movable == null) obj_movable = new List<GameObject>();
        foreach (GameObject o in obj_movable)
        {
            if(o!=null) GameObject.Destroy(o);
        }
        obj_movable.Clear();
        

        /*
        if (bag_list == null) bag_list = new List<GameObject>();
        else
        {
            foreach(GameObject o in bag_list)
            {
                GameObject.Destroy(o);
            }
            bag_list.Clear();
        }*/

    }
    // Update is called once per frame
    void Update()
    {       

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;

            count_object();
        }
    }
    public void loadPrompt()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        //initialize virtual chocolates
        setup_virtual_objects();
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Let's move the chocolates on the screen",
               true,
               new CallbackFunction(StartOperation),
               "none"
               ));

    }
    private void setup_virtual_objects()
    {
        Rect rt = ContentModuleRoot.GetComponent<ContentDiv>().obj_rect;
        RectTransform rt_V = tray.GetComponent<RectTransform>();
        rt_V.position = rt.position;
        rt_V.sizeDelta = rt.size;
        rt_V.localScale = Vector3.one;
        tray.SetActive(true);
        
        GameObject icon_obj = AssetManager.get_icon("blue chocolate");
        if(icon_obj==null)
        {           
            return;
        }
        List<Vector2> pos_list = ContentModuleRoot.GetComponent<ContentDiv>().obj_pos_list;
        foreach (Vector2 pos in pos_list)        
        {
            GameObject new_obj = ARMathUtils.create_2DPrefab(icon_obj, tray, pos);
            RectTransform r = new_obj.GetComponent<RectTransform>();

            r.position = pos;
            r.sizeDelta = new Vector2(120  , 120);
            r.Rotate(0, 0, Random.Range(0, 360));
            new_obj.SetActive(true);
            this.obj_movable.Add(new_obj);
        }

    }
    public void StartOperation(string p)
    {
        UserInteracting = true;
    }
    private void count_object()
    {
        if (!groups || !UserInteracting || obj_movable==null) return;

        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        int full_cells = groups.GetComponent<GroupGuide>().CheckBoxes(obj_movable, quotient);

        if (full_cells == divisor) OnCompletion();

    }
    private void OnCompletion()
    {
        UserInteracting = false;
        this.transform.parent.GetComponent<ContentSolver>().start_review();
        /*
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Good Job! How many bags do we need? [TODO:input UI]",
              true,
              new CallbackFunction(OnCompletion2),
              "none"
              ));
              */
        //Answer UI needs to be added

    }
    private void OnCompletion2(string p)
    {

        ContentModuleRoot.GetComponent<ContentDiv>().onSolved();
    }
   


    //private void UpdateBoard()
    //{
    //    string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
    //    int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
    //    int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;

    //    //if (board.activeSelf != false) board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
    //    problemboard_text.GetComponent<Text>().text = dividend + "(" + obj_name + "s) ÷ " + divisor + " ("+obj_name+"s) = ? (bags)";

    //}
    //public void onNewGroup(Vector2 group_center)
    //{
    //    //create a new virtual bag
    //    UnityEngine.GameObject label = ARMathUtils.create_2DPrefab(this.pre_bag_static, this.gameObject, group_center);

    //    this.bag_list.Add(label);

    //}
    //public void loadPrompt()
    //{
    //    string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
    //    int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
    //    int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;


    //    Debug.Log("[ARMath] -----------------------");
    //    Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
    //          "We want to place " + obj_name + "s in bags. Every bag can contain " + divisor + " " + obj_name + "s. How many bags do we need?",
    //          true,
    //          null,
    //          ""
    //          ));
    //    Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
    //           "Let's select a group of " + divisor + " " + obj_name + "s by drawing boundaries on the touchscreen. [TODO: demo interaction]",
    //           true,
    //           new CallbackFunction(StartOperation),
    //           "none"
    //           ));

    //}
    //private void count_object()
    //{
    //    if (bag_list == null || !UserInteracting) return;

    //    string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
    //    int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
    //    int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
    //    int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

    //    int full_cells = bag_list.Count;

    //    if (full_cells == quotient) OnCompletion();

    //}
}
