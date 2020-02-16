#define Graph_And_Chart_PRO
using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class AttStreamingGraph : MonoBehaviour
{
    public GraphChart Graph;
    public int TotalPoints = 5;
    float lastTime = 0f;
    float lastX = 0f;

    void Start()
    {
        if (Graph == null) // the ChartGraph info is obtained via the inspector
            return;
        float x = 3f * TotalPoints;
        Graph.DataSource.StartBatch(); // calling StartBatch allows changing the graph data without redrawing the graph for every change
        Graph.DataSource.ClearCategory("Attention"); // clear the "Attention" category. this category is defined using the GraphChart inspector
        Graph.DataSource.ClearCategory("Distractions"); // clear the "Distractions" category. this category is defined using the GraphChart inspector

        for (int i = 0; i < TotalPoints; i++)  //add random points to the graph
        {
            Graph.DataSource.AddPointToCategory("Attention", System.DateTime.Now - System.TimeSpan.FromSeconds(x), Random.value * 20f + 10f); // each time we call AddPointToCategory 
            Graph.DataSource.AddPointToCategory("Distractions", System.DateTime.Now  - System.TimeSpan.FromSeconds(x), Random.value * 10f); // each time we call AddPointToCategory 
            x -= Random.value * 3f;
            lastX = x;
        }

        Graph.DataSource.EndBatch(); // finally we call EndBatch , this will cause the GraphChart to redraw itself
    }

    void Update()
    {
        float time = Time.time;
        if (lastTime + 2f < time)
        {
            lastTime = time;
            lastX += Random.value * 3f;
//            System.DateTime t = ChartDateUtility.ValueToDate(lastX);
            Graph.DataSource.AddPointToCategoryRealtime("Attention", System.DateTime.Now, Random.value * 20f + 10f, 1f); // each time we call AddPointToCategory 
            Graph.DataSource.AddPointToCategoryRealtime("Distractions", System.DateTime.Now, Random.value * 10f, 1f); // each time we call AddPointToCategory
        }

    }
}
