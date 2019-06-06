using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivTangible : MonoBehaviour {



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

        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;
        int quotient = ContentModuleRoot.GetComponent<ContentDiv>().quotient;

        int full_cells = groups.GetComponent<GroupGuide>().CheckCells(quotient, obj_name);

        if (full_cells == divisor) OnCompletion();

    }

    private void OnCompletion()
    {
        UserInteracting = false;
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "How many " + obj_name + "s are there? [TODO:input UI]",
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
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;

        groups.SetActive(true);
        groups.GetComponent<GroupGuide>().Setup(divisor);
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "I want you to put the same number of "+obj_name+"s in each bag. How many "+obj_name+"s go in each bag?",
              true,
              null,
              ""
              ));
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Let's move " + obj_name + "s to the bags, and count the number of " + obj_name + "s in each bag.",
               true,
               new CallbackFunction(StartOperation),
               "none"
               ));

    }
    public void StartOperation(string p)
    {
        UserInteracting = true;

        UpdateBoard();

        problemboard.SetActive(true);
        board.SetActive(false);

    }

    private void UpdateBoard()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentDiv>().target_object_name;
        int dividend = ContentModuleRoot.GetComponent<ContentDiv>().dividend;
        int divisor = ContentModuleRoot.GetComponent<ContentDiv>().divisor;

        //if (board.activeSelf != false) board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
        problemboard_text.GetComponent<Text>().text = dividend + "(" + obj_name + "s) ÷ " + divisor + " (bags) = ? ";

    }
}
