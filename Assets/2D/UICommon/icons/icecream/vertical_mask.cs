using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vertical_mask : MonoBehaviour {

    public float visible_percent;
    private float height_full;
    public bool random_move;
    public float min_percent;
    public float max_percent;
    private float velocity;
	// Use this for initialization
	void Start () {
        height_full = this.GetComponent<RectTransform>().sizeDelta.y;
        
        
	}

    // Update is called once per frame
    void Update()
    {
        if (random_move)
        {
            visible_percent += velocity;
            if(visible_percent<min_percent)
            {
                visible_percent = min_percent;
                velocity = velocity * -1;
            } else if (visible_percent > max_percent)
            {
                visible_percent = max_percent;
                velocity = velocity * -1;
            }
            if (Mathf.Abs(velocity) > 0.01f)
            {
                velocity = Random.Range(-0.01f, 0.01f);
            } else  velocity += Random.Range(-0.001f, 0.001f);
                
        }
        Vector2 s = this.GetComponent<RectTransform>().sizeDelta;
        s.y = height_full * visible_percent;
        this.GetComponent<RectTransform>().sizeDelta = s;
        

    }

    
    private void OnEnable()
    {
        visible_percent = 0;
        if (random_move)
        {
            visible_percent = Random.Range(min_percent, max_percent);
            velocity = Random.Range(-0.01f, 0.01f);
        }
    }
    public void set_visible_percent(float t)
    {
        visible_percent = t;
    }

}
