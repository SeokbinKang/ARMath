using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionVirtual : MonoBehaviour
{

   

    


    public GameObject container;
    public GameObject root_movables;   

    public GameObject ContentModuleRoot;

    public GameObject[] movable_objects;
    public GameObject item_mask;
    public GameObject wallet;
    private List<GameObject> tap_list;


    public bool UserInteracting;
    
    private int total_n;
    private string target_object_name;
    // Use this for initialization
    
    private float nextActionTime = 0.0f;


    void Start()
    {
       
    }
    void OnEnable()
    {
        Debug.Log("[ARMath] this should not start");
        Reset();
        loadPrompt();
    }
    private void Reset()
    {
        
        
        container.SetActive(false);
        root_movables.SetActive(false);        
        UserInteracting = false;
        total_n = 0;
        tap_list = new List<GameObject>();        
       // arrange_movable_objects();

    }
    // Update is called once per frame
    void Update()
    {
        if (UserInteracting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            //  UpdateInteractiveObjects();
            count_object_fix_realones();
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
        foreach (GameObject o in movable_objects)
        {
            o.GetComponent<RawImage>().texture = icon_obj.GetComponent<RawImage>().texture;
            RectTransform rt = wallet.GetComponent<RectTransform>();            
            Vector3 targetPos = new Vector3(rt.position.x + random.Next(-80, 100), rt.position.y +random.Next(-50, 50), 0);
            RectTransform r = o.GetComponent<RectTransform>();
            r.position = targetPos;
            r.sizeDelta = new Vector2(w, h);
            r.Rotate(0, 0, random.Next(0, 360));
            o.GetComponent<DragObject>().SetAlphaAdjustment(true, 0.6f, 0.85f, 1);
        }
    }

    private void count_object_fix_realones()
    {
        if (!container || !UserInteracting) return;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int virtual_n = 0;
        int cur_n = 0;
        List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        bool[] number_label = new bool[init_n];
        //init_n = objs.Count;
        for (int i = 0; i < number_label.Length; i++)
        {  //TODO: detach higher number label
            int number_label_value = objs[i].get_number_feedback();
            if (number_label_value > 0 && number_label_value <= number_label.Length)
            {
                number_label[number_label_value - 1] = true;

            }
        }
        //check tangible objects in the bag
        for (int i = 0; i < number_label.Length; i++)
        {
            if (!number_label[i])
            {
                foreach (SceneObject so in objs)
                {
                    if (so.get_number_feedback() <= 0)
                    {
                        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                        GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, i + 1, true);
                        so.attach_object(label);
                        break;
                    }
                }
            }

        }
        virtual_n = 0;
        //check virtual objects in the bag
        foreach (GameObject o in movable_objects)
        {
            bool contained = container.GetComponent<ObjectContainer>().in_container(o);
            if (contained) virtual_n++;
        }



        cur_n = virtual_n + init_n;
        //ContentModuleRoot.GetComponent<ContentAddition>().init_object_count = init_n;

        if (ContentModuleRoot.GetComponent<ContentAddition>().current_object_count != cur_n)
        {
            item_mask.GetComponent<vertical_mask>().set_visible_percent(((float)cur_n / (float)goal_n));
            ContentModuleRoot.GetComponent<ContentAddition>().current_object_count = cur_n;
            OnCount();
        }
    }
    private void count_object()
    {
        if (!container || !UserInteracting) return;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int virtual_n = 0;        
        int cur_n =0;
        List<SceneObject> objs = SceneObjectManager.mSOManager.get_objects_by_name(target_object_name);
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        bool[] number_label = new bool[objs.Count];
        init_n = objs.Count;
        for (int i = 0; i < number_label.Length; i++)
        {  //TODO: detach higher number label
            int number_label_value = objs[i].get_number_feedback();
            if (number_label_value > 0 && number_label_value <= number_label.Length)
            {
                number_label[number_label_value - 1] = true;
                
            }
        }

        //check tangible objects in the bag
        for (int i = 0; i < number_label.Length; i++)
        {
            if (!number_label[i])
            {
                foreach (SceneObject so in objs)
                {
                    if (so.get_number_feedback() <= 0)
                    {
                        Vector3 targetPos = new Vector3(so.catalogInfo.Box.center.x, Screen.height - so.catalogInfo.Box.center.y, 0);
                        GameObject label = FeedbackGenerator.mThis.create_number_feedback(targetPos, i + 1, true);
                        so.attach_object(label);
                        break;
                    }
                }
            }

        }
        virtual_n = 0;
        //check virtual objects in the bag
        foreach(GameObject o in movable_objects)
        {
            bool contained = container.GetComponent<ObjectContainer>().in_container(o);
            if (contained) virtual_n++;
        }

        

        cur_n = virtual_n + init_n;
        //ContentModuleRoot.GetComponent<ContentAddition>().init_object_count = init_n;
        
        if (ContentModuleRoot.GetComponent<ContentAddition>().current_object_count != cur_n)
        {
            item_mask.GetComponent<vertical_mask>().set_visible_percent(((float)cur_n / (float)goal_n));
            ContentModuleRoot.GetComponent<ContentAddition>().current_object_count = cur_n;
            OnCount();
        }
    }
    public void OnCount()
    {
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int added = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count - init_n;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        if (added>0) TTS.mTTS.GetComponent<TTS>().StartTextToSpeech(added + " "+ target_object_name+"s added!");
            else TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("please add more "+ target_object_name + "s!");

        
        //sound effect
        //Debug.Log("[ARMath] result " + ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count + "  =? " + ContentModuleRoot.GetComponent<ContentAddition>().current_object_count);
        if (added == ContentModuleRoot.GetComponent<ContentAddition>().add_object_count)
        {
            UserInteracting = false;
            int[] ans = new int[4];
            ans[0] = goal_n - 2;
            ans[1] = goal_n;
            ans[2] = goal_n+2;
            ans[3] = goal_n+5;
            Dialogs.review(
                "I've got a question. How many coins do we need for an icecream in total?",
                ans,
                1,
                new CallbackFunction(OnCompletion)
                );
            
            
        }
    }
    public void OnCompletion(string t)
    {
        Dialogs.set_topboard(false, "");
        clearinteractiveobjects();
        ContentModuleRoot.GetComponent<ContentAddition>().onSolved();
    }
    public void loadPrompt()
    {

        //prompt_text.SetActive(true);
        target_object_name = ContentModuleRoot.GetComponent<ContentAddition>().target_object_name;
        int init_n = ContentModuleRoot.GetComponent<ContentAddition>().init_object_count;
        int goal_n = ContentModuleRoot.GetComponent<ContentAddition>().goal_object_count;
        int cur_n = ContentModuleRoot.GetComponent<ContentAddition>().current_object_count;
        int add_n = goal_n - init_n;


        Debug.Log("[ARMath] this is for virtual addition.");
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
            "Let's add " + add_n + " more " + target_object_name + "s by taking them out of the wallet",
            true,
            new CallbackFunction(StartOperation),
            "",
            5), 0
            );
       
        UserInteracting = false;
    }
    public void StartOperation(string o)
    {
        total_n = 0;
        UserInteracting = true;
        
        UpdateBoard();        
        container.SetActive(true);
        root_movables.SetActive(true);        
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

        item_mask.GetComponent<vertical_mask>().set_visible_percent(((float)cur_n / (float)goal_n));
        // board.GetComponent<board>().enable_number_only(init_n + sign + System.Math.Abs(cur_n - init_n) + " = " + cur_n);
        Dialogs.set_topboard(true, init_n + " (" + target_object_name + "s) + " + add_n + " (" + target_object_name + "s) = ?");

    }
    private void clearinteractiveobjects()
    {
        for (int k = 0; k < tap_list.Count; k++)
        {
            GameObject.Destroy(tap_list[k]);
        }
        tap_list.RemoveAll(s => s == null);
    }





}
