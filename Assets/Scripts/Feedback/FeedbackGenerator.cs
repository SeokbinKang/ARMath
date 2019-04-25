using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackGenerator : MonoBehaviour {

    public static FeedbackGenerator mThis;

    public GameObject prefabe_number_cartoon_digit;
	// Use this for initialization
	void Start () {
        mThis = this;
       /* create_number_feedback(new Vector2(0, 500), 0);
        create_number_feedback(new Vector2(200,500),1);
        create_number_feedback(new Vector2(400, 500), 2);
        create_number_feedback(new Vector2(600, 500), 2);
        create_number_feedback(new Vector2(800, 500), 2);
        create_number_feedback(new Vector2(1000, 500), 2);
        create_number_feedback(new Vector2(1200, 500), 2);
        create_number_feedback(new Vector2(1700, 500), 9);*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject create_number_feedback(Vector3 position, int value)
    {
        Vector3 targetPos = position;
        UnityEngine.GameObject label = Instantiate(prefabe_number_cartoon_digit, targetPos ,Quaternion.identity) as GameObject;                
        RectTransform r = label.GetComponent<RectTransform>();
        r.position = targetPos;        
       // label.GetComponent<RectTransform>().position = r.position;
        label.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<number_cartoon>().set_number(value);
        
        return label;
    }


}
