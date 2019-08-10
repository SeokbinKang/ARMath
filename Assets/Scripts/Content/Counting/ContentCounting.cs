using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentCounting : MonoBehaviour, IContentModule
{
    // Use this for initialization
    public GameObject sub_intro;
    public GameObject sub_explorer;
    public GameObject sub_opener;
    public GameObject sub_solver;
    public GameObject sub_context;
    
    public GameObject sub_review;


    public string[] countable_objects;

    public List<Vector2> obj_pos_list;
    public string target_object_name = "";
    public int object_count = 0;
    public string obj_name_plural;
    

    private bool is_idle = true;
    private bool is_solved = false;

    private float nextActionTime = 0.0f;
    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            //UpdateExplorer();
        }
    }
    void OnEnable()
    {
        Reset();
        load_prompt();
    }
    public void Reset()
    {
        sub_intro.SetActive(false);
        sub_explorer.SetActive(false);
        sub_opener.SetActive(false);
        sub_solver.SetActive(false);        
        sub_review.SetActive(false);
        
        sub_context.SetActive(false);
        is_idle = true;
        is_solved = false;
        SceneObjectManager.mSOManager.Reset();
        if (obj_pos_list == null) obj_pos_list = new List<Vector2>();
        else obj_pos_list.Clear();
        
    }
    public void load_prompt()
    {
        
        System.Random random = new System.Random();
        if (countable_objects == null || countable_objects.Length == 0)
        {
            SystemControl.onQuestionGlobal();
            return;
        }
        int randomNumber = random.Next(0,countable_objects.Length-1);
        target_object_name = countable_objects[randomNumber];
        Debug.Log("[ARMath] counting target " + target_object_name);
        obj_name_plural = AssetManager.Get_object_text(target_object_name, 2);
        obj_name_plural = obj_name_plural.Substring(obj_name_plural.IndexOf(' ') + 1);

      
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
          "Can you find some " + obj_name_plural+"?",
          true,
          new CallbackFunction(s1_find_objects),
          "",
          4), 0
          );

    }
    public void s1_find_objects(string t)
    {
        if (is_solved)
        {
            return;
        }
        
        Tools.finder_init(target_object_name, 1, new CallbackFunction2(s2_objectfound), "", "Let's find some "+ obj_name_plural, 1f);
    }
    public void s2_objectfound(string p, List<SceneObject> obj_list, Rect rt)
    {

        if (is_solved)
        {
            return;
        }
        SetIdle(false);

        object_count = obj_list.Count;
        foreach(var so in obj_list)
        {
            obj_pos_list.Add(so.get_screen_pos());
        }
        StartSolver();
    }
    public void StartSolver()
    {
        //sub_opener.SetActive(false);

        sub_solver.SetActive(true);
    }
    public void UpdateExplorer()
    {
    }
    public void onSolved()
    { }


        /*
        public void onSolved()
        {
            //sub_virtualsolver.SetActive(false);

            sub_ceremony.SetActive(true);
            EffectControl.ballon_ceremony();
            EffectControl.gem_ceremony(ProblemType.p1_counting);

            is_solved = true;

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
                this.object_count = randomNumber;
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




        }*/
        public void SetIdle(bool t)
    {
        is_idle = false;
    }
}
