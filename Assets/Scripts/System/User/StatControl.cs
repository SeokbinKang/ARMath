using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatControl : MonoBehaviour {

    public GameObject[] stat_panels;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadStats(UserInfo u)
    {
        Dictionary<ProblemType, List<Gem>> gems = u.gem_collection;
        int i = 0;
        foreach(ProblemType t in Enum.GetValues(typeof(ProblemType)))
        {
            
            if(gems[t].Count>0)
            {
                if(i<stat_panels.Length)
                {
                    stat_panels[i].GetComponent<GemPane>().UpdateGem(gems[t]);
                    i++;
                }
            }
            //search for the exisiting pane for the type
            
            

            //search for an empty pane
        }
    }
}
