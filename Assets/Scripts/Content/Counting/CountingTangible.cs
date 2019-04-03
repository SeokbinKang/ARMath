using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingTangible : MonoBehaviour {

    public GameObject prompt;
    
    public GameObject prompt_text;


    public GameObject board;
   

    public GameObject container;



    public GameObject ContentModuleRoot;
    public GameObject prefab_tap;
    private List<GameObject> tap_list;


    public bool IsCounting;

    private int counting_n;
    private string target_object_name;
    // Use this for initialization

    private float nextActionTime = 0.0f;

    
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
        }
    }
    private void count_object()
    {
        if (!container || !IsCounting) return;
        List<SceneObject> objs = container.GetComponent<ObjectContainer>().get_objects_in_rect();

        
        if (objs.Count> counting_n)
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

        if (ContentModuleRoot.GetComponent<ContentCounting>().found_object_count <= counting_n)
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
        prompt_text.GetComponent<Text>().text = "Let's give the minion "+ target_object_name + "by moving them to the red tray";
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
        board.GetComponent<board>().setMathText("= " + counting_n.ToString());
        board.GetComponent<board>().setIcon(target_object_name, counting_n);
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

        List<SceneObject> scene_objects = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);

        foreach (SceneObject i in scene_objects)
        {
            bool duplicate = false;
            for (int k = 0; k < tap_list.Count; k++)
            {
                Vector3 targetPos = new Vector3(i.catalogInfo.Box.center.x, Screen.height - i.catalogInfo.Box.center.y, 0);
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


    public void updateInteractiveObjects(List<CatalogItem> objs)
    {
        if (prefab_tap == null || !IsCounting) return;

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
    private void generateObjectPop(SceneObject i)
    {

        Vector3 targetPos = new Vector3(i.catalogInfo.Box.center.x, Screen.height - i.catalogInfo.Box.center.y, 0);

        UnityEngine.GameObject label = Instantiate(prefab_tap, targetPos, Quaternion.identity) as GameObject;
        Debug.Log("[ARMath] Generating tap Obejcts" + targetPos);
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        label.GetComponent<RectTransform>().position = r.position;
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<Button>().onClick.AddListener(() => { this.OnCount(); });
        tap_list.Add(label);
    }

    private void generateObjectPop(CatalogItem i)
    {

        Vector3 targetPos = new Vector3(i.Box.center.x, Screen.height - i.Box.center.y, 0);

        UnityEngine.GameObject label = Instantiate(prefab_tap, targetPos, Quaternion.identity) as GameObject;
        Debug.Log("[ARMath] Generating tap Obejcts" + targetPos);
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        label.GetComponent<RectTransform>().position = r.position;
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<Button>().onClick.AddListener(() => { this.OnCount(); });
        tap_list.Add(label);
    }
}
