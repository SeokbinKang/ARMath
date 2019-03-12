using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUser : MonoBehaviour
{

    public GameObject user_panels_control;
    public GameObject prefab_user_panel;

    private int pPanelWidth = 2200;
    // Use this for initialization
    void Start()
    {
        loadAllUserProfiles();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void loadAllUserProfiles()
    {
        List<UserInfo> test_user = testUserPanel();
        int x_leftmost = pPanelWidth / 2 * -1;
        int n_div = test_user.Count + 1;
        int x_offset = pPanelWidth / n_div;
        int i = 0;
        foreach (UserInfo t in test_user)
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
        u2.user_name = "abcdefg";
        u3.user_name = "erteryt";
        u4.user_name = "new user";

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
