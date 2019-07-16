using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultVirtual : MonoBehaviour {



    




    public GameObject ContentModuleRoot;
    public GameObject bag_base;
    public GameObject pre_bag_movable;
    public GameObject tree_group;

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
        
        UserInteracting = false;
        bag_movable = null;
        if (bag_movable==null) bag_movable = new List<GameObject>();
        bag_base.SetActive(true);
        tree_group = ContentModuleRoot.GetComponent<ContentMulti>().sub_trees;

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
    public List<GameObject> get_all_batteries()
    {
        List<GameObject> ret = new List<GameObject>();
        foreach (GameObject bag in bag_movable)
        {
            if (bag.transform.childCount > 1)
            {
                for(int i=1;i< bag.transform.childCount; i++)
                {
                    ret.Add(bag.transform.GetChild(i).gameObject);
                }
            }

        }
        return ret;
    }
    private void count_object()
    {
        if (!UserInteracting) return;
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;

        //check if all bags are unfolded
        bool complete = false;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        foreach (GameObject bag in bag_movable)
        {
            if (bag.transform.childCount > 1)
            {
                int ret = tree_group.GetComponent<GroupTree>().CheckCellProgressive(bag);
                if (ret == num_cells)
                {
                    complete = true;
                    break;
                }
            }

        }

        if (complete)
        {
            this.transform.parent.GetComponent<ContentSolver>().start_review();
            UserInteracting = false;                        

        }

    }
    public void OnQuestion(string q)
    {
        Dialogs.set_topboard_animated(true, 2, "= ?");
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
              "There are " + num_per_cell + " batteries in a bag. ",
              true,
              null,
              "",4
              ));
        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Can you move a bag to the tree? You can move it on the screen.",
               true,
               new CallbackFunction(StartOperation),
               "none",4
               ));

    }
    public void StartOperation(string p)
    {
        UserInteracting = true;

        UpdateBoard();

        

        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;

        for (int i = 0; i < num_celss; i++)
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

        //GameObject icon_obj = AssetManager.get_icon(obj_name);
        GameObject icon_obj = AssetManager.get_icon("battery");
        float w = icon_obj.GetComponent<RawImage>().texture.width;
        float h = icon_obj.GetComponent<RawImage>().texture.height;
        h = h * 70 / w;
        w = 70;

        GameObject new_bag = ARMathUtils.create_2DPrefab(pre_bag_movable, this.gameObject);
        //Vector2 fixed_bagsize = new Vector2(300, 300);
        new_bag.GetComponent<RectTransform>().position = bag_base.GetComponent<RectTransform>().position;
        //new_bag.GetComponent<RectTransform>().sizeDelta = fixed_bagsize;
        new_bag.GetComponent<RectTransform>().sizeDelta = bag_base.GetComponent<RectTransform>().sizeDelta;        
        for(int i = 0; i < num_per_cell; i++)
        {
            GameObject new_obj = ARMathUtils.create_2DPrefab(icon_obj, new_bag, new Vector2(500, 500));
            RectTransform r = new_obj.GetComponent<RectTransform>();
            r.localPosition = new Vector2(Random.Range(-150, 150), Random.Range(-150, 50));
            r.sizeDelta = new Vector2(w, h);
            r.Rotate(0, 0, Random.Range(0, 360));
            new_obj.SetActive(false);
            
        }
        //Debug.Log("[ARMath] new bag create at " + new_bag.GetComponent<RectTransform>().position);
        bag_movable.Add(new_bag);

    }
    private void UpdateBoard()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;

        
//        problemboard_text.GetComponent<Text>().text = num_per_cell + "(" + obj_name + "s) X " + num_celss + " (bags) = ?";

    }
}
