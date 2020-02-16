#define Graph_And_Chart_PRO
using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class BarChartFeed : MonoBehaviour {
	void Start () {
        BarChart barChart = GetComponent<BarChart>();
        if (barChart != null)
        {
            barChart.DataSource.SetValue("Applications", "Youtube", Random.value * 20);
            barChart.DataSource.SlideValue("Applications", "Outlook", Random.value * 60, 40f);
        }
    }
    private void Update()
    {
    }
}
