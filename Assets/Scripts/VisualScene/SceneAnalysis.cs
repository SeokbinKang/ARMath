using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnalysis {

    // Find a cluster of the same objects. A variation of Density-Based Spatial Clustering of Applications with Noise (DBSCAN)
    public static List<SceneObject> GetCluster(List<SceneObject> all_objects)
    {
        List<SceneObject> res = null;
        List<List<SceneObject>> clusters = new List<List<SceneObject>>();
        bool[] visited = new bool[all_objects.Count];

        while (true)
        {
            bool all_visited=true;
            int cluster_seed = -1;
            for(int i=0;i<visited.Length;i++)
            {
                if (!visited[i])
                {
                    cluster_seed = i;
                    break;
                }
            }
                
            if (cluster_seed==-1) break;

            //draw a random point
            
            List<int> expanded_point = new List<int>();
            int seed_index=0;
            expanded_point.Add(cluster_seed);
            visited[cluster_seed] = true;
            //expand the cluster until it has no neighboring point
            while (true)
            {
                if (seed_index >= expanded_point.Count) break;
                cluster_seed = expanded_point[seed_index];
                for (int i = 0; i < visited.Length; i++)
                {
                    if (visited[i]) continue;
                    Vector2 dist = all_objects[cluster_seed].catalogInfo.Box.center - all_objects[i].catalogInfo.Box.center;
                    if(dist.magnitude<SystemParam.cluster_neighboring_distance)
                    {
                        visited[i] = true;
                        expanded_point.Add(i);                        
                    }
                }
                seed_index++;

            }

            List<SceneObject> current_cluster = new List<SceneObject>();
            foreach(var i in expanded_point)
            {
                current_cluster.Add(all_objects[i]);
            }
            //next while
            clusters.Add(current_cluster);
        }

        //choose the most appropriate cluster. randomly pick a cluster on the left side. 
        List<SceneObject> right_most;
        int k = 0;
        //Debug.Log("[ARMath] num obj: " + all_objects.Count + "  clusters: " + clusters.Count);
        foreach (var c in clusters)
        {
        //    Debug.Log("[ARMath] cluster object: " + c[0].catalogInfo.DisplayName + "  count:" + c.Count + "   pos:" + c[0].catalogInfo.Box);
            if(c[0].catalogInfo.Box.center.x < Screen.width / 2 && c.Count >= SystemParam.cluster_min_count)
            {
                res = c;
                //break;
            }
        }
        if(res==null && clusters.Count>0)
        {
            //random pick
            res = clusters[0];
        }

        return res;
    }

    public static  Rect get_bounding_box(List<SceneObject> objects)
    {
        Rect res = new Rect();
        if (objects == null || objects.Count == 0) return res;
        Vector2 LT, RB;        
        LT.x = 99999;
        LT.y = 99999;
        RB.x = -99999;
        RB.y = -99999;
        foreach (var o in objects)
        {
            if (o.catalogInfo.Box.xMin < LT.x) LT.x = o.catalogInfo.Box.xMin;
            if (o.catalogInfo.Box.yMin < LT.y) LT.y = o.catalogInfo.Box.yMin;
            if (o.catalogInfo.Box.xMax > RB.x) RB.x = o.catalogInfo.Box.xMax;
            if (o.catalogInfo.Box.yMax > RB.y) RB.y = o.catalogInfo.Box.yMax;
        }
        RB.y = Screen.height - RB.y;
        LT.y = Screen.height - LT.y;

        res = new Rect(LT.x, RB.y, RB.x - LT.x,  LT.y - RB.y);
        return res;
    }
}
