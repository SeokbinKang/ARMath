using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppCountingUIExplore : MonoBehaviour {


    public GameObject gem;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LocateGem(Rect box, string objname)
    {
        RectTransform r= gem.GetComponent<RectTransform>();                
        r.position = new Vector3(box.center.x, Screen.height - box.center.y, 0);
        Debug.Log("GEM box center : " + box.center.x + "  " + box.center.y);
        Debug.Log("GEM screen : " + Screen.width + "  " + Screen.height);
        gem.GetComponent<AppGem>().objectName = objname;
        if (objname == "bottle") gem.GetComponent<RawImage>().color = Color.white;
        if (objname == "apple") gem.GetComponent<RawImage>().color = Color.red;
        if (objname == "remote") gem.GetComponent<RawImage>().color = Color.yellow;

    }

}
