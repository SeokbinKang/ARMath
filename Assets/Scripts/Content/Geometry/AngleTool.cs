using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTool : MonoBehaviour {

    public GameObject protractor;
    public GameObject angle;
    // Use this for initialization
    private float nextActionTime = 0.5f;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + SystemParam.system_update_period;
            // execute block of code here
            check_status();
        }
    }
    private void check_status()
    {
        if (!protractor.activeSelf || angle.activeSelf) return;
        if (protractor.GetComponent<Protractor>().IsFinished())
        {
            angle.SetActive(true);
            protractor.SetActive(false);
        }
    }
    public bool isFinished()
    {
        return angle.activeSelf;
    }
    private void OnEnable()
    {
        protractor.SetActive(true);
        angle.SetActive(false);
    }

}
