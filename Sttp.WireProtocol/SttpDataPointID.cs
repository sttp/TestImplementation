using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    /// <summary>
    /// This class contains all the variations of representing the PointID.
    /// 
    /// Ideally, all measurements will be mapped to a runtime ID, however, 
    /// for systems that contain millions of measurements, this is not a practical expectation.
    /// 
    /// For a system that could potentially have an unlimited number of PointIDs, the metadata must be stored with the point.
    /// This is the general use case for NamedSet. 
    /// Like financial institutions with billion of identifiers, but very low transactional count per identifier.
    /// 
    /// </summary>
    public class SttpDataPointID
    {
        #region [ Members ]

        public SttpValue PointID { get; set; }
        public int RuntimeID { get; set; } = -1;

        #endregion

        #region [ Constructors ]

        public SttpDataPointID()
        {
            PointID = SttpValue.Null;
        }

        #endregion

        #region [ Properties ]

        public override string ToString()
        {
            return "ID: " + RuntimeID + " " + PointID.ToString();
        }

        #endregion
    }
}
