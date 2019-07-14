using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidCallback(string param);

public class TimerCallback {

    public VoidCallback cb;
    // Use this for initialization
    float last_check;
    float time_out;
    string param;
    //execute the callback when there is no check for a timeout
    public TimerCallback(VoidCallback c, string param_,float timeout)
    {
        last_check = Time.time;
        cb = c;
        time_out = timeout;
        param = param_;
    }
    public bool tick()
    {
        if (Time.time > last_check + time_out)
        {
            cb(param);
            return true;
        }
        return false;
    }
    public void checkin()
    {
        last_check = Time.time;
    }

}
