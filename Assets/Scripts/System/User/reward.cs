using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reward : MonoBehaviour {
    public ProblemType problem_type;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onClick()
    {
        SystemUser.AddGem(problem_type);
        this.GetComponent<Animator>().SetTrigger("credit");
        //playmusic


        //how to disable?
    }

}
