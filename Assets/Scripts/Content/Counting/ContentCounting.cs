using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentCounting : MonoBehaviour, IContentModule
{
    // Use this for initialization
    public GameObject sub_intro;
    public GameObject sub_explorer;
    public GameObject sub_opener;
    public GameObject sub_virtualsolver;
    
    public GameObject sub_ceremony;
    public GameObject sub_review;

    public string target_object_name = "";
    public int found_object_count = 0;

    
    private bool is_idle = true;
    private bool is_solved = false;

    private float nextActionTime = 0.0f;
    
    void Start()
    {
        sub_intro.SetActive(true);
        sub_explorer.SetActive(false);
        sub_opener.SetActive(false);
        sub_virtualsolver.SetActive(false);
        
        sub_review.SetActive(false);
        sub_ceremony.SetActive(false);
        is_idle = true;
        is_solved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            UpdateExplorer();
        }
    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        sub_intro.SetActive(true);
        sub_explorer.SetActive(false);
        sub_opener.SetActive(false);
        sub_virtualsolver.SetActive(false);
        
        sub_review.SetActive(false);
        sub_ceremony.SetActive(false);
        is_idle = true;
        is_solved = false;

        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help a minion solve counting problems and collect red gems!");
    }
    public void onSolved()
    {
        //sub_virtualsolver.SetActive(false);
        sub_ceremony.SetActive(true);
        SystemUser.AddGem(ProblemType.p1_counting);
        is_solved = true;
        Debug.Log("Solved: " + target_object_name + "  " + found_object_count);
    }
    public void UpdateExplorer()
    {
        string dominant_object_name = "";
        Vector2 center_of_objects = new Vector2(0,0);
        int object_count = 0;
        SceneObjectManager.mSOManager.get_dominant_object(ref dominant_object_name, ref center_of_objects, ref object_count);
        if (is_solved)
        {
            sub_explorer.SetActive(false);
            return;
        }

        if (is_idle)
        {
            if (dominant_object_name == "")
            {
                sub_explorer.SetActive(false);
                return;
            }
            target_object_name = dominant_object_name;
            found_object_count = object_count;
            //pops up explorer
            sub_explorer.SetActive(true);
            RectTransform r = sub_explorer.GetComponent<RectTransform>();
            r.position = new Vector3(center_of_objects.x, Screen.height - center_of_objects.y, 0);

        } else
        {
            sub_explorer.SetActive(false);

        }
    }
    public void UpdateCVResult(CVResult cv)
    {
        if (cv.mObjects==null || is_solved)
        {
            sub_explorer.SetActive(false);
            return;
        }
        //find the type of dominant objects
        Dictionary<string, Vector2> center = new Dictionary<string, Vector2>();
        Dictionary<string, int> counter = new Dictionary<string, int>();
        foreach (CatalogItem item in cv.mObjects)
        {
            if (counter.ContainsKey(item.DisplayName))
            {
                counter[item.DisplayName]++;
            }
            else
            {
                counter[item.DisplayName] = 1;
                center[item.DisplayName] = new Vector2(0, 0);
            }
            center[item.DisplayName] += item.Box.center;
        }
        int max_freq = 0;
        string dominant_object_name = "";
        foreach (KeyValuePair<string, int> kvp in counter)
        {
            if (kvp.Value > max_freq)
            {
                max_freq = kvp.Value;
                dominant_object_name = kvp.Key;
            }
        }
      
        if (is_idle)
        {
            //explorer
            if (dominant_object_name == "")
            {
                sub_explorer.SetActive(false);
                return;
            }
            target_object_name = dominant_object_name;
            found_object_count = counter[dominant_object_name];

            Vector2 center_of_objects = center[dominant_object_name] / ((float)counter[dominant_object_name]);

            //pops up explorer
            sub_explorer.SetActive(true);
            RectTransform r = sub_explorer.GetComponent<RectTransform>();
            r.position = new Vector3(center_of_objects.x, Screen.height - center_of_objects.y, 0);
            //Debug.Log("[ARMATH] : " + box.center.x + "  " + box.center.y);
            //Debug.Log("GEM screen : " + Screen.width + "  " + Screen.height);
        }
        else
        {
            sub_explorer.SetActive(false);
            // active. pass the positions of objects to the counting unit.
            if (sub_virtualsolver.activeSelf)
            {
                List<CatalogItem> target_objects = new List<CatalogItem>();
                
                foreach (CatalogItem item in cv.mObjects)
                {
                    if (item.DisplayName == target_object_name) target_objects.Add(item);
                }
             //   Debug.Log("[ARMath] Passing objects list to sub_virtualsolver" + target_objects.Count);
                sub_virtualsolver.GetComponent<CountingVirtual>().updateInteractiveObjects(target_objects);

            }
        }
        


    }
    public void SetIdel(bool t)
    {
        is_idle = false;
    }
}
