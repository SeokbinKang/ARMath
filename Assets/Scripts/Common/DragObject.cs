using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IDragHandler, IDropHandler
{


    
    private bool hide_children_onDragging = true;

    public bool adjust_alpha;
    public float alpha_on_drop;
    public float alpha_on_move;
    public float alpha_on_init;

    public bool onDragging;
    public GameObject base_graphic;

    public bool fixed_size_onmove;
    public Vector2 fixed_size;
     private List<GameObject> attached_feedback_gameobject;

    private float last_interact_time;
    private float auto_drop_time = 2f;
	// Use this for initialization
	void Start () {
        adjust_alpha = false;
        attached_feedback_gameobject = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if(hide_children_onDragging) setchildrenvisibility();
        if (onDragging && Time.time > auto_drop_time) OnDrop(null);
    }
    void OnEnable()
    {
        
        Reset();
    }
    private void Reset()
    {
        onDragging = false;
        clear_all_feedback();
    }
    private void OnDisable()
    {
        clear_all_feedback();
    }
    public void SetAlphaAdjustment(bool enabled, float alpha_init, float alpha_moving, float alpha_drop)
    {
        adjust_alpha = enabled;
        alpha_on_init = alpha_init;
        alpha_on_move = alpha_moving;
        alpha_on_drop = alpha_drop;
        change_alpha(alpha_on_init);
    }
    private void change_alpha(float alpha)
    {
        RawImage ri = this.GetComponent<RawImage>();
        if (ri != null)
        {
            Color c = ri.color;
            c.a = alpha;
            ri.color = c;
        }
        Image ri2 = this.GetComponent<Image>();
        if (ri2 != null)
        {
            Color c = ri2.color;
            c.a = alpha;
            ri2.color = c;
        }
    }
    public void AlwaysShowChildren()
    {
        hide_children_onDragging = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
         
        };
        clear_all_feedback();
        last_interact_time = Time.time;
        onDragging = true;
        Vector2 mouse_pos = eventData.position;
        this.GetComponent<RectTransform>().position = new Vector3(mouse_pos.x, mouse_pos.y, 0);
        
        change_alpha(alpha_on_move);
        if(fixed_size_onmove)
        {
            this.GetComponent<RectTransform>().sizeDelta = this.fixed_size;
        }
     //   Debug.Log("[ARMath] on drag");

    }
    public void OnDrop(PointerEventData data)
    {
       
        onDragging = false;
        change_alpha(alpha_on_drop);
        string t = "[Virtual Object Drag-n-Drop] "+this.gameObject.name;
        SystemLog.WriteLog(t);
        if(this.GetComponent<AudioSource>()!=null) this.GetComponent<AudioSource>().Play();
        
    }
    public void setchildrenvisibility()
    {
        int i = 0;
        if (base_graphic != null) i = 1;
        int child_n = this.transform.childCount;
        if (child_n <= i) return;
        //Debug.Log("[ARMath] dragging status " + dragging + "   "+child_n);
        if(!onDragging)
        {
          
            if (this.transform.GetChild(i).gameObject.activeSelf) {
                
                for(; i < child_n; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }
            }            
        } else
        {
            
            if (base_graphic != null) i = 1;
            if (!this.transform.GetChild(i).gameObject.activeSelf)
            {
               
                for (; i < child_n; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        
    }

    public void clear_all_feedback()
    {
        if (attached_feedback_gameobject != null)
        {
            foreach (var i in attached_feedback_gameobject)
            {

                if(i!=null) GameObject.Destroy(i);
            }
            attached_feedback_gameobject.Clear();
        }

    }
    public void clear_number_feedback()
    {
        if (attached_feedback_gameobject != null)
        {
            foreach (var i in attached_feedback_gameobject)
            {
                number_cartoon n_c = i.GetComponent<number_cartoon>();
                if (n_c != null)
                {
                    GameObject.Destroy(i);
                }
            }

        }
    }
    public bool is_feedback_attached()
    {
        attached_feedback_gameobject.RemoveAll(s => s == null);
        if (attached_feedback_gameobject.Count > 0) return true;
        return false;
    }

    public int get_number_feedback()
    {
        int ret = -1;
        if (attached_feedback_gameobject != null)
        {
            foreach (GameObject o in attached_feedback_gameobject)
            {
                if (o == null) continue;
                number_cartoon n_c = o.GetComponent<number_cartoon>();
                if (n_c != null) return n_c.num;
            }
        }
        return ret;
    }
    public int get_all_feedback_count()
    {
        int ret = -1;
        if (attached_feedback_gameobject != null)
        {
            ret = 0;
            foreach(var o in attached_feedback_gameobject)
            {
                if (o != null) ret++;
            }
            return ret;
        }
        return 0;
    }
    public bool attach_object(GameObject feedback_go)
    {
        if (attached_feedback_gameobject == null) attached_feedback_gameobject = new List<GameObject>();
        if (feedback_go != null) this.attached_feedback_gameobject.Add(feedback_go);
        return true;
    }
    public GameObject attached_button_visibility(float alpha)
    {
        foreach (GameObject o in attached_feedback_gameobject)
        {
            if (o.GetComponent<Button>() == null || o.GetComponent<Image>() == null) continue;
            Color c = o.GetComponent<Image>().color;
            c.a = alpha;
            o.GetComponent<Image>().color = c;
            return o;
        }
        return null;
    }
}
