using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_chracter : MonoBehaviour {

    public GameObject[] models;

    private int model_index = 0;
    private float nextActionTime = 0.0f;
    public float period = 1f;
    // Use this for initialization
    void Start () {

        this.UpdateCharacter();
    }


    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // execute block of code here

            this.UpdateCharacter();
        }

    }
    public void set_chracter(int index)
    {
        if (index >= models.Length) model_index = models.Length - 1;
        else model_index = index;
        UpdateCharacter();
    }
    // Update is called once per frame
    private void UpdateCharacter () {
        for (int i=0;i<models.Length;i++)
        {
            if (i == model_index) models[i].SetActive(true);
                else models[i].SetActive(false);
        }
		
	}


}
