using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using IronPython;
using IronPython.Hosting;

namespace El3arda
{
    public partial class Form1 : Form
    {
        public string constring = "Data Source=eaziserver.database.windows.net;Initial Catalog=Fel3arda_Data;Persist Security Info=True;User ID=EZGI;Password=Cracker2019";
        public static string Nat = "Egypt",pydest,cor;
        public static string lastSkill= "";
        public Color DotColor = Color.Black,LineColor = Color.Maroon;
        public Form1()
        {
            InitializeComponent();
        }
        public void PythonScript()
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = pydest;

            // 2) Provide script and arguments
            var script = @"main.py";
            var N = Nat;
            var end = textBox2.Text;


            psi.Arguments = $"\"{script}\" \"{N}\" \"{end}\"";
            
            
            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output


            var process = Process.Start(psi);
        }

        public void Draw()
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            chart1.ChartAreas[0].AxisX.Title = " Player Age";
            chart1.ChartAreas[0].AxisY.Title = lastSkill;
            if (chart1.Series.IndexOf("Average") == -1)
            {
                chart1.Series.Add("Data Points");
                chart1.Series.Add("Average");
                chart1.Series["Average"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.Series["Data Points"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
                chart1.Series["Average"].BorderWidth = 2;
                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.Series[0].Color = DotColor;
                chart1.Series[1].Color = LineColor;

            }
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
            List<Tuple<int, int>> myData = new List<Tuple<int, int>>();
            Series dataSeries = chart1.Series[0];
            try
            {
                SqlConnection con = new SqlConnection(constring);
                string query = "select * from Players_Sample order by Age";
                SqlCommand sda = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = sda.ExecuteReader();
                int temp1, temp2;
                Tuple<int, int> TempPair;
                while (dr.Read())
                {
                    temp1 = Convert.ToInt32((dr["Age"].ToString()));
                    temp2 = Convert.ToInt32(dr[lastSkill].ToString());
                    TempPair = new Tuple<int, int>(temp1, temp2);
                    myData.Add(TempPair);
                }
        }
            catch
            {
                MessageBox.Show("Please Click Again To Refresh ..");
            };
            foreach (var el in myData)
            {
                dataSeries.Points.AddXY(el.Item1, el.Item2);
            }

            Series avgSeries = chart1.Series[1];

            var grouped = dataSeries.Points.GroupBy(x => x.XValue).Select(x => new { xval = x.Key, yavg = x.Average(y => y.YValues[0]) }).ToList();
            foreach (var kv in grouped)
            {
                avgSeries.Points.AddXY(kv.xval, kv.yavg);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            lastSkill = "Sprint Speed";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            lastSkill = "Stamina";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            lastSkill = "Overall";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Int32 count;
            using (SqlConnection sqlConnection = new SqlConnection(constring))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT Distinct Nationality FROM Players_DataSet ORDER BY Nationality ASC", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    comboBox1.Items.Add(sqlReader["Nationality"].ToString());
                }
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM Players_DataSet", sqlConnection);
                sqlReader.Close();
                count = (Int32)comm.ExecuteScalar();
            }
            textBox2.Text = count.ToString();
            comboBox1.Text = "Egypt";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastSkill == "" || textBox3.Text == "")
                MessageBox.Show("Please Choose a Skill .. And Python.exe file destination");
            else
            {
                PythonScript();
                Draw();
                Data_View page = new Data_View();
                page.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                LineColor = Color.Maroon;
            else
                LineColor = Color.Transparent;
            if (chart1.Series.IndexOf("Average") != -1)
                chart1.Series[1].Color = LineColor;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                DotColor = Color.Black;
            else 
                DotColor = Color.Transparent;
            if (chart1.Series.IndexOf("Dot Points") == -1)
                chart1.Series[0].Color = DotColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false})
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    pydest = fi.FullName;
                    textBox3.Text = pydest;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                Nat =comboBox1.SelectedItem.ToString();
                int count;
                using (SqlConnection sqlConnection = new SqlConnection(constring))
                {
                    sqlConnection.Open();
                    SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM Players_DataSet where Nationality = '"+Nat+"'", sqlConnection);
                    count = (Int32)comm.ExecuteScalar();
                }
                textBox2.Text = count.ToString();
        }
    }
}
