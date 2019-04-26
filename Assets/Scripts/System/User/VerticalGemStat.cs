using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VerticalGemStat : MonoBehaviour {


    public GameObject prefab_grid_element;
  
    
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Reset()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void loadStats(UserInfo u)
    {
        foreach (ProblemType t in Enum.GetValues(typeof(ProblemType)))
        {
            if (u.gem_collection[t].Count > 0)
            {
                //search for exisiting row                
                GameObject grid_element = null;
                for(int i = 0; i < this.transform.childCount; i++)
                {
                    GameObject child = this.transform.GetChild(i).gameObject;
                    if(child.GetComponent<VerticalGemStat_Element>().problem_type==t)
                    {
                        grid_element = child;
                        break;
                    }
                }
                if (grid_element == null)
                {
                    //create a new stat
                    GameObject newstat = Instantiate(prefab_grid_element, Vector3.zero, Quaternion.identity) as GameObject;
                    newstat.transform.parent = this.gameObject.transform;
                    newstat.GetComponent<RectTransform>().localScale = Vector3.one;
                    newstat.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                    newstat.GetComponent<VerticalGemStat_Element>().UpdateGem((int)t, u.gem_collection[t].Count,t);
                } else
                {
                    //update stat
                    grid_element.GetComponent<VerticalGemStat_Element>().UpdateGem((int)t, u.gem_collection[t].Count,t);
                }
                
            }
        }
    }
}

