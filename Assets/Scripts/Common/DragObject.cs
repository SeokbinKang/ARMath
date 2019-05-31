using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DragObject : MonoBehaviour, IDragHandler, IDropHandler
{


    private bool dragging=true;
    private bool hide_children_onDragging = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(hide_children_onDragging) setchildrenvisibility();

    }
    void OnEnable()
    {
        
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
        
    }
    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            dragging = false;
        }
        
    }
    public void setchildrenvisibility()
    {
        int child_n = this.transform.childCount;
        if (child_n == 0) return;
        Debug.Log("[ARMath] dragging status " + dragging + "   "+child_n);
        if(dragging)
        {
            if (this.transform.GetChild(0).gameObject.activeSelf) {
                for(int i = 0; i < child_n; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }
            }            
        } else
        {
            if (!this.transform.GetChild(0).gameObject.activeSelf)
            {
                for (int i = 0; i < child_n; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        
    }
}
