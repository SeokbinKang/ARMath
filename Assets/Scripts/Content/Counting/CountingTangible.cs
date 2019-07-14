using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingTangible : MonoBehaviour
{

    public GameObject prompt;

    public GameObject prompt_text;


    public GameObject board;


    public GameObject container;
    public GameObject problemboard;
    public GameObject problemboard_text;


    public GameObject ContentModuleRoot;
    public GameObject prefab_tap;
    private List<GameObject> tap_list;


    public bool IsCounting;

    private int counting_n;
    private string target_object_name;
    // Use this for initialization

    private float nextActionTime = 0.0f;
    private TimerCallback interaction_prompt;
    private SceneObject object2interact;

    void Start()
    {
        prompt.SetActive(true);
        board.SetActive(false);
        IsCounting = false;
        counting_n = 0;
        tap_list = new List<GameObject>();
    }
    void OnEnable()
    {
        Reset();
        loadPrompt();
    }
    private void Reset()
    {
        prompt.SetActive(true);
        board.SetActive(false);
        IsCounting = false;
        counting_n = 0;
        tap_list = new List<GameObject>();
        problemboard.SetActive(false);
        container.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsCounting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            //  UpdateInteractiveObjects();
            count_object();
            if(interaction_prompt!=null) interaction_prompt.tick();
        }
    }
    private void count_object()
    {
        if (!container || !IsCounting) return;
        List<SceneObject> object_out_of_rect = new List<SceneObject>();
        List<SceneObject> objs = container.GetComponent<ObjectContainer>().get_objects_in_rect(target_object_name, ref object_out_of_rect);
        bool[] number_label = new bool[objs.Count];
        

        foreach (SceneObject so in objs)
        {
            if (so.get_all_feedback_count() <= 0)
            {
                Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                GameObject label = FeedbackGenerator.mThis.create_check_feedback(targetPos, 1, true);
                so.attach_object(label);
                break;
            }
        }

        if (object_out_of_rect.Count == 0) object2interact = null;
        else
        {
            object2interact = object_out_of_rect[(int)Random.Range((int)0, (int)object_out_of_rect.Count - 1)];
        }


        if (objs.Count != counting_n)
        {

            counting_n = objs.Count;
            OnCount();
        }
    }
    public void OnCount()
    {

        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(counting_n + "!");

        UpdateBoard();
        //sound effect
        interaction_prompt.checkin();
        if (ContentModuleRoot.GetComponent<ContentCounting>().found_object_count == counting_n)
        {
            OnCompletion();
        }
    }
    private void OnCompletion()
    {
        IsCounting = false;
        clearinteractiveobjects();
        ContentModuleRoot.GetComponent<ContentCounting>().onSolved();
        interaction_prompt = null;
    }
    public void loadPrompt()
    {
        prompt.SetActive(true);
        //prompt_text.SetActive(true);
        target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
        prompt_text.GetComponent<Text>().text = "Let's give the minion " + target_object_name + "by moving them to the red tray";
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(prompt_text.GetComponent<Text>().text);
        IsCounting = false;
    }
    public void StartCounting()
    {
        counting_n = 0;
        IsCounting = true;
        board.SetActive(true);
        UpdateBoard();
        problemboard.SetActive(true);
        container.SetActive(true);
        interaction_prompt = new TimerCallback(Interaction_Prompt, "",SystemParam.timeout_for_interaction_prompt + 3);
    }
    public void Interaction_Prompt(string param)
    {
        //Dialogs.Prompt_RightBot("Move " + target_object_name + "s to the table");      
        if (object2interact != null) show_interaction_prompt(object2interact);

    }
    private void UpdateBoard()
    {
        int goal_n = ContentModuleRoot.GetComponent<ContentCounting>().found_object_count;
        board.GetComponent<board>().enable_both(target_object_name, counting_n, "= " + counting_n.ToString());
        //board.GetComponent<board>().setMathText("= " + counting_n.ToString());
        //board.GetComponent<board>().setIcon(target_object_name, counting_n);
        problemboard_text.GetComponent<Text>().text = "Can you get me " + goal_n + " " + target_object_name + "s ?";
    }
    private void clearinteractiveobjects()
    {
        for (int k = 0; k < tap_list.Count; k++)
        {
            GameObject.Destroy(tap_list[k]);
        }
        tap_list.RemoveAll(s => s == null);
    }
    public void show_interaction_prompt(SceneObject so)
    {
        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
        EffectControl.prompt_move_left(targetPos);
    }





}
