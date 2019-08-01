using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class user_panel : MonoBehaviour {

    public GameObject character;
    public GameObject stats;
    public GameObject select;
    public GameObject name_label;

    private int uid;
	// Use this for initialization
	void Start () {

        //load user account


        //create character
        

        //creatre span

        

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        LoadUser(SystemUser.current_user);
    }
    public void LoadUser(UserInfo u)
    {
        //update character
        int total_gem = 0;
        foreach (ProblemType t in Enum.GetValues(typeof(ProblemType)))
        {
            total_gem += u.gem_collection[t].Count;
        }
        int level = total_gem / 3;

        character.GetComponent<user_chracter>().set_chracter(level);


        //udpate name
        name_label.GetComponent<Text>().text = u.user_name;

        //udpate stats
        stats.GetComponent<StatControl>().loadStats(u);

        this.uid = u.user_id;

    }

    public void OnSelected()
    {
        SystemUser.SetCurrentUser(this.uid);
        SystemControl.onContentGlobal();

    }
}
