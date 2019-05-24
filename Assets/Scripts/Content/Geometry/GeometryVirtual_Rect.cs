using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeometryVirtual_Rect : MonoBehaviour {


    public GameObject prompt;

    public GameObject prompt_text;


    public GameObject board;


    public GameObject container;
    public GameObject geoprimitives;
    public GameObject problemboard;
    public GameObject problemboard_text;

    public GameObject ContentModuleRoot;

    

    


    public bool UserInteracting;

    private int total_n;
    private string target_object_name;
    // Use this for initialization

    private float nextActionTime = 0.0f;

    public int mStep = 0;
    /*** mStep def
     * 0: show the initial prompt
     * 1: prompt for vertex
     * 2: work for vertex
     * 3: prompt for sides
     * 4: work for sides
     * 5: prompt for angles
     * 6: work for angles
     ***/
    void Start()
    {

    }
    void OnEnable()
    {
        Debug.Log("[ARMath] Solver started");
        Reset();
     
    }
    private void Reset()
    {
        prompt.SetActive(true);
        board.SetActive(false);
        container.SetActive(false);
        geoprimitives.SetActive(false);
        problemboard.SetActive(false);
        UserInteracting = false;
        total_n = 0;
      
        // arrange_movable_objects();

        mStep = 0;

    }
    // Update is called once per frame
    void Update()
    {
        //        if (UserInteracting) UpdateBoard();//
        processStep();


    }
    public void nextStep(int step)
    {
        mStep = step;
    }
    private void processStep()
    {
        string target_object_name = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_name;
        Debug.Log("[ARMath] Solver step: "+mStep);
        if (mStep == 0)
        {
            prompt.SetActive(true);
            board.SetActive(false);
            container.SetActive(false);
            geoprimitives.SetActive(false);
            problemboard.SetActive(false);
            if(prompt_text.GetComponent<Text>().text!= "Let's find vertics, sides, and angles in the " + target_object_name + " by dragging the icons"){
                prompt_text.GetComponent<Text>().text = "Let's find vertics, sides, and angles in the " + target_object_name + " by dragging the icons";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(prompt_text.GetComponent<Text>().text);
            }
            
            return;
        }
        if (mStep == 2)
        {
            prompt.SetActive(false);

            board.SetActive(false);
            board.GetComponent<board>().setMathText("Vertices:4 \nSides: ?\n Angles: ?");

            
            container.SetActive(true);


            geoprimitives.SetActive(true);

    
            problemboard.SetActive(true);
            if (problemboard_text.GetComponent<Text>().text != "Where are the vertices in the " + target_object_name + " rectangle?")
            {
                problemboard_text.GetComponent<Text>().text = "Where are the vertices in the " + target_object_name + " rectangle?";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
            }

            

        }

        if(mStep == 4)
        {
            prompt.SetActive(false);

            board.SetActive(false);
            board.GetComponent<board>().setMathText("Vertices:4 \nSides: 2\n Angles: ?");


            container.SetActive(true);


            geoprimitives.SetActive(true);


            problemboard.SetActive(true);

            if (problemboard_text.GetComponent<Text>().text != "What do the sides of " + target_object_name + " rectangle look like?")
            {
                problemboard_text.GetComponent<Text>().text = "What do the sides of " + target_object_name + " rectangle look like?";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
                geoprimitives.GetComponent<GridPrimitives>().Reset(GeometryPrimitives.side_horizontal);

            }
            
        }
        if (mStep == 6)
        {
            prompt.SetActive(false);

            board.SetActive(false);
            board.GetComponent<board>().setMathText("Vertices:4 \nSides: 4\n Angles: ?");


            container.SetActive(true);


            geoprimitives.SetActive(true);


            problemboard.SetActive(true);

            if (problemboard_text.GetComponent<Text>().text != "What do the sides of " + target_object_name + " rectangle look like? ")
            {
                problemboard_text.GetComponent<Text>().text = "What do the sides of " + target_object_name + " rectangle look like? ";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
                geoprimitives.GetComponent<GridPrimitives>().Reset(GeometryPrimitives.side_vertical);

            }

        }

        if (mStep == 8)
        {
            prompt.SetActive(false);

            board.SetActive(false);
            board.GetComponent<board>().setMathText("Vertices:4 \nSides: 4\n Angles: ?");


            container.SetActive(true);


            geoprimitives.SetActive(true);


            problemboard.SetActive(true);

            if (problemboard_text.GetComponent<Text>().text != "What are the angles of the corners in " + target_object_name + "?")
            {
                problemboard_text.GetComponent<Text>().text = "What are the angles of the corners in " + target_object_name + "?";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
                geoprimitives.GetComponent<GridPrimitives>().Reset(GeometryPrimitives.angle);

            }

        }
        if (mStep == 8)
        {
            prompt.SetActive(false);

            board.SetActive(true);
            board.GetComponent<board>().setMathText("Vertices:4 \nSides: 4\n Angles: 90 ");


            container.SetActive(true);


            geoprimitives.SetActive(true);


            problemboard.SetActive(true);

            if (problemboard_text.GetComponent<Text>().text != "What are the angles of the corners in " + target_object_name + "?")
            {
                problemboard_text.GetComponent<Text>().text = "What are the angles of the corners in " + target_object_name + "?";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
                geoprimitives.GetComponent<GridPrimitives>().Reset(GeometryPrimitives.angle);

            }

        }

        if (mStep == 10)
        {
            OnCompletion();
        }
    }
    private void arrange_movable_objects()
    {
        target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        GameObject icon_obj = AssetManager.get_icon(target_object_name);
        System.Random random = new System.Random();
        float w = icon_obj.GetComponent<RawImage>().texture.width;
        float h = icon_obj.GetComponent<RawImage>().texture.height;
        h = h * 150 / w;
        w = 150;
       
    }
    
    public void OnCount()
    {
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int added = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count - init_n;
        if (added > 0) TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(added + " " + target_object_name + "s added!");
        else TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("please add more " + target_object_name + "s!");

        UpdateBoard();
        //sound effect
        //Debug.Log("[ARMath] result " + ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count + "  =? " + ContentModuleRoot.GetComponent<ContentAddition>().current_object_count);
        if (added == ContentModuleRoot.GetComponent<ContentAddition>().add_object_count)
        {
            OnCompletion();
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Nice Job! The answer is " + (init_n + added) + "!");
        }
    }
    private void OnCompletion()
    {
        UserInteracting = false;
        
        ContentModuleRoot.GetComponent<ContentGeometry>().onSolved();
    }
  
    public void StartOperation()
    {
        total_n = 0;
        UserInteracting = true;
        board.SetActive(true);
        UpdateBoard();
        container.SetActive(true);
        geoprimitives.SetActive(true);
        problemboard.SetActive(true);
        arrange_movable_objects();
    }

    private void UpdateBoard()
    {
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int cur_n = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count;
        int add_n = ContentModuleRoot.GetComponent<ContentAddition>().add_object_count;
        string sign = " + ";
        if (cur_n - init_n < 0) sign = " - ";
        board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
        problemboard_text.GetComponent<Text>().text = "If we add " + add_n + " " + target_object_name + "s to " + init_n + " " + target_object_name + "s, \nhow many are there in total?";

    }
   
}
