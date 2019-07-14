using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IDragHandler, IDropHandler
{


    private bool dragging=true;
    private bool hide_children_onDragging = true;

    public bool adjust_alpha;
    public float alpha_on_drop;
    public float alpha_on_move;
    public float alpha_on_init;

    public GameObject base_graphic;
	// Use this for initialization
	void Start () {
        adjust_alpha = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(hide_children_onDragging) setchildrenvisibility();

    }
    void OnEnable()
    {
        
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
        Vector2 mouse_pos = eventData.position;
        this.GetComponent<RectTransform>().position = new Vector3(mouse_pos.x, mouse_pos.y, 0);
        dragging = true;
        change_alpha(alpha_on_move);

     //   Debug.Log("[ARMath] on drag");

    }
    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            dragging = false;
        }
        change_alpha(alpha_on_drop);

        this.GetComponent<AudioSource>().Play();
    }
    public void setchildrenvisibility()
    {
        int child_n = this.transform.childCount;
        if (child_n == 0) return;
        Debug.Log("[ARMath] dragging status " + dragging + "   "+child_n);
        if(dragging)
        {
            int i = 0;
            if (base_graphic != null) i = 1;
            if (this.transform.GetChild(i).gameObject.activeSelf) {
                
                for(; i < child_n; i++)
                {
                    
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }
            }            
        } else
        {
            int i = 0;
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
}
