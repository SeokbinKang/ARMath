using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidCallback(string param);
public delegate void VoidCallback2(int idx,string param);

public class TimerCallback {

    public VoidCallback cb;
    public VoidCallback2 cb2;
    // Use this for initialization
    float last_check;
    float time_out;
    int param_idx;
    string param;
    //execute the callback when there is no check for a timeout
    public TimerCallback(VoidCallback c, string param_,float timeout)
    {
        last_check = Time.time;
        cb = c;
        cb2 = null;
        time_out = timeout;
        param = param_;
    }
    public TimerCallback(VoidCallback2 c, int idx, string param_, float timeout)
    {
        last_check = Time.time;
        param_idx = idx;
        cb = null;
        cb2 = c;
        time_out = timeout;
        param = param_;
    }

    public bool tick()
    {
        if (Time.time > last_check + time_out)
        {
            if(cb!=null)  cb(param);
            if (cb2 != null) cb2(param_idx, param);
            return true;
        }
        return false;
    }
    public void checkin()
    {
        last_check = Time.time;
    }

}
