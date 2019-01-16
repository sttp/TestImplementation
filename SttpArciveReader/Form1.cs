using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTP.IO;

namespace SttpArciveReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance|BindingFlags.NonPublic).SetValue(dataGridView1, true);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Command", typeof(string));
            dt.Columns.Add("Length", typeof(int));
            dt.Columns.Add("Record", typeof(string));
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    using (var fs = new FileStream(dlg.FileName, FileMode.Open))
                    using (var ctp = new CtpFileStream(fs, false))
                    {
                        while (true)
                        {
                            var cmd = ctp.Read();
                            if ((object) cmd == null)
                                break;
                            dt.Rows.Add(cmd.RootElement, cmd.Length, cmd);
                        }
                    }

                    dataGridView1.DataSource = dt;
                }

            }
        }
    }
}
