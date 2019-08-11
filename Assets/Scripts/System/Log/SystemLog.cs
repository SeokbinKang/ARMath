using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SystemLog : MonoBehaviour {
    public static SystemLog mThis;
    public float log_timer;
    public string user_log_directory;
    private float nextActionTime = 0.0f;

    // Use this for initialization
    void Start () {
        if (log_timer < 3) log_timer = 3;
        nextActionTime = 0;
        mThis = this;

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime)
        {
            //Debug.Log("[ARMath] Screenshot tick");
            nextActionTime = Time.time + log_timer;
            // execute block of code here
            save_screenshot();
        }
    }
    private string get_user_directory()
    {
        string uname = "default";
        if (SystemUser.current_user != null) uname = SystemUser.current_user.user_name;

        if (Application.platform == RuntimePlatform.Android)
        {
            //user_log_directory = Application.persistentDataPath + @"/"+ uname;
            user_log_directory = uname+"/";
            if (!Directory.Exists(Application.persistentDataPath + @"/" + uname))
            {
                Debug.Log("[ARMath] creating log dir: " + Application.persistentDataPath + @"/" + uname);
                Directory.CreateDirectory(Application.persistentDataPath + @"/" + uname);

            }
            
        }
        else
        {
            if (!Directory.Exists( uname))
            {
                Directory.CreateDirectory(uname);
            }
            user_log_directory =  uname+"/"; 
        }
        
        return user_log_directory;
        
    }
    private void save_screenshot()
    {
        
        string uname = "default";
        if (SystemUser.current_user != null) uname = SystemUser.current_user.user_name;
        
        string path = get_user_directory();
        string filename  = uname+DateTime.Now.ToString("ddHHmmss")+".png";
        
        ScreenCapture.CaptureScreenshot(path+filename);
        //Debug.Log("[ARMath] Saving screenshot done..." + path + filename);
    }
    public static void WriteLog(string lines)
    {
        string uname = "default";
        string path = mThis.get_user_directory();
        if (SystemUser.current_user != null) uname = SystemUser.current_user.user_name;
        
        lines = DateTime.Now.ToString("ddHHmmss") + "    " + lines+"\n";
        string file_path = Application.persistentDataPath + @"/" + path + uname + "_log.txt";
        
        

        File.AppendAllText(file_path, lines);
    }
}
