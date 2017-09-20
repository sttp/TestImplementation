using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// The set of all metadata supported by the protocol.
    /// </summary>
    public class MetadataSet
    {
        /// <summary>
        /// The metadata versioning is only good for this instance.
        /// if GUID is Empty, revisioning won't be supported.
        /// </summary>
        public Guid InstanceID { get; private set; }

        /// <summary>
        /// This is the version number of the last time this individual table was modified.
        /// </summary>
        internal long RevisionSequenceNumber;

        internal Dictionary<string, MetadataTable> Tables;
        internal SortedList<long, MetadataPatchDetails> Revisions = new SortedList<long, MetadataPatchDetails>();
        public MetadataSet()
        {
            InstanceID = Guid.Empty;
            RevisionSequenceNumber = 0;
            Tables = new Dictionary<string, MetadataTable>();
        }



        public bool LogRevisions
        {
            get
            {
                return InstanceID != Guid.Empty;
            }
            set
            {
                if (LogRevisions != value)
                {
                    if (value)
                    {
                        Revisions.Clear();
                        InstanceID = Guid.NewGuid();
                        RevisionSequenceNumber = 0;
                    }
                    else
                    {
                        Revisions.Clear();
                        InstanceID = Guid.Empty;
                    }
                }

            }
        }

        public void InitializeDefaults()
        {
            FillSchema("Measurement", "DeviceID", ValueType.Guid);
            FillSchema("Measurement", "PointTag", ValueType.String);
            FillSchema("Measurement", "SignalReference", ValueType.String);
            FillSchema("Measurement", "SignalTypeID", ValueType.Int32);
            FillSchema("Measurement", "Adder", ValueType.Decimal);
            FillSchema("Measurement", "Multiplier", ValueType.Decimal);
            FillSchema("Measurement", "Description", ValueType.String);
            FillSchema("Measurement", "ChannelName", ValueType.String);
            FillSchema("Measurement", "SignalType", ValueType.String);
            FillSchema("Measurement", "PositionIndex", ValueType.Int32);
            FillSchema("Measurement", "PhaseDesignation", ValueType.String);
            FillSchema("Measurement", "EngineeringUnits", ValueType.String);
            FillSchema("Measurement", "EngineeringScale", ValueType.Decimal);
            //ToDo: Add More
        }

        public void FillSchema(string tableName, string columnName, ValueType columnType)
        {
            if (InstanceID != Guid.Empty)
            {
                RevisionSequenceNumber++;
                Revisions[RevisionSequenceNumber] = MetadataPatchDetails.FromFillSchema(RevisionSequenceNumber, tableName, columnName, columnType);
            }
            MetadataTable table;
            if (!Tables.TryGetValue(tableName, out table))
            {
                table = new MetadataTable(tableName);
                Tables[tableName] = table;
            }
            table.FillSchema(columnName, columnType);


        }

        public void FillData(string tableName, string columnName, int recordID, object fieldValue)
        {
            if (InstanceID != Guid.Empty)
            {
                RevisionSequenceNumber++;
                Revisions[RevisionSequenceNumber] = MetadataPatchDetails.FromFillData(RevisionSequenceNumber, tableName, columnName, recordID, fieldValue);
            }
            Tables[tableName].FillData(columnName, recordID, fieldValue);
        }

        public bool TryBuildPatchData(Guid instanceID, long cachedVersionID, out List<MetadataPatchDetails> patchingDetails)
        {
            patchingDetails = null;
            if (InstanceID != instanceID || !LogRevisions)
                return false;

            int starting = Revisions.IndexOfKey(cachedVersionID);
            if (starting >= 0)
            {
                patchingDetails = new List<MetadataPatchDetails>();
                for (int x = starting; x < Revisions.Count; x++)
                {
                    patchingDetails.Add(Revisions.Values[x]);
                }
                return true;
            }
            return false;
        }

        public void ClearRevisionHistory(long clearBeforeRevision)
        {
            while (Revisions.Count > 0 && Revisions.Keys[0] < clearBeforeRevision)
            {
                Revisions.RemoveAt(0);
            }
        }
    }
}
