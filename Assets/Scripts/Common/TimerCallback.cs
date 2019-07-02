using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidCallback();

public class TimerCallback {

    public VoidCallback cb;
    // Use this for initialization
    float last_check;
    float time_out;
    //execute the callback when there is no check for a timeout
    public TimerCallback(VoidCallback c, float timeout)
    {
        last_check = Time.time;
        cb = c;
        time_out = timeout;
    }
    public void tick()
    {
        if (Time.time > last_check + time_out) cb();
    }
    public void checkin()
    {
        last_check = Time.time;
    }

}
