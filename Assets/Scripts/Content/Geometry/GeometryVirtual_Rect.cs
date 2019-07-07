using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeometryVirtual_Rect : MonoBehaviour {


   


    public GameObject board;


    public GameObject container;
    public GameObject geoprimitives;


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
        
        
        container.SetActive(false);
        geoprimitives.SetActive(false);
        
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
    public void OnNextStep(string step)
    {
        mStep = Convert.ToInt32(step);
    }
    private void processStep()
    {
        string target_object_name = ContentModuleRoot.GetComponent<ContentGeometry>().target_object_name;
        string target_shape_name = ARMathUtils.shape_name(ContentModuleRoot.GetComponent<ContentGeometry>().target_object_shape);
        if (mStep == 0)
        {  //DEPRECATED
                
            
            
            container.SetActive(false);
            geoprimitives.SetActive(false);
           
            /*

            if(prompt_text.GetComponent<Text>().text!= "Let's find vertics, sides, and angles in the " + target_object_name ){
                prompt_text.GetComponent<Text>().text = "Let's find vertics, sides, and angles in the " + target_object_name ;
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(prompt_text.GetComponent<Text>().text);
            }*/
            
            return;
        }
        if (mStep == 2)
        {
            
            container.SetActive(true);


            //geoprimitives.SetActive(true);
            container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.vertex);

            
          
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Let's count how many vertices the "+ target_shape_name + " have. You can tap them on the screen.",
                true,
                new CallbackFunction(ShowProblem),
                "How many vertices are there in a rectangle?",
                5
                ));

        }

        if(mStep == 4)
        {
            container.SetActive(true);


            //geoprimitives.SetActive(true);
            container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.side_short);


            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Let's find a pair of short sides in the "+ target_shape_name+". You can tap them on the screen",
                true,
                new CallbackFunction(ShowProblem),
                "Where are two short sides in a "+target_shape_name+"?",
                5
                ));
        }
        if (mStep == 5)
        {
            container.SetActive(true);


            //geoprimitives.SetActive(true);
            container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.side_long);


            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Let's find a pair of long sides in the " + target_shape_name + ". You can tap them on the screen",
                true,
                new CallbackFunction(ShowProblem),
                "Where are two long sides in a " + target_shape_name + "?",
                5
                ));
        }

        /*
        if (mStep == 6)
        {
          

            board.SetActive(false);
            board.GetComponent<board>().setMathText("Vertices:4 \nSides: 4\n Angles: ?");


            container.SetActive(true);


            geoprimitives.SetActive(true);


            

            if (problemboard_text.GetComponent<Text>().text != "What do the sides of " + target_object_name + " rectangle look like? ")
            {
                problemboard_text.GetComponent<Text>().text = "What do the sides of " + target_object_name + " rectangle look like? ";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
             

            }

        }*/

        if (mStep == 8)
        {
          

            container.SetActive(true);


            geoprimitives.SetActive(true);


            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Let's measure the angles using a protractor and find their names.",
                true,
                new CallbackFunction(ShowProblem),
                "What are the names for the angles in the " + target_shape_name + "?",
                5
                ));
            geoprimitives.GetComponent<GridPrimitives>().Reset(GeometryPrimitives.angle);

            /*
            problemboard.SetActive(true);
            container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.angle);
            if (problemboard_text.GetComponent<Text>().text != "What are the angles of the corners in " + target_object_name + "?")
            {
                problemboard_text.GetComponent<Text>().text = "What are the angles of the corners in " + target_object_name + "?";
                TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(problemboard_text.GetComponent<Text>().text);
                geoprimitives.GetComponent<GridPrimitives>().Reset(GeometryPrimitives.angle);
               
            } */

        }

        if (mStep == 10)
        {
            OnCompletion();
        }
    }
    public void ShowProblem(string txt)
    {
        Dialogs.set_topboard(true, txt);
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
        
        
        container.SetActive(true);
        geoprimitives.SetActive(true);
        
        arrange_movable_objects();
    }

   
}
