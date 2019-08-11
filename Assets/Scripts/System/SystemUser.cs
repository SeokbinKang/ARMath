using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SystemUser : MonoBehaviour
{
    public static SystemUser mThis;
    public static UserInfo current_user=null;
    public GameObject user_panels_control;
    public GameObject prefab_user_panel;
    public GameObject new_user_prompt;
    public GameObject new_user_prompt_lastusers;
    public GameObject user_status;
    public GameObject input_name;

    private int pPanelWidth = 2200;
    private string xmlLoc;

    public static List<UserInfo> user_list;
    // Use this for initialization
    void Start()
    {
        mThis = this;



    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        if (current_user == null)
        {
            initUserProfile();
            new_user_prompt.SetActive(true);
            user_status.SetActive(false);
        }
         else
        {
            new_user_prompt.SetActive(false);
         //   this.prefab_user_panel.GetComponent<user_panel>().LoadUser(current_user);
            user_status.SetActive(true);
        }
        //start the user name input / selection menu

    }
    private void initUserProfile()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            xmlLoc = Application.persistentDataPath + @"/ARMath_user_profile.xml";
        } else
        {
            xmlLoc = "ARMath_user_profile.xml";
        }
        if (!File.Exists(xmlLoc))
        {
            createUserXML();
        }
        loadUserProfiles(0);

        
        //loadAllUserProfiles();
    }
    void createUserXML()
    {
        try
        {
            //File.Create(xmlLoc).Dispose(); // Break the stream with file immediately after file creation
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);
            XmlNode rootu = xmlDoc.CreateNode("element", "users", "");
            xmlDoc.AppendChild(rootu);
            xmlDoc.Save(xmlLoc);
            /*try
            {
                using (StreamWriter sW = new StreamWriter(xmlLoc))
                { // This will dispose he resources used by StreamWriter
                    sW.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sW.WriteLine("<users>");                    
                    sW.WriteLine("</users>");
                }
            }
            catch (IOException ex)
            {
                Debug.Log("Error setting up root element for XML : " + ex.TargetSite);
            }*/
        }
        catch (IOException ex)
        {
            Debug.Log("Error creating xml file : " + ex.TargetSite);
        }
    }
    private void loadUserProfiles(int u_id) {
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.Load(xmlLoc); // load the file.
        XmlNodeList userList = xmlDoc.GetElementsByTagName("user");
        XmlNodeList userRoot = xmlDoc.GetElementsByTagName("users");
        user_list = new List<UserInfo>();
        if(userRoot==null || userRoot.Count==0)
        {
            Debug.Log("Error <users> doesn't exist in user profile xml file : ");
            return;
        }
        
        foreach(XmlNode userXML in userList)
        {
            UserInfo u = new UserInfo();
            //Debug.Log("[ARMath] loading user "+userXML.InnerXml);
            XmlNodeList userproperties = userXML.ChildNodes;
            foreach(XmlNode prop in userproperties)
            {
                if(prop.Name=="lastused")
                {
                    u.lastused = System.Convert.ToInt32(prop.InnerXml);
                } else if(prop.Name =="name")
                {
                    u.user_name = prop.InnerXml;
                } else if (prop.Name == "id")
                {
                    u.user_id = System.Convert.ToInt32(prop.InnerXml);
                } else if (prop.Name == "gems")
                {
                    //Debug.Log("[ARMath] loading user gems" + prop.InnerXml);
                    foreach (XmlNode gem in prop.ChildNodes)
                    {
                        if (gem.Name == "gem")
                        {
                            //Debug.Log("[ARMath] loading user gem" + gem.InnerXml);
                            Gem g = new Gem();
                            foreach (XmlNode gemprop in gem.ChildNodes)
                            {
                               // Debug.Log("[ARMath] loading user prop" + gemprop.InnerXml);
                                if (gemprop.Name == "type")
                                {
                                    g.problem_type = (ProblemType) System.Convert.ToInt32(gemprop.InnerXml);
                                } else if(gemprop.Name == "interaction")
                                {
                                    if (gemprop.InnerXml == "virtual") g.is_virtual_interaction = true;
                                    else g.is_virtual_interaction = false;
                                }
                            }
                            u.AddGem(g);
                        }
                    }
                }
            }
            user_list.Add(u);
            
        }
        SetCurrentUser(u_id);
        new_user_prompt_lastusers.GetComponent<usericons>().load_users(user_list);
        //load


    }
    private void update_user(UserInfo u)
    {
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.Load(xmlLoc); // load the file.
        XmlNodeList userList = xmlDoc.GetElementsByTagName("user");
        XmlNodeList userRoot = xmlDoc.GetElementsByTagName("users");
        user_list = new List<UserInfo>();
        if (userRoot == null || userRoot.Count == 0)
        {
            Debug.Log("Error <users> doesn't exist in user profile xml file : ");
            return;
        }
        XmlNode user_profile_xml = null;
        foreach (XmlNode userXML in userList)
        {            
            XmlNodeList userproperties = userXML.ChildNodes;
            foreach (XmlNode prop in userproperties)
            {
                
                if (prop.Name == "id" && prop.InnerXml == u.user_id.ToString())
                {
                    user_profile_xml = userXML;

                }
            }
            
        }
        if(user_profile_xml==null)
        {
            user_profile_xml = xmlDoc.CreateNode("element", "user", "");
            userRoot[0].AppendChild(user_profile_xml);
            XmlNode xml_id = xmlDoc.CreateNode("element", "id", "");
            xml_id.InnerXml = u.user_id.ToString();
            XmlNode xml_name = xmlDoc.CreateNode("element", "name", "");
            xml_name.InnerXml = u.user_name;
            XmlNode xml_lastused = xmlDoc.CreateNode("element", "lastused", "");
            xml_lastused.InnerXml = DateTime.Now.ToString("MMddHHmm");
            XmlNode xml_gem = xmlDoc.CreateNode("element", "gems", "");
            xml_gem.InnerXml ="";
            foreach(KeyValuePair<ProblemType,List<Gem>> gpair in u.gem_collection)
            {
                foreach(var g in gpair.Value)
                {
                    XmlNode xml_gem_node = xmlDoc.CreateNode("element", "gem","");
                    xml_gem_node.InnerXml = "";
                    XmlNode xml_gem_type = xmlDoc.CreateNode("element", "type", "");
                    xml_gem_type.InnerXml = ((int) g.problem_type).ToString();
                    XmlNode xml_gem_interaction = xmlDoc.CreateNode("element", "interaction", "");
                    if (g.is_virtual_interaction) xml_gem_interaction.InnerXml = "virtual";
                    else xml_gem_interaction.InnerXml = "tangible";
                    xml_gem_node.AppendChild(xml_gem_type);
                    xml_gem_node.AppendChild(xml_gem_interaction);
                    xml_gem.AppendChild(xml_gem_node);

                }
            }
            user_profile_xml.AppendChild(xml_id);
            user_profile_xml.AppendChild(xml_name);
            user_profile_xml.AppendChild(xml_lastused);
            user_profile_xml.AppendChild(xml_gem);

        } else
        {
            foreach (XmlNode prop in user_profile_xml)
            {
                if (prop.Name == "lastused")
                {
                    prop.InnerXml = DateTime.Now.ToString("MMddHHmm"); 
                    
                } else if (prop.Name == "name")
                {
                    prop.InnerXml = u.user_name;
                } else if (prop.Name == "id")
                {
                    prop.InnerXml = u.user_id.ToString();
                } else if(prop.Name == "gems")
                {
                    while (prop.HasChildNodes)
                    {
                        prop.RemoveChild(prop.FirstChild);
                    }                    
                    foreach (KeyValuePair<ProblemType, List<Gem>> gpair in u.gem_collection)
                    {
                        foreach (var g in gpair.Value)
                        {
                            XmlNode xml_gem_node = xmlDoc.CreateNode("element", "gem", "");
                            xml_gem_node.InnerXml = "";
                            XmlNode xml_gem_type = xmlDoc.CreateNode("element", "type", "");
                            xml_gem_type.InnerXml = ((int)g.problem_type).ToString();
                            XmlNode xml_gem_interaction = xmlDoc.CreateNode("element", "interaction", "");
                            if (g.is_virtual_interaction) xml_gem_interaction.InnerXml = "virtual";
                            else xml_gem_interaction.InnerXml = "tangible";
                            xml_gem_node.AppendChild(xml_gem_type);
                            xml_gem_node.AppendChild(xml_gem_interaction);
                            prop.AppendChild(xml_gem_node);
                            Debug.Log("[ARMath] adding a gem");

                        }
                    }
                }
                
            }
        }
     //   Debug.Log("[ARMath] user root: " + userRoot[0].InnerXml);
     //   Debug.Log("[ARMath] user: " + user_profile_xml.InnerXml);
        xmlDoc.Save(xmlLoc);
    }
    public void new_user()
    {
        string name= input_name.GetComponent<Text>().text;
        if (name == "")
        {
            TTS.mTTS.GetComponent<TTS>().StartTextToSpeech("Please enter your name");
            return;
        }
        UserInfo u = new UserInfo();
        u.user_name = name;
        Debug.Log("1" + DateTime.Now.ToString("HHmmss"));
        u.user_id = System.Convert.ToInt32("1"+DateTime.Now.ToString("HHmmss"));
        update_user(u);
        loadUserProfiles(u.user_id);

        user_status.SetActive(true);
        new_user_prompt.SetActive(false);
        user_status.GetComponentInChildren<user_panel>().LoadUser(current_user);

    }
    public static void OnSelectUser(int user_id)
    {
        SetCurrentUser(user_id);
        mThis.user_status.SetActive(true);
        mThis.new_user_prompt.SetActive(false);
        mThis.user_status.GetComponentInChildren<user_panel>().LoadUser(current_user);
    }
    
    public static int get_problem_level(ProblemType p)
    {
        int lvl = 0;
        if (current_user == null) return 0;
        bool virtual_mode = SystemControl.mSystemControl.get_system_setup_interaction_touch();
        if (current_user.gem_collection.ContainsKey(p))
        {
            foreach(var o in current_user.gem_collection[p])
            {
                if (o.is_virtual_interaction == virtual_mode) lvl++;
            }
        }
        return lvl;
    }
    public static void AddGem(ProblemType p)
    {
        Gem g = new Gem();
        bool interaction_virtual_enabled = SystemControl.mSystemControl.get_system_setup_interaction_touch();
        g.is_virtual_interaction = interaction_virtual_enabled;
        g.problem_type = p;
        current_user.AddGem(g);
        mThis.GetComponent<AudioSource>().Play();
        mThis.update_user(current_user);
        SystemControl.mSystemControl.onUser();

    }
    public static void SetCurrentUser(int uid)
    {
        foreach (UserInfo u in user_list)
        {
            if (uid == u.user_id)
            {
                current_user = u;
                Debug.Log("[ARMath] Set the current user for UID:" + uid);
                mThis.update_user(current_user);
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
            newUserPane.GetComponent<RectTransform>().localPosition = new Vector3(anchor_pos.x, anchor_pos.y, 0);
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
        u1.user_name = "group1";
        u2.user_name = "group2";
        u3.user_name = "group3";
        u4.user_name = "group4";
        u1.user_id = 343;
        u2.user_id = 23232;
        u3.user_id = 343454;
        u4.user_id = 34;
        Gem g = new Gem();
        g.problem_type = ProblemType.p1_counting;
        u1.AddGem(g);
        u2.AddGem(g);
        u3.AddGem(g);
        u4.AddGem(g);
        /*
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
        }*/

        List<UserInfo> ret = new List<UserInfo>();
        ret.Add(u1);
        ret.Add(u2);
        ret.Add(u3);
        ret.Add(u4);

      
        return ret;
    }

    
}
