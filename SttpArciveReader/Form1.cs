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
using Sttp;

namespace SttpArciveReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dataGridView1, true);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (chkReadRaw.Checked)
                    {
                        NewMethod(dlg.FileName);
                    }
                    else
                    {
                        NewMethod2(dlg.FileName);

                    }

                }
            }

        }

        private void NewMethod(string fileName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Command", typeof(string));
            dt.Columns.Add("Length", typeof(int));
            dt.Columns.Add("Record", typeof(string));

            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var ctp = new CtpFileStream(fs, false))
            {
                while (true)
                {
                    var cmd = ctp.Read();
                    if ((object)cmd == null)
                        break;
                    dt.Rows.Add(cmd.RootElement, cmd.Length, cmd);
                }
            }

            dataGridView1.DataSource = dt;
        }

        private void NewMethod2(string fileName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Command", typeof(string));
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("Record", typeof(string));

            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var ctp = new SttpFileReader(fs, false))
            {
                while (true)
                {
                    switch (ctp.Next())
                    {
                        case FileReaderItem.ProducerMetadata:
                            var md = ctp.GetMetadata();
                            dt.Rows.Add("Metadata", md.ToCommand().ToXML().Length, md.ToString());
                            break;
                        case FileReaderItem.DataPoint:
                            var sb = new StringBuilder();
                            var dp = new SttpDataPoint();
                            int cnt = 0;
                            while (ctp.ReadDataPoint(dp))
                            {
                                cnt++;
                                sb.Append(dp.Value.AsString);
                                sb.Append(" ");
                            }
                            dt.Rows.Add("Points", cnt, sb.ToString());
                            break;
                        case FileReaderItem.EndOfStream:
                            dataGridView1.DataSource = dt;
                            return;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }
    }
}
