using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class effect_gem : MonoBehaviour {

    public Texture[] gem_sprites;
    public ProblemType problem_type;
    
	// Use this for initialization
	void Start () {
        
    
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnEnable()
    {
        
    }

    public void load_gem(ProblemType p)
    {
        this.GetComponent<RawImage>().texture = gem_sprites[(int)p];
        problem_type = p;
        this.GetComponent<Animator>().SetBool("credit", false);
    }

    public void onClick()
    {
        SystemUser.AddGem(problem_type);
        this.GetComponent<Animator>().SetBool("credit", true);

        //how to disable?
    }

}
