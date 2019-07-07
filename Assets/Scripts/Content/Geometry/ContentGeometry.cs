using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentGeometry : MonoBehaviour {


    public GameObject sub_intro;
    public GameObject sub_explorer;

    
    public GameObject sub_builder;
    public GameObject sub_helper;
    public GameObject sub_solver;

    public GameObject sub_ceremony;
    public GameObject sub_review;

    
    public string target_object_name = "";
    public GeometryShapes target_object_shape= GeometryShapes.Rectangle;
    public Rect target_object_rect;
    public Rect final_object_shape_rect;
   



    private bool is_idle = true;
    private bool is_solved = false;

    private float nextActionTime = 0.0f;
    public string[] rectangle_objects;
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
            s1_UpdateExplorer();
        }
    }
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        Drawing2D.Reset();
        sub_intro.SetActive(true);
        sub_explorer.SetActive(false);

        sub_solver.SetActive(false);
        sub_review.SetActive(false);
        sub_ceremony.SetActive(false);
        sub_helper.SetActive(false);
        sub_builder.SetActive(false);
        is_idle = true;
        is_solved = false;    
        SceneObjectManager.mSOManager.Reset();
        TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Help the minion solve geometry problems and collect blue gems!");
    }
    public void onSolved()
    {
        sub_solver.SetActive(false);
        sub_ceremony.SetActive(true);
        EffectControl.ballon_ceremony();
        EffectControl.gem_ceremony(ProblemType.p4_geometry);
        is_solved = true;

    }
    public void s1_UpdateExplorer()
    {
        System.Random random = new System.Random();

        string dominant_object_name = "";
        Vector2 center_of_objects = new Vector2(0, 0);
        int object_count = 0;

        if (is_solved || sub_intro.activeSelf || !is_idle)
        {
            sub_explorer.SetActive(false);
            return;
        }


        List<string> target_rectangle_objects = new List<string>(this.rectangle_objects);
        List<SceneObject> geometry_objects;
        geometry_objects = SceneObjectManager.mSOManager.get_objects_by_name(target_rectangle_objects);
        if (geometry_objects.Count == 0)
        {
            sub_explorer.SetActive(false);
            return;
        }
        if (is_idle)
        {           
            target_object_name = geometry_objects[0].catalogInfo.DisplayName;
            center_of_objects= geometry_objects[0].catalogInfo.Box.center;
            target_object_rect = geometry_objects[0].catalogInfo.Box;
            target_object_shape = GeometryShapes.Rectangle;
            //Debug.Log("[ARMath] geometry object target : " + target_object_name);
            bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
            if (interaction_touch_enalbed)
            {                
            }
            else
            {                
            }
            //pops up explorer
            if(sub_explorer.activeSelf==false)
            {
                //show prompt for the first explorer
                Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Oh! There is something look like a " + ARMathUtils.shape_name(target_object_shape)+". Can you tap it on the screen?",
                true,
                null,
                "",
                10
                ));
            }
            sub_explorer.SetActive(true);
            RectTransform r = sub_explorer.GetComponent<RectTransform>();
            r.position = new Vector3(center_of_objects.x, Screen.height - center_of_objects.y, 0);

        }
        else
        {
            sub_explorer.SetActive(false);

        }
    }
    public void s2_OnExplorer()
    {
        SetIdle(false);
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Oh! That "+ target_object_name+" looks like a " + ARMathUtils.shape_name(target_object_shape),
                true,
                null,
                "",
                4
                ));
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Let's draw the "+ ARMathUtils.shape_name(target_object_shape) +" on the screen. ",
                true,
                new CallbackFunction(s3_findtheshape),
                ARMathUtils.shape_name(target_object_shape)
                ));


    }
    public void s3_findtheshape(string param)
    {
        sub_builder.SetActive(true);
        sub_builder.GetComponent<ShapeBuilder>().shape = target_object_shape;
    }
    public void s4_startsolver()
    {
        sub_builder.SetActive(false);
        sub_solver.SetActive(true);
    }
  
    public void SetIdle(bool t)
    {
        is_idle = t;
    }
}
