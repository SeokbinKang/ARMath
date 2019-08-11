using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSolver : MonoBehaviour {

    public GameObject solver_virtual;
    public GameObject solver_tangible;
    public GameObject review;
    public bool force_virtual;
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
    public void close_solvers()
    {
        if (solver_tangible != null) solver_tangible.SetActive(false);
        if (solver_virtual != null) solver_virtual.SetActive(false);
    }
    private void OnDisable()
    {
        review.SetActive(false);
        if(solver_tangible!=null) solver_tangible.SetActive(false);
        if (solver_virtual != null) solver_virtual.SetActive(false);
    }
    public void start_review()
    {
        review.SetActive(true);
    }
    public void init_solver()
    {
        if (force_virtual)
        {
            if (solver_virtual != null) solver_virtual.SetActive(true);
            if (solver_tangible != null) solver_tangible.SetActive(false);
            review.SetActive(false);
            return;
        }
        bool interaction_touch_enalbed = SystemControl.mSystemControl.get_system_setup_interaction_touch();
        bool interaction_object_enabled = SystemControl.mSystemControl.get_system_setup_interaction_object();

       
        if (interaction_touch_enalbed)
        { //turn on either of two solvers.
            if(solver_virtual!=null) solver_virtual.SetActive(true);
            if (solver_tangible != null) solver_tangible.SetActive(false);
        } else if (interaction_object_enabled)
        {
            SceneObjectManager.mSOManager.Reset();
            solver_virtual.SetActive(false);
            solver_tangible.SetActive(true);
        } else
        {
            solver_virtual.SetActive(true);
            solver_tangible.SetActive(false);
        }
        review.SetActive(false);
    }
    
}
