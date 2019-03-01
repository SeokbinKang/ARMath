using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AppCountingUICounting : MonoBehaviour {


    public string TargetObjectName;
    public int count = 0;
    // Use this for initialization
    public GameObject Target;
    public GameObject TotalCount;
    public GameObject TTS;
    public GameObject PrefabCountingTarget;
    public GameObject PrefabCountingLabel;

    private List<GameObject> mLabels;
	void Start () {
        if (mLabels==null) mLabels = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        TotalCount.GetComponent<Text>().text = count.ToString();
    }
    void onAwake()
    {
        count = 0;
        if (mLabels == null) mLabels = new List<GameObject>();
        mLabels.Clear();

    }
    public bool AddTargetObject(Rect box, string objname)
    {
        if (objname != TargetObjectName) return false;
        Vector3 targetPos = new Vector3(box.center.x, Screen.height - box.center.y, 0);
        bool duplicate = false;
        foreach (var t in mLabels)
        {
            Vector3 dist = t.GetComponent<RectTransform>().position-targetPos;
            if (dist.magnitude < 150) duplicate = true;
            Debug.Log("GEM dist " + dist.magnitude + " " + targetPos);
        }
        if (!duplicate)
        {
            RectTransform r = Target.GetComponent<RectTransform>();
            r.position = targetPos;
            return true;
        }
        return false;
        
    }
    public void AddCount(Vector3 pos)
    {
        count++;

        //update object label


        UnityEngine.GameObject label = Instantiate(PrefabCountingLabel, pos, Quaternion.identity) as GameObject;
        label.transform.SetParent(this.gameObject.transform);
        label.GetComponent<Text>().text = count.ToString();
        mLabels.Add(label);
        //update sum label


        //reads the number
        TTS.GetComponent<TTS>().StartTextToSpeech(count.ToString());

    }

    
}
