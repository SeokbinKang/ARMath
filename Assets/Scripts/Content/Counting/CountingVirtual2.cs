using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingVirtual2 : MonoBehaviour {

    public GameObject ContentModule;

    private int last_count;
    private bool isCounting;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        check_count();
        loadprompt();
    }
    public void loadprompt()
    {
        string obj_name_plural = ContentModule.GetComponent<ContentCounting>().obj_name_plural;
        last_count = 0;
        isCounting = false;
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
         "Let's count the number of " + obj_name_plural + " by tapping them on the screen!",
         true,
         new CallbackFunction(s1_initCount),
         "",
         7), 0
         );
    }
    public void s1_initCount(string t)
    {
        List<Vector2> pos_list = ContentModule.GetComponent<ContentCounting>().obj_pos_list;

        FeedbackGenerator.init_counter(new CallbackFunction(after_counting_all), pos_list.Count);
        float delay = 0;
        for (int i = 0; i < pos_list.Count; i++)
        {  //TODO: detach higher number label            
            FeedbackGenerator.create_target(pos_list[i], delay, 600, 2,true,true);
            delay += 0.5f;
        }
        //global counter callback
        isCounting = true;
    }
    private void check_count()
    {
        if (!isCounting) return;
        int g_counter = FeedbackGenerator.get_global_counter();
        if(g_counter>0 && g_counter>last_count)
        {
            string obj_name = ContentModule.GetComponent<ContentCounting>().target_object_name;
            Dialogs.set_topboard_animated(true, 3, AssetManager.Get_object_text(obj_name, g_counter));
            if (g_counter > 1)
            {
                Dialogs.set_topboard_highlight(true, 3, 0);
            }
            last_count = g_counter;
        }
    }
    public void after_counting_all(string t)
    {
        isCounting = false;
        this.transform.parent.GetComponent<ContentSolver>().start_review();
    }
}
