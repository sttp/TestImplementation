using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Codec;
using Sttp.Codec.DataPoint;
using Sttp.Codec.Metadata;
using Sttp.Data;

namespace Sttp.Tests
{
    [TestClass]
    public class GetDataTest
    {
        [TestMethod]
        public void TestGetMetadata()
        {
            Queue<byte[]> packets = new Queue<byte[]>();

            var writer = new WireEncoder();
            var reader = new WireDecoder();

            writer.NewPacket += (bytes, start, length) => packets.Enqueue(Clone(bytes, start, length));

            var markup = new SttpMarkupWriter("Historical");
            {
                markup.WriteValue("Start", DateTime.Parse("12/1/2017 1:00 AM"));
                markup.WriteValue("Stop", DateTime.Parse("12/1/2017 2:00 AM"));
                using (markup.StartElement("PointList"))
                {
                    markup.WriteValue("ID", 1);
                    markup.WriteValue("ID", 2);
                    markup.WriteValue("ID", 3);
                    markup.WriteValue("ID", 4);
                }
            }
            writer.SendCustomCommand(markup);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.FillBuffer(data, 0, data.Length);
            }

            CommandObjects cmd = reader.NextCommand();
            Assert.AreEqual(cmd.CommandName, "Historical");

            Console.WriteLine(cmd.Unknown.Markup.ToYAML());

            var request = cmd.Unknown.Markup.MakeReader().ReadEntireElement();
            DateTime startTime = request.GetValue("Start").AsDateTime;
            DateTime stopTime = request.GetValue("Stop").AsDateTime;
            var points = request.GetElement("PointList").ForEachValue("ID").Select(x => x.AsInt32).ToList();

            var con = new SqlConnection("Server=phasordb;Database=PhasorValues_5_1PerMin_2017;Trusted_Connection=True;");
            con.Open();

            var dt = new DataTable();
            var ta = new SqlDataAdapter($"SELECT [Timestamp],[TermID],[IM_1],[IA_1],[VM_1],[VA_1],[Freq],[DFDT],[Status] FROM [DATA]"
                + $" where Timestamp between '{startTime}' and '{stopTime}' and TermID in ({string.Join(",", points)})", con);
            ta.Fill(dt);
            con.Close();

            var enc = new BasicEncoder(1000);
            int pointCount = 0;
            var dataPoint = new SttpDataPoint();
            foreach (DataRow row in dt.Rows)
            {
                var termid = (int)(short)row["TermID"];
                var time = (DateTime)row["Timestamp"];
                var vm = (float)row["VM_1"];
                var va = (float)row["VA_1"];
                var im = (float)row["IM_1"];
                var ia = (float)row["IA_1"];
                var freq = (float)row["Freq"];
                var dfdt = (float)row["DFDT"];
                var status = (short)row["Status"];

                dataPoint.DataPointID = new SttpDataPointID();
                dataPoint.DataPointID.RuntimeID = termid * 10;
                dataPoint.Time = new SttpTime(time);
                dataPoint.TimestampQuality = 0;
                dataPoint.ValueQuality = 0;
                dataPoint.Value = (SttpValue)vm;

                enc.AddDataPoint(dataPoint);
                dataPoint.DataPointID.RuntimeID++;
                dataPoint.Value = (SttpValue)va;
                enc.AddDataPoint(dataPoint);
                dataPoint.DataPointID.RuntimeID++;
                dataPoint.Value = (SttpValue)im;
                enc.AddDataPoint(dataPoint);
                dataPoint.DataPointID.RuntimeID++;
                dataPoint.Value = (SttpValue)ia;
                enc.AddDataPoint(dataPoint);
                dataPoint.DataPointID.RuntimeID++;
                dataPoint.Value = (SttpValue)freq;
                enc.AddDataPoint(dataPoint);
                dataPoint.DataPointID.RuntimeID++;
                dataPoint.Value = (SttpValue)dfdt;
                enc.AddDataPoint(dataPoint);
                dataPoint.DataPointID.RuntimeID++;
                dataPoint.Value = (SttpValue)status;
                enc.AddDataPoint(dataPoint);
                pointCount += 7;
            }

            byte[] d2 = enc.ToArray();
            writer.DataPointReply(Guid.Empty, true, 0, d2);

            while (packets.Count > 0)
            {
                var data = packets.Dequeue();
                reader.FillBuffer(data, 0, data.Length);
            }

            cmd = reader.NextCommand();
            Assert.AreEqual(cmd.CommandName, "DataPointReply");

            var dec = new BasicDecoder();
            dec.Load(cmd.DataPointReply.Data);

            dataPoint = new SttpDataPoint();
            while (dec.Read(dataPoint))
            {
                Console.WriteLine(dataPoint.ToString());
            }
        }


        private byte[] Clone(byte[] data, int pos, int length)
        {
            byte[] rv = new byte[length];
            Array.Copy(data, pos, rv, 0, length);
            return rv;
        }

    }
}
