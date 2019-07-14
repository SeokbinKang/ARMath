using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingVirtual : MonoBehaviour {

    public GameObject prompt;
    public GameObject prompt_text;

    public GameObject board;
    public GameObject board_math_text;
    public GameObject board_icons;
    public GameObject ContentModuleRoot;
    public GameObject prefab_tap;
    
    public GameObject problemboard;
    public GameObject problemboard_text;

    public bool IsCounting;

    private int counting_n;
    private string target_object_name;
    // Use this for initialization

    private float nextActionTime = 0.0f;

    private GameObject active_interaction_prompt;
    
    private TimerCallback interaction_prompt;
    void Start () {        
        prompt.SetActive(true);
        board.SetActive(false);
        IsCounting = false;
        counting_n = 0;
        
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
        
        problemboard.SetActive(false);
        active_interaction_prompt = null;
    }
    // Update is called once per frame
    void Update () {
        if (IsCounting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            UpdateInteractiveObjects();
            interaction_prompt.tick();
        }
    }
    public void OnCount(int object_id)
    {
        
        SceneObject so = SceneObjectManager.mSOManager.get_object(object_id);
        if (so != null)
        {
            if (so.is_interactive())
            {
                so.interact();
                so.clear_feedback();
                
            }
            else return;
        } else return;
        interaction_prompt.checkin();
        counting_n++;
        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
        //GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, counting_n,true);
        GameObject label = FeedbackGenerator.mThis.create_check_feedback(targetPos, counting_n, true);
        so.attach_object(label);
        //Debug.Log("[ARMath] label " + targetPos + "  -->  " + label.GetComponent<RectTransform>().position+"  or  "+ label.GetComponent<RectTransform>().localPosition+" s  "+ label.GetComponent<RectTransform>().localScale);
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(counting_n + "!");

        UpdateBoard();
        //sound effect
      
        
        if(ContentModuleRoot.GetComponent<ContentCounting>().found_object_count<=counting_n)
        {
            OnCompletion();
        }
    }
    private void OnCompletion()
    {
        IsCounting = false;
        clearinteractiveobjects();
        ContentModuleRoot.GetComponent<ContentCounting>().onSolved();
    }
    public void loadPrompt()
    {
        prompt.SetActive(true);
        //prompt_text.SetActive(true);
        target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
        prompt_text.GetComponent<Text>().text = "Let's collect " + target_object_name+ "s for the minion by TAPPING them on the screen";
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
        interaction_prompt = new TimerCallback(Interaction_Prompt, "",SystemParam.timeout_for_interaction_prompt);
        active_interaction_prompt = null;
    }

    private void UpdateBoard()
    {
        int goal_n = ContentModuleRoot.GetComponent<ContentCounting>().found_object_count;
        board.GetComponent<board>().enable_both(target_object_name, counting_n, "= " + counting_n.ToString());
        /*board.GetComponent<board>().setMathText("= " + counting_n.ToString());
        board.GetComponent<board>().setIcon(target_object_name, counting_n);*/
        problemboard_text.GetComponent<Text>().text = "Can you get me " + goal_n + " " + target_object_name + "s ?";
    }
    private void clearinteractiveobjects()
    {
        interaction_prompt = null;
       
    }
    public void Interaction_Prompt(string param)
    {
        if(active_interaction_prompt!=null)
        {
            Color c = active_interaction_prompt.GetComponent<Image>().color;
            c.a = 0;
            active_interaction_prompt.GetComponent<Image>().color = c;
            active_interaction_prompt = null;
        }
        List<SceneObject> scene_objects = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        foreach (SceneObject i in scene_objects)
        {
            if (i.is_interactive() && i.is_feedback_attached())
            {
                active_interaction_prompt = i.attached_button_visibility(1f);
                if (active_interaction_prompt != null)
                {
                    return;
                }

            }
        }

    }
    private void UpdateInteractiveObjects()
    {
        if (prefab_tap == null || !IsCounting) return;


        List<SceneObject> scene_objects = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);

   
        SceneObject target = null;
        foreach (SceneObject i in scene_objects)
        {
            if (i.is_interactive() && !i.is_feedback_attached())
            {
                generateObjectPop(i);

            }           
        }

      
    }
    private void generateObjectPop(SceneObject i)
    {
        
        Vector3 targetPos = new Vector3(i.catalogInfo.Box.center.x, Screen.height - i.catalogInfo.Box.center.y, 0);
        UnityEngine.GameObject label = Instantiate(prefab_tap, targetPos, Quaternion.identity) as GameObject;
        //TODO increase the object size
        if (label == null) return;
        int scene_object_id = i.id;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        label.GetComponent<RectTransform>().position = r.position;
        label.transform.SetParent(this.gameObject.transform);
        Color c = label.GetComponent<Image>().color;
        c.a = 0;
        label.GetComponent<Image>().color = c;
        label.GetComponent<Button>().onClick.AddListener(() => { this.OnCount(scene_object_id); });
        i.attach_object(label);
    }
    /******* DEPRECATED *******/
    
}
