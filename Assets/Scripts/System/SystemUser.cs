using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUser : MonoBehaviour
{
    public static UserInfo current_user;
    public GameObject user_panels_control;
    public GameObject prefab_user_panel;

    private int pPanelWidth = 2200;

    public static List<UserInfo> user_list;
    // Use this for initialization
    void Start()
    {
        current_user = null;
        loadAllUserProfiles();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void AddGem(ProblemType p)
    {
        Gem g = new Gem();
        g.problem_type = p;
        current_user.AddGem(g);
    }
    public static void SetCurrentUser(float uid)
    {
        foreach (UserInfo u in user_list)
        {
            if (uid == u.user_id)
            {
                current_user = u;
                Debug.Log("[ARMath] Set the current user for UID:" + uid);
                return;
            }
        }
        Debug.Log("[ARMath] Fail to Set the current user for UID:" + uid);
    }
    private void loadAllUserProfiles()
    {
        if(user_list==null) user_list = testUserPanel();
            else
        {
            user_list.Clear();
            user_list = testUserPanel();
        }
        int x_leftmost = pPanelWidth / 2 * -1;
        int n_div = user_list.Count + 1;
        int x_offset = pPanelWidth / n_div;
        int i = 0;
        foreach (UserInfo t in user_list)
        {
            GameObject newUserPane = Instantiate(prefab_user_panel, Vector3.zero, Quaternion.identity) as GameObject;
            newUserPane.transform.parent = user_panels_control.gameObject.transform;
            newUserPane.GetComponent<RectTransform>().localScale = Vector3.one;
            Vector2 anchor_pos = newUserPane.GetComponent<RectTransform>().anchoredPosition;
            anchor_pos.x = x_leftmost + x_offset * (i + 1);
            anchor_pos.y = 0;
            newUserPane.GetComponent<RectTransform>().anchoredPosition = anchor_pos;

            newUserPane.GetComponent<user_panel>().LoadUser(t);
            i++;
        }

    }

    private List<UserInfo> testUserPanel()
    {
        UserInfo u1 = new UserInfo();
        UserInfo u2 = new UserInfo();
        UserInfo u3 = new UserInfo();
        UserInfo u4 = new UserInfo();
        u1.user_name = "seokbin";
        u2.user_name = "leylana";
        u3.user_name = "virginia";
        u4.user_name = "new user";
        u1.user_id = 343f;
        u2.user_id = 23232f;
        u3.user_id = 343454f;
        u4.user_id = 34f;
        for (int i = 0; i < 9; i++)
        {
            Gem g = new Gem();
            g.problem_type = (ProblemType)(i % 4);
            u1.AddGem(g);
        }
        for (int i = 0; i < 6; i++)
        {
            Gem g = new Gem();
            g.problem_type = (ProblemType)(i % 4);
            u3.AddGem(g);
        }
        for (int i = 0; i < 3; i++)
        {
            Gem g = new Gem();
            g.problem_type = (ProblemType)(i % 4);
            u2.AddGem(g);
        }

        List<UserInfo> ret = new List<UserInfo>();
        ret.Add(u1);
        ret.Add(u2);
        ret.Add(u3);
        ret.Add(u4);

      
        return ret;
    }

    
}
