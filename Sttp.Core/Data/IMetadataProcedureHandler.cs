using System.Data.Common;
using CTP;

namespace Sttp.Core.Data
{
    public interface IMetadataProcedureHandler
    {
        DbDataReader ProcessRequest(CtpDocument options);
    }
}