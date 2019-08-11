using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultTangible : MonoBehaviour {

    


    


    public GameObject tree_group;
    
    

    public GameObject ContentModuleRoot;

    
    public bool UserInteracting;
    // Use this for initialization
    private float nextActionTime = 0.0f;

    private int compelte_cells;
    private TimerCallback mCallback;
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
       
        UserInteracting = false;
        compelte_cells = 0;
    }
    // Update is called once per frame
    void Update()
    {        

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period*4;
            if (mCallback != null && mCallback.tick()) mCallback = null;
            count_object();
        }
    }    

    private void count_object()
    {
        if (!tree_group || !UserInteracting) return;
        
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        bool complete = false;
        int ret = tree_group.GetComponent<GroupTree>().CheckCellsProgressive(num_per_cell,obj_name,ProblemType.p3_multiplication);

        if (ret == num_cells)
        {
            complete = true;

        }
        else if (ret > compelte_cells)
        {
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
             "Let's turn on the next tree",
             true,
             null,
             "none", 4
             ), 4);
            compelte_cells = ret;

        }
        if (complete)
        {
            mCallback = new TimerCallback(OnCompletion, "", 4);
            UserInteracting = false;
        }

        
    }
    
    private void OnCompletion(string t)
    {
        UserInteracting = false;
        this.transform.parent.GetComponent<ContentSolver>().start_review();

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
             "Can you put the "+ num_per_cell + " batteries next to each tree? You can use batteries on the table.",
             true,
             new CallbackFunction(StartOperation),
             "none", 10
             ));

        
    }
    public void StartOperation(string p)
    {
        FeedbackGenerator.init_create_dialog_term();
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        UserInteracting = true;       
        /*groups.SetActive(true);
        groups.GetComponent<GroupGuide>().Setup(num_cells);*/

    }


}
