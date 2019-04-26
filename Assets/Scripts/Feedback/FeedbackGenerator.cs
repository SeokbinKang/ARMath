using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackGenerator : MonoBehaviour {

    public static FeedbackGenerator mThis;

    public GameObject prefabe_number_cartoon_digit;
    public GameObject prefab_stricker_o;
    public GameObject prefab_stricker_x;
    // Use this for initialization
    void Start () {
        mThis = this;
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
