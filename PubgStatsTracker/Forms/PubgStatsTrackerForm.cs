using MaterialSkin;
using MaterialSkin.Controls;
using PubgStatsTracker.BusinessLogic;
using PubgStatsTracker.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PubgStatsTracker
{
    public partial class PubgStatsTrackerForm : MaterialForm
    {
        public PubgStatsTrackerForm()
        {
            MaterialSkinManager.Instance.AddFormToManage(this);
            InitializeComponent();

            installButton.Text = AppState.DoesServiceExist ? "Uninstall" : "Install";

            addMapChart();
        }

        private void addMapChart()
        {
            Chart chart = new();
            chart.Series.Clear();
            const string chartArea = "chartArea1";
            const string legend = "legend1";
            chart.ChartAreas.Add(chartArea);
            chart.Legends.Add(legend);

            List<MatchHistoryModel> matchHistory = MatchHistoryModel.GetAllHistory();
            matchHistory.GroupBy(mh => mh.MapId).ToList().ForEach(g =>
            {
                string map = g.FirstOrDefault()?.Map.MapName ?? "unknown";
                Series series = new(map);
                chart.Series.Add(series);
                matchHistory.Select(mh => mh.DateTimePlayed.Date).Distinct().ToList().ForEach(d =>
                    {
                        chart.Series[map].Points.AddXY(d,
                            g.Count(mh => mh.DateTimePlayed.Date == d)
                        );
                    }
                );
                series.ChartType = SeriesChartType.StackedColumn100;
                series.ChartArea = chartArea;
                series.Name = map;
                series.Legend = legend;
                series.XValueType = ChartValueType.DateTime;
            });
            chart.Visible = true;
            chart.Size = new Size(600, 300);
            chart.TabIndex = 0;
            chart.Location = new Point(0, 70);
            Axis x = chart.ChartAreas[chartArea].AxisX;
            x.LabelStyle.Format = "MM-dd";
            x.Interval = 1;
            x.IntervalType = DateTimeIntervalType.Days;
            x.IntervalOffset = 1;
            
            Controls.Add(chart);
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            if (AppState.DoesServiceExist)
            {
                new UninstallForm().ShowDialog();
            }
            else
            {
                new InstallForm().ShowDialog();
            }
        }
    }
}