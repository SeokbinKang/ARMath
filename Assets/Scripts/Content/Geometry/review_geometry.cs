using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class review_geometry : MonoBehaviour {
    public GameObject chest_default;
    public GameObject chest_open;
    public GameObject key;
    public GameObject victor;
    public GameObject gem;
    public GameObject blacksmith;
    public GameObject visual_rect;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkin();


    }
    private void OnEnable()
    {
        Reset();
    }
    private void checkin()
    {
        if (blacksmith.activeSelf)
        {
            if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("reward_keychest"))
            {
                blacksmith.SetActive(false);
            }
        }
    }
    private void Reset()
    {
        chest_default.SetActive(false);
        chest_open.SetActive(false);
        key.SetActive(true);
        victor.SetActive(true);
        gem.SetActive(false);
    }



}
