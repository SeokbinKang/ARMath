using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultTangible : MonoBehaviour {

    


    public GameObject board;


    public GameObject groups;
    
    public GameObject problemboard;
    public GameObject problemboard_text;

    public GameObject ContentModuleRoot;

    
    public bool UserInteracting;
    // Use this for initialization
    private float nextActionTime = 0.0f;



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
        groups.SetActive(true);        
        problemboard.SetActive(false);
        UserInteracting = false;
        



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
        if (!groups || !UserInteracting) return;
        
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;

        int full_cells = groups.GetComponent<GroupGuide>().CheckCellsProgressive(num_per_cell,obj_name,ProblemType.p3_multiplication);

        if (full_cells == num_cells) OnCompletion();
        
    }
    
    private void OnCompletion()
    {
        UserInteracting = false;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Nice job! Each of "+ num_cells+" friends got "+ num_per_cell+" "+obj_name + "s. How many "+obj_name + "s did you give them in total?",
              true,
              new CallbackFunction(OnCompletion2),
              "none"
              ));
        
        //Answer UI needs to be added
        
    }
    private void OnCompletion2(string p)
    {

        ContentModuleRoot.GetComponent<ContentMulti>().onSolved();
    }
    public void loadPrompt()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;

        
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "I have " + num_cells + " friends coming over and I want give " + num_per_cell + " " + obj_name + "s to each friend. Can you help?",
              true,
              null,
              ""
              ));
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Let's give each friend "+obj_name+"s by moving them on the table.",
               true,
               new CallbackFunction(StartOperation),
               "none"
               ));
        
    }
    public void StartOperation(string p)
    {
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        UserInteracting = true;        
        UpdateBoard();          
        problemboard.SetActive(true);
        board.SetActive(false);
        groups.SetActive(true);
        groups.GetComponent<GroupGuide>().Setup(num_cells);

    }

    private void UpdateBoard()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        
        //if (board.activeSelf != false) board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
        problemboard_text.GetComponent<Text>().text = num_per_cell+"("+obj_name+"s) X "+num_celss+" (friends) = ?";

    }
}
