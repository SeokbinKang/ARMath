using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconContainer : MonoBehaviour {



    private GridLayoutGroup grid;

    private int item_per_row = 10;
	// Use this for initialization
	void Start () {
        grid = this.GetComponent<GridLayoutGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetIcon(string obj_name, int cnt)
    {
        GameObject icon_obj = null;
        Vector2 icon_row_size;
        Vector2 cellSize;
        int rows_in_grid;
        int cols_in_grid;
        //retrieving icon 
        //Debug.Log("[ARMath] Setting IconContainer:" + obj_name + "    " + cnt);
        if (obj_name.Contains("coin")) obj_name = "coin";
        icon_obj = AssetManager.get_icon(obj_name);
        
        
        if (icon_obj == null) return;
        grid = this.GetComponent<GridLayoutGroup>();
        icon_row_size.x = icon_obj.GetComponent<RawImage>().texture.width;
        icon_row_size.y = icon_obj.GetComponent<RawImage>().texture.height;

        //adjust cell size
        rows_in_grid = (cnt-1) / 10+1;

        cols_in_grid = cnt % 11;
        if (cnt >= 10) cols_in_grid = 10;

        cellSize.x = this.gameObject.GetComponent<RectTransform>().rect.width / cols_in_grid;
        cellSize.y = this.gameObject.GetComponent<RectTransform>().rect.height / rows_in_grid;

        if(cellSize.x/cellSize.y > icon_row_size.x/icon_row_size.y)
        {
            cellSize.x = cellSize.y * icon_row_size.x / icon_row_size.y;
        } else
        {
            cellSize.y = cellSize.x * icon_row_size.y / icon_row_size.x;
        }

        grid.cellSize = cellSize;


        //add icons
        //delete all children gems
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < cnt; i++)
        {
            UnityEngine.GameObject label = Instantiate(icon_obj, Vector3.zero, Quaternion.identity) as GameObject;
            label.transform.SetParent(this.gameObject.transform);
            label.GetComponent<RectTransform>().localScale = Vector3.one;
            
        }

    }
}
