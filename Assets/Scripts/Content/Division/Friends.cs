using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends : MonoBehaviour {

    public GameObject contentModule;
    public int num_friends;
    public GameObject[] friends;
    // Use this for initialization

    private float nextActionTime = 0.75f;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time+0.7f;
            introduce_friend();
        }
    }
    private void OnEnable()
    {
        initFriends();
        nextActionTime = Time.time + 4f;
    }
    private void initFriends()
    {
        foreach (var f in friends)
            f.SetActive(false);
        num_friends = (int)Random.Range(2, 5);
        contentModule.GetComponent<ContentDiv>().divisor = num_friends;
        //introduce_friend();
        
    }
    private void introduce_friend()
    {
        for (int i = 0; i < friends.Length && i < num_friends; i++)
        {
            if (!friends[i].activeSelf)
            {
                friends[i].SetActive(true);

                return; 
            }
        }
    }
}
