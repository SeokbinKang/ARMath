using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree : MonoBehaviour {
    public GameObject mask_obj;

    public bool auto_turnoff;
    public float auto_turnoff_time;
    public GameObject character;
    private float target_percent;
    private float start_time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (auto_turnoff)
        {
            if (Time.time > start_time + auto_turnoff_time)
            {
                stop_randome_light(0.1f);
                //auto_turnoff = false;
            }
        }
        if(target_percent>=0) update_light();
        if (character.activeSelf)
        {
            if (character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("finish"))
            {
                character.SetActive(false);
            }
        }
	}
    private void OnEnable()
    {
        start_time = Time.time;
        character.SetActive(false);
        target_percent = -1;
    }
    private void update_light()
    {
        
        if (Mathf.Abs(mask_obj.GetComponent<vertical_mask>().visible_percent - target_percent) < 0.05)
        {
            target_percent = -1;
            return;
        }
        mask_obj.GetComponent<vertical_mask>().visible_percent = target_percent * 0.1f + mask_obj.GetComponent<vertical_mask>().visible_percent * 0.9f;
        
    }
    public void stop_randome_light(float percent)
    {
        target_percent = percent;
        mask_obj.GetComponent<vertical_mask>().random_move = false;
        auto_turnoff = false;
    }
    public void character_nicejob()
    {
        this.character.SetActive(true);
    }
}
