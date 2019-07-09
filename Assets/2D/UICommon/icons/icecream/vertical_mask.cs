using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vertical_mask : MonoBehaviour {

    public float visible_percent;
    private float height_full;

	// Use this for initialization
	void Start () {
        height_full = this.GetComponent<RectTransform>().sizeDelta.y;
        visible_percent = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 s = this.GetComponent<RectTransform>().sizeDelta;
        s.y = height_full * visible_percent;
        this.GetComponent<RectTransform>().sizeDelta = s;
    }

    public void set_visible_percent(float t)
    {
        visible_percent = t;
    }

}
