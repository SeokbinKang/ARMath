using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionTangible : MonoBehaviour
{

    public GameObject prompt;

    public GameObject prompt_text;


    public GameObject board;


    public GameObject container;



    public GameObject ContentModuleRoot;
    
    private List<GameObject> tap_list;


    public bool IsAdding;

    private int total_n;
    private string target_object_name;
    // Use this for initialization

    private float nextActionTime = 0.0f;


    void Start()
    {
        prompt.SetActive(true);
        board.SetActive(false);
        IsAdding = false;
        total_n = 0;
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
        IsAdding = false;
        total_n = 0;
        tap_list = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsAdding) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            //  UpdateInteractiveObjects();
            count_object();
        }
    }
    private void count_object()
    {
        if (!container || !IsAdding) return;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int cur_n = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count;
        List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        bool[] number_label = new bool[objs.Count];
        for (int i = 0; i < number_label.Length; i++)
        {
            int number_label_value = objs[i].get_number_feedback();
            if (number_label_value > 0 && number_label_value <= number_label.Length)
            {
                
                number_label[number_label_value - 1] = true;
            }
        }
        for (int i = 0; i < init_n && i< number_label.Length; i++)
        {
            //attach invisible marker to the initial set of objects
            if (!number_label[i])
            {
                foreach (SceneObject so in objs)
                {
                    if (so.get_number_feedback() <= 0)
                    {
                        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                        GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, i + 1,true);
                        so.attach_object(label);
                        break;
                    }
                }
            }
            
        }
        for (int i = init_n + 1; i <= goal_n &&  i <= number_label.Length; i++)
        {
            if (!number_label[i - 1])
            {
                foreach (SceneObject so in objs)
                {
                    if (so.get_number_feedback() <= 0)
                    {
                        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                        GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, i, true);
                        so.attach_object(label);
                        Debug.Log("[ARMath] generating O sticker");
                        label = FeedbackGenerator.mThis.create_sticker_ox(targetPos, true, true);
                        so.attach_object(label);


                        break;
                    }
                }
            }
        }
        for (int i = goal_n+1; i <= number_label.Length; i++)
        { //attach X maker to added (extra) objects
            if (!number_label[i - 1])
            {
                foreach (SceneObject so in objs)
                {
                    if (so.get_number_feedback() <= 0)
                    {
                        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                        GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, i, false);
                        so.attach_object(label);
                        Debug.Log("[ARMath] generating X sticker");
                        label = FeedbackGenerator.mThis.create_sticker_ox(targetPos, false, true);
                        so.attach_object(label);
                        break;
                    }
                }
            }

        }


        if (objs.Count !=cur_n)
        {
            cur_n = objs.Count;
            ContentModuleRoot.GetComponent<ContentAddition>().current_object_count = cur_n;
            OnCount();
        }
    }
    public void OnCount()
    {

        int added = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count - ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        if (added > 0) TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(added + " " + target_object_name + "s added!");
        else TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("please add more " + target_object_name + "s!");

        UpdateBoard();
        //sound effect

        if (ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count <= ContentModuleRoot.GetComponent<ContentAddition>().current_object_count)
        {
            OnCompletion();
        }
    }
    private void OnCompletion()
    {
        IsAdding = false;
        clearinteractiveobjects();
        ContentModuleRoot.GetComponent<ContentAddition>().onSolved();
    }
    public void loadPrompt()
    {
        prompt.SetActive(true);
        //prompt_text.SetActive(true);
        target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        prompt_text.GetComponent<Text>().text = "Let's give the minion more " + target_object_name + "s by adding them on the table";
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(prompt_text.GetComponent<Text>().text);
        IsAdding = false;
    }
    public void StartOperation()
    {
        total_n = 0;
        IsAdding = true;
        board.SetActive(true);
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int cur_n = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count;
        string sign = " + ";
        if (cur_n - init_n < 0) sign = " - ";
            board.GetComponent<board>().setMathText(init_n + sign + System.Math.Abs(cur_n-init_n) + " = " + cur_n);
        
    }
    private void clearinteractiveobjects()
    {
        for (int k = 0; k < tap_list.Count; k++)
        {
            GameObject.Destroy(tap_list[k]);
        }
        tap_list.RemoveAll(s => s == null);
    }





}
