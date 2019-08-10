using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultVirtual : MonoBehaviour {



    




    public GameObject ContentModuleRoot;
    public GameObject bag_base;
    public GameObject pre_bag_movable;
    public GameObject tree_group;
    public GameObject tray;
    public bool UserInteracting;
    // Use this for initialization
    private float nextActionTime = 0.0f;

    private List<GameObject> bag_movable;
    private List<GameObject> batter_movable;
    private List<GameObject> battery_in_cells;
    private float bag_scale = 1.3f;
    private TimerCallback mCallback;
    private int compelte_cells;
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
        compelte_cells = 0;
        if (bag_movable==null) bag_movable = new List<GameObject>();
        if (batter_movable == null) batter_movable = new List<GameObject>();
        else
        {
            foreach (GameObject o in batter_movable)
            {
                if (o != null) GameObject.Destroy(o);
            }
            batter_movable.Clear();
        }
        if (battery_in_cells == null) battery_in_cells = new List<GameObject>();
        else battery_in_cells.Clear();
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
            if (mCallback!=null && mCallback.tick()) mCallback = null;
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
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        //check if all bags are unfolded
        bool complete = false;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        
        complete = false;
        int ret = tree_group.GetComponent<GroupTree>().CheckCellProgressive(batter_movable, num_per_cell);
        if (ret == num_cells)
        {
            complete = true;                   
            
        } else if(ret> compelte_cells)
        {
            Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
             "Let's turn on the next tree",
             true,
             null,
             "none", 4
             ),4);
            compelte_cells = ret;
           
        }
        if (complete)
        {
            mCallback = new TimerCallback(OnCompletion, "",4);
            UserInteracting = false;                        
        }

    }
    
    public void OnQuestion(string q)
    {
        Dialogs.set_topboard_animated(true, 2, "= ?");
    }
    private void OnCompletion(string p)
    {
        //tray.SetActive(false);
        this.transform.parent.GetComponent<ContentSolver>().start_review();
        //ContentModuleRoot.GetComponent<ContentMulti>().onSolved();
    }
    public void loadPrompt()
    {
        string obj_name = ContentModuleRoot.GetComponent<ContentMulti>().target_object_name;
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_cells = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        //Rect cluster_rect = ContentModuleRoot.GetComponent<ContentMulti>().target_object_cluster;        

        setup_virtual_objects();
        float target_delay = 2;
      

        Dialogs.add_dialog(new DialogItem(DialogueType.left_bottom_plain,
               "Please move the batteries on the screen. Here are some extra batteries. ",
               true,
               new CallbackFunction(StartOperation),
               "none",10
               ));

    }
    private void setup_virtual_objects()
    {
        int num_per_cell = ContentModuleRoot.GetComponent<ContentMulti>().target_base_num;
        int num_celss = ContentModuleRoot.GetComponent<ContentMulti>().target_mult_num;
        Rect rt = ContentModuleRoot.GetComponent<ContentMulti>().obj_rect;
        RectTransform rt_V = tray.GetComponent<RectTransform>();
        
        rt_V.position = rt.position;
        rt_V.sizeDelta = rt.size*1.2f;
        rt_V.localScale = Vector3.one;
        tray.SetActive(true);
        
        GameObject icon_obj = AssetManager.get_icon("battery");
        if (icon_obj == null)
        {
            return;
        }
        float w = icon_obj.GetComponent<RawImage>().texture.width;
        float h = icon_obj.GetComponent<RawImage>().texture.height;
        h = h * 80 / w;
        w = 80;
        List<Vector2> pos_list = ContentModuleRoot.GetComponent<ContentMulti>().obj_pos_list;
        foreach (Vector2 pos in pos_list)
        {
            GameObject new_obj = ARMathUtils.create_2DPrefab(icon_obj, tray, pos);
            RectTransform r = new_obj.GetComponent<RectTransform>();
            r.position = pos;
            r.sizeDelta = new Vector2(w, h);
            r.Rotate(0, 0, Random.Range(0, 360));
            new_obj.SetActive(true);
            this.batter_movable.Add(new_obj);
        }
        float delay = 4.5f;
        for(int i=0;i< num_per_cell* num_celss; i++)
        {
            GameObject new_obj = ARMathUtils.create_2DPrefab(icon_obj, tray, Vector2.one);
            RectTransform r = new_obj.GetComponent<RectTransform>();
            r.localPosition =  new Vector2(Random.Range(rt.size.x * -0.5f, rt.size.x*0.5f), Random.Range(rt.size.y * -0.5f, rt.size.y * 0.5f));
            r.sizeDelta = new Vector2(w, h);
            r.Rotate(0, 0, Random.Range(0, 360));
            new_obj.GetComponent<DelayedImage>().setDelay(delay);
            delay += 0.3f;
            new_obj.SetActive(true);
            this.batter_movable.Add(new_obj);
        }
              
       
    }
    public void StartOperation(string p)
    {
        UserInteracting = true;

        UpdateBoard();        

    }
    private void create_virtual_batteries(int k)
    {
        GameObject icon_obj = AssetManager.get_icon("battery");
        Vector2 bag_size = this.bag_base.GetComponent<RectTransform>().sizeDelta;
        float w = icon_obj.GetComponent<RawImage>().texture.width;
        float h = icon_obj.GetComponent<RawImage>().texture.height;
        h = h * 70 / w;
        w = 70;
        bag_size /= 2;
        for (int i = 0; i < k; i++)
        {
            GameObject new_obj = ARMathUtils.create_2DPrefab(icon_obj, bag_base, new Vector2(500, 500));
            RectTransform r = new_obj.GetComponent<RectTransform>();
            r.localPosition = new Vector2(Random.Range(bag_size.x*-1, bag_size.x), Random.Range(bag_size.y*-1, bag_size.y * 0.5f));
            r.sizeDelta = new Vector2(w, h);
            r.Rotate(0, 0, Random.Range(0, 360));
            new_obj.SetActive(true);
            this.batter_movable.Add(new_obj);
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
