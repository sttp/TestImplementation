using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Data;

namespace Sttp.Tests
{
    [TestClass]
    public class GetOpenPDCDataSet
    {
        [TestMethod]
        public void WriteToXML()
        {
            DataSet ds = new DataSet("openPDC");

            var con = new SqlConnection("Server=phasor3\\sqlexpress;Database=openPDC;Trusted_Connection=True;");
            con.Open();

            FillData("Company", con, ds);
            FillData("Node", con, ds);
            FillData("Device", con, ds);
            FillData("VendorDevice", con, ds);
            FillData("Vendor", con, ds);
            FillData("Protocol", con, ds);
            FillData("Interconnection", con, ds);
            FillData("Measurement", con, ds);
            FillData("SignalType", con, ds);

            con.Close();

            AddPrimayKey("Company", "ID", ds);
            AddPrimayKey("Node", "ID", ds);
            AddPrimayKey("Device", "ID", ds);
            AddPrimayKey("VendorDevice", "ID", ds);
            AddPrimayKey("Vendor", "ID", ds);
            AddPrimayKey("Protocol", "ID", ds);
            AddPrimayKey("Interconnection", "ID", ds);
            AddPrimayKey("Measurement", "SignalID", ds);
            AddPrimayKey("SignalType", "ID", ds);


            AddForeignKey("Node", "CompanyID", "Company", "ID", ds);
            AddForeignKey("Device", "CompanyID", "Company", "ID", ds);
            AddForeignKey("Device", "ParentID", "Device", "ID", ds);
            AddForeignKey("Device", "InterconnectionID", "Interconnection", "ID", ds);
            AddForeignKey("Device", "NodeID", "Node", "ID", ds);
            AddForeignKey("Device", "ProtocolID", "Protocol", "ID", ds);
            AddForeignKey("Device", "VendorDeviceID", "VendorDevice", "ID", ds);
            AddForeignKey("VendorDevice", "VendorID", "Vendor", "ID", ds);
            AddForeignKey("Measurement", "DeviceID", "Device", "ID", ds);
            AddForeignKey("Measurement", "SignalTypeID", "SignalType", "ID", ds);

            using (var fs = new FileStream("c:\\temp\\openPDC-sttp.xml", FileMode.Create))
            {
                ds.WriteXml(fs, XmlWriteMode.WriteSchema);
            }

        }

        private static void FillData(string tableName, SqlConnection con, DataSet ds)
        {
            var companyDA = new SqlDataAdapter($"SELECT * FROM {tableName}", con);
            companyDA.TableMappings.Add("Table", tableName);
            companyDA.Fill(ds);
        }

        private static void AddPrimayKey(string table, string column, DataSet ds)
        {
            ds.Tables[table].PrimaryKey = new[] { ds.Tables[table].Columns[column] };
        }

        private static void AddForeignKey(string table, string column, string foreignTable, string foreignColumn, DataSet ds)
        {
            ds.Relations.Add(ds.Tables[foreignTable].Columns[foreignColumn], ds.Tables[table].Columns[column]);
        }
    }
}
