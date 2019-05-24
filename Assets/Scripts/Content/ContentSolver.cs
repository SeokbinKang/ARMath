using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSolver : MonoBehaviour {

    public GameObject solver_virtual;
    public GameObject solver_tangible;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        init_solver();
    }

    public void init_solver()
    {
        /*bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
        bool interaction_object_enabled = SystemControl.mSystemControl.get_system_setup_interaction_object();*/
        bool interaction_touch_enalbed = false;
        bool interaction_object_enabled = true;

        if (interaction_touch_enalbed)
        { //turn on either of two solvers.
            solver_virtual.SetActive(true);
            solver_tangible.SetActive(false);
        } else if (interaction_object_enabled)
        {
            solver_virtual.SetActive(false);
            solver_tangible.SetActive(true);
        } else
        {
            solver_virtual.SetActive(true);
            solver_tangible.SetActive(false);
        }
    }
}
