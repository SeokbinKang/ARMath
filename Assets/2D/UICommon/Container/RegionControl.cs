using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionControl : MonoBehaviour {

    public GameObject[] regions;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        foreach(GameObject r in regions)
        {
            r.SetActive(false);            
        }
    }
    public void setRegion(int idx, Rect r, bool enable)
    {
        if (idx >= regions.Length) return;
        regions[idx].SetActive(enable);
        RectTransform rt = regions[idx].GetComponent<RectTransform>();
        rt.position = r.position;
        rt.sizeDelta = r.size;
        rt.localScale = Vector3.one;

    }
    public void enaleRegion(int idx, bool enable)
    {
        if (idx >= regions.Length) return;
        regions[idx].SetActive(enable);
    }
    public GameObject getRegion(int idx)
    {
        if (idx >= regions.Length) return null;
        return regions[idx];
    }
}
