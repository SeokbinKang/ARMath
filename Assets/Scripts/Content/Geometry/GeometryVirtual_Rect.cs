using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeometryVirtual_Rect : MonoBehaviour {


   


    


    public GameObject container;
    public GameObject geoprimitives;

    public GameObject blacksmith;
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
        blacksmith.SetActive(true);
        // arrange_movable_objects();
        mStep = 2;

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
      //  Debug.Log("[ARMath] rectangl step " + mStep);
        if (mStep == 0)
        {  //DEPRECATED

       


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
            Dialogs.set_topboard_animated(false, 3, "");

            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Hmm... I need to know more about the rectangle. Can you help me?",
                true,
                null,
                "",
                6
                ),
                2);

            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,                
                "Where are vertices of the "+target_shape_name + "? Can you point to them on the screen?",
                true,
                new CallbackFunction(ShowProblem),
                "Where are the vertices?",
                5
                ),
                0);
            mStep = 3;

        }

        if(mStep == 4)
        {
            container.SetActive(true);
            blacksmith.GetComponent<Animator>().SetTrigger("hammer");
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Thank you! let me start making the vertices of the key");
            //geoprimitives.SetActive(true);
            container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.side_short);

            Dialogs.set_topboard_animated(false, 3, "");
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Can you point to the two shorter sides of the "+ target_shape_name+"?",
                true,
                new CallbackFunction(ShowProblem),
                "Where are two shorter sides?",
                5
                ),
                7.5f);
            mStep = 5;
        }
        if (mStep == 6)
        {
            container.SetActive(true);


            //geoprimitives.SetActive(true);
            container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.side_long);

            Dialogs.set_topboard_animated(false, 3, "");
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Can you point to the two longer sides of the " + target_shape_name + "?",
                true,
                new CallbackFunction(ShowProblem),
                "Where are two longer sides ?",
                5
                ),
                3);
            mStep = 7;
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
            blacksmith.GetComponent<Animator>().SetTrigger("hammer");
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Thank you! let me make the four sides of the key");

            
            Dialogs.set_topboard_animated(false, 3, "");
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Can you measure the corner angles? I have a protractor.",
                true,
                new CallbackFunction(start_angle),
                "What are the corner angles?",
                6
                ), 7.5f
                );           
            mStep = 9;

        }

        if (mStep == 10)
        {
            Dialogs.set_topboard_animated(false, 3, "");
            blacksmith.GetComponent<Animator>().SetTrigger("hammer");            
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Great job! Let me finish the key with the angles");
            Dialogs.add_dialog(new DialogItem(DialogueType.Dummy,
                         "",
                          true,
                         new CallbackFunction(OnCompletion),
                         "",
                         6f
                         ));
            //OnCompletion();*/
            mStep = 11;
        }
    }
    public void move_to_step(string t)
    {
        this.mStep = Convert.ToInt32(t);
    }
    public void start_angle(string t)
    {
        container.SetActive(true);
        container.GetComponent<GeometryVisContainer>().Solve_Properties(GeometryPrimitives.angle);
        Dialogs.set_topboard_animated(true, 3, t);

    }
    public void ShowProblem(string txt)
    {
        Dialogs.set_topboard_animated(true, 3, txt);
        

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
    
  
    private void OnCompletion(string t)
    {
        UserInteracting = false;

        this.transform.parent.GetComponent<ContentSolver>().start_review();
        
    }
  
   
   
}
