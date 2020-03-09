#define Graph_And_Chart_PRO
using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class AppsAttBarChartFeed : MonoBehaviour {
	void Start () {
        BarChart barChart = GetComponent<BarChart>();
        if (barChart != null)
        {
            barChart.DataSource.SetValue("Applications", "Youtube", Random.value * 60);
            barChart.DataSource.SetValue("Applications", "Visual Studio", Random.value * 100);

            barChart.DataSource.SlideValue("Applications", "Outlook", Random.value * 100, 10f);
        }
    }
}
