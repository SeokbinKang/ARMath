using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPane : MonoBehaviour {


    public GameObject[] gem_prefabs;

    private int control_width = 180;
	// Use this for initialization
	void Start () {
       // Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Reset()
    {
        //delete all children gems
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void UpdateGem(List<Gem> g_list)
    {
        Reset();
        int length = g_list.Count;
        int x_leftborder = -1*control_width/2;

        if (length <= 0) return;
        int div_n = length + 1;
        int offset = control_width / div_n;
        offset = 50;
        
        for(int i = 0; i < length; i++)
        {
            GameObject prefab = GetGemIcon(g_list[i]);
            if (prefab == null) Debug.Log("cannot find a prefab for gem");
            GameObject newGemIcon = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            newGemIcon.transform.parent = this.gameObject.transform;
            Vector2 anchor_pos = newGemIcon.GetComponent<RectTransform>().anchoredPosition;
            newGemIcon.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 1);
            anchor_pos.x = x_leftborder + offset * i ;
            anchor_pos.y = 10;
            newGemIcon.GetComponent<RectTransform>().anchoredPosition = anchor_pos;
            
        }

    }
    public GameObject GetGemIcon(Gem g)
    {
        int index = (int) g.problem_type;
        if (gem_prefabs.Length <= 0) return null;
        if (index >= gem_prefabs.Length) index = gem_prefabs.Length - 1;
        return gem_prefabs[index];

    }
}
