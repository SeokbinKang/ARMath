using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultVirtual : MonoBehaviour {



    public GameObject board;




    public GameObject problemboard;
    public GameObject problemboard_text;

    public GameObject ContentModuleRoot;
    public GameObject bag_base;
    public GameObject pre_bag_movable;

    public bool UserInteracting;
    // Use this for initialization
    private float nextActionTime = 0.0f;

    private List<GameObject> bag_movable;
    private float bag_scale = 1.3f;

    void Start()
    {

    }
    void OnEnable()
    {
        Reset();
        loadPrompt();
    }
    private void Reset()
    {
        board.SetActive(false);        
        problemboard.SetActive(true);
        UserInteracting = false;
        bag_movable = null;
        if (bag_movable==null) bag_movable = new List<GameObject>();
        bag_base.SetActive(true);
        

    }
    // Update is called once per frame
    void Update()
    {
        if (UserInteracting) UpdateBoard();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;

            count_object();
        }
    }

    private void count_object()
    {
        if (!UserInteracting) return;
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;

        //check if all bags are unfolded
        bool complete = true;
        foreach(GameObject bag in bag_movable)
        {
            if (bag.transform.childCount == 1 || !bag.transform.GetChild(1).gameObject.activeSelf)
            {
                complete = false;
                break;
            }

        }

        if (complete)
        {
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
                "Now let's count how many " + obj_name + "s are there in total.",
               true,
               new CallbackFunction(OnCompletion),
               "none"
               ));
            UserInteracting = false;
            //Answer UI needs to be added
            
        }

    }
    
    private void OnCompletion(string p)
    {
        
        ContentModuleRoot.GetComponent<ContentMulti>().onSolved();
    }
    public void loadPrompt()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        Rect cluster_rect = ContentModuleRoot.GetComponent<ContentMulti>().target_object_cluster;
        ARMathUtils.SetRecttrasnform(bag_base, cluster_rect);
        
       
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
              "There are " + num_per_cell + " " + obj_name + "s in a red bag. " + "If we have "+num_cells+" identical bags, how many " + obj_name + "s are there in total?",
              true,
              null,
              ""
              ));
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Let's drag and drop the bag, and count how many " + obj_name + "s are there.",
               true,
               new CallbackFunction(StartOperation),
               "none"
               ));

    }
    public void StartOperation(string p)
    {
        UserInteracting = true;

        UpdateBoard();

        problemboard.SetActive(true);
        board.SetActive(false);

        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;

        for (int i = 1; i < num_celss; i++)
        { //1 is for real objects
            create_bag();
        }

    }
    private void create_bag()
    {
        
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        if (bag_movable.Count >= num_celss) return;

        GameObject icon_obj = AssetManager.get_icon(obj_name);
        float w = icon_obj.GetComponent<RawImage>().texture.width;
        float h = icon_obj.GetComponent<RawImage>().texture.height;
        h = h * 120 / w;
        w = 120;

        GameObject new_bag = ARMathUtils.create_2DPrefab(pre_bag_movable, this.gameObject);
        new_bag.GetComponent<RectTransform>().position = bag_base.GetComponent<RectTransform>().position;
        new_bag.GetComponent<RectTransform>().sizeDelta = bag_base.GetComponent<RectTransform>().sizeDelta;        
        for(int i = 0; i < num_per_cell; i++)
        {
            GameObject new_obj = ARMathUtils.create_2DPrefab(icon_obj, new_bag, new Vector2(500, 500));
            RectTransform r = new_obj.GetComponent<RectTransform>();
            r.localPosition = new Vector2(Random.Range(-200, 200), Random.Range(-200, 50));
            r.sizeDelta = new Vector2(w, h);
            r.Rotate(0, 0, Random.Range(0, 360));
            new_obj.SetActive(false);
            
        }
        Debug.Log("[ARMath] new bag create at " + new_bag.GetComponent<RectTransform>().position);
        bag_movable.Add(new_bag);

    }
    private void UpdateBoard()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;

        
        problemboard_text.GetComponent<Text>().text = num_per_cell + "(" + obj_name + "s) X " + num_celss + " (bags) = ?";

    }
}
