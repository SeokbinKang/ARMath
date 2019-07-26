using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackGenerator : MonoBehaviour {

    public static FeedbackGenerator mThis;

    
    public GameObject prefabe_number_cartoon_digit;
    public GameObject prefab_stricker_o;
    public GameObject prefab_stricker_x;
    public GameObject prefab_check;
    public GameObject prefab_target;

    public Color[] color_terms;
    public GameObject[] sounds;
    private List<GameObject_timer> timer_feedback;
    private List<GameObject> temporary_feedback;
    private float last_temporary_feedback_time;
    // Use this for initialization
    void Start () {
        mThis = this;
        temporary_feedback = new List<GameObject>();
        last_temporary_feedback_time = Time.time;
        timer_feedback = new List<GameObject_timer>();
        // create_number_feedback(new Vector2(0, 500), 0,true);
        // create_number_feedback(new Vector2(200,500),1, true);
        // create_number_feedback(new Vector2(400, 200), 2, true);
        // create_number_feedback(new Vector2(600, 400), 5, true);
        // create_number_feedback(new Vector2(800, 600), 6, true);
        // create_number_feedback(new Vector2(1000, 800), 7, true);
        // create_number_feedback(new Vector2(1200, 1000), 8, true);
        // create_number_feedback(new Vector2(1700, 1200), 9, true);

        //FeedbackGenerator.mThis.create_sticker_ox(new Vector2(200, 500), true, true);
        //FeedbackGenerator.mThis.create_sticker_ox(new Vector2(400, 200), true, true);
        //FeedbackGenerator.mThis.create_sticker_ox(new Vector2(600, 400), true, true);
        //FeedbackGenerator.mThis.create_sticker_ox(new Vector2(1200, 1000), true, true);
        //FeedbackGenerator.mThis.create_sticker_ox(new Vector2(1700, 1200), true, true);
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.time - last_temporary_feedback_time > 10)
        {
            foreach(GameObject o in temporary_feedback)
            {
                Object.Destroy(o);                    
            }
            temporary_feedback.Clear();
        }
        int i = 0;
        for(i = 0; i < timer_feedback.Count; i++)
        {
            if (timer_feedback[i].check_timer())
            {
                timer_feedback.RemoveAt(i);
            }
        }
	}
    public static void create_target(Vector3 pos, float start_delay, float lifetime)
    {
        Vector3 targetPos = pos;
        
        UnityEngine.GameObject label = Instantiate(mThis.prefab_target, targetPos, Quaternion.identity) as GameObject;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;        
        label.transform.SetParent(mThis.gameObject.transform);
        label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        label.SetActive(false);
        mThis.timer_feedback.Add(new GameObject_timer(label,start_delay,lifetime));        
        
    }
    public static void create_target(Vector3 pos, float start_delay, float lifetime, int color_index)
    {
        Vector3 targetPos = pos;

        UnityEngine.GameObject label = Instantiate(mThis.prefab_target, targetPos, Quaternion.identity) as GameObject;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        label.transform.SetParent(mThis.gameObject.transform);
        label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        label.GetComponent<Image>().color = AssetManager.getColors()[color_index];
            //mThis.color_terms[color_index];
        label.SetActive(false);
        mThis.timer_feedback.Add(new GameObject_timer(label, start_delay, lifetime));

    }
    public static GameObject create_number_feedback(Vector3 position, int value, float start_delay, float lifetime)
    {
        Vector3 targetPos = position;
        UnityEngine.GameObject label = Instantiate(mThis.prefabe_number_cartoon_digit, targetPos, Quaternion.identity) as GameObject;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        // label.GetComponent<RectTransform>().position = r.position;
        label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        label.transform.SetParent(mThis.gameObject.transform);
        label.GetComponent<number_cartoon>().set_number(value);
        label.SetActive(false);
        mThis.timer_feedback.Add(new GameObject_timer(label, start_delay, lifetime));
        return label;
    }

    public static void create_target(Vector3 pos, float start_delay, float lifetime, int color_index, int sound_index)
    {
        if(sound_index>=0 && sound_index < mThis.sounds.Length)
        {
            mThis.sounds[sound_index].GetComponent<AudioSource>().Play();
        }
        create_target( pos,  start_delay,  lifetime,  color_index);

    }
    public static void create_sticker_ox_dispose(Vector3 position, bool ox){
        position.y += 30;
        GameObject o = mThis.create_sticker_ox(position, ox, true);
        mThis.temporary_feedback.Add(o);
        mThis.last_temporary_feedback_time = Time.time;

    }
    public GameObject create_sticker_ox(Vector3 position, bool ox, bool active_)
    {
        Vector3 targetPos = position;
        GameObject prefab = prefab_stricker_o;
        if (!ox) prefab = prefab_stricker_x;
        UnityEngine.GameObject label = Instantiate(prefab, targetPos, Quaternion.identity) as GameObject;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        // label.GetComponent<RectTransform>().position = r.position;        
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);


        label.SetActive(active_);
        return label;
    }

    public GameObject create_check_feedback(Vector3 position, int value, bool active_)
    {
        Vector3 targetPos = position;
        UnityEngine.GameObject label = Instantiate(prefab_check, targetPos, Quaternion.identity) as GameObject;
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;
        // label.GetComponent<RectTransform>().position = r.position;
        label.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        label.transform.SetParent(this.gameObject.transform);
        //label.GetComponent<number_cartoon>().set_number(value);

        label.SetActive(active_);

        return label;
    }
    public GameObject create_number_feedback(Vector3 position, int value, bool active_)
    {
        Vector3 targetPos = position;
        UnityEngine.GameObject label = Instantiate(prefabe_number_cartoon_digit, targetPos ,Quaternion.identity) as GameObject;                
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;        
       // label.GetComponent<RectTransform>().position = r.position;
        label.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<number_cartoon>().set_number(value);

        label.SetActive(active_);
        
        return label;
    }


}

public class GameObject_timer{
    private float enable_delay;
    private float lifetime;
    private GameObject go;
    private float start_time;
    public GameObject_timer(GameObject go_,float start_delay, float delete_timer)
    {
        go = go_;
        lifetime = delete_timer;
        enable_delay = start_delay;
        start_time = Time.time;
    }

    //return: if need to destory
    public bool check_timer()
    {
        if (go == null) return true;
        if(!go.activeSelf)
        {
            if (Time.time > start_time + enable_delay) go.SetActive(true);
            return false;
        }
        if (Time.time > start_time + enable_delay + lifetime)
        {
            GameObject.Destroy(go);
            return true;
        }
        return false;
    }



}