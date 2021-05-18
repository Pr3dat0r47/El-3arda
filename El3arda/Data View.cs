using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace El3arda
{
    public partial class Data_View : Form
    {
        public static string lastSkill;
        public Data_View()
        {
            InitializeComponent();
        }
        public void Fine()
        {
            List<Label> L = new List<Label>();
            L.Add(label17);
            L.Add(label26);
            L.Add(label20);
            L.Add(label24);
            L.Add(label19);
            L.Add(label23);
            L.Add(label28);
            L.Add(label22);
            L.Add(label21);

            var psi = new ProcessStartInfo();
            psi.FileName = Form1.pydest;

            // 2) Provide script and arguments
            var script = @"fine.py";
            var p2 = lastSkill;

            psi.Arguments = $"\"{script}\" \"{p2}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            using (var process = Process.Start(psi))
            {
                for (int i = 0; i<9; i++)
                {
                    var results = process.StandardOutput.ReadLine();
                    L[i].Text = results;
                }
            }

        }
        public void correlation()
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = Form1.pydest;

            // 2) Provide script and arguments
            var script = @"correlation.py";
            var p2 = lastSkill;

            psi.Arguments = $"\"{script}\" \"{p2}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            using (var process = Process.Start(psi))
            {
                var results = process.StandardOutput.ReadToEnd();
                label2.Text = results;
            }
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Data_View_Load(object sender, EventArgs e)
        {
            lastSkill = Form1.lastSkill;
            correlation();
            Fine();
        }
    }
}
