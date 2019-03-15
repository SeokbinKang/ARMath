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
    private string object_name;
    // Use this for initialization
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
	}
    public void OnCount()
    {        
        counting_n++;
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
        string target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
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
        string target_object_name = ContentModuleRoot.GetComponent<ContentCounting>().target_object_name;
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
