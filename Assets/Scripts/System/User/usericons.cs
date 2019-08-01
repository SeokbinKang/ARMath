using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class usericons : MonoBehaviour {

    public GameObject prefab_usericon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onSelect(int id_)
    {
        SystemUser.OnSelectUser(id_);
    }
    public void load_users(List<UserInfo> u_list)
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //ordering u_list;
        u_list.Sort(delegate (UserInfo a, UserInfo b)
        {
            
            if (a.lastused == b.lastused) return 0;
            if (a.lastused < b.lastused) return 1;
            return -1;            
        });
        int k = 0;
        foreach(var u in u_list)
        {
            GameObject icon = ARMathUtils.create_2DPrefab(prefab_usericon, this.gameObject);
            icon.GetComponentInChildren<Text>().text = u.user_name;
            icon.GetComponentInChildren<Button>().onClick.AddListener(() => { this.onSelect(u.user_id); });
            k++;
            if (k >= 3) break;
        }
    }
}

