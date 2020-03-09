#define Graph_And_Chart_PRO
using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class DistBarChartFeed : MonoBehaviour {
	void Start () {
        BarChart barChart = GetComponent<BarChart>();
        if (barChart != null)
        {
            barChart.DataSource.SlideValue("Distractions", "4pm", Random.value * 30, 20f);
        }
    }
    private void Update()
    {
    }
}
