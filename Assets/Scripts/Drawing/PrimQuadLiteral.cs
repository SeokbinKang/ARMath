﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class PrimQuadLiteral : Graphic
{

    public Vector2[] mVect;

    public void setVertices(Vector2[] vs)
    {
        if (mVect == null || mVect.Length!=vs.Length)
        {
            mVect = (Vector2[])vs.Clone();
            return ;
        }
        for(int i = 0; i < mVect.Length; i++)
        {
            mVect[i] = vs[i];
        }
        return;
    }
    public void setColor(Color c)
    {
        this.color = c;
    }

    protected override void Start()
    {
        
      
    }
    private void draw_mVectors(VertexHelper vh)
    {
        if (mVect.Length < 4) return;
        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        vert.position = new Vector2(mVect[0].x, mVect[0].y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(mVect[1].x, mVect[1].y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(mVect[2].x, mVect[2].y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(mVect[3].x, mVect[3].y);
        vert.color = color;
        vh.AddVert(vert);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        draw_mVectors(vh);
        return;
        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0f;
        corner1.y = 0f;
        corner2.x = 1f;
        corner2.y = 1f;

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;

        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        vert.position = new Vector2(corner1.x, corner1.y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(corner1.x, corner2.y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x, corner2.y);
        vert.color = color;
        vh.AddVert(vert);

        vert.position = new Vector2(corner2.x*0.5f, corner1.y);
        vert.color = color;
        vh.AddVert(vert);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}