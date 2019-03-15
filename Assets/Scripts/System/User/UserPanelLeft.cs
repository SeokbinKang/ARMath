using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanelLeft : MonoBehaviour {
    public GameObject character;
    public GameObject stats_vertical_grid;    
    public GameObject name_label;
    private float nextActionTime = 0.0f;
    public float period =1.5f;


    // Use this for initialization
    void Start () {
        //LoadUser();

    }

    // Update is called once per frame
    void Update()
    {     
        
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            // execute block of code here
            LoadUser();
        }
    }

    public void LoadUser()
    {
        //Debug.Log("[ARMath] Loading User Profile");
        //update character
        UserInfo u = SystemUser.current_user;
        if (u == null) return;
        int total_gem = 0;
       // Debug.Log("[ARMath] Loading User Profile:"+u.user_name);
        foreach (ProblemType t in Enum.GetValues(typeof(ProblemType)))
        {
            total_gem += u.gem_collection[t].Count;
        }
        int level = total_gem / 3;

        character.GetComponent<user_chracter>().set_chracter(level);


        //udpate name
        name_label.GetComponent<Text>().text = u.user_name;

        //udpate stats
        stats_vertical_grid.GetComponent<VerticalGemStat>().loadStats(u);

    }
}
