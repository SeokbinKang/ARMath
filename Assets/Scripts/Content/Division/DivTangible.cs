using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivTangible : MonoBehaviour {



    
    public GameObject groups;
    public GameObject ContentModuleRoot;
    public GameObject dummy_timer3;

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
        dummy_timer3.SetActive(false);
        UserInteracting = false;
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

    private void count_object()
    {
        if (!groups || !UserInteracting) return;

        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        int full_cells = groups.GetComponent<GroupGuide>().CheckBoxes(obj_name,quotient);

        if (full_cells == divisor) OnCompletion();

    }

    private void OnCompletion()
    {
        UserInteracting = false;

        this.transform.parent.GetComponent<ContentSolver>().start_review();

        //Answer UI needs to be added

    }
    public void loadPrompt()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;      
      
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Let's move the chocolates on the table",
               true,
               new CallbackFunction(StartOperation),
               "none"
               ));

    }
    private void OnCompletion2(string p)
    { // put an extra delay
        dummy_timer3.GetComponent<DelayedImage>().setCallback(new CallbackFunction(OnCompletion3),"");
        dummy_timer3.SetActive(true);

       
    }
    public void OnCompletion3(string p)
    {
        ContentModuleRoot.GetComponent<ContentDiv>().onSolved();
    }
    public void StartOperation(string p)
    {
        UserInteracting = true;        
    }

    
}
