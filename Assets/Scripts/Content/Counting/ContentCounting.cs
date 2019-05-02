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
        SceneObjectManager.mSOManager.Reset();
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help the minion solve counting problems and collect red gems!");
    }
    public void onSolved()
    {
        //sub_virtualsolver.SetActive(false);
        sub_ceremony.SetActive(true);
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p1_counting);
        is_solved = true;
        Debug.Log("Solved: " + target_object_name + "  " + found_object_count);
    }
    public void UpdateExplorer()
    {
        System.Random random = new System.Random();
       
        string dominant_object_name = "";
        Vector2 center_of_objects = new Vector2(0,0);
        int object_count = 0;
        SceneObjectManager.mSOManager.get_dominant_object(ref dominant_object_name, ref center_of_objects, ref object_count);
        if (is_solved || sub_intro.activeSelf)
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
            int randomNumber = random.Next(object_count-3, object_count);
            if (randomNumber < 1) randomNumber = object_count;
            found_object_count = randomNumber;
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
      
        


    }
    public void SetIdel(bool t)
    {
        is_idle = false;
    }
}
