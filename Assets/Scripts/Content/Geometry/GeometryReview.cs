using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeometryReview : MonoBehaviour {

    public GameObject chest_default;
    public GameObject chest_open;
    public GameObject key;
    public GameObject victor;
    public GameObject gem;
    public GameObject blacksmith;
    public GameObject visual_rect;

    public GameObject msg1;
    public GameObject msg2;
    public GameObject msg3;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkin();


    }
    private void OnEnable()
    {
        Reset();
    }
    private void checkin()
    {
        if (blacksmith.activeSelf)
        {
            if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("reward_keychest"))
            {
                blacksmith.SetActive(false);
            }
        }
    }
    private void Reset()
    {
        init_review();
        chest_default.SetActive(false);
        chest_open.SetActive(false);
        key.SetActive(true);
        victor.SetActive(true);
        gem.SetActive(false);
    }

    private void OnDisable()
    {
        
        chest_default.SetActive(false);
        chest_open.SetActive(false); 
        key.SetActive(false);
        victor.SetActive(false);
        gem.SetActive(false);
    }

    private void init_review()
    {
        msg1.GetComponent<Text>().text = "Alright! We found a rectangle that has 4 vertices, 2 paires of sides, and 4 right angles.";
        msg2.GetComponent<Text>().text = "Can you pick a rectangle out of 4 shapes?";

        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
        visual_rect.GetComponent<Animator>().SetTrigger("highlight");
        
        msg2.GetComponent<DelayedImage>().setCallback(show_top_answer, "Pick a rectangle from the shapes");
        msg3.GetComponent<DelayedImage>().setCallback(show_selection, "");

    }
    public void show_top_answer(string t)
    {
        Dialogs.set_topboard_animated(true, 3, t);

    }
    public void show_selection(string t)
    {
       
        Dialogs.review_shape("", new CallbackFunction(OnCompletion)  );
    }
    public void OnCompletion(string t)
    {
        this.GetComponent<Animator>().SetTrigger("open");        
        
    }
    private void highlight_objects_virtual(float delay1, float delay2)
    {




    }
}