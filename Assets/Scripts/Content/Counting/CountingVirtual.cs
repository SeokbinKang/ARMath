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
    private List<GameObject> tap_list;


    public bool IsCounting;

    private int counting_n;
    private string target_object_name;
    // Use this for initialization

    private float nextActionTime = 0.0f;
    

    void Start () {        
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
    }
    // Update is called once per frame
    void Update () {
        if (IsCounting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            UpdateInteractiveObjects();
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
        }
        else return;
        counting_n++;
        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
        GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, counting_n,true);
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
    }

    private void UpdateBoard()
    {
        board.GetComponent<board>().enable_both(target_object_name, counting_n, "= " + counting_n.ToString());
        /*board.GetComponent<board>().setMathText("= " + counting_n.ToString());
        board.GetComponent<board>().setIcon(target_object_name, counting_n);*/
    }
    private void clearinteractiveobjects()
    {
        for (int k = 0; k < tap_list.Count; k++)
        {  
                GameObject.Destroy(tap_list[k]);
        }
        tap_list.RemoveAll(s => s == null);
    }
    private void UpdateInteractiveObjects()
    {
        if (prefab_tap == null || !IsCounting) return;


        List<SceneObject> scene_objects = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);

        bool interaction_indicator_exists = false;
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
        
        if (label == null) return;
        int scene_object_id = i.id;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        label.GetComponent<RectTransform>().position = r.position;
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<Button>().onClick.AddListener(() => { this.OnCount(scene_object_id); });
        i.attach_object(label);
    }
    /******* DEPRECATED *******/
    public void updateInteractiveObjects(List<CatalogItem> objs)
    {
        if (prefab_tap == null || !IsCounting ) return;

        bool[] is_valid;
        if (tap_list.Count > 0)
        {
            is_valid = new bool[tap_list.Count];            
            for (int k = 0; k < tap_list.Count; k++)
            {
                is_valid[k] = false;
            }
        }
        else is_valid = null;

        foreach (CatalogItem i in objs)
        {
            bool duplicate = false;
            for (int k = 0; k < tap_list.Count; k++)
            {
                Vector3 targetPos = new Vector3(i.Box.center.x, Screen.height - i.Box.center.y, 0);
                Vector3 dist = tap_list[k].GetComponent<RectTransform>().position - targetPos;
                if (dist.magnitude < 150)
                {
                    duplicate = true;
                    is_valid[k] = true;
                }

            }
            if (!duplicate)
            {
                generateObjectPop(i);
            }

        }

        for (int k = 0; k < is_valid.Length; k++)
        {
            if (!is_valid[k])
            {
                GameObject.Destroy(tap_list[k]);
                tap_list[k] = null;
            }
        }
        tap_list.RemoveAll(s => s == null);

        foreach (CatalogItem i in objs)
        {
            bool duplicate = false;
            for (int k = 0; k < tap_list.Count; k++)
            {
                Vector3 targetPos = new Vector3(i.Box.center.x, Screen.height - i.Box.center.y, 0);
                Vector3 dist = tap_list[k].GetComponent<RectTransform>().position - targetPos;
                if (dist.magnitude < 150)
                {
                    duplicate = true;
                    is_valid[k] = true;
                }

            }
            if (!duplicate)
            {
                generateObjectPop(i);
            }

        }
        
        for (int k = 0; k < is_valid.Length; k++)
        {
            if (!is_valid[k])
            {
                GameObject.Destroy(tap_list[k]);
                tap_list[k] = null;                
            }
        }
        tap_list.RemoveAll(s => s == null);
    }
    

    /******* DEPRECATED *******/
    private void generateObjectPop(CatalogItem i)
    {          
        Vector3 targetPos = new Vector3(i.Box.center.x, Screen.height - i.Box.center.y, 0);

        UnityEngine.GameObject label = Instantiate(prefab_tap, targetPos, Quaternion.identity) as GameObject;
        Debug.Log("[ARMath] Generating tap Obejcts" + targetPos);
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        label.GetComponent<RectTransform>().position = r.position;
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<Button>().onClick.AddListener(() => { this.OnCount(0); });
        tap_list.Add(label);
    }
}
